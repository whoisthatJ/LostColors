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
        if (transform.position.x < -7.38f)
            transform.position = new Vector3(-7.38f, transform.position.y);
        if(transform.position.x > 17.47f)
            transform.position = new Vector3(17.47f, transform.position.y);
        if (Mathf.Abs(transform.position.y - 3.15f) > 0.1f)
            transform.position = new Vector3(transform.position.x, 3.15f);
    }
}
