using UnityEngine;

public class BoardModel
{
    private GameObject[,] squares = new GameObject[8,8];

    public void SetPiece(int row, int col, GameObject piece)
    {
        squares[row, col] = piece;
    }

    public GameObject GetPiece(int row, int col)
    {
        return squares[row, col];
    }

    public bool IsOccupied(int row, int col)
    {
        return squares[row, col] != null;
    }

    public bool IsInBounds(int row, int col)
    {
        return (row >=0 && row <8 && col >=0 && col <8);
    }

    public void ClearSquare(int row, int col)
    {
        squares[row, col] = null;
    }
}
