using LumosLIB.Kernel;
using LumosLIB.Tools;
using org.dmxc.lumos.Kernel.Input.v2;

namespace Os2lPlugin
{
    public class Os2lBeatStrengthInputSource : AbstractInputSource
    {
        public Os2lBeatStrengthInputSource()
            : base("{9e364d13-42fc-4ec9-ba9e-1ea9c173ff19}", "Beat Strength", new ParameterCategory("OS2L", new ParameterCategory("Beat Params")))
        {
        }

        public void SetStrength(double strength)
        {
            this.CurrentValue = LumosTools.Limit(strength, 0.0, 1.0);
        }

        public override EWellKnownInputType AutoGraphIOType => EWellKnownInputType.NUMERIC;
        public override object Min => 0.0;
        public override object Max => 1.0;
    }
}
