# SOS Game

A Windows Forms application implementing the SOS board game with both Simple and General game modes.
36745
## Project Information

**Course:** CS 449 - Software Engineering  
**Sprint:** Sprint 2  

## Sprint 2 Deliverables

This sprint implements the core gameplay functionality:

### User Story 4: Make a Move in Simple Game
- **AC 4.1:** Blue player can select a cell and place S or O
- **AC 4.2:** After Blue's move, turn switches to Red player

### User Story 6: Make a Move in General Game
- **AC 6.1:** Blue player can select a cell and place S or O
- **AC 6.2:** After Blue's move, turn switches to Red player

## Features Implemented

### Game Functionality
- ✅ Dynamic n×n game board (size ≥ 3)
- ✅ Cell selection with visual highlight
- ✅ Place S or O moves
- ✅ Automatic turn switching between Blue and Red players
- ✅ Move validation (prevent duplicate placements)
- ✅ New Game functionality without closing window
- ✅ Support for both Simple and General game modes
- ✅ Intuitive UI with turn-based button enabling/disabling

### User Interface
- Clean, intuitive Windows Forms interface
- Visual turn indicator with color coding
- Smart button states (only active player's button is enabled)
- Clear player identification (Blue/Red)
- Cell highlighting on selection
- Error messages for invalid moves

## How to Build and Run

### Prerequisites
- .NET 8.0 SDK or later
- Windows OS
- Visual Studio 2022 (recommended) or VS Code

### Build Instructions

```bash
# Clone or navigate to the repository
cd SOSGame-2

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project SOSGame-2
```

### Using Visual Studio
1. Open `SOSGame-2.sln`
2. Press `F5` or click "Start" to run
3. The application will launch

## How to Play

1. **Start Screen:**
   - Enter board size (minimum 3)
   - Select game mode (Simple or General)
   - Click "Start Game"

2. **Game Screen:**
   - Blue player goes first
   - Click on any empty cell to select it (cell highlights yellow)
   - Click the enabled button to place your move:
     - Blue player: "Place S" button (light blue)
     - Red player: "Place O" button (light coral)
   - Turn automatically switches to the other player
   - Click "New Game" to reset the board

3. **Game Rules:**
   - Players alternate turns
   - Blue player places S
   - Red player places O
   - Cannot place on occupied cells
   - Game continues until board is full

## Project Structure

```
SOSGame-2/
├── SOSGame-2/
│   ├── Models/
│   │   ├── Board.cs           # Board state management
│   │   ├── CellValue.cs       # Cell value enumeration
│   │   ├── GameMode.cs        # Game mode enumeration
│   │   ├── GameState.cs       # Game logic and state
│   │   └── Player.cs          # Player enumeration
│   ├── GameForm.cs            # Main game window logic
│   ├── GameForm.Designer.cs   # Main game window UI design
│   ├── StartForm.cs           # Start screen logic
│   ├── StartForm.Designer.cs  # Start screen UI design
│   └── Program.cs             # Application entry point
├── SOSGame-2.Tests/           # Unit test project
└── SOSGame-2.sln              # Solution file
```

## Architecture

### Design Patterns
- **Model-View separation:** Models contain game logic, Forms handle UI
- **Encapsulation:** Private fields with public properties
- **Single Responsibility:** Each class has a focused purpose

### Key Classes

#### Models
- **Board:** Manages the game board state and cell operations
- **GameState:** Controls game flow, turns, and move validation
- **CellValue:** Enum for cell contents (Empty, S, O)
- **GameMode:** Enum for game types (Simple, General)
- **Player:** Enum for player identification (Blue, Red)

#### Forms
- **StartForm:** Home screen for game setup
- **GameForm:** Main game board and gameplay interface

## Testing

### Running Unit Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests with coverage (if configured)
dotnet test /p:CollectCoverage=true
```

### Test Coverage

The test project includes unit tests for:
- Board initialization and validation
- Move placement logic
- Turn switching mechanism
- Game state management
- Input validation

See `SOSGame-2.Tests/` for test implementation.

## Development Notes

### Code Quality
- Clean, readable code following C# conventions
- Proper naming conventions (PascalCase, camelCase)
- No compiler warnings
- OOP best practices applied

### Git Workflow
```bash
# Check status
git status

# Stage changes
git add .

# Commit changes
git commit -m "Your commit message"

# View commit history
git log --oneline
```

## Known Limitations

- Sprint 2 scope: Win detection not yet implemented
- No SOS sequence detection (planned for Sprint 3)
- No score tracking (planned for Sprint 3)
- No game save/load functionality

## Future Enhancements (Sprint 3+)

- Implement SOS sequence detection
- Add win condition logic
- Implement score tracking
- Add game recording/replay
- Add computer player (AI)

## License

This is a course project for educational purposes.

## Contact

For questions or issues, please contact through the course management system.

---

**Build Status:** ✅ Passing  
**Tests:** ✅ All tests passing  
**Sprint 2:** ✅ Complete
