using UnityEngine;

public class PlayerController : ObjectController
{
    public float MoveSpeed { get; } = 6f;
    public float JumpPower { get; } = 10f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateMove();
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
        var r = 0.2f;
        var layer = 1 << LayerMask.NameToLayer("Ground");
        var result = Physics2D.OverlapCircle(p, r, layer) != null;
        return result;
    }
}
