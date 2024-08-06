using System.Collections;
using TMPro;
using UnityEngine;

public class DisappearingText : MonoBehaviour
{
    [SerializeField] float disappearAfter;
    [SerializeField] [Tooltip("After disappearAfter seconds, the text will fade in fadeTime seconds.")] float fadeTime;

    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearAfter);

        Color newColor = text.color;

        for(float elapsedTime=0f; elapsedTime < fadeTime; elapsedTime += Time.deltaTime)
        {
            Debug.Log(elapsedTime);
            newColor.a = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
            text.color = newColor;
            yield return null; // Ensures the coroutine yields control every frame, allowing smooth fading
        }

        newColor.a = 0f;
        text.color = newColor; // Ensure the alpha is set to 0

        Destroy(this.gameObject);
    }
}
