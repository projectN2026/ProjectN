using UnityEngine;

public class Game : SingletonBehaviour<Game>
{
    [field: SerializeField, DescendantField] 
    public CameraController Camera { get; private set; }
    [field: SerializeField, DescendantField] 
    public ObjectController Object { get; private set; }

    public float RemainingTime { get; private set; } = 60f;


    protected override void Update()
    {
        base.Update();

        RemainingTime -= Time.deltaTime;
    }
}
