using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyGif : MonoBehaviour
{
    public bool loop = true;
    public float totalTime = 5.0f;
    float spriteTime;
    [PreviewField] public List<Sprite> sprites = new List<Sprite>();

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteTime = totalTime / (float)sprites.Count;
        StartCoroutine(Gif());
    }

    IEnumerator Gif()
    {
        do
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                yield return new WaitForSeconds(spriteTime);
                spriteRenderer.sprite = sprites[i];
            }
        }
        while (loop);
    }
}
