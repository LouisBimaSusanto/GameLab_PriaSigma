using UnityEngine;
public interface ICollectible
{
    void OnCaptured(Transform target);
    void Collect();
}