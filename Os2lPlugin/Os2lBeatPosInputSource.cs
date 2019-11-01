using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;

namespace Os2lPlugin
{
    public class Os2lBeatPosInputSource : AbstractInputSource
    {
        public Os2lBeatPosInputSource()
            : base("{86c29be1-f12d-4965-a947-d8d47423bb3a}", "Beat Pos", new ParameterCategory("OS2L", new ParameterCategory("Beat Params")))
        {
        }

        public void SetPos(long pos)
        {
            this.CurrentValue = pos;
        }

        public override EWellKnownInputType AutoGraphIOType => EWellKnownInputType.NUMERIC;
        public override object Min => long.MinValue;
        public override object Max => long.MaxValue;
    }
}
