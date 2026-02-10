using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PieceSpawner pieceSpawner;
    private BoardModel board;
    private readonly TileView[,] tiles = new TileView[8, 8];

    private TileView _selectedTile;
    public void RegisterTile(TileView tile)
    {
        tiles[tile.Row, tile.Col] = tile;
    }

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
