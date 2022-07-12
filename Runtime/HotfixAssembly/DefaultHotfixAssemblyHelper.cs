using System;
using GameFramework;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认代码热更辅助器。
    /// </summary>
    public class DefaultHotfixAssemblyHelper : HotfixAssemblyHelperBase
    {
        public override void Load(GameFrameworkAction assembliesLoadSuccessCallback, GameFrameworkAction assembliesLoadFailureCallback)
        {
            throw new NotImplementedException("Hotfix code is not supported in GameFramework by default.");
        }

        public override void Run(GameFrameworkAction assembliesRunSuccessCallback, GameFrameworkAction assembliesRunFailureCallback)
        {
            throw new NotImplementedException("Hotfix code is not supported in GameFramework by default.");
        }

        public override void Stop(GameFrameworkAction assembliesStopSuccessCallback, GameFrameworkAction assembliesStopFailureCallback)
        {
            throw new NotImplementedException("Hotfix code is not supported in GameFramework by default.");
        }
    }
}
