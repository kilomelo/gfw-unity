using System;
using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    public class HotfixAssemblyLoadedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HotfixAssemblyLoadedEventArgs).GetHashCode();
        public override int Id => EventId;
        
        public string AssemblyName
        {
            get;
            private set;
        }
        
        public string Hash
        {
            get;
            private set;
        }
        
        public bool IsFromCache
        {
            get;
            private set;
        }
        
        public HotfixAssemblyLoadedEventArgs()
        {
            AssemblyName = null;
            Hash = null;
            IsFromCache = false;
            TimeStamp = DateTime.MinValue;
        }
        
        public DateTime TimeStamp
        {
            get;
            private set;
        }
        
        public static HotfixAssemblyLoadedEventArgs Create(string assemblyName, string hash, bool isFromCache, DateTime timeStamp)
        {
            var hotfixAssemblyLoadedEventArgs = ReferencePool.Acquire<HotfixAssemblyLoadedEventArgs>();
            hotfixAssemblyLoadedEventArgs.AssemblyName = assemblyName;
            hotfixAssemblyLoadedEventArgs.Hash = hash;
            hotfixAssemblyLoadedEventArgs.IsFromCache = isFromCache;
            hotfixAssemblyLoadedEventArgs.TimeStamp = timeStamp;
            return hotfixAssemblyLoadedEventArgs;
        }
        
        public override void Clear()
        {
            AssemblyName = null;
            Hash = null;
            IsFromCache = false;
            TimeStamp = DateTime.MinValue;
        }
    }
}
