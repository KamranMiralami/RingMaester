using DG.Tweening;
using RingMaester;
using RingMaester.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : AbstractAttachedToCircle
{
    [Header("Properties")]
    [SerializeField] float appearDuration=0.1f;
    public float CurAngle
    {
        get;
        private set;
    }

    public override void GetHit(Collider2D other)
    {
        GameManager.Instance.EndGame();
    }

    public void InitKnob(float angle,PosType posType)
    {
        base.Init();
        CurAngle = angle;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, CurAngle));
        var scale = new Vector3(model.transform.localScale.x, model.transform.localScale.y, model.transform.localScale.z);
        if(CurPosType!=posType)
            TogglePosition();
        model.transform.DOKill(true);
        model.transform.localScale = Vector3.zero;
        model.transform.DOScale(scale, appearDuration);
    }
}
