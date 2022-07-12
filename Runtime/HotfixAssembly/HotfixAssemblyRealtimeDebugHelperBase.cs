using GameFramework;
using GameFramework.HotfixAssembly;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public abstract class HotfixAssemblyRealtimeDebugHelperBase : MonoBehaviour, IHotfixAssemblyRealtimeDebugHelper
    {
        public abstract void CheckRemoteDllInfo(GameFrameworkAction<object> dllCacheUpdateCallback);
    }
}