using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RingMaester;

namespace RingMaester.Managers
{
    public class RewardManager : SingletonBehaviour<RewardManager>
    {
        [Header("References")]
        [SerializeField] Transform RewardParent;
        List<Reward> rewardList;
        public void Init()
        {
            rewardList = new();
            foreach (Transform t in RewardParent)
            {
                Destroy(t.gameObject);
            }
        }
        public void PauseRewards()
        {
            foreach (var reward in rewardList)
            {
                reward.Pause();
            }
        }
        public void ResumeRewards()
        {
            foreach (var reward in rewardList)
            {
                reward.Resume();
            }
        }
        public void MakeReward()
        {
            var Reward = Instantiate(GameResourceHolder.Instance.RewardPrefab, RewardParent);
            Reward.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            int posT = Enum.GetValues(typeof(AbstractAttachedToCircle.PosType)).Length;
            AbstractAttachedToCircle.PosType randomType = (AbstractAttachedToCircle.PosType)UnityEngine.Random.Range(0, posT);
            var rewardAngle = GetRandomRewardAngle(randomType);

            Reward.InitReward(rewardAngle, randomType);
            rewardList.Add(Reward);
        }
        public float GetRandomRewardAngle(AbstractAttachedToCircle.PosType posType)
        {
            int tries = 0;
            var random = UnityEngine.Random.Range(0, 360);
            var knobList = KnobManager.Instance.KnobList;

            int threshold = 10;
            if (rewardList.Count > 0)
                threshold = 360 / rewardList.Count - 20;
            threshold = Mathf.Clamp(threshold, 5, 60);

            while (true)
            {
                var tmp = rewardList.FirstOrDefault(x => Mathf.Abs(x.CurAngle - random) < threshold
                || Mathf.Abs(x.CurAngle + 360 - random) < threshold);
                var tmp2 = knobList.FirstOrDefault(x => Mathf.Abs(x.CurAngle - random) < 20
                || Mathf.Abs(x.CurAngle + 360 - random) < 20);
                var tmp3 = Mathf.Abs(GameManager.Instance.GetPlayerAngle() - random) < 60;
                if (tmp == null && tmp2 == null && !tmp3)
                    break;
                random = UnityEngine.Random.Range(0, 360);


                tries++;
                if (tries > 1000)
                {
                    GameDebug.LogError("We cant find a proper angle, continuing with angle " + random);
                    break;
                }
            }
            //WriteDebug(random,threshold);
            return random;
        }
        void WriteDebug(int random, int threshold)
        {
            Debug.Log("///////////////");
            var knobList = KnobManager.Instance.KnobList;
            foreach (var knob in knobList)
            {
                Debug.Log(knob.CurAngle + " " + knob.CurPosType);
            }
            Debug.Log("and random/threshold is " + random + " " + threshold);
            Debug.Log("///////////////");
        }
    }
}