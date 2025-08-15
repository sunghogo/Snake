using UnityEngine;

public class SnakePart : MonoBehaviour
{
    [field: SerializeField] public Vector2 Direction { get; private set; } = Vector2.up;
    SpriteRenderer sr;
    float stepSize;
    Vector3 originalPosition;

    public void SetDirection(Vector2 newDirection)
    {
        Direction = newDirection;
    }

    public void Move()
    {
        transform.Translate(Direction * stepSize, 0f);
    }

    void Reset()
    {
        transform.position = originalPosition;
        Direction = Vector2.up;
    }
    
    void Hide()
    {
        sr.enabled = false;
    }

    void Show()
    {
        Reset();
        sr.enabled = true;
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        stepSize = sr.bounds.size.x;
        GameManager.Instance.stepSize = stepSize;
        originalPosition = transform.position;

        GameManager.OnGameStart += Show;
        GameManager.OnGameOver += Hide;
    }

    void OnDestroy()
    {
        GameManager.OnGameStart -= Show;
        GameManager.OnGameOver -= Hide;
    }
}
