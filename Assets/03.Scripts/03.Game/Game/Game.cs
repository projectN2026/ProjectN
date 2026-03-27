using UnityEngine;

public class Game : SingletonBehaviour<Game>
{
    [field: SerializeField, DescendantField] 
    public CameraController Camera { get; private set; }
    [field: SerializeField, DescendantField] 
    public CharacterController Character { get; private set; }
}
