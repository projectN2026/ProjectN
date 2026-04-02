using UnityEngine;

public class ObjectManager : SingletonBehaviour<ObjectManager>
{
    private PlayerController _player;
    public static PlayerController Player 
    {
        get => Instance._player;
        private set => Instance._player = value;
    }

    public static void SummonPlayer(Vector2 position)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Object/Player");
        var go = Instantiate(prefab);
        go.transform.position = position;
        Player = go.GetComponent<PlayerController>();
    }

    public static void SummonEnemy(Vector2 position)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Object/Enemy");
        var go = Instantiate(prefab);
        go.transform.position = position;
    }
}
