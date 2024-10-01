using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

namespace PanelSystem
{
    public abstract class Panel : MonoBehaviour
    {
        public bool IsOpen { get; protected set; }
        [Header("DOTween Properties")]
        [SerializeField] protected Vector3 offsetPos;
        [SerializeField] protected float duration;
        [SerializeField] protected Transform TweenTransform;

        Vector3 startPos;
        Vector3 endPos;
        protected void Awake()
        {
            if (TweenTransform == null)
            {
                TweenTransform= transform;
            }
            startPos = TweenTransform.position + offsetPos;
            endPos = TweenTransform.position;
        }

        public void Open()
        {
            if (IsOpen)
                return;

            IsOpen = true;
            OnOpenStarted();
            if (duration <=0)
            {
                OnOpenFinished();
            }
            else
            {
                TweenTransform.DOKill(true);
                TweenTransform.position =startPos;
                TweenTransform.DOMove(endPos, duration).OnComplete(() =>
                {
                    OnOpenFinished();
                });
            }
        }
        public void InstantClose()
        {
            if (!IsOpen)
            {
                return;
            }
            IsOpen = false;
            OnCloseStarted();
            OnCloseFinished();
        }
        public void Close()
        {
            if (!IsOpen)
            {
                return;
            }
            IsOpen = false;
            OnCloseStarted();
            if (duration <=0)
            {
                OnCloseFinished();
                return;
            }
            else
            {
                TweenTransform.DOKill(true);
                TweenTransform.DOMove(startPos, duration).OnComplete(() =>
                {
                    TweenTransform.position = endPos;
                    OnCloseFinished();
                });
            }
        }

        public abstract void Init();
        protected abstract void OnOpenFinished();
        protected abstract void OnCloseFinished();

        protected abstract void OnOpenStarted();

        protected abstract void OnCloseStarted();
    }
}