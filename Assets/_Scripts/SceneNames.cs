using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingMaester
{
    [Serializable]
    [CreateAssetMenu(fileName = "SceneNames", menuName = "ScriptableObjects/SceneNames")]
    public class SceneNames : SingletonScriptableObject<SceneNames,ICreationMethodLocated>
    {
        public string SplashSceneName;
        public string MainMenuSceneName;
        public string GameSceneName;
    }
}