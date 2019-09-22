using UnityEngine;
using System.Collections;

public class CRGTFader : MonoBehaviour {

    public CanvasGroup faderGroup;
    public float fadeTime = 0.25f;
    public float fadeFinal = 1.0f;

    void Start()
    {
        faderGroup = GetComponentInChildren<CanvasGroup>();
    }

    void OnEnable()
    {
        if (faderGroup == null)
            faderGroup = GetComponentInChildren<CanvasGroup>();

        faderGroup.alpha = 0;

        StartCoroutine(StartFade());
    }

    private IEnumerator StartFade(bool fade = true)
    {
        float elapsedTime = 0.0f;
        float wait = fadeTime;

        yield return null;

        while (elapsedTime < wait)
        {
            if (fade)
            {
                faderGroup.alpha = (elapsedTime / wait) * fadeFinal;
            }
            else
                faderGroup.alpha = 1.0f - (elapsedTime / wait);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
