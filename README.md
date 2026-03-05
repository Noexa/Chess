# Unity Chess
![Unity](https://img.shields.io/badge/Engine-Unity-000000?logo=unity)
![C#](https://img.shields.io/badge/Language-C%23-blue)
![Status](https://img.shields.io/badge/Project-Complete-brightgreen)

A chess game built in Unity (UI-based board) with core rules implemented in C#.

## Features
- Piece movement + captures
- Castling (blocked if passing through/into check)
- Check detection (warning only; allows blunders)
- En passant
- Pawn promotion UI (Q/R/B/N)
- Forfeit + restart
- Move highlighting + message popup system

## How to Run
1. Open the project in Unity (Last developed with version: 6000.3.2f1)
2. Open scene: Assets/Chess/Scenes/Main.unity
3. Press Play

## Controls
- Click a piece to select
- Click a highlighted square to move
- Promotion: choose Q/R/B/N from the popup

## Architecture (High level)
- `BoardModel`: stores board state + special move state (castling/en passant) + king references
- `MovementLogic`: generates valid moves + check/castle constraints
- `GameController`: input orchestration, turn order, executes moves, UI messaging

## Known Limitations
- Checkmate/stalemate not enforced. Blundering your king is permitted
- No save/load system
- Undo not implemented
