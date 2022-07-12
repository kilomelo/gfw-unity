using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// realtime debug hotfix assembly cache updated
    /// </summary>
    public class HotfixAssemblyDebugCacheUpdatedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HotfixAssemblyDebugCacheUpdatedEventArgs).GetHashCode();
        public override int Id => EventId;
        
        public object UpdatedAssemblyInfo
        {
            get;
            private set;
        }
        public HotfixAssemblyDebugCacheUpdatedEventArgs()
        {
        }

        public static HotfixAssemblyDebugCacheUpdatedEventArgs Create(GameFramework.HotfixAssembly.HotfixAssemblyDebugCacheUpdatedEventArgs e)
        {
            var networkClosedEventArgs = ReferencePool.Acquire<HotfixAssemblyDebugCacheUpdatedEventArgs>();
            networkClosedEventArgs.UpdatedAssemblyInfo = e.UpdatedAssemblyInfo;
            return networkClosedEventArgs;
        }
        
        public override void Clear()
        {
            UpdatedAssemblyInfo = null;
        }
    }
}
