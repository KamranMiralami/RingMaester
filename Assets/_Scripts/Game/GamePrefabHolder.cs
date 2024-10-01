using System;
using UnityEngine;

namespace RingMaester
{

    [Serializable]
    [CreateAssetMenu(fileName = "GamePrefabHolder", menuName = "ScriptableObjects/GamePrefabHolder")]
    public class GamePrefabHolder : SingletonScriptableObject<GamePrefabHolder,ICreationMethodLocated>
    {
        public Knob KnobPrefab;
        public Reward RewardPrefab;
    }
}