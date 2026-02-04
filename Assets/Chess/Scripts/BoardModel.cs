using UnityEngine;

public class BoardModel
{
    private GameObject[,] squares = new GameObject[8,8];

    public void SetPiece(int row, int col, GameObject piece)
    {
        squares[row, col] = piece;
    }

    public GameObject GetPiece(int row, int col, GameObject piece)
    {
        return squares[row, col];
    }

    public bool IsOccupied(int row, int col)
    {
        return squares[row, col] != null;
    }

    public void ClearSquare(int row, int col)
    {
        squares[row, col] = null;
    }
}
