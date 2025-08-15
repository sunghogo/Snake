using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [SerializeField] Snake snake;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
            GameManager.Instance.IncrementScore();
            other.GetComponent<Apple>().TeleportEmptySpace();
            snake.Grow();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.EndGame();
        }
    }
}
