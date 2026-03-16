using UnityEngine;

public class CarController : MonoBehaviour
{
    InputHandler input;
    CarMovement movement;
    CarSteering steering;
    DriftController drift;
    CarJump jump;

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
        drift.HandelDrift(input.Horizontal);

        if (drift.isDrifting)
        {
            steering.turnSpeed = 350f;
        }
        else
        {
            steering.turnSpeed = 200f;
        }

        if (input.JumpPressed)
        {
            jump.Jump();
        }
    }

    private void FixedUpdate()
    {
        movement.MoveForward();
        steering.Steer(input.Horizontal);
    }
}
