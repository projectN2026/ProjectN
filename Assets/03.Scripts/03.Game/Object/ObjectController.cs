using UnityEngine;

public class ObjectController : BaseBehaviour
{
    [field: SerializeField, ComponentField] public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField, DescendantField] public Collider2D MainCollider { get; private set; }
    [field: SerializeField, DescendantField] public SpriteRenderer MainSprite { get; private set; }
}
