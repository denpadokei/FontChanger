using BS_Utils.Utilities;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace FontChanger
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        private static readonly string FontPath = Path.Combine(Environment.CurrentDirectory, "UserData", "FontChanger");

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            Log.Info("FontChanger initialized.");
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");
            BSEvents.lateMenuSceneLoadedFresh += this.BSEvents_lateMenuSceneLoadedFresh;
        }

        private void BSEvents_lateMenuSceneLoadedFresh(ScenesTransitionSetupDataSO obj)
        {
            foreach (var fontPath in Font.GetPathsToOSFonts()) {
                var font = new Font(fontPath);
                if (font.name.ToLower() != "meiryob") {
                    continue;
                }
                var asset = TMP_FontAsset.CreateFontAsset(font, 90, 5, UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA, 8192, 4096);
                foreach (var fontAsset in Resources.FindObjectsOfTypeAll<TMP_FontAsset>()) {
                    fontAsset.fallbackFontAssetTable = new List<TMP_FontAsset>();
                    fontAsset.fallbackFontAssetTable.Add(asset);
                }
            }
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");
            BSEvents.lateMenuSceneLoadedFresh -= this.BSEvents_lateMenuSceneLoadedFresh;
        }
    }
}
