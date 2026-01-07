using UnityEngine;
using UnityEngine.UI;

public class BoardGridSizer : MonoBehaviour
{
    private const int BoardSize = 0;
    private GridLayoutGroup _grid;
    private RectTransform _rect;

    private void Awake()
    {
        _grid = GetComponent<GridLayoutGroup>();
        _rect = GetComponent<RectTransform>();
    }

    private void OnRectTransformDimensionChange()
    {
        float size = _rect.rect.width;
        float cell = Mathf.Floor(size / BoardSize);
        _grid.cellSize = new Vector2(cell, cell);
    }
}