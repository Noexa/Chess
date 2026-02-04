using UnityEngine;
using UnityEngine.UI;

public class ChessBoardUI : MonoBehaviour
{

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameController gameController;

    private const int BoardSize = 8; // Chess boards are 8x8
    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                GameObject tile = Instantiate(tilePrefab, transform);
                TileView tv = tile.GetComponent<TileView>();

                Debug.Assert(tv != null, "Tile prefab is missing TileView component");
                Debug.Assert(gameController != null, "Chessboard UI is missing GameController reference");
                tv.Init(row, col, gameController);

            }
        }
    }
}
