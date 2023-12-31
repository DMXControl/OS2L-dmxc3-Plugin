using LumosLIB.Kernel;
using LumosProtobuf;
using LumosProtobuf.Input;
using org.dmxc.lumos.Kernel.Input.v2;

namespace Os2lPlugin
{
    public class Os2lBeatPosInputSource : AbstractInputSource
    {
        private static readonly ParameterCategory CATEGORY = ParameterCategoryTools.FromNameWithSub("OS2L", "Beat Params");

        public Os2lBeatPosInputSource()
            : base("{86c29be1-f12d-4965-a947-d8d47423bb3a}", "Beat Pos", CATEGORY, 0)
        {
        }

        public void SetPos(long pos)
        {
            this.CurrentValue = pos;
        }

        public override EWellKnownInputType AutoGraphIOType => EWellKnownInputType.Numeric;
        public override object Min => long.MinValue;
        public override object Max => long.MaxValue;
    }
}
