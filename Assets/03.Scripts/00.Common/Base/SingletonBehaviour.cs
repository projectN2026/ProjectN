using UnityEngine;

public class SingletonBehaviour<T> : BaseBehaviour where T : SingletonBehaviour<T>
{
    private static T s_instance;
    public static T Instance
    {
        get 
        { 
            if (s_instance != null)
                return s_instance; 

            s_instance = FindAnyObjectByType<T>();
            if (s_instance != null)
                return s_instance;

            var go = new GameObject(typeof(T).Name);
            DontDestroyOnLoad(go);
            s_instance = go.AddComponent<T>();

            return s_instance;
        }
    }

    protected override void OnInit()
    {
        base.OnInit();

        if (s_instance == null)
        {
            s_instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
