using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingMaester
{

    [Serializable]
    [CreateAssetMenu(fileName = "MainMenuResourceHolder", menuName = "ScriptableObjects/MainMenuResourceHolder")]
    public class MainMenuResourceHolder : SingletonScriptableObject<MainMenuResourceHolder, ICreationMethodLocated>
    {
        public Color DeselectedColor;
        public Color SelectedColor;
        public Color LeaderboardSelfColor;
        public Color LeaderboardOthersColor;
        public LeaderBoardProfile LeaderBoardProfilePrefab;
    }
}
