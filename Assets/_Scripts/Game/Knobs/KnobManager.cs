using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RingMaester.Managers
{
    public class KnobManager : SingletonBehaviour<KnobManager>
    {
        [Header("References")]
        [SerializeField] Transform KnobParent;
        [HideInInspector]
        public List<Knob> KnobList;
        public void Init(int initKnobs)
        {
            foreach (Transform t in KnobParent)
            {
                Destroy(t.gameObject);
            }

            KnobList = new();
            for (int i = 0; i < initKnobs; i++)
            {
                CreateKnob();
            }
            GameManager.Instance.PlayerGotBonus += CreateKnob;
        }
        public void CreateKnob()
        {
            var knob = Instantiate(GameResourceHolder.Instance.KnobPrefab, KnobParent);
            knob.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            var knobAngle = GetRandomKnobAngle();

            int posT = Enum.GetValues(typeof(AbstractAttachedToCircle.PosType)).Length;
            AbstractAttachedToCircle.PosType randomType = (AbstractAttachedToCircle.PosType)UnityEngine.Random.Range(0, posT);

            knob.InitKnob(knobAngle, randomType);
            KnobList.Add(knob);
        }
        public float GetRandomKnobAngle()
        {
            int tries = 0;
            var random = UnityEngine.Random.Range(0, 360);
            while (true)
            {
                int threshold = 10;
                if (KnobList.Count > 0)
                    threshold = 360 / KnobList.Count - 40;
                threshold = Mathf.Clamp(threshold, 5, 60);
                var tmp = KnobList.FirstOrDefault(x => Mathf.Abs(x.CurAngle - random) < threshold
                    || Mathf.Abs(x.CurAngle + 360 - random) < threshold);
                var tmp3 = Mathf.Abs(GameManager.Instance.GetPlayerAngle() - random) < 45
                    || Mathf.Abs(GameManager.Instance.GetPlayerAngle() + 360 - random) < 45;
                if (tmp == null && !tmp3)
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
        private void OnDestroy()
        {
            GameManager.Instance.PlayerGotBonus -= CreateKnob;
        }
    }
}