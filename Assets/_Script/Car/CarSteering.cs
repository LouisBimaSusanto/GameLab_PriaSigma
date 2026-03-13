using UnityEngine;

public class CarSteering : MonoBehaviour
{
    public float turnSpeed = 200f;

    public void Steer(float direction)
    {
        transform.Rotate(Vector3.forward * -direction * turnSpeed * Time.deltaTime);
    }
}
