﻿using System.Reflection;
using ColossalFramework;
using Transit.Framework.Builders;
using Transit.Framework.Interfaces;
using UnityEngine;

namespace Transit.Framework
{
    public static class PrefabInfoExtensions
    {
        public static T Clone<T>(this T originalPrefabInfo, string newName)
            where T : PrefabInfo
        {
            var gameObject = Object.Instantiate(originalPrefabInfo.gameObject);
            gameObject.transform.parent = originalPrefabInfo.gameObject.transform; // N.B. This line is evil and removing it is killoing the game's performances
            gameObject.name = newName;

            var info = gameObject.GetComponent<T>();
            info.m_prefabInitialized = false;

            return info;
        }

        public static void SetUICategory(this PrefabInfo info, string category)
        {
            typeof(PrefabInfo).GetField("m_UICategory", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(info, category);
        }

        public static void SetMenuItemConfig(this PrefabInfo info, IMenuItemBuilder config)
        {
            info.m_UIPriority = config.UIOrder;
            info.SetUICategory(config.UICategory);

            if (!config.ThumbnailsPath.IsNullOrWhiteSpace())
            {
                var thumbnails = AssetManager.instance.GetThumbnails(config.GetCodeName(), config.ThumbnailsPath);
                info.m_Atlas = thumbnails;
                info.m_Thumbnail = thumbnails.name;
            }

            if (!config.InfoTooltipPath.IsNullOrWhiteSpace())
            {
                var infoTips = AssetManager.instance.GetInfoTooltip(config.GetCodeName(), config.InfoTooltipPath);
                info.m_InfoTooltipAtlas = infoTips;
                info.m_InfoTooltipThumbnail = infoTips.name;
            }
        }
    }
}
