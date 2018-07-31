using LumosLIB.Kernel.Log;
using org.dmxc.lumos.Kernel.Input;
using org.dmxc.lumos.Kernel.Plugin;

namespace Os2lPlugin
{
    public class Os2lPlugin : KernelPluginBase
    {
        private const string PLUGIN_ID = "{b43ef54b-0124-413a-b0eb-763fc936be4a}";

        private static readonly ILumosLog log = LumosLogger.getInstance<Os2lPlugin>();

        private Os2lInputLayer _inputLayer;

        public Os2lPlugin() : base(PLUGIN_ID, "OS2L Plugin")
        {
            log.Info("Os2lPlugin()");
        }

        protected override void initializePlugin()
        {
            log.Info("initializePlugin()");
        }

        protected override void startupPlugin()
        {
            log.Info("startupPlugin()");

            _inputLayer = new Os2lInputLayer();
            InputLayerManager.getInstance().registerInputLayer(_inputLayer);
        }

        protected override void shutdownPlugin()
        {
            log.Info("shutdownPlugin()");

            InputLayerManager.getInstance().deregisterInputLayer(_inputLayer);
            _inputLayer.Dispose();
            _inputLayer = null;
        }
    }
}
