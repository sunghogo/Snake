using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SnakePart bodyPrefab;
    [SerializeField] SnakePart tailPrefab;
    [field: SerializeField] public List<SnakePart> Parts { get; private set; } = new List<SnakePart>();

    [Header("Properties")]
    [SerializeField] float movesPerSecond = 5f;
    [field: SerializeField] public Vector2 Direction { get; private set; } =  Vector2.up;
    [field: SerializeField] public Vector2 NextDirection { get; private set; } =  Vector2.up;
    float moveDuration;
    float time = 0f;
    bool isGrowing = false;
    Vector3 originalPosition;

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) && Direction != Vector2.down)
        {
            NextDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Direction != Vector2.up)
        {
            NextDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Direction != Vector2.right)
        {
            NextDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Direction != Vector2.left)
        {
            NextDirection = Vector2.right;
        }
    }

    void GetAllSnakeParts()
    {
        Parts.AddRange(GetComponentsInChildren<SnakePart>(true));
    }

    void MoveAllParts()
    {
        Vector3 tailPosition = Parts[Parts.Count - 1].gameObject.transform.position;
        Vector3 tailDirection = Parts[Parts.Count - 1].Direction;
        Quaternion tailRotation = Parts[Parts.Count - 1].gameObject.transform.rotation;
        
        Vector2 directionForNextPart = Direction;
        Vector2 directionForCurrentPart = Direction;
        foreach (SnakePart part in Parts)
        {
            directionForNextPart = part.Direction;
            part.SetDirection(directionForCurrentPart);
            part.Move();
            directionForCurrentPart = directionForNextPart;
        }

        if (isGrowing)
        {
            SnakePart oldTail = Parts[Parts.Count - 1];
            Parts.Remove(oldTail);
            SnakePart newBody = Instantiate(bodyPrefab, oldTail.gameObject.transform.position, oldTail.gameObject.transform.rotation, transform);
            newBody.SetDirection(oldTail.Direction);
            Parts.Add(newBody);
            Destroy(oldTail.gameObject);

            SnakePart newTail = Instantiate(tailPrefab, tailPosition, tailRotation, transform);
            newTail.SetDirection(tailDirection);
            Parts.Add(newTail);
            isGrowing = false;
        }
    }

    public void Grow()
    {
        isGrowing = true;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }


    void Awake()
    {
        originalPosition = transform.position;
        GameManager.OnGameOver += HandleGameOver;
    }

    void OnDestroy()
    {
        GameManager.OnGameOver -= HandleGameOver;
    }

    void HandleGameOver()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        GetAllSnakeParts();
        moveDuration = 1 / movesPerSecond;
    }

    void Update()
    {
        if (GameManager.Instance.GameOver) return;

        ProcessInput();
        if (time >= moveDuration)
        {
            Direction = NextDirection;
            MoveAllParts();
            time = 0;
        }
        time += Time.deltaTime;
    }
}
