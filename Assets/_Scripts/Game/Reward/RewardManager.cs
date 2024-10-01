using RingMaester;
using RingMaester.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AbstractAttachedToCircle;

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
    public void MakeReward()
    {
        var Reward = Instantiate(GamePrefabHolder.Instance.RewardPrefab, RewardParent);
        Reward.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        int posT = Enum.GetValues(typeof(PosType)).Length;
        PosType randomType = (PosType)UnityEngine.Random.Range(0, posT);
        var rewardAngle = GetRandomRewardAngle(randomType);

        Reward.InitReward(rewardAngle, randomType);
        rewardList.Add(Reward);
    }
    public float GetRandomRewardAngle(PosType posType)
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
            var tmp = rewardList.Where(x => Mathf.Abs(x.CurAngle - random) < threshold);
            var tmp2 = knobList.Where(x => Mathf.Abs(x.CurAngle - random) < threshold && posType== x.CurPosType);
            var tmp3 = Mathf.Abs(GameManager.Instance.GetPlayerAngle() - random) < 20;
            if (!tmp.Any() && !tmp2.Any() && !tmp3)
                break;
            random = UnityEngine.Random.Range(0, 360);


            tries++;
            if (tries > 1000)
            {
                GameDebug.LogError("We cant find a proper angle, continuing with angle " + random);
                break;
            }
        }
        return random;
    }
}
