using PanelSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingMaester
{
    public class ShopPanel : Panel
    {
        public override void Init()
        {
        }

        protected override void OnCloseFinished()
        {
            gameObject.SetActive(false);
        }

        protected override void OnCloseStarted()
        {
        }

        protected override void OnOpenFinished()
        {
        }

        protected override void OnOpenStarted()
        {
            gameObject.SetActive(true);
        }
    }
}