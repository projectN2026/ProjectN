using System;
using System.Collections.Generic;
using UnityEngine;

public class Managers 
{
    private static Managers s_instance;
    private static Managers Instance => s_instance ?? (s_instance = new Managers());

    private Dictionary<Type, object> _subInstances = new Dictionary<Type, object>();
    private static Dictionary<Type, object> SubInstances => Instance._subInstances;

    public static PopupManager PopupManager => Get<PopupManager>();
    public static ObjectManager ObjectManager => Get<ObjectManager>();
    public static WaveUpdater WaveUpdater => Get<WaveUpdater>();

    private static T Get<T>() where T : class, new()
    {
        if (SubInstances.TryGetValue(typeof(T), out var cached))
            return cached as T;

        if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
        {
            var find = GameObject.FindAnyObjectByType(typeof(T)) as T;
            if (find != null)
            {
                SubInstances.Add(typeof(T), find);
                return find;
            }
            else
            {
                var go = new GameObject(typeof(T).Name);
                var com = go.AddComponent(typeof(T)) as T;
                SubInstances.Add(typeof(T), com);         
                return com;
            }
        }
        else
        {
            var newInstance = new T();
            SubInstances.Add(typeof(T), newInstance);
            return newInstance;
        }
    }
}
