using UnityEngine;

public class DriftController : MonoBehaviour
{
    public float driftThershold = 2f;
    public bool isDrifting {  get; private set; }

    float holdTimer = 0f;

    public void HandelDrift(float input)
    {
        if (Mathf.Abs(input) > 0.1f)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= driftThershold)
            {
                isDrifting = true;
            }
        }
        else
        {
            holdTimer = 0f;
            isDrifting = false;
        }
    }

}
