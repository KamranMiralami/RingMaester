using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingMaester
{
    [Serializable]
    [CreateAssetMenu(fileName = "LeaderboardSettings", menuName = "ScriptableObjects/LeaderboardSettings")]
    public class LeaderboardSettings : SingletonScriptableObject<LeaderboardSettings,ICreationMethodLocated>
    {
        public int NumberOfRandomNumbers = 5000;
        public string FileNameN = "RandomNumbers.txt";
        public string FileNameS = "RandomStrings.txt";
        public int VisibleNumbers = 20;
    }
}