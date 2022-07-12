using System;
using System.IO;
using GameFramework;
using GameFramework.Config;
using Tommy;
using Tommy.Extensions;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class TomlConfigHelper : ConfigHelperBase
    {
        // private static readonly string TextAssetExtension = ".txt";

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configAsset">全局配置资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否读取全局配置成功。</returns>
        public override bool ReadData(IConfigManager configManager, string configAssetName, object configAsset, object userData)
        {
            TextAsset configTextAsset = configAsset as TextAsset;
            if (configTextAsset != null)
            {
                return configManager.ParseData(configTextAsset.text, userData);
            }

            Log.Warning("Config asset '{0}' is invalid.", configAssetName);
            return false;
        }

        /// <summary>
        /// 读取全局配置。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configBytes">全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否读取全局配置成功。</returns>
        public override bool ReadData(IConfigManager configManager, string configAssetName, byte[] configBytes, int startIndex, int length, object userData)
        {
            return configManager.ParseData(Utility.Converter.GetString(configBytes, startIndex, length), userData);
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configString">要解析的全局配置字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public override bool ParseData(IConfigManager configManager, string configString, object userData)
        {
            try
            {
                using var parser = new TOMLParser(new StringReader(configString));
                if (parser.TryParse(out var rootNode, out var errors))
                {
                    // var node = rootNode.FindNode("UnityGameFramework.HuatuoSettings.HuatuoEditorMode");
                    // return null != node?.AsBoolean && node.AsBoolean.Value;
                    foreach (var config in rootNode.Keys)
                    {
                        Log.Debug($"config: [ {config} ]");
                        // todo add config to configManager
                        // if (!configManager.AddConfig(config, configValue))
                        // {
                        //     Log.Warning("Can not add config with config name '{0}' which may be invalid or duplicate.", configName);
                        //     return false;
                        // }
                    }
                    return true;
                }
                Debug.LogError("Parse config failed with those error(s):");
                foreach (var error in errors)
                {
                    Debug.LogError(error);
                }
                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse config string with exception '{0}'.", exception);
                return false;
            }
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configBytes">要解析的全局配置二进制流。</param>
        /// <param name="startIndex">全局配置二进制流的起始位置。</param>
        /// <param name="length">全局配置二进制流的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public override bool ParseData(IConfigManager configManager, byte[] configBytes, int startIndex, int length, object userData)
        {
            try
            {
                using var parser = new TOMLParser(new StreamReader(new MemoryStream(configBytes, startIndex, length, false)));
                if (parser.TryParse(out var rootNode, out var errors))
                {
                    // todo add config to configManager

                }
                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse config bytes with exception '{0}'.", exception);
                return false;
            }
        }

        /// <summary>
        /// 释放全局配置资源。
        /// </summary>
        /// <param name="configManager">全局配置管理器。</param>
        /// <param name="configAsset">要释放的全局配置资源。</param>
        public override void ReleaseDataAsset(IConfigManager configManager, object configAsset)
        {
            UGFIF.Resource.UnloadAsset(configAsset);
        }
    }
}
