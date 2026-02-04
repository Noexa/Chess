using UnityEngine;
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
    [SerializeField] private bool isWhite;

    public PieceType Type => type;
    public bool IsWhite => isWhite;

    public int Row {get; private set;}
    public int Col {get; private set;}

    public void Init(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public void SetGridPos(int row, int col)
    {
        Row = row;
        Col = col;
    }
}