using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using LumosLIB.Kernel.Log;
using Mono.Zeroconf;
using Newtonsoft.Json.Linq;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Log;
using org.dmxc.lumos.Kernel.Plugin;

namespace Os2lPlugin
{
    public class Os2lPlugin : KernelPluginBase
    {
        private const string PLUGIN_ID = "{b43ef54b-0124-413a-b0eb-763fc936be4a}";
        private const int OS2L_PORT_MIN = 8010;
        private const int OS2L_PORT_MAX = 8060;

        private static readonly ILumosLog log = LumosLogger.getInstance<Os2lPlugin>();

        private Os2lBeatInputSource _beatInputSource;

        private bool _shutdown;
        private TcpListener _server;
        private Thread _thread;
        private RegisterService _zeroconfService;

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
            InputManager.getInstance().RegisterSource(_beatInputSource);

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
                catch (Exception e)
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
                _zeroconfService = new RegisterService();
                // Do not set Name option, default name is hostname.
                _zeroconfService.RegType = "_os2l._tcp";
                _zeroconfService.Port = (short) port;
                _zeroconfService.Register();
            }
            catch (Exception e)
            {
                log.Warn("Bonjour init failed!");
                KernelLogManager
                    .getInstance()
                    .sendUserNotification("OS2L Service could not be initialized!\n" +
                                          "Please make sure that Bonjour is installed:\n" +
                                          "https://support.apple.com/kb/DL999",
                                          EventLogEntryType.Warning);
            }
        }

        protected override void shutdownPlugin()
        {
            _shutdown = true;
            _zeroconfService?.Dispose();
            _server?.Stop();
            _thread?.Join();

            if (_beatInputSource != null)
            {
                log.Debug("Unregister Os2lBeatInputSource");
                InputManager.getInstance().UnregisterSource(_beatInputSource);
                _beatInputSource.Dispose();
                _beatInputSource = null;
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
                        JObject obj = JObject.Parse(data);
                        String evt = obj["evt"].Value<String>();
                        if (evt == "beat")
                        {
                            _beatInputSource.IncrementBeat();
                        }

                        // TODO other events and other beat properties
                        //Console.WriteLine("Received: {0}", data);
                    }

                    client.Close();
                }
            }
            catch (Exception e)
            {
                // _server.AcceptTcpClient() is a blocking call and throws an exception on _server.Stop()
            }
        }
    }
}
