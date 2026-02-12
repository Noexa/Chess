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
  public IEnumerator SpawnRoutine(BoardModel board)
  {
    // Waits until chess tiles have been fully populated
    while (gridRoot.childCount < BoardSize * BoardSize)
    {
        yield return null;
    }

    Canvas.ForceUpdateCanvases();
    LayoutRebuilder.ForceRebuildLayoutImmediate(gridRoot);
    PopulateBoard(board);
  }

  private void PopulateBoard(BoardModel board)
  {
    bool white = true;
    bool black = false;

    // White Pieces
    SpawnPawns(1, white, board);
    SpawnBackrow(0, white, board);

    // Black Pieces
    SpawnPawns(6, black, board);
    SpawnBackrow(7, black, board);
  }

  private void SpawnPawns(int row, bool isWhite, BoardModel board)
  {
    for (int col = 0; col < BoardSize; col++)
    {
      SpawnPiece(GetPrefab(isWhite, whitePawn, blackPawn), row, col, isWhite, board);
    }
  }

  private void SpawnBackrow(int row, bool isWhite, BoardModel board)
    {
        SpawnPiece(GetPrefab(isWhite, whiteRook,   blackRook),   row, 0, isWhite, board);
        SpawnPiece(GetPrefab(isWhite, whiteKnight, blackKnight), row, 1, isWhite, board);
        SpawnPiece(GetPrefab(isWhite, whiteBishop, blackBishop), row, 2, isWhite, board);
        SpawnPiece(GetPrefab(isWhite, whiteQueen,  blackQueen),  row, 3, isWhite, board);
        SpawnPiece(GetPrefab(isWhite, whiteKing,   blackKing),   row, 4, isWhite, board);
        SpawnPiece(GetPrefab(isWhite, whiteBishop, blackBishop), row, 5, isWhite, board);
        SpawnPiece(GetPrefab(isWhite, whiteKnight, blackKnight), row, 6, isWhite, board);
        SpawnPiece(GetPrefab(isWhite, whiteRook,   blackRook),   row, 7, isWhite, board);
    }

  private void SpawnPiece(GameObject prefab, int row, int col, bool isWhite, BoardModel board)
  {
    RectTransform cellRt = GetCellRect(row, col);

    GameObject piece = Instantiate(prefab);
    RectTransform pieceRt = piece.GetComponent<RectTransform>();

    pieceRt.SetParent(cellRt, false);

    board.SetPiece(row, col, piece);
  
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

    // Assign board-state
    PieceView view = piece.GetComponent<PieceView>();
    Debug.Assert(view != null, "Piece prefab is missing PieceView component");

    view.Init(row, col, isWhite);
  }

  private RectTransform GetCellRect(int row, int col)
  {
    int index = (row * BoardSize) + col;
    return (RectTransform)gridRoot.GetChild(index); //GetChild returns a transform so a rect transform cast is needed
  }

  private GameObject GetPrefab(bool isWhite, GameObject whitePrefab, GameObject blackPrefab)
  {
    return isWhite ? whitePrefab : blackPrefab;
  }
}