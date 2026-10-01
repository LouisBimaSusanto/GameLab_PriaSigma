using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public bool JumpPressed { get; private set; }

    [Header("Mobile UI References")]
    public VirtualJoystick steeringJoystick; 

    private bool mobileJump;

    private void Update()
    {
        float pcHorizontal = Input.GetAxisRaw("Horizontal");

        float joyHorizontal = steeringJoystick != null ? steeringJoystick.InputDirection.x : 0f;

        Horizontal = (Mathf.Abs(joyHorizontal) > 0.05f) ? joyHorizontal : pcHorizontal;

        JumpPressed = Input.GetKeyDown(KeyCode.Space) || mobileJump;

        if (mobileJump)
        {
            mobileJump = false;
        }
    }

    public void PointerDownJump() => mobileJump = true;
}