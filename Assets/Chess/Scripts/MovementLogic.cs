using System.Collections.Generic;
using UnityEngine;

public class MovementLogic
{
    public List<Vector2Int> GetValidMoves(BoardModel board, PieceView piece)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int r = piece.Row;
        int c = piece.Col;

        return moves;
    }
}