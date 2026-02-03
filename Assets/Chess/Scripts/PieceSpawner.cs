using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PieceSpawner : MonoBehaviour
{
  [SerializeField] private RectTransform gridRoot;
  
  [Range(0.6f, 1.2f)]
  [SerializeField] private float pieceScale = 0.90f;

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

    Canvas.ForceUpdateCanvases();
    LayoutRebuilder.ForceRebuildLayoutImmediate(gridRoot);
    PopulateBoard();
  }

  private void PopulateBoard()
  {
    bool white = true;
    bool black = false;

    // White Pieces
    SpawnPawns(1, white);
    SpawnBackrow(0, white);

    // Black Pieces
    SpawnPawns(6, black);
    SpawnBackrow(7, black);
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
    RectTransform cellRt = GetCellRect(row, col);

    GameObject piece = Instantiate(prefab);
    RectTransform pieceRt = piece.GetComponent<RectTransform>();

    pieceRt.SetParent(cellRt, false);
  
    // Centers in cell
    pieceRt.anchorMin = new Vector2(0.5f, 0.5f);
    pieceRt.anchorMax = new Vector2(0.5f, 0.5f);
    pieceRt.pivot     = new Vector2(0.5f, 0.5f);
    pieceRt.anchoredPosition = Vector2.zero;

    pieceRt.localScale = Vector3.one * 0.85f;
    // Sizes piece relative to cell size
    float side = Mathf.Min(cellRt.rect.width, cellRt.rect.height) * pieceScale;
    pieceRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, side);
    pieceRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, side);

    pieceRt.SetAsLastSibling();
  }

  private RectTransform GetCellRect(int row, int col)
  {
    int index = (row * BoardSize) + col;
    return (RectTransform)gridRoot.GetChild(index);
  }

}