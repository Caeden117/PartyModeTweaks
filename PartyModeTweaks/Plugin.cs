using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPA.Old;
using Harmony;

namespace PartyModeTweaks
{
    #pragma warning disable CS0618 //fuck off DaNike
    public class Plugin : IPlugin
    {
        public string Name => "Party Mode Tweaks";
        public string Version => "0.0.1";

        public void OnApplicationStart()
        {
            HarmonyInstance instance = HarmonyInstance.Create("com.Caeden117.PartyModeTweaks");
            instance.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "MenuCore") new GameObject("Party Mode Tweaks").AddComponent<PartyModeTweaksGameObject>();
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
        }

        public void OnLevelWasLoaded(int level) { }
        public void OnLevelWasInitialized(int level) { }
        public void OnUpdate() { }
        public void OnFixedUpdate() { }
    }
}
