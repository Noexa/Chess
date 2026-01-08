using System.Collections;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
  [SerializeField] private Transform pieceRoot;
  [SerializeField] private RectTransform gridRoot;

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
  private IEnumerator Start()
  {
    // Waits until chess tiles have been fully populated
    while (gridRoot.childCount < BoardSize * BoardSize)
    {
        yield return null;
    }
    PopulateBoard();
  }

  private void PopulateBoard()
  {
    // White Pieces
    SpawnPawns(1, true);
    SpawnBackrow(0, true);

    // Black Pieces
    SpawnPawns(6, false);
    SpawnBackrow(7, false);
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
        SpawnPiece((isWhite? whiteRook : blackRook), row, 0);
        SpawnPiece((isWhite? whiteKnight : blackKnight), row, 1);
        SpawnPiece((isWhite? whiteBishop : blackBishop), row, 2);

        SpawnPiece((isWhite? whiteQueen : blackQueen), row, 3);
        SpawnPiece((isWhite? whiteKing : blackKing), row, 4);

        SpawnPiece((isWhite? whiteBishop : blackBishop), row, 5);
        SpawnPiece((isWhite? whiteKnight : blackKnight), row, 6);
        SpawnPiece((isWhite? whiteRook : blackRook), row, 7);
  }

  private void SpawnPiece(GameObject prefab, int row, int col)
  {
    GameObject piece = Instantiate(prefab, pieceRoot);

    RectTransform pieceRt = piece.GetComponent<RectTransform>();
    RectTransform cellRt = GetCellRect(row, col);

    Vector3 worldCenter = cellRt.TransformPoint(cellRt.rect.center);
    Vector3 localInPieceRoot = pieceRoot.InverseTransformPoint(worldCenter);

    pieceRt.anchorMin = new Vector2(0.5f, 0.5f);
    pieceRt.anchorMax = new Vector2(0.5f, 0.5f);
    pieceRt.pivot = new Vector2(0.5f, 0.5f);

    pieceRt.localPosition = localInPieceRoot;
  }

  private RectTransform GetCellRect(int row, int col)
  {
    int index = (row * BoardSize) + col;
    return (RectTransform)gridRoot.GetChild(index);
  }

}
