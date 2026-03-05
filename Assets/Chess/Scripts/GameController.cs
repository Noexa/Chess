using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PieceSpawner pieceSpawner;
    [SerializeField] private UIMessagePopup messagePopup;
    [SerializeField] private PromotionUI promotionUI;
    private bool _awaitingPromotion;
    private int _promoRow;
    private int _promoCol;
    private bool _promoIsWhite;
    private BoardModel _board;
    private bool _whiteTurn = true;
    private MovementLogic _movementLogic;
    private readonly TileView[,] tiles = new TileView[8, 8];
    private bool _gameOver = false;

    private TileView _selectedTile;
    private PieceView _selectedPiece;
    private List<Vector2Int> _validMoves;
    public void RegisterTile(TileView tile)
    {
        tiles[tile.Row, tile.Col] = tile;
    }

    public void OnTileClick(TileView tile)
    {
        if (_gameOver || _awaitingPromotion) return;

        // Deselects if selecting current tile
        if (_selectedTile == tile)
        {
            ClearSelection();
            return;
        }

        if (_selectedPiece != null && _validMoves.Contains(new Vector2Int(tile.Row, tile.Col)))
        {
            ExecuteMove(_selectedPiece, tile);
            ClearSelection();
            return;
        }

        ClearSelection();

        GameObject pieceGo = _board.GetPiece(tile.Row, tile.Col);
        if (pieceGo != null)
        {
            PieceView candidate = pieceGo.GetComponent<PieceView>();
            if (candidate.IsWhite != _whiteTurn)
            {
                return;
            }
            _selectedPiece = candidate;
            _selectedTile = tile;
            _selectedTile.SetHighlight(true);

            _validMoves = _movementLogic.GetValidMoves(_board, _selectedPiece, includeCastling : true);

            if (!string.IsNullOrEmpty(_movementLogic.LastMessage))
            {
                messagePopup.Show(_movementLogic.LastMessage);
            }
            foreach (var move in _validMoves)
            {
                tiles[move.x, move.y].SetHighlight(true);
            }
        }
    }

    private void ExecuteMove(PieceView piece, TileView destination)
    {
        int fromRow = piece.Row;
        int fromCol = piece.Col;
        int toRow = destination.Row;
        int toCol = destination.Col;

        GameObject targetGo = _board.GetPiece(toRow, toCol);
        if (targetGo != null)
        {
            PieceView target = targetGo.GetComponent<PieceView>();
            if (target != null && target.Type == PieceType.King && target.IsWhite != piece.IsWhite)
            {
                messagePopup.Show($"Game over! {(piece.IsWhite ? "White" : "Black")} wins! King captured.");
                _gameOver = true;
            }
        }

        bool isCastle = _board.IsCastlingMove(piece, fromRow, fromCol, toRow, toCol);

        if (isCastle)
        {
            int rookFromCol = (toCol > fromCol) ? 7 : 0;
            int rookToCol = (toCol > fromCol) ? (toCol - 1) : (toCol + 1);

            GameObject rookGo = _board.GetPiece(fromRow, rookFromCol);

            // Move king
            _board.MovePiece(fromRow, fromCol, toRow, toCol);
            piece.SetGridPos(toRow, toCol);
            piece.transform.position = destination.transform.position;

            // Move rook
            _board.MovePiece(fromRow, rookFromCol, fromRow, rookToCol);
            PieceView rook = rookGo.GetComponent<PieceView>();
            TileView rookDestTile = tiles[fromRow, rookToCol];
            rook.SetGridPos(fromRow, rookToCol);
            rook.transform.position = rookDestTile.transform.position;
        }
        else
        {
            // En passant
            bool isEnPassant = IsEnPassantMove(piece, fromRow, fromCol, toRow, toCol);
            if (isEnPassant)
            {
                ExecuteEnPassantCapture(fromRow, toCol);
            }
            // Normal move
            _board.MovePiece(fromRow, fromCol, toRow, toCol);
            piece.SetGridPos(toRow, toCol);
            piece.transform.position = destination.transform.position;

            // Pawn Promotion
            if (piece.Type == PieceType.Pawn)
            {
                int promoRow = piece.IsWhite ? 7 : 0;
                if (toRow == promoRow)
                {
                    BeginPromotion(toRow, toCol, piece.IsWhite);
                    return;
                }
            }
        }
        UpdateEnPassantState(piece, fromRow, fromCol, toRow);
        _whiteTurn = !_whiteTurn;
    }

    private void BeginPromotion(int row, int col, bool isWhite)
    {
        _awaitingPromotion = true;
        _promoRow = row;
        _promoCol = col;
        _promoIsWhite = isWhite;

        promotionUI.Show();
    }

    private void OnPromotionPicked(PieceType type)
    {
        promotionUI.Hide();

        GameObject pawnGo = _board.GetPiece(_promoRow, _promoCol);
        if (pawnGo != null)
        {
            _board.ClearSquare(_promoRow, _promoCol);
            Destroy(pawnGo);
        }

        pieceSpawner.SpawnPromotedPiece(_promoRow, _promoCol, _promoIsWhite, type, _board);

        _awaitingPromotion = false;
        _whiteTurn = !_whiteTurn;
    }

    private bool IsEnPassantMove(PieceView pawn, int fromRow, int fromCol, int toRow, int toCol)
    {
        if (pawn.Type != PieceType.Pawn) return false;

        if (!_board.TryGetEnPassant(out int epRow, out int epCol)) return false;

        bool isEpTarget = (toRow == epRow && toCol == epCol);
        bool isDiagonalStep = Mathf.Abs(toCol - fromCol) == 1;

        return isEpTarget && isDiagonalStep && !_board.IsOccupied(toRow, toCol);
    }

    private void ExecuteEnPassantCapture(int fromRow, int fromCol)
    {
        int capturedRow = fromRow;
        int capturedCol = fromCol;

        GameObject capturedGo = _board.GetPiece(capturedRow, capturedCol);
        if (capturedGo != null)
        {
            _board.ClearSquare(capturedRow, capturedCol);
            Destroy(capturedGo);
        }
    }

    private void UpdateEnPassantState(PieceView piece, int fromRow, int fromCol, int toRow)
    {
        _board.ClearEnPassant();
        if (piece.Type == PieceType.Pawn && Mathf.Abs(toRow - fromRow) == 2 )
        {
            int midRow = (fromRow + toRow ) / 2;
            _board.SetEnPassant(midRow, fromCol);
        }
    }
    private void ClearSelection()
    {
        if (_selectedTile != null)
        {
            _selectedTile.SetHighlight(false);
        }
        if (_validMoves != null)
        {
            foreach (var move in _validMoves)
            {
                if (tiles[move.x, move.y] != null)
                {
                    tiles[move.x, move.y].SetHighlight(false);
                }
            }
            _validMoves.Clear();
        }
        _selectedTile = null;
        _selectedPiece = null;
    }
    public void Forfeit()
    {
        if (_gameOver) return;

        string winner = _whiteTurn ? "Black" : "White";
        messagePopup.Show($"{winner} wins by forfeit!");
        _gameOver = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Awake()
    {
        _board = new BoardModel();
        _movementLogic = new MovementLogic();
        _validMoves = new List<Vector2Int>(); 
    }

    void Start()
    {
        StartCoroutine(pieceSpawner.SpawnRoutine(_board));
        promotionUI.OnPick = OnPromotionPicked;
    }   
}