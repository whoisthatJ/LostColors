using System.Collections;
using UnityEngine;
using TMPro;

public class DialogTyper : MonoBehaviour {
    [Header("UI")]
    public TextMeshProUGUI dialogText;

    [Header("Typing")]
    public float letterDelay = 0.04f;

    [Header("Audio")]
    public AudioSource audioSource;
    public SoundBank soundBank;

    private string visibleText = "";
    private string currentWord = "";
    private string pendingKey = null;
    private bool readingKey = false;

    private void Start() {
        StartDialog("Hi, my name is Yasmina[taptap2]! What is your name[question]?");
    }
    public void StartDialog(string rawText) {
        StopAllCoroutines();
        dialogText.text = "";
        StartCoroutine(TypeText(rawText));
    }

    private IEnumerator TypeText(string rawText) {
        visibleText = "";
        currentWord = "";
        pendingKey = null;
        readingKey = false;

        for (int i = 0; i < rawText.Length; i++) {
            char c = rawText[i];

            // Start reading key
            if (c == '[') {
                readingKey = true;
                pendingKey = "";
                continue;
            }

            // End reading key
            if (c == ']') {
                readingKey = false;
                continue;
            }

            // Collect key characters
            if (readingKey) {
                pendingKey += c;
                continue;
            }

            // Normal visible text
            visibleText += c;
            dialogText.text = visibleText;

            // Track current word
            if (char.IsLetter(c)) {
                currentWord += c;
            }
            else {
                TryPlayPendingSound();
                currentWord = "";
            }

            yield return new WaitForSeconds(letterDelay);
        }

        // End of line
        TryPlayPendingSound();
    }

    private void TryPlayPendingSound() {
        if (string.IsNullOrEmpty(pendingKey)) return;

        AudioClip clip = soundBank.Get(pendingKey);
        if (clip != null) {
            audioSource.PlayOneShot(clip);
        }

        pendingKey = null;
    }
}
