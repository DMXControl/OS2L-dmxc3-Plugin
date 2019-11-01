using LumosLIB.Kernel.Log;
using org.dmxc.lumos.Kernel.Input.v2;
using org.dmxc.lumos.Kernel.Plugin;

namespace Os2lPlugin
{
    public class Os2lPlugin : KernelPluginBase
    {
        private const string PLUGIN_ID = "{b43ef54b-0124-413a-b0eb-763fc936be4a}";

        private static readonly ILumosLog log = LumosLogger.getInstance<Os2lPlugin>();

        private Os2lInputSource _inputSource;

        public Os2lPlugin() : base(PLUGIN_ID, "OS2L Plugin")
        {
        }

        protected override void initializePlugin()
        {
        }

        protected override void startupPlugin()
        {
            log.Debug("Register Os2lInputSource");
            _inputSource = new Os2lInputSource();
            InputManager.getInstance().RegisterSource(_inputSource);
        }

        protected override void shutdownPlugin()
        {
            if (_inputSource != null)
            {
                log.Debug("Unregister Os2lInputSource");
                InputManager.getInstance().UnregisterSource(_inputSource);
                _inputSource.Dispose();
                _inputSource = null;
            }
        }
    }
}
