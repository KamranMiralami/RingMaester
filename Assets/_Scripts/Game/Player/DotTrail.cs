using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class DotTrail : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SpriteRenderer dotPrefab;
    [Header("Properties")]
    [SerializeField] float dotSpawnInterval = 0.2f;
    [SerializeField] float dotFadeTime = 1.0f;
    [SerializeField] float dotScaleReduceSpeed = 0.1f;
    [SerializeField] int maxDots = 3;

    private Queue<SpriteRenderer> dots;

    void Start()
    {
        dots = new();
        SpawnDots();
    }
    async UniTask SpawnDots()
    {
        while (true)
        {
            SpriteRenderer dot = Instantiate(dotPrefab, transform.position, Quaternion.identity); //TODO : Object pool
            dots.Enqueue(dot);
            FadeDot(dot, dotFadeTime);
            if (dots.Count > maxDots)
            {
                SpriteRenderer oldDot = dots.Dequeue();
                Destroy(oldDot.gameObject);
            }
            await UniTask.Delay((int)(dotSpawnInterval*1000));
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
