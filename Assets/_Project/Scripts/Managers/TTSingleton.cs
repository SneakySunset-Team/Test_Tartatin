using Sirenix.OdinInspector;
using UnityEngine;

public class TTSingleton<T> : SerializedMonoBehaviour where T : TTSingleton<T>
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (Application.isPlaying) return _instance;
            else return FindFirstObjectByType<T>();
        }
        private set
        {
            if (_instance != null)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already exists, destroying object!");
                Destroy(Instance);
            }
            else
            {
                _instance = value;
            }
        }
    }

    protected virtual void Awake()
    {
        Instance = this as T;
    }
}
