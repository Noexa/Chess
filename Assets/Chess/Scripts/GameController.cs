using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField] private PieceSpawner pieceSpawner;
    private BoardModel _board;
    private MovementLogic _movementLogic;
    private readonly TileView[,] tiles = new TileView[8, 8];

    private TileView _selectedTile;
    private PieceView _selectedPiece;
    private List<Vector2Int> _validMoves;
    public void RegisterTile(TileView tile)
    {
        tiles[tile.Row, tile.Col] = tile;
    }

    public void OnTileClick(TileView tile)
    {

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
            _selectedPiece = pieceGo.GetComponent<PieceView>();
            _selectedTile = tile;
            _selectedTile.SetHighlight(true);

            _validMoves = _movementLogic.GetValidMoves(_board, _selectedPiece, includeCastling : true);
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

        bool isCastle = _board.IsCastlingMove(piece, fromRow, fromCol, toRow, toCol);

        if (isCastle)
        {
            //FIXME need to implement logic from future ticket to ensure all tiles are not in/through/or would cause a check

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
            _board.MovePiece(fromRow, fromCol, toRow, toCol);
            piece.SetGridPos(toRow, toCol);
            piece.transform.position = destination.transform.position;
        }
    }

    private void ClearSelection()
    {
        if (_selectedTile != null)
        {
            _selectedTile.SetHighlight(false);
        }
        foreach (var move in _validMoves)
        {
            tiles[move.x, move.y].SetHighlight(false);
        }

        _selectedTile = null;
        _selectedPiece = null;
        _validMoves.Clear();
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
    }
}
