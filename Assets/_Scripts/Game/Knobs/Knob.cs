using DG.Tweening;
using RingMaester.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingMaester
{
    public class Knob : AbstractAttachedToCircle
    {
        [Header("Properties")]
        [SerializeField] float appearDuration = 0.1f;
        public float CurAngle
        {
            get;
            private set;
        }

        public override void GetHit(Collider2D other)
        {
            GameManager.Instance.EndGame();
#if UNITY_ANDROID
            if(Settings.Instance.VibrationMult>0.1f)
                Vibration.Vibrate();
#endif
        }
        public void InitKnob(float angle, PosType posType)
        {
            base.Init();
            CurAngle = angle;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, CurAngle));
            var scale = new Vector3(model.transform.localScale.x, model.transform.localScale.y, model.transform.localScale.z);
            if (CurPosType != posType)
                TogglePosition();
            model.transform.DOKill(true);
            model.transform.localScale = Vector3.zero;
            model.transform.DOScale(scale, appearDuration);
            GameManager.Instance.PlayerGotReward += CheckToggle;
        }

        private void CheckToggle()
        {
            if (!isInit) return;
            var Rand = UnityEngine.Random.Range(0, 3);
            if (Rand == 0)
                TogglePosition();
        }
        private void OnDestroy()
        {
            GameManager.Instance.PlayerGotReward -= CheckToggle;
        }
    }
}