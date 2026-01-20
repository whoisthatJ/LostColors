using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public Button start;
    public Button exit;
    public AudioSource audioSource;
    public AudioClip beep;
    public AudioClip bye;
    void Start()
    {
        start.onClick.AddListener(() => { StartCoroutine(StartGame()); });
        exit.onClick.AddListener(() => { StartCoroutine(ExitGame()); });
    }

    IEnumerator StartGame() { 
        audioSource.clip = beep;
        audioSource.Play();
        yield return new WaitForSeconds(beep.length);
        SceneManager.LoadScene("Level0");
    }
    IEnumerator ExitGame() {
        audioSource.clip = bye;
        audioSource.Play();
        yield return new WaitForSeconds(bye.length);
        Application.Quit();
    }
}
