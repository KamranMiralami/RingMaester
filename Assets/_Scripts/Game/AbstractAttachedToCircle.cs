using DG.Tweening;
using RingMaester;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAttachedToCircle : MonoBehaviour
{
    public enum PosType
    {
        InWard,
        OutWard
    }
    [Header("Abstract Properties")]
    [SerializeField] protected float InWardYDifference;
    [SerializeField] protected float transitionDuration;
    [SerializeField] protected PosType defaultPosType;
    [Header("Abstract References")]
    [SerializeField] protected GameObject model;
    public PosType CurPosType
    {
        protected set;
        get;
    }
    public virtual void Init()
    {
        CurPosType = defaultPosType;
    }
    /// <summary>
    /// Changed the current position of player.
    /// </summary>
    /// <param forced>= forces the position if not 0. Outward for forced < 0 and Inward for forced > 0 </param>
    protected virtual void TogglePosition(int forced = 0)
    {
        PosType targetPos;
        if (forced == 0)
        {
            switch (CurPosType)
            {
                case PosType.InWard: targetPos = PosType.OutWard; break;
                case PosType.OutWard: targetPos = PosType.InWard; break;
                default: GameDebug.LogError("This PosType is not supported"); return;
            }
        }
        else
        {
            targetPos = forced > 0 ? PosType.InWard : PosType.OutWard;
        }
        if (targetPos == CurPosType) return;

        var yChange = targetPos == PosType.InWard ? -InWardYDifference : InWardYDifference;
        model.transform.DOComplete(true);
        model.transform.DOLocalMoveY(model.transform.localPosition.y + yChange, transitionDuration);

        CurPosType = targetPos;
    }
    public abstract void GetHit(Collider2D other);

}
