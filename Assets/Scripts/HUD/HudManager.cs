
using UnityEngine;

class HudManager : MonoBehaviour
{
    public static HudManager Instance { get; private set; }

    public Canvas hudCanvas;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("HudManager already exists, destroying this instance.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}