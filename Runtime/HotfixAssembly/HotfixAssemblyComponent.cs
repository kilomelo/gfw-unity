using GameFramework;
using GameFramework.HotfixAssembly;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// hotfix assembly component
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/HotfixAssembly")]
    public sealed class HotfixAssemblyComponent : GameFrameworkComponent
    {
        private IHotfixAssemblyManager _hotfixAssemblyManager;
        private EventComponent _eventComponent;
        [SerializeField]
        private string m_HotfixAssemblyHelperTypeName = "UnityGameFramework.Runtime.DefaultHotfixAssemblyHelper";
        [SerializeField]
        private HotfixAssemblyHelperBase m_CustomHotfixAssemblyHelper;
        [SerializeField]
        private string m_HotfixAssemblyRealtimeDebugHelperTypeName = "UnityGameFramework.Runtime.DefaultHotfixAssemblyRealtimeDebugHelper";
        [SerializeField]
        private HotfixAssemblyRealtimeDebugHelperBase m_CustomHotfixAssemblyRealtimeDebugHelper = null;
        
        /// <summary>
        /// hotfix assemblies update interval in development build
        /// </summary>
        [SerializeField]
        private int _realtimeDebugPollingInterval = 10000;
        /// <summary>
        /// hotfix assembly realtime debug condition control
        /// </summary>
        [SerializeField]
        private EHotfixAssemblyActiveRealtimeDebugType _hotfixAssemblyActiveRealtimeDebugType =
            EHotfixAssemblyActiveRealtimeDebugType.AlwaysClose;

        public bool RealtimeDebugActive => _hotfixAssemblyActiveRealtimeDebugType switch
        {
            EHotfixAssemblyActiveRealtimeDebugType.AlwaysOpen => _realtimeDebugPollingInterval > 0,
            EHotfixAssemblyActiveRealtimeDebugType.OnlyOpenWhenDevelopment => Debug.isDebugBuild && _realtimeDebugPollingInterval > 0,
            EHotfixAssemblyActiveRealtimeDebugType.OnlyOpenInEditor => Application.isEditor && _realtimeDebugPollingInterval > 0,
            _ => false
        };
        
        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _hotfixAssemblyManager = GameFrameworkEntry.GetModule<IHotfixAssemblyManager>();
            if (_hotfixAssemblyManager != null) return;
            throw new GameFrameworkException("Implementation of IHotfixAssemblyManager not found.");
        }

        private void Start()
        {
            _eventComponent = GameEntry.GetComponent<EventComponent>();
            if (_eventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }
            
            var helper = Helper.CreateHelper(m_HotfixAssemblyHelperTypeName, m_CustomHotfixAssemblyHelper);
            if (helper == null)
            {
                Log.Error("Can not create hotfix assembly helper.");
                return;
            }

            helper.name = "Hotfix assembly Helper";
            var trans = helper.transform;
            trans.SetParent(transform);
            trans.localScale = Vector3.one;

            _hotfixAssemblyManager.SetHotfixAssemblyHelper(helper);
            
            var realtimeDebugHelper = Helper.CreateHelper(m_HotfixAssemblyRealtimeDebugHelperTypeName, m_CustomHotfixAssemblyRealtimeDebugHelper);
            if (realtimeDebugHelper == null)
            {
                Log.Error("Can not create hotfix assembly realtime debug helper.");
                return;
            }

            realtimeDebugHelper.name = "Hotfix assembly Helper";
            trans = realtimeDebugHelper.transform;
            trans.SetParent(transform);
            trans.localScale = Vector3.one;

            if (RealtimeDebugActive)
            {
                _hotfixAssemblyManager.SetHotfixAssemblyRealtimeDebugHelper(realtimeDebugHelper);
                _hotfixAssemblyManager.SetRealtimeDebugPollingInterval(_realtimeDebugPollingInterval);
                _hotfixAssemblyManager.DebugCacheUpdatedHandler += OnDebugCacheUpdated;
            }
        }

        public void Load(GameFrameworkAction assembliesLoadSuccessCallback, GameFrameworkAction assembliesLoadFailureCallback)
        {
            if (null == _hotfixAssemblyManager)
            {
                throw new GameFrameworkException("Hotfix assembly manager is invalid.");
            }
            _hotfixAssemblyManager.Load(assembliesLoadSuccessCallback, assembliesLoadFailureCallback);
        }
        
        public void Run(GameFrameworkAction assembliesRunSuccessCallback, GameFrameworkAction assembliesRunFailureCallback)
        {
            if (null == _hotfixAssemblyManager)
            {
                throw new GameFrameworkException("Hotfix assembly manager is invalid.");
            }
            _hotfixAssemblyManager.Run(assembliesRunSuccessCallback, assembliesRunFailureCallback);
        }
        
        public void Stop(GameFrameworkAction assembliesStopSuccessCallback, GameFrameworkAction assembliesStopFailureCallback)
        {
            if (null == _hotfixAssemblyManager)
            {
                throw new GameFrameworkException("Hotfix assembly manager is invalid.");
            }
            _hotfixAssemblyManager.Stop(assembliesStopSuccessCallback, assembliesStopFailureCallback);
        }
        
        private void OnDebugCacheUpdated(object sender, GameFramework.HotfixAssembly.HotfixAssemblyDebugCacheUpdatedEventArgs e)
        {
            _eventComponent.Fire(this, HotfixAssemblyDebugCacheUpdatedEventArgs.Create(e));
        }
    }
}
