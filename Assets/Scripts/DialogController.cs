using System;
using UnityEngine;

public class DialogController : MonoBehaviour {
    public static DialogController Instance { get; private set; }

    public DialogAsset [] dialogs;
    public DialogTyper playerTyper;
    public DialogTyper sunTyper;

    private int currentLineIndex = 0;

    private int currentDialogIndex = 0;
    private Action onEnd;
    void Awake()
    {
        // Singleton enforcement
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartDialog(Action onEnd) {
        currentLineIndex = 0;
        this.onEnd = onEnd;
        playerTyper.gameObject.SetActive(true);
        sunTyper.gameObject.SetActive(true);
        PlayCurrentLine();
    }

    public void NextLine() {
        currentLineIndex++;

        if (currentLineIndex >= dialogs[currentDialogIndex].lines.Count) {
            EndDialog();
            return;
        }

        PlayCurrentLine();
    }

    private void PlayCurrentLine() {
        DialogLine line = dialogs[currentDialogIndex].lines[currentLineIndex];

        // Здесь можно менять визуал в зависимости от isPlayer
        if(line.isPlayer)
            playerTyper.StartDialog(line.text);
        else
            sunTyper.StartDialog(line.text);

        // Пример:
        // ui.SetSide(line.isPlayer);
        // ui.SetColor(line.isPlayer);
        // ui.SetPortrait(line.isPlayer);
    }

    private void EndDialog() {
        Debug.Log("Dialog finished");
        playerTyper.gameObject.SetActive(false);
        sunTyper.gameObject.SetActive(false);
        currentDialogIndex++;
        onEnd?.Invoke();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
