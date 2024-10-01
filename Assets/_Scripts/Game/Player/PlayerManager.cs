using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
namespace RingMaester.Managers
{
    public class PlayerManager : AbstractAttachedToCircle
    {

        [Header("Movement Properties")]
        [SerializeField] private bool isClockWise;
        [SerializeField] private float rotationSpeed;

        [Header("References")]
        [SerializeField] PlayerCollisionHandler playerCollisionHandler;
        public float CurAngle
        {
            get
            {
                return transform.localRotation.eulerAngles.z;
            }
            private set
            {
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, value));
            }
        }
        private bool canMove;
        private bool canChangePos;
        public override void Init()
        {
            canMove = true;
            canChangePos = true;
            playerCollisionHandler.Init(this);
            base.Init();
        }
        private void Update()
        {
            if (!canMove) return;
            HandleMovement();
            if (canChangePos)
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    TogglePosition();
                }
            }
        }
        void HandleMovement()
        {
            var mult = isClockWise ? -1 : 1;
            var curRot = transform.localRotation.eulerAngles.z;
            var newRot = curRot + rotationSpeed * Time.deltaTime * mult* GameManager.Instance.GameSpeed;
            CurAngle = newRot;
        }
        public void ChangeMovement(bool val)
        {
            canMove = val;
            canChangePos = val;
        }
        public override void GetHit(Collider2D other)
        {

        }
    }
}