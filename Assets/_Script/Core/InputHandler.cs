using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float Horizontal {  get; private set; }
    public bool JumpPressed { get; private set; }

    private void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        JumpPressed = Input.GetKeyDown(KeyCode.Space);
    }
}
