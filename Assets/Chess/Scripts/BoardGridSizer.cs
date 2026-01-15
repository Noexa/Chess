using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(GridLayoutGroup))]
public sealed class BoardGridSizer : MonoBehaviour
{
    private const int BoardSize = 8;

    private UnityEngine.UI.GridLayoutGroup _grid;
    private RectTransform _rect;
    private bool _initialized;

    private IEnumerator Start()
    {
        if (_rect == null) yield break;

        while (_rect.rect.width <= 0f || _rect.rect.height <= 0f)
        {
            yield return null;
        }
        ResizeCells();
    }

    private void Awake()
    {
        _grid = GetComponent<UnityEngine.UI.GridLayoutGroup>();
        _rect = GetComponent<RectTransform>();

        ConfigureGrid();
        _initialized = true;
    }

    private void OnRectTransformDimensionsChange()
    {
        if (!_initialized)
        {
            return;
        }

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