using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(HotfixAssemblyComponent))]
    internal sealed class HotfixAssemblyComponentInspector : GameFrameworkInspector
    {
        private HelperInfo<HotfixAssemblyHelperBase> _hotfixAssemblyHelperInfo = new HelperInfo<HotfixAssemblyHelperBase>("HotfixAssembly");
        private HelperInfo<HotfixAssemblyRealtimeDebugHelperBase> _hotfixAssemblyRealtimeDebugHelperInfo = new HelperInfo<HotfixAssemblyRealtimeDebugHelperBase>("HotfixAssemblyRealtimeDebug");
        private SerializedProperty _realtimeDebugPollingInterval = null;
        private SerializedProperty _hotfixAssemblyActiveRealtimeDebugType = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var t = (HotfixAssemblyComponent)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                _hotfixAssemblyHelperInfo.Draw();
                _hotfixAssemblyRealtimeDebugHelperInfo.Draw();
                EditorGUILayout.PropertyField(_realtimeDebugPollingInterval);
                EditorGUILayout.PropertyField(_hotfixAssemblyActiveRealtimeDebugType);
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
            RefreshTypeNames();
        }

        private void OnEnable()
        {
            _hotfixAssemblyHelperInfo.Init(serializedObject);
            _hotfixAssemblyRealtimeDebugHelperInfo.Init(serializedObject);
            _realtimeDebugPollingInterval = serializedObject.FindProperty("_realtimeDebugPollingInterval");
            _hotfixAssemblyActiveRealtimeDebugType = serializedObject.FindProperty("_hotfixAssemblyActiveRealtimeDebugType");
            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            _hotfixAssemblyHelperInfo.Refresh();
            _hotfixAssemblyRealtimeDebugHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
