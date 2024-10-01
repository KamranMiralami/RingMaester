using System;
using UnityEngine;

namespace RingMaester.Managers
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [Header("References")]
        [SerializeField] PlayerManager playerManager;
        [SerializeField] KnobManager knobManager;
        [SerializeField] RewardManager rewardManager;
        private void Start()
        {
            playerManager.Init();
            knobManager.Init(3);
            rewardManager.Init();
            rewardManager.MakeReward();
        }
        public void EndGame()
        {
            playerManager.ChangeMovement(false);
        }

        public void GiveReward(int amount)
        {
        }
        public float GetPlayerAngle() => playerManager.CurAngle;
    }
}