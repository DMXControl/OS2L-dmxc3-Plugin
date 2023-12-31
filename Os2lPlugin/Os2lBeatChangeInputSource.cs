using LumosLIB.Kernel;
using LumosProtobuf;
using LumosProtobuf.Input;
using org.dmxc.lumos.Kernel.Input.v2;

namespace Os2lPlugin
{
    public class Os2lBeatChangeInputSource : AbstractInputSource
    {
        private static readonly ParameterCategory CATEGORY = ParameterCategoryTools.FromNameWithSub("OS2L", "Beat Params");

        public Os2lBeatChangeInputSource()
            : base("{02d0c640-ddd0-4bea-8339-dea8e0f6c8da}", "Beat Change", CATEGORY, false)
        {
        }

        public void SetChange(bool change)
        {
            this.CurrentValue = change;
        }

        public override EWellKnownInputType AutoGraphIOType => EWellKnownInputType.Bool;
        public override object Min => false;
        public override object Max => true;
    }
}
