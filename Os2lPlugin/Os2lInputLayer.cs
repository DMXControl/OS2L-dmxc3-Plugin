using System;
using org.dmxc.lumos.Kernel.Beat;
using org.dmxc.lumos.Kernel.Input;

namespace Os2lPlugin
{
    public class Os2lInputLayer : AbstractKernelInputLayer, IDisposable
    {
        private BeatChannel _beat;

        public Os2lInputLayer()
            : base(new InputID("{dc94cd84-66ac-474c-8e06-1202a604692a}", InputLayerManager.getInstance().SessionName), "OS2L")
        {
            _beat = new BeatChannel("{ea70b486-544a-454b-941a-475c9cdd793a}", this);
            _beat.Name = "OS2L Beat";
            AddInputChannel(_beat);
        }

        public void Dispose()
        {
        }
    }
}
