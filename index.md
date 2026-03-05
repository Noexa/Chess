# Unity Chess

A fully playable chess game built in Unity

## Features
- Legal movement system
- Castling
- En passant
- Pawn promotion UI
- Move highlighting
- Forfeit + restart

## Architecture
The project separates gameplay logic from UI

Core systems:
- BoardModel – board state and special rules
- MovementLogic – validates moves
- GameController – player input and game flow
- PieceSpawner – spawns UI pieces

## How to Run
1. Open project in Unity (version: 6000.3.2f1)
2. Open `Assets/Chess/Scenes/Main.unity`
3. Press Play

## Future Improvements
- Checkmate / stalemate detection
- Save / load / undo
- AI opponent
