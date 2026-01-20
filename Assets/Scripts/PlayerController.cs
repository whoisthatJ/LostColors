using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    Animator animator;
    public AudioSource audioSource;
    public AudioClip[] taps;
    Rigidbody2D rb;
    public float speed = 5f;
    public bool isGrounded = true;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        if(SceneManager.GetActiveScene().name == "Level0")
            DialogController.Instance.StartDialog(FirstEnded);
        isLocked = true;
    }

    private void FirstEnded() {
        DialogController.Instance.StartDialog(SecondEnded);
    }

    private void SecondEnded() {
        isLocked = false;
    }

    public bool isLocked = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocked) {
            return;
        }
        float x = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(x));
        rb.MovePosition(rb.position + new Vector2(x * speed * Time.fixedDeltaTime, 0));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isLocked)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(new Vector2(0, 300f));
            isGrounded = false;
        }
        if(Input.GetMouseButtonDown(0))
        {
            DialogController.Instance.NextLine();
        }
    }
    public void PlayTapSound()
    {
        int index = Random.Range(0, taps.Length);
        audioSource.clip = taps[index];
        audioSource.Play();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
