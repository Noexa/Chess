using UnityEngine;
using UnityEngine.UI;
public enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}
public class PieceView : MonoBehaviour
{
    [SerializeField] private PieceType type;
     private bool isWhite;

    public PieceType Type => type;
    public bool IsWhite {get; private set;}

    public int Row {get; private set;}
    public int Col {get; private set;}

    public void Init(int row, int col, bool isWhite)
    {
        Row = row;
        Col = col;
        IsWhite = isWhite;

        Image img = GetComponentInChildren<Image>();
        Debug.Log($"Piece at ({row},{col}) isWhite={isWhite}, Image color={img?.color}");
    }
    public void SetGridPos(int row, int col)
    {
        Row = row;
        Col = col;
    }
}