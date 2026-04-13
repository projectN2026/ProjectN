using UnityEngine;

public class ObjectManager 
{
    private PlayerController _player;
    public PlayerController Player 
    {
        get => _player;
        private set => _player = value;
    }

    public void SummonPlayer(Vector2 position)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Object/Player");
        var go = GameObject.Instantiate(prefab);
        go.transform.position = position;
        Player = go.GetComponent<PlayerController>();
    }

    public static void SummonEnemy(Vector2 position)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Object/Enemy");
        var go = GameObject.Instantiate(prefab);
        go.transform.position = position;
    }
}
