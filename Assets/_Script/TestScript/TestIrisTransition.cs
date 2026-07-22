// IrisTest.cs — hapus setelah test
using UnityEngine;

public class TestIrisTransition : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            IrisTransition.Instance.PlayTransition(
                onMidpoint: () => Debug.Log("Midpoint! — taruh UI change di sini"),
                onComplete: () => Debug.Log("Transition selesai!")
            );
        }
    }
}