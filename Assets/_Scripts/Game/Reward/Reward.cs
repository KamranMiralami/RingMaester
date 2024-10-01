using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using RingMaester.Managers;

namespace RingMaester
{
    public class Reward : AbstractAttachedToCircle
    {
        [Header("Properties")]
        [SerializeField] float appearDuration = 0.1f;
        [SerializeField] float rotationSpeed = 2f;
        [SerializeField] int rewardAmount = 1;
        public float CurAngle
        {
            get;
            private set;
        }
        public void Pause()
        {
            model.transform.DOPause();
        }
        public void Resume()
        {
            model.transform.DORestart();
        }
        public override void GetHit(Collider2D other)
        {
            GameManager.Instance.GiveReward(rewardAmount);
            model.transform.DOScale(Vector3.zero, appearDuration).OnComplete(() =>
            {
                Destroy(gameObject);
            });
#if UNITY_ANDROID
            Vibration.VibrateAndroid((long)(Settings.Instance.VibrationMult * 1000));
#endif
        }

        public void InitReward(float angle, PosType posType)
        {
            base.Init();
            CurAngle = angle;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, CurAngle));
            if (CurPosType != posType)
                TogglePosition();
            var scale = new Vector3(model.transform.localScale.x, model.transform.localScale.y, model.transform.localScale.z);
            model.transform.DOKill(true);
            model.transform.localScale = Vector3.zero;
            model.transform.DOScale(scale, appearDuration).OnComplete(() =>
            {
                PlayeAnimation();
            });
        }
        void PlayeAnimation()
        {
            model.transform.DORotate(new Vector3(0, 0, 360), rotationSpeed, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .SetSpeedBased(true);
        }
    }
}