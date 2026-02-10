using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TileView : MonoBehaviour, IPointerClickHandler
{
    public int Row {get; private set;}
    public int Col {get; private set;}

    [SerializeField] private Image highlightImage;
    [SerializeField] private Image moveHighlightImage;
    private GameController controller;
    private void Awake()
    {
        if (highlightImage == null)
        {
            Transform t = transform.Find("Highlight");
            if (t != null)
            {
                highlightImage = t.GetComponent<Image>();
            }
        }
        if (moveHighlightImage == null)
        {
            Transform t = transform.Find("MoveHighlight");
            if (t != null)
            {
                moveHighlightImage = t.GetComponent<Image>();
            }
        }
        ClearAllHighlights();
    }

    public void Init(int row, int col, GameController gameController)
    {
        Row = row;
        Col = col;
        controller = gameController;

        controller.RegisterTile(this);
        ClearAllHighlights();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller != null)
        {
            controller.OnTileClick(this);
        }
    }

    public void SetHighlight(bool on)
    {
        if (highlightImage == null)
        {
            return;
        }

        if (highlightImage != null)
        {
            highlightImage.enabled = on;
        }
    }

    public void SetMoveHighlight(bool on)
    {
        if (moveHighlightImage != null)
        {
            moveHighlightImage.enabled = on;
        }
    }

    public void ClearAllHighlights()
    {
        if (highlightImage != null)
        {
            highlightImage.enabled = false;
        }
        if (moveHighlightImage != null)
        {
            moveHighlightImage.enabled = false;
        }
    }

}
