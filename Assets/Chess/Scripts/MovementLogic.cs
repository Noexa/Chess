using System.Collections.Generic;
using UnityEngine;

public class MovementLogic
{
    private static readonly Vector2Int[] KnightMoves = 
    {
        new( 2, 1), new( 2, -1),
        new(-2, 1), new(-2, -1),
        new( 1, 2), new( 1, -2),
        new(-1, 2), new(-1, -2)
    };

    private static readonly Vector2Int[] KingMoves = 
    {
        new(-1, 1), new( 0, 1), new( 1, 1),
        new(-1, 0),             new( 1, 0),
        new(-1, -1), new( 0, -1), new( 1, -1)
    };

    private static readonly Vector2Int[] RookDirections =
    {
        new(1,0), new(-1,0), new(0,1), new(0,-1)
    };

    private static readonly Vector2Int[] BishopDirections = 
    {
        new(1,1), new(-1,-1), new(1,-1), new(-1,1)
    };
    public List<Vector2Int> GetValidMoves(BoardModel board, PieceView piece)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int r = piece.Row;
        int c = piece.Col;

        return moves;
    }

    private void AddRay(BoardModel board, PieceModel piece, int row, int col, int deltaRow, int deltaCol, List<Vector2Int> moves)
    {
        int r = row + deltaRow;
        int c = col + deltaCol;
        
        while (board.IsInbounds(r,c))
        {
            if (!board.IsOccupied)
            {
                moves.Add(new Vector2Int(r,c))
            }
            else
            {
                if (IsEnemy)
                {
                    moves.Add(new Vector2Int(r,c))
                }
                break;
            }
        r += deltaRow;
        c += deltaCol;
        }
    }

    private bool IsEnemy(BoardModel board, PieceView mover, int row, int col)
    {
        bool enemy = false;

        GameObject targetGo = board.GetPiece(row, col);
        if (targetGo != null)
        {
            PieceView target = targetGo.GetComponent<PieceView>();
            if (target != null)
            {
                enemy = (target.IsWhite != mover.IsWhite);
            }
        }
        return enemy;
    }

    private void TryAddStep(BoardModel board, PieceView piece, int row, int col, List<Vector2Int> moves)
    {
        if (!board.IsInBounds)
        {
            return;
        }
        if (!board.IsOccupied)
        {
            moves.Add(new Vector2Int(row, col));
        }
        else
        {
            if (IsEnemy(board, piece, row, col))
            {
                moves.Add(new Vector2Int(row, col));
            }
        }
    }
    public void TryAddCapture(BoardModel board, PieceView piece, int row, int col, List<Vector2Int> moves)
    {
        if (board.IsInBounds(row,col) && board.IsOccupied && IsEnemy(board, piece, row, col))
        {
            moves.Add(new Vector2Int(row, col));
        }
    }
}