using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PieceSpawner pieceSpawner;
    private BoardModel board;

    private TileView _selectedTile;

    public void OnTileClick(TileView tile)
    {

        if (_selectedTile == tile)
        {
            _selectedTile.SetHighlight(false);
            _selectedTile = null;
            return;
        }

        if (_selectedTile != null)
        {
            _selectedTile.SetHighlight(false);
        }

        _selectedTile = tile;
        _selectedTile.SetHighlight(true);
    }

    void Awake()
    {
        board = new BoardModel();
    }

    void Start()
    {
        StartCoroutine(pieceSpawner.SpawnRoutine(board));
    }
}
