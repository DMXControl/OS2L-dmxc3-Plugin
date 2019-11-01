using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;

namespace Os2lPlugin
{
    public class Os2lBeatChangeInputSource : AbstractInputSource
    {
        public Os2lBeatChangeInputSource()
            : base("{02d0c640-ddd0-4bea-8339-dea8e0f6c8da}", "Beat Change", new ParameterCategory("OS2L", new ParameterCategory("Beat Params")))
        {
        }

        public void SetChange(bool change)
        {
            this.CurrentValue = change;
        }

        public override EWellKnownInputType AutoGraphIOType => EWellKnownInputType.BOOL;
        public override object Min => false;
        public override object Max => true;
    }
}
