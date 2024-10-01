using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RingMaester
{
    public class CameraShake : SingletonBehaviour<CameraShake>
    {
        public async UniTaskVoid Shake(float duration, float magnitude)
        {
            Vector3 originalPosition = transform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(x, y, originalPosition.z);

                elapsed += Time.deltaTime;

                await UniTask.Yield();
            }

            transform.localPosition = originalPosition;
        }
    }
}