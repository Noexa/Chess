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
    public List<Vector2Int> GetValidMoves(BoardModel board, PieceView piece, bool includeCastling = true)
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
                AddKingMoves(board, piece, row, col, moves, includeCastling);
                if (includeCastling)
                {
                    AddCastleMoves(board, piece, row, col, moves);
                }
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
    private void AddKingMoves(BoardModel board, PieceView piece, int row, int col, List<Vector2Int> moves, bool includeCastling)
    {
        foreach (var offset in KingOffsets)
        {
            TryAddStep(board, piece, row + offset.x, col + offset.y, moves);
        }  

        if (includeCastling)
        {
            AddCastleMoves(board, piece, row, col, moves);

        }
    }

    private void AddCastleMoves(BoardModel board, PieceView king, int row, int col, List<Vector2Int> moves)
    {
        if (king == null || king.Type  != PieceType.King || king.HasMoved)
        {
            return;
        }

        int homeRow = king.IsWhite ? 0 : 7;
        if (row != homeRow || col != 4)
        {
            return;
        }

        //FIXME need to implement logic from future ticket to ensure all tiles are not in/through/or would cause a check

        TryAddCastle(board, king, homeRow, rookCol: 7, throughCol: 5, destCol: 6, moves: moves, extraEmptyCol: -1);
        TryAddCastle(board, king, homeRow, rookCol: 0, throughCol: 3, destCol: 2, moves: moves, extraEmptyCol: 1);
    }

    private void TryAddCastle(
        BoardModel board,
        PieceView king,
        int homeRow,
        int rookCol,
        int throughCol,
        int destCol,
        List<Vector2Int> moves,
        int extraEmptyCol = -1) // -1 = Queen-sided castling,  1 = King-sided
    {
        GameObject rookGo = board.GetPiece(homeRow, rookCol);
        if (rookGo == null)
        {
            Debug.Log("Castle blocked: No rook found");
            return;
        }

        PieceView rook = rookGo.GetComponent<PieceView>();
        bool invalidRook = rook == null ||
            rook.Type != PieceType.Rook ||
            rook.IsWhite != king.IsWhite ||
            rook.HasMoved;

        if (invalidRook)
        {
            Debug.Log("Castle blocked: Rook invalid or has moved");
            return;
        }

        bool pathBlocked =
            board.IsOccupied(homeRow, throughCol) ||
            board.IsOccupied(homeRow, destCol) ||
            (extraEmptyCol != -1 && board.IsOccupied(homeRow, extraEmptyCol));

        if (pathBlocked)
        {
            return;
        }

        bool kingInCheck = IsSquareAttacked(board, homeRow, 4, !king.IsWhite);

        bool throughCheck = IsSquareAttacked(board, homeRow, throughCol, !king.IsWhite);

        bool landInCheck = IsSquareAttacked(board, homeRow, destCol, !king.IsWhite);

        if (kingInCheck || throughCheck || landInCheck)
        {
            Debug.Log("Castle blocked: King would pass through or land in check");
            return;
        }

        moves.Add(new Vector2Int(homeRow, destCol));
    }
    public bool IsSquareAttacked(BoardModel board, int row, int col, bool byWhite)
    {
        Vector2Int target = new Vector2Int(row, col);
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                GameObject go = board.GetPiece(r,c);
                if (go == null) continue;

                PieceView piece = go.GetComponent<PieceView>();
                if (piece == null || piece.IsWhite != byWhite) continue;

                List<Vector2Int> moves =GetValidMoves(board, piece, includeCastling : false);
                if (moves.Contains(target)) return true;
            }
        }
        return false;
    }
    public bool IsInCheck(BoardModel board, bool isWhite)
    {
        PieceView king = board.GetKing(isWhite);
        if (king == null)
        {
            return false;
        }
        return IsSquareAttacked(board, king.Row, king.Col, !isWhite);
    }
}


