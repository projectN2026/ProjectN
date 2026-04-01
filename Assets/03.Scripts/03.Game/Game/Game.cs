using System;
using UnityEngine;

public class Game : SingletonBehaviour<Game>
{
    [field: SerializeField, DescendantField] 
    public CameraController Camera { get; private set; }


    public float RemainingTime { get; private set; } = 60f;

    protected override void Start()
    {
        base.Start();

        ObjectManager.Instance.SummonPlayer(new Vector3(0f, 0f));
        ObjectManager.Instance.SummonEnemy(new Vector3(-10f, 0f));
    }
    protected override void Update()
    {
        base.Update();

        RemainingTime -= Time.deltaTime;
    }
}
