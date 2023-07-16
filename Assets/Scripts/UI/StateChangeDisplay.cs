using System.Collections;
using UnityEngine;
using TMPro;

public class StateChangeDisplay : MonoBehaviour
{
    [SerializeField] Color defaultVertexColor = new Color(0.27f, 0.31f, 0.36f, 0.4f);
    [SerializeField] float fadeOutSpeed = 0.01f;
    [SerializeField] float fadeOutAmount = 0.15f;
    float startingOpacity = 0.4f;
    float currentOpacity = 0.4f;
    TMP_Text display;
    Coroutine currentDisplay;
    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        display.color = new Color(0, 0, 0, 0);
    }
    public void DisplayState(string text)
    {
        if (currentDisplay != null)
            StopCoroutine(currentDisplay);
        display.text = text;
        display.color = defaultVertexColor;
        currentOpacity = startingOpacity;
        StartCoroutine(DisplayTextEffect());
    }

    IEnumerator DisplayTextEffect()
    {
        while (currentOpacity >= 0)
        {
            display.color = new Color(0.27f, 0.31f, 0.36f, currentOpacity);
            currentOpacity -= Time.deltaTime * fadeOutAmount;
            yield return new WaitForSeconds(fadeOutSpeed);
        }
        display.color = new Color(0, 0, 0, 0);
    }
}
