using UnityEngine;

public class CarController : MonoBehaviour
{
    InputHandler input;
    CarMovement movement;
    CarSteering steering;
    DriftController drift;
    CarJump jump;

    public bool CanControl = true;

    [Header("Audio Settings")]
    public string engineSFXName = "mesin";
    private bool isEnginePlaying = false;

    private void Awake()
    {
        input = GetComponent<InputHandler>();
        movement = GetComponent<CarMovement>();
        steering = GetComponent<CarSteering>();
        drift = GetComponent<DriftController>();
        jump = GetComponent<CarJump>();
    }

    private void Update()
    {
        HandleEngineSound();

        if (!CanControl) return;

        drift.HandelDrift(input.DriftHolding);

        if (drift.isDrifting)
        {
            steering.turnSpeed = 350f;
        }
        else
        {
            steering.turnSpeed = 200f;
        }

/*        // Memanggil fungsi lompat jika tombol ditekan
        if (input.JumpPressed)
        {
            jump.Jump();
        }*/
    }

    private void FixedUpdate()
    {
        if (!CanControl) return;

        movement.MoveForward();
        steering.Steer(input.Horizontal);

        drift.ApplyDriftPhysics();
    }

    private void HandleEngineSound()
    {
        if (AudioManager.Instance == null) return;

        if (CanControl && !isEnginePlaying)
        {
            AudioManager.Instance.PlayEngineSFX(engineSFXName);
            isEnginePlaying = true;
        }
        else if (!CanControl && isEnginePlaying)
        {
            AudioManager.Instance.StopEngineSFX();
            isEnginePlaying = false;
        }
    }

    private void OnDisable()
    {
        if (AudioManager.Instance != null && isEnginePlaying)
        {
            AudioManager.Instance.StopEngineSFX();
            isEnginePlaying = false;
        }
    }
}