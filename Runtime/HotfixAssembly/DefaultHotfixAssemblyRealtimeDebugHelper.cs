using GameFramework;

namespace UnityGameFramework.Runtime
{
    public class DefaultHotfixAssemblyRealtimeDebugHelper : HotfixAssemblyRealtimeDebugHelperBase
    {
        public override void CheckRemoteDllInfo(GameFrameworkAction<object> dllCacheUpdateCallback)
        {
            // do nothing
        }
    }
}