using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingMaester
{
    [Serializable]
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings")]
    public class Settings : SingletonScriptableObject<Settings,ICreationMethodLocated>
    {
        public float SFXMult;
        public float VibrationMult;
        public float MusicMult;
    }
}