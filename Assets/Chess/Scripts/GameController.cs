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
            _selectedTile.SetHighlight(false);
            _selectedTile = null;
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

            _validMoves = _movementLogic.GetValidMoves(_board, _selectedPiece);
            foreach (var move in _validMoves)
            {
                tiles[move.x, move.y].SetHighlight(true);
            }
        }
    }

    private void ExecuteMove(PieceView piece, TileView destination)
    {
        _board.MovePiece(piece.Row, piece.Col, destination.Row, destination.Col);
        piece.SetGridPos(destination.Row, destination.Col);
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
