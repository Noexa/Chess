using UnityEngine;

public class BoardModel
{
    private PieceView _whiteKing;
    private PieceView _blackKing;
    private GameObject[,] squares = new GameObject[8,8];

    public void RegisterKing(PieceView king)
    {
        if (king.IsWhite)
            _whiteKing = king;
        else
            _blackKing = king;
    }

    public PieceView GetKing(bool isWhite)
    {
        return isWhite ? _whiteKing : _blackKing;
    }

    public void SetPiece(int row, int col, GameObject piece)
    {
        squares[row, col] = piece;
    }

    public GameObject GetPiece(int row, int col)
    {
        return squares[row, col];
    }

    public void MovePiece(int fromRow, int fromCol, int toRow, int toCol)
    {
        GameObject moverGo = GetPiece(fromRow, fromCol);
        GameObject targetGo = GetPiece(toRow, toCol);

        if (targetGo != null)
        {
            PieceView mover = moverGo.GetComponent<PieceView>();
            PieceView target = targetGo.GetComponent<PieceView>();

            if (mover.IsWhite != target.IsWhite)
            {
                if (target.Type == PieceType.King)
                {
                    Debug.Log($"{(target.IsWhite ? "White" : "Black")} King captured. Game Over.");
                }
                Object.Destroy(targetGo);
                SetPiece(toRow, toCol, null);
            }
        }

        SetPiece(toRow, toCol, moverGo);
        SetPiece(fromRow, fromCol, null);
    }

    public  bool IsCastlingMove(PieceView piece, int fromRow, int fromCol, int toRow, int toCol)
    {
        bool isCastle = false;
        if (piece != null && piece.Type == PieceType.King)
        {
            int deltaCol = toCol -fromCol;
            isCastle = (fromRow == toRow && (deltaCol == 2 || deltaCol == -2));
        }
        return isCastle;
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
