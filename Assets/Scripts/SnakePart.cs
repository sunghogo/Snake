using UnityEngine;

public class SnakePart : MonoBehaviour
{
    [Header("Properties")]
    [field: SerializeField] public Vector2 Direction { get; private set; } = Vector2.up;
    SpriteRenderer sr;
    float stepSize;
    Vector3 originalPosition;

    public void SetDirection(Vector2 newDirection)
    {
        Direction = newDirection;
    }

    public void Rotate()
    {
        if (Direction == Vector2.up) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (Direction == Vector2.left) transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        else if (Direction == Vector2.down) transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        else if (Direction == Vector2.right) transform.rotation = Quaternion.Euler(0f, 0f, 270f);
    }

    public void Move()
    {
        Rotate();
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
        originalPosition = transform.position;

        GameManager.OnGameStart += Show;
        GameManager.OnGameOver += Hide;
    }

    void Start()
    {
        stepSize = GameManager.Instance.stepSize;
    }

    void OnDestroy()
    {
        GameManager.OnGameStart -= Show;
        GameManager.OnGameOver -= Hide;
    }
}
