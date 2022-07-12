using GameFramework;
using GameFramework.HotfixAssembly;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// hotfix assembly helper base class
    /// </summary>
    public abstract class HotfixAssemblyHelperBase : MonoBehaviour, IHotfixAssemblyHelper
    {
        public abstract void Load(GameFrameworkAction assembliesLoadSuccessCallback, GameFrameworkAction assembliesLoadFailureCallback);
        public abstract void Run(GameFrameworkAction assembliesRunSuccessCallback, GameFrameworkAction assembliesRunFailureCallback);
        public abstract void Stop(GameFrameworkAction assembliesStopSuccessCallback, GameFrameworkAction assembliesStopFailureCallback);
    }
}
