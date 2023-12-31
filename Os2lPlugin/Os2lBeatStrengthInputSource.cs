using LumosLIB.Kernel;
using LumosLIB.Tools;
using LumosProtobuf;
using LumosProtobuf.Input;
using org.dmxc.lumos.Kernel.Input.v2;

namespace Os2lPlugin
{
    public class Os2lBeatStrengthInputSource : AbstractInputSource
    {
        private static readonly ParameterCategory CATEGORY = ParameterCategoryTools.FromNameWithSub("OS2L", "Beat Params");

        public Os2lBeatStrengthInputSource()
            : base("{9e364d13-42fc-4ec9-ba9e-1ea9c173ff19}", "Beat Strength", CATEGORY, 0.0)
        {
        }

        public void SetStrength(double strength)
        {
            this.CurrentValue = strength.Limit(0.0, 1.0);
        }

        public override EWellKnownInputType AutoGraphIOType => EWellKnownInputType.Numeric;
        public override object Min => 0.0;
        public override object Max => 1.0;
    }
}
