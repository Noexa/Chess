using UnityEngine;
using UnityEngine.UI;

public class ChessBoardUI : MonoBehaviour
{

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Color lightColor;
    [SerializeField] private Color darkColor;
    private const int BoardSize = 8; // Chess boards are 8x8


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
                Image image = tile.GetComponent<Image>();
                // bool isLight = (row+col) % 2 == 0;
                // image.color = isLight? lightColor : darkColor;
                // As of 1/8/26 color is antiquated. Board sprite is now used, but this will be left in for debugging

            }
        }

    }

}
