using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Newtonsoft.Json.Linq;
using org.dmxc.lumos.Kernel.Beat;
using org.dmxc.lumos.Kernel.Input;
using org.dmxc.lumos.Kernel.Log;

namespace Os2lPlugin
{
    static class Os2lBonjour
    {
        [DllImport("os2l_bonjour.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool os2l_init(int port);

        [DllImport("os2l_bonjour.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool os2l_close();
    }

    public class Os2lInputLayer : AbstractKernelInputLayer, IDisposable
    {
        private const int OS2L_PORT_MIN = 8010;
        private const int OS2L_PORT_MAX = 8060;

        private BeatChannel _beat;

        private bool _shutdown;
        private TcpListener _server;
        private Thread _thread;

        public Os2lInputLayer()
            : base(new InputID("{dc94cd84-66ac-474c-8e06-1202a604692a}", InputLayerManager.getInstance().SessionName), "OS2L")
        {
            _beat = new BeatChannel("{ea70b486-544a-454b-941a-475c9cdd793a}", this);
            _beat.Name = "OS2L Beat";
            AddInputChannel(_beat);

            _shutdown = false;

            // TODO: find unused port between OS2L_PORT_MIN and OS2L_PORT_MAX
            _server = new TcpListener(IPAddress.Any, OS2L_PORT_MIN);
            _server.Start();

            // TODO: check result, if false show information that probably Bonjour is not installed.
            if (!Os2lBonjour.os2l_init(OS2L_PORT_MIN)) {
                KernelLogManager
                    .getInstance()
                    .sendUserNotification("OS2L Service could not be initialized!\n" +
                                          "Please make sure that Bonjour is installed:\n" +
                                          "https://support.apple.com/kb/DL999",
                                          EventLogEntryType.Warning);
            }

            _thread = new Thread(Listen);
            _thread.Start();
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
                        if (evt == "beat") {
                            _beat.IncrementBeat();
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

        public void Dispose()
        {
            Console.WriteLine("### Dispose() start ###");
            _shutdown = true;
            _server.Stop();
            Os2lBonjour.os2l_close();
            _thread.Join();
            Console.WriteLine("### Dispose() done ###");
        }
    }
}
