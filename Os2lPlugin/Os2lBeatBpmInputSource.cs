using LumosLIB.Kernel;
using LumosProtobuf;
using LumosProtobuf.Input;
using org.dmxc.lumos.Kernel.Input.v2;
using System;

namespace Os2lPlugin
{
    public class Os2lBeatBpmInputSource : AbstractInputSource
    {
        private static readonly ParameterCategory CATEGORY = ParameterCategoryTools.FromNameWithSub("OS2L", "Beat Params");

        public Os2lBeatBpmInputSource()
            : base("{ee42a4d5-8360-47f3-94e5-5197e2d5b1e9}", "Beat BPM", CATEGORY, 0.0)
        {
        }

        public void SetBpm(double bpm)
        {
            this.CurrentValue = Math.Max(0.0, bpm);
        }

        public override EWellKnownInputType AutoGraphIOType => EWellKnownInputType.Numeric;
        public override object Min => 0.0;
        public override object Max => double.MaxValue;
    }
}
