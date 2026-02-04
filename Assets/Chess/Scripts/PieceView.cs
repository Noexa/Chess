using UnityEngine;

public class PieceView : MonoBehaviour
{
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