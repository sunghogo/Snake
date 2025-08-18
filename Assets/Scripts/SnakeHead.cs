using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Snake snake;
    [SerializeField] AudioClip chompClip;
    AudioSource audioSource;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
            audioSource.PlayOneShot(chompClip);
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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


}
