using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public bool JumpPressed { get; private set; }

    private float mobileHorizontal;
    private bool mobileJump;

    private void Update()
    {
        float pcHorizontal = Input.GetAxisRaw("Horizontal");

        Horizontal = (mobileHorizontal != 0) ? mobileHorizontal : pcHorizontal;

        JumpPressed = Input.GetKeyDown(KeyCode.Space) || mobileJump;

        if (mobileJump)
        {
            mobileJump = false;
        }
    }

    public void PointerDownLeft() => mobileHorizontal = -1f;

    public void PointerDownRight() => mobileHorizontal = 1f;

    public void PointerUpHorizontal() => mobileHorizontal = 0f;

    public void PointerDownJump() => mobileJump = true;
}