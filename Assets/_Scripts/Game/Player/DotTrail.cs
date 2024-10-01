using Cysharp.Threading.Tasks;
using RingMaester.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace RingMaester
{
    public class DotTrail : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] float dotSpawnInterval = 0.2f;
        [SerializeField] float dotFadeTime = 1.0f;
        [SerializeField] float dotScaleReduceSpeed = 0.1f;
        [SerializeField] int maxDots = 3;

        private Queue<Dot> dots;

        public void Init()
        {
            dots = new();
            SpawnDots();
        }
        async UniTask SpawnDots()
        {
            while (true)
            {
                //var Dot = Instantiate(GameResourceHolder.Instance.DotPrefab, transform.position, Quaternion.identity); //TODO : Object pool
                var Dot=Splash.DotObjectPool.Get();
                Dot.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                Dot.transform.localScale = 0.075f*Vector3.one ;
                var dot = Dot.DotSprite;
                dots.Enqueue(Dot);
                FadeDot(dot, dotFadeTime);
                if (dots.Count > maxDots)
                {
                    Dot oldDot = dots.Dequeue();
                    //Destroy(oldDot.gameObject);
                    Splash.DotObjectPool.Release(oldDot);
                }
                await UniTask.Delay((int)(dotSpawnInterval * 1000 / GameManager.Instance.GameSpeed));
            }
        }

        async UniTask FadeDot(SpriteRenderer dotRenderer, float fadeTime)
        {
            Color originalColor = dotRenderer.color;
            float elapsed = 0f;
            while (elapsed < fadeTime)
            {
                try
                {
                    elapsed += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
                    dotRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

                    dotRenderer.transform.localScale -= dotScaleReduceSpeed * Time.deltaTime * Vector3.one;
                }
                catch (MissingReferenceException)
                {
                    return;
                }
                await UniTask.Yield();
            }
        }
    }
}