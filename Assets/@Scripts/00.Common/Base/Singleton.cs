using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{
    private static T s_instance;
    protected static T Instance
    {
        get
        {
            if (s_instance != null)
                return s_instance;

            s_instance = new T();
            return s_instance;
        }
    }
}
