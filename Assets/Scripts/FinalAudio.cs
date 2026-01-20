using System.Collections;
using UnityEngine;

public class FinalAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource sourceA;

    [Header("Clips")]
    public AudioClip[] clips; // size = 3

    [Header("Delay Settings")]
    public float minDelay = 0.3f;
    public float maxDelay = 1.2f;

    private AudioSource currentSource;
    private int lastClipIndex = -1;

    void Start() {
        currentSource = sourceA;
        StartCoroutine(PlayLoop());
    }

    IEnumerator PlayLoop() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            int clipIndex;
            do {
                clipIndex = Random.Range(0, clips.Length);
            }
            while (clipIndex == lastClipIndex && clips.Length > 1);

            lastClipIndex = clipIndex;

            currentSource.clip = clips[clipIndex];
            currentSource.Play();
            yield return new WaitForSeconds(currentSource.clip.length);

        }
    }
}
