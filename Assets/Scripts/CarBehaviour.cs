using DG.Tweening;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public WheelJoint2D frontWheel;
    public WheelJoint2D backWheel;
    public float maxSpeed = 1000f; // motor speed magnitude (degrees/sec)
    public float maxMotorTorque = 1000f; // how strong the motor is
    public float jumpForce = 500f; // force applied when jumping

    [Header("Engine Audio")]
    public AudioSource engineSource; // should be on the car object (or assigned in inspector)
    public AudioClip engineClip; // 0.5s sample (must be set to loop in code)
    public float idleVolume = 0.15f;
    public float maxVolume = 1f;
    public float minPitch = 0.9f;
    public float maxPitch = 1.6f;

    Rigidbody2D rb;

    public GameObject steeringWheel;
    bool isLocked = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (engineSource != null && engineClip != null)
        {
            engineSource.clip = engineClip;
            engineSource.loop = true;
            engineSource.playOnAwake = false;
            engineSource.volume = idleVolume;
            engineSource.pitch = minPitch;

            if (!engineSource.isPlaying)
            {
                engineSource.Play();
            }
        }
        PlayerController.Instance.isLocked = true;
        steeringWheel.transform.DOLocalRotate(Vector3.forward * 20, 1).SetLoops(-1, LoopType.Yoyo);
        DialogController.Instance.StartDialog(() => {
            PlayerController.Instance.gameObject.transform.SetParent(transform);
            PlayerController.Instance.gameObject.transform.localPosition = new Vector3(0, 4, 0);
            PlayerController.Instance.isLocked = true;
            isLocked = false;
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rb != null)
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
        }
    }

    // FixedUpdate is called on the physics step
    void FixedUpdate()
    {
        if(isLocked)
            return;
        float axis = Input.GetAxis("Horizontal");

        // Compute desired motor speed.
        float desiredSpeed = axis * maxSpeed;

        // Apply to front wheel
        if (frontWheel != null)
        {
            JointMotor2D motor = frontWheel.motor;
            motor.motorSpeed = desiredSpeed;
            motor.maxMotorTorque = maxMotorTorque;
            frontWheel.motor = motor;
            frontWheel.useMotor = Mathf.Abs(axis) > 0.01f || Mathf.Abs(motor.motorSpeed) > 0.01f;
        }

        // Apply to back wheel
        if (backWheel != null)
        {
            JointMotor2D motor = backWheel.motor;
            motor.motorSpeed = desiredSpeed;
            motor.maxMotorTorque = maxMotorTorque;
            backWheel.motor = motor;
            backWheel.useMotor = Mathf.Abs(axis) > 0.01f || Mathf.Abs(motor.motorSpeed) > 0.01f;
        }

        // Engine audio control (scale pitch & volume with throttle)
        if (engineSource != null && engineClip != null)
        {
            float throttle = Mathf.Clamp01(Mathf.Abs(axis)); // 0 .. 1 based on input
            // Optionally include vehicle forward speed to affect sound more realistically:
            float speedFactor = Mathf.Clamp01(Mathf.Abs(rb != null ? rb.linearVelocity.x / maxSpeed : 0f));
            float engineFactor = Mathf.Max(throttle, speedFactor);

            engineSource.pitch = Mathf.Lerp(minPitch, maxPitch, engineFactor);
            engineSource.volume = Mathf.Lerp(idleVolume, maxVolume, engineFactor);

            // Ensure clip is looping — safe guard in case inspector clip had loop off
            if (!engineSource.loop) engineSource.loop = true;
            if (!engineSource.isPlaying) engineSource.Play();
        }
    }
}
