using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingMaester
{
    [Serializable]
    [CreateAssetMenu(fileName = "GameBalance", menuName = "ScriptableObjects/GameBalance")]
    public class GameBalance : SingletonScriptableObject<GameBalance, ICreationMethodLocated>
    {
        public float ScoreBonusCD;
        public int ScoreBonusAmount;
        public AnimationCurve GameSpeedCurve;
        public float MaxGameSpeed;
        public float MaxGameSpeedTime;
        public int TargetStreakForBonus;
        public float DefaultGameSpeed;
    }
}