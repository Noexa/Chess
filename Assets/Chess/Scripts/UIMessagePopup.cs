using UnityEngine;
using TMPro;
using System.Collections;

public class UIMessagePopup : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float seconds = 2.0f;

    private Coroutine _routine;

    public void Show(string message)
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
        }

        _routine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine (string message)
    {
        messageText.text = message;
        panel.SetActive(true);

        yield return new WaitForSeconds(seconds);

        panel.SetActive(false);
        _routine = null;
    }

    private void Awake()
    {
        panel.SetActive(false);
    }
}
