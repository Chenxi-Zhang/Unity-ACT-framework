
using UnityEngine;

public class SingletonHolder : MonoBehaviour
{

    private static SingletonHolder _instance;
    public static SingletonHolder Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SingletonHolder>();
                if (_instance == null)
                {
                    GameObject singletonObject = new ("SingletonHolder");
                    _instance = singletonObject.AddComponent<SingletonHolder>();
                }
            }
            return _instance;
        }
    }

    public HitCounterConfig hitCounterConfig;
    public ShockConfig shockConfig;
    public BuffConfig buffConfig;

}