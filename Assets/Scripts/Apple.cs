using UnityEngine;
using System.Collections.Generic;
public class Apple : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] SnakeSpawner snakeSpawner;
    SpriteRenderer sr;

    public void TeleportEmptySpace()
    {
        float step = GameManager.Instance.stepSize;
        float minX = GameManager.Instance.minX;
        float maxX = GameManager.Instance.maxX;
        float minY = GameManager.Instance.minY;
        float maxY = GameManager.Instance.maxY;

        // How many whole cells fit along each axis
        int cellsX = Mathf.FloorToInt((maxX - minX) / step);
        int cellsY = Mathf.FloorToInt((maxY - minY) / step);

        if (cellsX <= 0 || cellsY <= 0) return;


        // Mark occupied cells (grid indices) by the snake
        HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();
        foreach (var part in snakeSpawner.currentSnake.Parts)
        {
            if (part == null) continue;
            Vector2 p = part.transform.position;
            int ix = Mathf.FloorToInt((p.x - minX) / step);
            int iy = Mathf.FloorToInt((p.y - minY) / step);
            if (ix >= 0 && ix < cellsX && iy >= 0 && iy < cellsY)
                occupied.Add(new Vector2Int(ix, iy));
        }

        // Build list of free cells
        List<Vector2Int> freeCells = new List<Vector2Int>(cellsX * cellsY - occupied.Count);
        for (int iy = 0; iy < cellsY; iy++)
        {
            for (int ix = 0; ix < cellsX; ix++)
            {
                var cell = new Vector2Int(ix, iy);
                if (!occupied.Contains(cell))
                    freeCells.Add(cell);
            }
        }

        if (freeCells.Count == 0) return;

        // Choose a random free cell
        Vector2Int chosen = freeCells[Random.Range(0, freeCells.Count)];

        // Convert grid -> world at cell center
        float x = minX + (chosen.x + 0.5f) * step;
        float y = minY + (chosen.y + 0.5f) * step;
        transform.position = new Vector2(x, y);
    }

    void Hide()
    {
        sr.enabled = false;
    }

    void Show()
    {
        sr.enabled = true;
        TeleportEmptySpace();   
    }

    void Awake()
    {
        GameManager.OnGameStart += Show;
        GameManager.OnGameOver += Hide;
    }

    void OnDestroy()
    {
        GameManager.OnGameStart -= Show;
        GameManager.OnGameOver -= Hide;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        TeleportEmptySpace();   
    }
}
