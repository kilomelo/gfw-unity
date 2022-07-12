namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// hotfix assembly realtime debug condition control
    /// </summary>
    public enum EHotfixAssemblyActiveRealtimeDebugType : byte
    {
        /// <summary>
        /// 总是打开。
        /// </summary>
        AlwaysOpen = 0,

        /// <summary>
        /// 仅在开发模式时打开。
        /// </summary>
        OnlyOpenWhenDevelopment,

        /// <summary>
        /// 仅在编辑器中打开。
        /// </summary>
        OnlyOpenInEditor,

        /// <summary>
        /// 总是关闭。
        /// </summary>
        AlwaysClose,
    }
}