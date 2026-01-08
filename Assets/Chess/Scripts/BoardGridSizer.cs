using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public sealed class BoardGridSizer : MonoBehaviour
{
    private const int BoardSize = 8;

    private GridLayoutGroup _grid;
    private RectTransform _rect;

    private void Awake()
    {
        _grid = GetComponent<GridLayoutGroup>();
        _rect = GetComponent<RectTransform>();

        ConfigureGrid();
        ResizeCells();
    }

    private void OnRectTransformDeimensionsChange()
    {
        ResizeCells();
    }

    private void ConfigureGrid()
    {
        _grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _grid.constraintCount = BoardSize;
        _grid.spacing = Vector2.zero;
        _grid.padding = new RectOffset();
    }

    private void ResizeCells()
    {
        if (_rect.rect.width <= 0f || _rect.rect.height <= 0f) // Unity may call function with invalid size even outside of program start. Do not move
        {
            return;
        }

        float boardSize = Mathf.Min(_rect.rect.width, _rect.rect.height);
        float cellSize = Mathf.Floor( boardSize / BoardSize);

        _grid.cellSize = new Vector2(cellSize, cellSize);
    }
}