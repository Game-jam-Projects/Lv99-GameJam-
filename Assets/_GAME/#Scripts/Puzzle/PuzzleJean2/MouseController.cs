using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    [SerializeField] private int point;
    public Image image;
    public float fadeInTime = 0.5f;
    [SerializeField] float timeDuration = 4f;
    [SerializeField] GameObject effect;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());

    }

    public void SetDetroy()
    {
        
        if (effect != null)
        {
            image.enabled = false;
            effect.SetActive(true);

        }
        if (gameObject.name == "Bom(Clone)")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Puzzles/Bubble/Bubble Hit", GetComponent<Transform>().position);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Puzzles/Bubble/Bubble Pop", GetComponent<Transform>().position);
        }
        if (gameObject.name == "Ruim(Clone)")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Puzzles/Bubble/Bubble Error", GetComponent<Transform>().position);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Puzzles/Bubble/Bubble Miss", GetComponent<Transform>().position);
        }
        Puzzle3.Instance.totalPoints += point;
        Puzzle3.Instance.UpdatePoint();
        Destroy(gameObject,2f);
        
    }

    private IEnumerator TimeDuration()
    {
        yield return new WaitForSecondsRealtime(timeDuration);
        StartCoroutine(FadeOut());

    }

    private IEnumerator FadeIn()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);

        yield return null;

        float t = 0.0f;
        while (t < fadeInTime)
        {
            t += Time.unscaledDeltaTime;
            float normalizedTime = t / fadeInTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, normalizedTime);
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
        StartCoroutine(TimeDuration());

    }

    private IEnumerator FadeOut()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);

        yield return null;

        float t = 0.0f;
        while (t < fadeInTime)
        {
            t += Time.unscaledDeltaTime;
            float normalizedTime = t / fadeInTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, normalizedTime);
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(gameObject);
    }
}
