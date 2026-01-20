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
    private bool readingAngle = false; // new: ignore content inside '<' '>'

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
        readingAngle = false;

        for (int i = 0; i < rawText.Length; i++) {
            char c = rawText[i];

            // Sound key handling using [key]
            if (c == '[') {
                readingKey = true;
                pendingKey = "";
                continue;
            }

            if (c == ']') {
                readingKey = false;
                continue;
            }

            if (readingKey) {
                pendingKey += c;
                continue;
            }

            // Ignore text inside angle brackets (e.g. markup like <color=...>)
            if (c == '<') {
                readingAngle = true;                
            }

            if (c == '>') {
                readingAngle = false;                
            }

            /*if (readingAngle) {
                // skip everything inside <...>
                continue;
            }*/

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
            if(!readingAngle)
                yield return new WaitForSeconds(letterDelay);
        }

        // End of line
        TryPlayPendingSound();
    }

    private void TryPlayPendingSound() {
        if (string.IsNullOrEmpty(pendingKey)) {
            AudioClip clip = soundBank.Get("badya");
            if (clip != null && !audioSource.isPlaying) {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
        else {
            AudioClip clip = soundBank.Get(pendingKey);
            if (clip != null) {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
        pendingKey = null;
    }
}
