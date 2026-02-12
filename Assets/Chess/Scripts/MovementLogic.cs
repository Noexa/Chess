using System.Collections.Generic;
using UnityEngine;

public class MovementLogic
{
    private static readonly Vector2Int[] KnightOffsets = 
    {
        new( 2, 1), new( 2, -1),
        new(-2, 1), new(-2, -1),
        new( 1, 2), new( 1, -2),
        new(-1, 2), new(-1, -2)
    };

    private static readonly Vector2Int[] KingOffsets = 
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
        int row = piece.Row;
        int col = piece.Col;

        switch(piece.Type)
        {
            case PieceType.Pawn:
                AddPawnMoves(board, piece, row, col, moves);
                break;
            case PieceType.Knight:
                AddKnightMoves(board, piece, row, col, moves);
                break;
            case PieceType.Bishop:
                AddBishopMoves(board, piece, row, col, moves);
                break;
            case PieceType.Rook:
                AddRookMoves(board, piece, row, col, moves);
                break;                
            case PieceType.Queen:
                AddQueenMoves(board, piece, row, col, moves);
                break;
            case PieceType.King:
                AddKingMoves(board, piece, row, col, moves);
                break;
        }
        return moves;
    }

    // Helper functions
    private void AddRay(BoardModel board, PieceView piece, int row, int col, int deltaRow, int deltaCol, List<Vector2Int> moves)
    {
        int r = row + deltaRow;
        int c = col + deltaCol;
        
        while (board.IsInBounds(r,c))
        {
            if (!board.IsOccupied(r,c))
            {
                moves.Add(new Vector2Int(r,c));
            }
            else
            {
                if (IsEnemy(board, piece, r,c))
                {
                    moves.Add(new Vector2Int(r,c));
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
        if (!board.IsInBounds(row, col))
        {
            return;
        }
        if (!board.IsOccupied(row, col))
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
        if (board.IsInBounds(row,col) && board.IsOccupied(row, col) && IsEnemy(board, piece, row, col))
        {
            moves.Add(new Vector2Int(row, col));
        }
    }

    // Individual Piece logic
    private void AddPawnMoves(BoardModel board, PieceView piece, int row, int col, List<Vector2Int> moves)
    {
        int dir = piece.IsWhite ? 1 : -1;
        int startRow = piece.IsWhite ? 1 : 6;
        int frontRow = row + dir;

        // Initial double move
        if (row == startRow)
        {
            int doubleRow = row + (dir * 2);
            if (board.IsInBounds(doubleRow, col) && !board.IsOccupied(frontRow, col) && !board.IsOccupied(doubleRow, col))
            {
                moves.Add(new Vector2Int(doubleRow, col));
            }
        }

        // Standard move
        if (board.IsInBounds(frontRow, col) && !board.IsOccupied(frontRow, col))
        {
            moves.Add(new Vector2Int(frontRow, col));
        }

        TryAddCapture(board, piece, row + dir, col - 1, moves);
        TryAddCapture(board, piece, row + dir, col + 1, moves);

        //FIXME promotion, en passant
    }
    private void AddKnightMoves(BoardModel board, PieceView piece, int row, int col, List<Vector2Int> moves)
    {
        foreach (var offset in KnightOffsets)
        {
            TryAddStep(board, piece, row + offset.x, col + offset.y, moves);
        }
    }
    private void AddBishopMoves(BoardModel board, PieceView piece, int row, int col, List<Vector2Int> moves)
    {
        foreach (var dir in BishopDirections)
        {
            AddRay(board, piece, row, col, dir.x, dir.y, moves);
        }
    }
    private void AddRookMoves(BoardModel board, PieceView piece, int row, int col, List<Vector2Int> moves)
    {
        foreach (var dir in RookDirections)
        {
            AddRay(board, piece, row, col, dir.x, dir.y, moves);
        }
    }
    
    private void AddQueenMoves(BoardModel board, PieceView piece, int row, int col, List<Vector2Int> moves)
    {
        AddBishopMoves(board, piece, row, col, moves);
        AddRookMoves(board, piece, row, col, moves);
    }
    private void AddKingMoves(BoardModel board, PieceView piece, int row, int col, List<Vector2Int> moves)
    {
        foreach (var offset in KingOffsets)
        {
            TryAddStep(board, piece, row + offset.x, col + offset.y, moves);
        }  
    }

}