using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TileView : MonoBehaviour, IPointerClickHandler
{
    public int Row {get; private set;}
    public int Col {get; private set;}

    [SerializeField] private Image highlightImage;
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
        SetHighlight(false);
    }

    public void Init(int row, int col, GameController gameController)
    {
        Row = row;
        Col = col;
        controller = gameController;
        ClearHighlight();
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

    public void ClearHighlight()
    {
        if (highlightImage != null)
        {
            highlightImage.enabled = false;
        }
    }

}
