using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;

namespace Os2lPlugin
{
    public class Os2lBeatInputSource : AbstractInputSource
    {
        public Os2lBeatInputSource()
            : base("{dc94cd84-66ac-474c-8e06-1202a604692a}", "Beat", new ParameterCategory("OS2L"))
        {
        }

        public void IncrementBeat()
        {
            if (!(this.CurrentValue is ulong))
            {
                this.CurrentValue = (ulong) 1;
            }
            else
            {
                this.CurrentValue = (ulong) this.CurrentValue + 1;
            }
        }

        public override EWellKnownInputType AutoGraphIOType => EWellKnownInputType.BEAT;
        public override object Min => ulong.MinValue;
        public override object Max => ulong.MaxValue;
    }
}
