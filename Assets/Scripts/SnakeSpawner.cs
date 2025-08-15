using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Snake snakePrefab;
    public Snake currentSnake;
    Vector3 originalSnakePosition;
    Vector2 originalDirection;

    void Awake()
    {
        GameManager.OnGameStart += HandleGameStart;
    }

    void OnDestroy()
    {
        GameManager.OnGameStart -= HandleGameStart;
    }

    void Start()
    {
        currentSnake = GetComponentInChildren<Snake>();
        originalSnakePosition = currentSnake.transform.position;
        originalDirection = currentSnake.Direction;
    }

    void HandleGameStart()
    {
        currentSnake = Instantiate(snakePrefab, originalSnakePosition, Quaternion.identity, transform);
        currentSnake.SetDirection(originalDirection);
    }
}
