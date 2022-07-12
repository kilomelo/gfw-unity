using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using Tommy;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    [CustomDebuggerWindow("Hotfix")]
    public class HotfixDebugWindow : ScrollableDebuggerWindowBase
    {
        private DebuggerComponent _debuggerComponent;
        private TomlNode _updatedAssemblyInfo;
        private Dictionary<string, LoadedAssemblyInfo> _loadedAssemblyHash = new Dictionary<string, LoadedAssemblyInfo>();
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            UGFIF.Event.Subscribe(HotfixAssemblyDebugCacheUpdatedEventArgs.EventId, OnHotfixAssemblyDebugCacheUpdated);
            UGFIF.Event.Subscribe(HotfixAssemblyLoadedEventArgs.EventId, OnHotfixAssemblyLoaded);
            _debuggerComponent = GameEntry.GetComponent<DebuggerComponent>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void Shutdown()
        {
            base.Shutdown();
            UGFIF.Event.Unsubscribe(HotfixAssemblyDebugCacheUpdatedEventArgs.EventId, OnHotfixAssemblyDebugCacheUpdated);
            UGFIF.Event.Unsubscribe(HotfixAssemblyLoadedEventArgs.EventId, OnHotfixAssemblyLoaded);
        }

        /// <summary>
        /// 调试器窗口绘制。
        /// </summary>
        protected override void OnDrawScrollableWindow()
        {
            GUILayout.Label("<b>Loaded assembly info</b>");
            using var itor = _loadedAssemblyHash.GetEnumerator();
            GUILayout.BeginVertical("box");
            while (itor.MoveNext())
            {
                // GUILayout.Label(itor.Current.Value.ToString());
                itor.Current.Value.OnGUI();
            }
            GUILayout.EndVertical();
            
            GUILayout.Label("<b>Updated cached assembly info</b>");
            if (null != _updatedAssemblyInfo)
            {
                var keys = _updatedAssemblyInfo.Keys;
                GUILayout.BeginVertical("box");
                var anyAssemblyUpdated = false;
                foreach (var key in keys)
                {
                    DrawItem(key, _updatedAssemblyInfo[key]);
                    if (_loadedAssemblyHash.TryGetValue(key, out var loadedAssemblyInfo) &&
                        string.Compare(loadedAssemblyInfo.Hash, _updatedAssemblyInfo[key].AsString?.Value ?? loadedAssemblyInfo.Hash, StringComparison.Ordinal) != 0)
                    {
                        anyAssemblyUpdated = true;
                    }
                }
                GUILayout.EndVertical();
                if (anyAssemblyUpdated)
                {
                    if (GUILayout.Button("New Assemblies Available, Click to Quit App", GUILayout.Height(30f)))
                        GameEntry.Shutdown(ShutdownType.Quit);
                }
                else GUILayout.Label("Everything is UP-to-DATE", GUILayout.Height(30f));
            }
            else
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("No info");
                GUILayout.EndVertical();
            }
        }

        private void OnHotfixAssemblyDebugCacheUpdated(object sender, GameEventArgs e)
        {
            if (e is not HotfixAssemblyDebugCacheUpdatedEventArgs {UpdatedAssemblyInfo: TomlNode updatedAssemblyInfo})
            {
                Log.Error($"HotfixAssemblyDebugCacheUpdatedEventArgs fired with invalid args.");
                return;
            }
            Log.Debug($"HotfixDebugWindow.OnHotfixAssemblyDebugCacheUpdated, cachedDllInfo: [ {updatedAssemblyInfo} ]");
            _updatedAssemblyInfo = updatedAssemblyInfo;
        }
        
        private void OnHotfixAssemblyLoaded(object sender, GameEventArgs e)
        {
            if (e is not HotfixAssemblyLoadedEventArgs
                {
                    AssemblyName: { } assemblyName,
                    Hash: { } hash,
                    IsFromCache: { } isFromCache,
                    TimeStamp: { } timeStamp
                })
            {
                Log.Error("HotfixAssemblyLoadedEventArgs fired with invalid args.");
                return;
            }
            // Log.Debug($"HotfixDebugWindow.OnHotfixAssemblyLoaded, assemblyName: [ {assemblyName} ], Hash: [ {hash} ]");
            _loadedAssemblyHash[assemblyName] = new LoadedAssemblyInfo()
            {
                AssemblyName = assemblyName,
                Hash = hash,
                IsFromCache = isFromCache,
                TimeStamp = timeStamp
            };
        }

        private struct LoadedAssemblyInfo
        {
            private const float TitleWidth = 240f;
            
            public string AssemblyName;
            public string Hash;
            public bool IsFromCache;
            public DateTime TimeStamp;
            public override string ToString()
            {
                return Utility.Text.Format("{0}    {1}    {2}    {3}", AssemblyName, Hash, IsFromCache, TimeStamp);
            }

            public void OnGUI()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(AssemblyName, GUILayout.Width(TitleWidth));
                    var content = Utility.Text.Format("{0}    {1}    {2}", Hash, IsFromCache, TimeStamp);
                    if (GUILayout.Button(content, "label"))
                    {
                        CopyToClipboard(content);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
