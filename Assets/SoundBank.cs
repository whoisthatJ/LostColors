using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/Sound Bank")]
public class SoundBank : ScriptableObject {
    [System.Serializable]
    public class SoundEntry {
        public string key;
        public AudioClip clip;
    }

    public List<SoundEntry> sounds = new List<SoundEntry>();

    private Dictionary<string, AudioClip> lookup;

    public void Init() {
        lookup = new Dictionary<string, AudioClip>();
        foreach (var s in sounds) {
            lookup[s.key.ToLower()] = s.clip;
        }
    }

    public AudioClip Get(string key) {
        if (lookup == null) Init();
        lookup.TryGetValue(key.ToLower(), out var clip);
        return clip;
    }
}
