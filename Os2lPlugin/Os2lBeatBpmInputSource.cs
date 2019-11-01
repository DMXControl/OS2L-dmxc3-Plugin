using System;
using LumosLIB.Kernel;
using org.dmxc.lumos.Kernel.Input.v2;

namespace Os2lPlugin
{
    public class Os2lBeatBpmInputSource : AbstractInputSource
    {
        public Os2lBeatBpmInputSource()
            : base("{ee42a4d5-8360-47f3-94e5-5197e2d5b1e9}", "Beat BPM", new ParameterCategory("OS2L", new ParameterCategory("Beat Params")))
        {
        }

        public void SetBpm(double bpm)
        {
            this.CurrentValue = Math.Max(0.0, bpm);
        }

        public override EWellKnownInputType AutoGraphIOType => EWellKnownInputType.NUMERIC;
        public override object Min => 0.0;
        public override object Max => double.MaxValue;
    }
}
