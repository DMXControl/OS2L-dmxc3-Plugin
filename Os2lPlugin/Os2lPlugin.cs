using LumosProtobuf;
using Makaretu.Dns;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Log;
using org.dmxc.lumos.Kernel.Plugin;
using Os2lPlugin.MessageFormats;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using K = LumosLIB.Kernel.Log;

namespace Os2lPlugin
{
    public class Os2lPlugin : KernelPluginBase
    {
        private const string PLUGIN_ID = "{b43ef54b-0124-413a-b0eb-763fc936be4a}";
        private const int OS2L_PORT_MIN = 8010;
        private const int OS2L_PORT_MAX = 8060;

        private static readonly K.ILumosLog log = K.LumosLogger.getInstance<Os2lPlugin>();

        private static readonly K.ILumosLog seLog = new K.SingleExceptionDecorator(log)
        {
            ReLogCount = 10
        };

        private Os2lBeatInputSource _beatInputSource;
        private Os2lBeatChangeInputSource _beatChangeInputSource;
        private Os2lBeatPosInputSource _beatPosInputSource;
        private Os2lBeatBpmInputSource _beatBpmInputSource;
        private Os2lBeatStrengthInputSource _beatStrengthInputSource;

        private bool _shutdown;
        private TcpListener _server;
        private Thread _thread;
        private ServiceDiscovery _serviceDiscovery;

        public Os2lPlugin() : base(PLUGIN_ID, "OS2L Plugin")
        {
        }

        protected override void initializePlugin()
        {
        }

        protected override void startupPlugin()
        {
            log.Debug("Register Os2lBeatInputSource");
            _beatInputSource = new Os2lBeatInputSource();
            _beatChangeInputSource = new Os2lBeatChangeInputSource();
            _beatPosInputSource = new Os2lBeatPosInputSource();
            _beatBpmInputSource = new Os2lBeatBpmInputSource();
            _beatStrengthInputSource = new Os2lBeatStrengthInputSource();
            InputManager.getInstance().RegisterSources(new IInputSource[]{
                _beatInputSource,
                _beatChangeInputSource,
                _beatPosInputSource,
                _beatBpmInputSource,
                _beatStrengthInputSource
            });

            _shutdown = false;

            // find unused port between OS2L_PORT_MIN and OS2L_PORT_MAX
            int port = OS2L_PORT_MIN;
            bool started = false;
            while (!started && port <= OS2L_PORT_MAX)
            {
                try
                {
                    _server = new TcpListener(IPAddress.Any, port);
                    _server.Start();
                    started = true;
                    log.Info("OS2L Server listens on port {0}", port);
                }
                catch (Exception)
                {
                    log.Info("Failed to listen on Port {0}", port);
                    port++;
                }
            }

            if (port > OS2L_PORT_MAX)
            {
                log.Warn("OS2L Server start failed!");
                return;
            }

            _thread = new Thread(Listen);
            _thread.Start();

            try
            {
                _serviceDiscovery = new ServiceDiscovery();
                var service = new ServiceProfile(Dns.GetHostName(), "_os2l._tcp", (ushort)port);
                _serviceDiscovery.Advertise(service);
            }
            catch (Exception e)
            {
                log.Warn("Zeroconf init failed: {0}", e.Message);
                KernelLogManager
                    .getInstance()
                    .sendUserNotification("OS2L Service could not be initialized!", ELogLevel.Warning);
            }
        }

        protected override void shutdownPlugin()
        {
            _shutdown = true;
            _serviceDiscovery?.Dispose();
            _server?.Stop();
            _thread?.Join();

            if (_beatInputSource != null)
            {
                log.Debug("Unregister Os2lBeatInputSource");
                InputManager.getInstance().UnregisterSource(_beatInputSource);
                _beatInputSource = null;
            }

            if (_beatChangeInputSource != null)
            {
                log.Debug("Unregister Os2lBeatChangeInputSource");
                InputManager.getInstance().UnregisterSource(_beatChangeInputSource);
                _beatChangeInputSource = null;
            }

            if (_beatPosInputSource != null)
            {
                log.Debug("Unregister Os2lBeatPosInputSource");
                InputManager.getInstance().UnregisterSource(_beatPosInputSource);
                _beatPosInputSource = null;
            }

            if (_beatBpmInputSource != null)
            {
                log.Debug("Unregister Os2lBeatBpmInputSource");
                InputManager.getInstance().UnregisterSource(_beatBpmInputSource);
                _beatBpmInputSource = null;
            }

            if (_beatStrengthInputSource != null)
            {
                log.Debug("Unregister Os2lBeatStrengthInputSource");
                InputManager.getInstance().UnregisterSource(_beatStrengthInputSource);
                _beatStrengthInputSource = null;
            }
        }

        private void Listen()
        {
            try
            {
                Byte[] bytes = new Byte[1024];
                String data = null;

                while (!_shutdown)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        try
                        {
                            JsonNode messageNode = JsonNode.Parse(data)!;

                            string evt = "";
                            try
                            {
                                evt = messageNode["evt"].GetValue<string>();
                            }
                            catch
                            {
                                // The JSON parameter "evt" is not in the message so it seems to not be a propper OS2L message
                            }

                            if (evt == "beat")
                            {
                                var beatMsg = JsonSerializer.Deserialize<OS2LBeatMessage>(data);

                                _beatInputSource.IncrementBeat();
                                _beatChangeInputSource.SetChange(beatMsg.Change);
                                _beatPosInputSource.SetPos(beatMsg.Position);
                                _beatBpmInputSource.SetBpm(beatMsg.Bpm);
                                _beatStrengthInputSource.SetStrength(beatMsg.Strength);
                            }
                        }
                        catch (Exception e)
                        {
                            seLog.Warn(e.Message, e);
                        }

                        // TODO other events and other beat properties
                        //Console.WriteLine("Received: {0}", data);
                    }

                    client.Close();
                }
            }
            catch (Exception)
            {
                // _server.AcceptTcpClient() is a blocking call and throws an exception on _server.Stop()
            }
        }
    }
}
