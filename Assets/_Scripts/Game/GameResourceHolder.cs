using System;
using UnityEngine;

namespace RingMaester
{

    [Serializable]
    [CreateAssetMenu(fileName = "GameResourceHolder", menuName = "ScriptableObjects/GameResourceHolder")]
    public class GameResourceHolder : SingletonScriptableObject<GameResourceHolder,ICreationMethodLocated>
    {
        public Knob KnobPrefab;
        public Reward RewardPrefab;
        public Sprite PauseSprite;
        public Sprite PlaySprite;
        public ParticleSystem PlayerDeathParticle;
    }
}