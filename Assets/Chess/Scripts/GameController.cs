using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PieceSpawner pieceSpawner;
    private BoardModel board;

    void Awake()
    {
        board = new BoardModel();
    }

    void Start()
    {
        StartCoroutine(pieceSpawner.SpawnRoutine(board));
    }
}
