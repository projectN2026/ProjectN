using UnityEngine;

public class ObjectController : BaseBehaviour
{
    [field: SerializeField, ComponentField] public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField, DescendantField] public Collider2D MainCollider { get; private set; }
    [field: SerializeField, DescendantField] public SpriteRenderer MainSprite { get; private set; }
    public float Gravity { get; } = 0.5f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateFall();
    }

    private void UpdateFall()
    {
        Rigidbody.linearVelocityY -= Gravity;
    }
}
