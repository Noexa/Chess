using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
  [SerializeField] private Transform pieceRoot;
  [Header("White Pieces")]
  [SerializeField] private GameObject whiteRook;
  [SerializeField] private GameObject whiteKnight;
  [SerializeField] private GameObject whiteBishop;
  [SerializeField] private GameObject whiteKing;
  [SerializeField] private GameObject whiteQueen;
  [SerializeField] private GameObject whitePawn;


  [Header("Black Pieces")]
  [SerializeField] private GameObject blackRook;
  [SerializeField] private GameObject blackKnight;
  [SerializeField] private GameObject blackBishop;
  [SerializeField] private GameObject blackKing;
  [SerializeField] private GameObject blackQueen;
  [SerializeField] private GameObject blackPawn;
private const int BoardSize = 8;
  private void Start()
  {
    PopulateBoard();
  }

  private void PopulateBoard()
  {
    // White Pieces
    SpawnPawns(1, true);
    SpawnBackrow(0, true);

    // Black Pieces
    SpawnPawns(7, false);
    SpawnBackrow(8, false);
  }

  private void SpawnPawns(int row, bool isWhite)
  {
    for (int col = 0; col < BoardSize; col++)
    {
        SpawnPiece((isWhite? whitePawn : blackPawn), row, col);
    }
  }

  private void SpawnBackrow(int row, bool isWhite)
  {
    for (int col = 0; col < BoardSize; col++)
    {
        SpawnPiece((isWhite? whiteRook : blackRook), row, 1);
        SpawnPiece((isWhite? whiteKnight : blackKnight), row, 2);
        SpawnPiece((isWhite? whiteBishop : blackBishop), row, 3);

        SpawnPiece((isWhite? whiteQueen : blackQueen), row, 4);
        SpawnPiece((isWhite? whiteKing : blackKing), row, 5);

        SpawnPiece((isWhite? whiteBishop : blackBishop), row, 6);
        SpawnPiece((isWhite? whiteKnight : blackKnight), row, 7);
        SpawnPiece((isWhite? whiteRook : blackRook), row, 8);
    }
  }

  private void SpawnPiece(GameObject prefab, int row, int col)
  {
    GameObject piece = Instantiate(prefab, pieceRoot);

    piece.transform.localPosition = Vector3.zero; // Needs rework to align to grid/cell
  }
}
