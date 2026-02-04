using UnityEngine;
using unityEngine.EventSystems;
using UnityEngine.UI;

//TODO: 
// Add ClearHighlight & fix highlightImage functionality
public class TileView : MonoBehaviour, IPointerClickHandler
{
    public int Row {get; private set;}
    public int Col {get; private set;}

    [SerializeField] private Image highlightImage; // BUGFIX: better way to highlight? Manually inserting this seems tedious
    private GameController controller;

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

    public void SetHighlight(Color color)
    {
        if (highlghtImage != null)
        {
            highlightImage.enabled = true;
            highlightImage.color= color;
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
