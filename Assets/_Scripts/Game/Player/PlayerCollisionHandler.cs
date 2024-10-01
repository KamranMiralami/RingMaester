using RingMaester.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RingMaester
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody2D rb;
        [HideInInspector]
        public PlayerManager Manager;
        bool CanHit;
        public void Init(PlayerManager manager)
        {
            Manager = manager;
            CanHit = true;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!CanHit) return;
            if (collision.gameObject.CompareTag("Knob"))
            {
                GameDebug.Log("Player Hit Knob", GameDebug.DebugEnum.Player);
                var knob = collision.transform.parent.gameObject.GetComponent<Knob>();
                knob.GetHit(collision);
            }
            if (collision.gameObject.CompareTag("Reward"))
            {
                GameDebug.Log("Player Hit Reward", GameDebug.DebugEnum.Player);
                var reward = collision.transform.parent.gameObject.GetComponent<Reward>();
                reward.GetHit(collision);
            }
        }
    }
}