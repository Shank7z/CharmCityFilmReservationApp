using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingAnimation : MonoBehaviour
{
    public Image image;
    public Sprite[] frames;
    public float frameRate = 12f;

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        int frame = 0;

        while (true)
        {
            image.sprite = frames[frame];

            frame++;
            if (frame >= frames.Length)
                frame = 0;

            yield return new WaitForSeconds(1f / frameRate);
        }
    }
}