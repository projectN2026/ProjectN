using UnityEngine;

public class CharacterController : BaseBehaviour
{
    [field: SerializeField, ComponentField] public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField, DescendantField] public Collider2D MainCollider { get; private set; }
    [field: SerializeField, DescendantField] public SpriteRenderer MainSprite { get; private set; }
    public float MoveSpeed { get; } = 6f;
    public float JumpPower { get; } = 10f;
    public float Gravity { get; } = 0.5f;
    public bool IsGround { get; private set; }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        IsGround = true;
    }
    protected override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
        IsGround = false;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateMove();
        UpdateFall();
    }
    protected override void Update()
    {
        base.Update();
        UpdateJump();
    }

    private void UpdateMove()
    {
        var input = 0f;
        if (Input.GetKey(KeyCode.A)) input--;
        if (Input.GetKey(KeyCode.D)) input++;

        Rigidbody.linearVelocityX = input * MoveSpeed;
    }
    private void UpdateFall()
    {
        Rigidbody.linearVelocityY -= Gravity;
    }
    private void UpdateJump()
    {
        if (CheckGround() && Input.GetKeyDown(KeyCode.Space))
            Rigidbody.linearVelocityY = JumpPower;
    }
    private bool CheckGround()
    {
        var x = MainCollider.bounds.center.x;
        var y = MainCollider.bounds.min.y;
        var p = new Vector2(x, y);
        var r = 0.5f;
        var layer = 1 << LayerMask.NameToLayer("Ground");
        var result = Physics2D.OverlapCircle(p, r, layer) != null;
        return result;
    }
}
