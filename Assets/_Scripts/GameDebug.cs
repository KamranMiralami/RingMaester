using System;
using UnityEngine;
namespace RingMaester
{
    [Serializable]
    [CreateAssetMenu(fileName = "GameDebug", menuName = "ScriptableObjects/GameDebug")]
    public class GameDebug : SingletonScriptableObject<GameDebug, ICreationMethodLocated>
    {
        public DebugEnum DebugLevel;
        private static bool CanShowDebug => true;
        public static void Log(string log, DebugEnum debug = DebugEnum.Custom)
        {
            if (CanShowDebug && CanShowDebug && HasTag(debug))
                Debug.Log(GetPrefix(debug) + log);
        }
        public static void LogFormat(string log, DebugEnum debug, params object[] args)
        {
            if (CanShowDebug && HasTag(debug))
                Debug.LogFormat(GetPrefix(debug) + log, args);
        }
        public static void LogError(string log, DebugEnum debug = DebugEnum.Custom)
        {
            if (CanShowDebug && HasTag(debug))
                Debug.LogError(GetPrefix(debug) + log);
        }
        public static void LogErrorFormat(string log, DebugEnum debug, params object[] args)
        {
            if (CanShowDebug && HasTag(debug))
                Debug.LogErrorFormat(GetPrefix(debug) + log, args);
        }

        public static void Log(string log, string CustomPrefix)
        {
            if (HasTag(DebugEnum.Custom))
                Debug.Log(CustomPrefix + " " + log);
        }
        public static void LogFormat(string log, string CustomPrefix, params object[] args)
        {
            if (HasTag(DebugEnum.Custom))
                Debug.LogFormat(CustomPrefix + " " + log, args);
        }
        public static void LogError(string log, string CustomPrefix)
        {
            if (HasTag(DebugEnum.Custom))
                Debug.LogError(CustomPrefix + " " + log);
        }
        public static void LogErrorFormat(string log, string CustomPrefix, params object[] args)
        {
            if (HasTag(DebugEnum.Custom))
                Debug.LogErrorFormat(CustomPrefix + " " + log, args);
        }
        static bool HasTag(DebugEnum debug)
        {
            return (Instance.DebugLevel & debug) == debug;
        }
        static string GetPrefix(DebugEnum debug)
        {
            switch (debug)
            {
                case DebugEnum.Physics:
                    return "Physics: ";
                case DebugEnum.Logic:
                    return "Logic Simulation: ";
                case DebugEnum.Visual:
                    return "Visual Simulation: ";
                case DebugEnum.TODO:
                    return "TODO: ";
                case DebugEnum.Match:
                    return "Match: ";
                case DebugEnum.Socket:
                    return "Socket: ";
                case DebugEnum.Managers:
                    return "Managers: ";
                case DebugEnum.StartUp:
                    return "Start Up: ";
                case DebugEnum.UI:
                    return "UI : ";
                case DebugEnum.Player:
                    return "PlayerInput : ";
                default:
                    return "";
            }
        }

        [Flags]
        public enum DebugEnum
        {
            Physics = 1,
            Logic = 2,
            Visual = 4,
            TODO = 8,
            Match = 16,
            Buff = 32,
            Socket = 64,
            Managers = 128,
            Custom = 256,
            StartUp = 512,
            Restful = 1024,
            UI = 2048,
            Player = 4096,
        }
    }
}

