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
            if (!Directory.Exists(FontPath)) {
                Directory.CreateDirectory(FontPath);
            }

            foreach (var fontPath in Directory.EnumerateFiles(FontPath, "*.ttf", SearchOption.TopDirectoryOnly)) {
                var font = new Font(fontPath);
                var asset = TMP_FontAsset.CreateFontAsset(font, 90, 5, UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA, 8192, 8192);
                foreach (var text in Resources.FindObjectsOfTypeAll<TextMeshPro>()) {
                    text.font = asset;
                }
                foreach (var text in Resources.FindObjectsOfTypeAll<TextMeshProUGUI>()) {
                    text.font = asset;
                }
            }
            //new GameObject("FontChangerController").AddComponent<FontChangerController>();

        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");

        }
    }
}
