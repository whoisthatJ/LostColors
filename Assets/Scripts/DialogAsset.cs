using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/Dialog Asset")]
public class DialogAsset : ScriptableObject {
    public List<DialogLine> lines = new List<DialogLine>();
}
[Serializable]
public class DialogLine {
    [TextArea(2, 5)]
    public string text;

    public bool isPlayer;
}