using DG.Tweening;
using SFXSystem;
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
        [SerializeField] DotTrail dotTrail;
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
            dotTrail.Init();
            base.Init();
        }
        private void Update()
        {
            if (!canMove) return;
            HandleMovement();
            if (canChangePos)
            {
                if (Input.GetMouseButtonDown(0) && !IsPointerOverGameObject())
                {
                    TogglePosition();
                }
            }
        }
        public bool IsPointerOverGameObject()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return true;
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return true;
            }

            return false;
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
        public void Kill()
        {
            CameraShake.Instance.Shake(0.5f, 0.15f);
            SoundSystemManager.Instance.PlaySFX("Loose");
            var particle = Instantiate(GameResourceHolder.Instance.PlayerDeathParticle,model.transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
        public override void GetHit(Collider2D other)
        {

        }
    }
}