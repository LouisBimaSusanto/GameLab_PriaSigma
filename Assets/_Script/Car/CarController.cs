using UnityEngine;

public class CarController : MonoBehaviour
{
    InputHandler input;
    CarMovement movement;
    CarSteering steering;
    DriftController drift;

    private void Awake()
    {
        input = GetComponent<InputHandler>();
        movement = GetComponent<CarMovement>();
        steering = GetComponent<CarSteering>();
        drift = GetComponent<DriftController>();
    }

    private void Update()
    {
        drift.HandelDrift(input.Horizontal);

        steering.Steer(input.Horizontal);

        if (drift.isDrifting)
        {
            steering.turnSpeed = 350f;
        }
        else
        {
            steering.turnSpeed = 200f;
        }
    }

    private void FixedUpdate()
    {
        movement.MoveForward();
    }
}
