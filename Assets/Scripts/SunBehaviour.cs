using DG.Tweening;
using UnityEngine;

public class SunBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    [SerializeField] private float raysScale = 1.2f;
    [SerializeField] private float raysTime = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DORotate(Vector3.forward * speed, 4).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        transform.GetChild(0).DOScale(raysScale, raysTime).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
