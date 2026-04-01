using UnityEngine;

public class ObjectManager : SingletonBehaviour<ObjectManager>
{
    public PlayerController Player { get; private set; }

    public void SummonPlayer(Vector2 position)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Player");
        var go = Instantiate(prefab);
        go.transform.position = position;
        Player = go.GetComponent<PlayerController>();
    }

    public void SummonEnemy(Vector2 position)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Enemy");
        var go = Instantiate(prefab);
        go.transform.position = position;
    }
}
