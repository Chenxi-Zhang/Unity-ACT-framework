
using UnityEngine;

public class BaseSfxContainer : MonoBehaviour
{
    public virtual void Destroy(float delay)
    {
        Destroy(gameObject, delay);
    }
}