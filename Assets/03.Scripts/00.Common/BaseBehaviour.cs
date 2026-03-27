using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    protected bool IsInit { get; private set; }

    protected void Awake() => TryInit();
    protected void OnDestroy() => OnClean();
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void LateUpdate() { }
    protected virtual void FixedUpdate() { }
    protected virtual void OnTriggerEnter2D(Collider2D collision) { }
    protected virtual void OnCollisionEnter2D(Collision2D collision) { }

    protected void TryInit()
    {
        if (!IsInit) OnInit();
    }
    protected virtual void OnInit()
    {
        IsInit = true;
    }
    protected virtual void OnClean() 
    {
      
    }
}
