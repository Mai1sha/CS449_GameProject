# SOS Game

A Windows Forms application implementing the SOS board game with both Simple and General game modes, featuring an intelligent computer opponent.

## Project Information

**Course:** CS 449 - Software Engineering  
**Current Sprint:** Sprint 4  
**GitHub:** https://github.com/ishraqfatin7/SOSGame

## Sprint 4 Deliverables - Computer Opponent ✅

This sprint implements intelligent computer players with the following features:

### New User Stories (Sprint 4)

- **US 8:** Select player types (Human or Computer for Blue/Red players)
- **US 9:** Computer automatically makes intelligent moves
- **US 10:** Computer uses winning strategies (create SOS, block opponent)
- **US 11:** Visual feedback during computer turns
- **US 12:** Computer plays complete Simple games
- **US 13:** Computer plays complete General games

### Computer AI Features

- **Strategy Hierarchy:**
  1. Create SOS when possible (immediate win/score)
  2. Block opponent from making SOS (defensive)
  3. Strategic positioning (prefer center, adjacent cells)
  4. Random valid move (fallback)
- Works with both Simple and General game modes
- Supports Human vs Computer, Computer vs Computer
- Visual feedback with "thinking" indicator
- 500ms delay for better user experience

## Previous Sprint Deliverables

### Sprint 3 - Game Completion Logic

- SOS sequence detection (horizontal, vertical, diagonal)
- Win condition for Simple mode (first SOS wins)
- Score tracking for General mode (count all SOS)
- Winner determination for General mode (highest score)
- Visual SOS highlighting (planned)

### Sprint 2 - Core Gameplay

- Dynamic n×n game board (size ≥ 3)
- Cell selection and move placement
- Turn-based gameplay
- Move validation
- Simple and General game modes

## Features Implemented

### Game Modes

- ✅ Simple Game (first SOS wins)
- ✅ General Game (most SOSs wins)
- ✅ Configurable board size (3×3 to any size)

### Player Types (New in Sprint 4)

- ✅ Human players (manual input via UI)
- ✅ Computer players (intelligent AI)
- ✅ Mixed configurations (Human vs Computer)
- ✅ Computer vs Computer (auto-play)

### Computer AI

- ✅ Offensive strategy (create SOS)
- ✅ Defensive strategy (block opponent)
- ✅ Strategic positioning (center preference)
- ✅ Works in both Simple and General modes
- ✅ Visual feedback ("Computer is thinking...")

### Game Functionality

- ✅ Dynamic n×n game board (size ≥ 3)
- ✅ Cell selection with visual highlight
- ✅ Place S or O moves
- ✅ Automatic turn switching
- ✅ SOS detection (all 8 directions)
- ✅ Win condition detection
- ✅ Score tracking (General mode)
- ✅ Move validation
- ✅ New Game functionality
- ✅ Game over handling

### User Interface

- Clean, intuitive Windows Forms interface
- Visual turn indicator with color coding
- Player type selection (Human/Computer)
- Smart button states based on player type
- "Computer is thinking..." feedback
- Clear player identification (Blue/Red)
- Cell highlighting on selection
- Error messages for invalid moves
- Score display (General mode)
- Game status indicators

## How to Build and Run

### Prerequisites

- .NET 8.0 SDK or later
- Windows OS
- Visual Studio 2022 (recommended) or VS Code

### Build Instructions

```bash
# Clone or navigate to the repository
cd SOSGame

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project SOSGame
```

### Using Visual Studio

1. Open `SOSGame.sln`
2. Press `F5` or click "Start" to run
3. The application will launch

## How to Play

1. **Start Screen:**

   - Enter board size (minimum 3)
   - Select game mode (Simple or General)
   - Choose Blue player type (Human or Computer)
   - Choose Red player type (Human or Computer)
   - Click "Start Game"

2. **Game Screen (Human Player):**

   - Blue player goes first
   - Click on any empty cell to select it (cell highlights yellow)
   - Click "Place S" or "Place O" button to make your move
   - Turn automatically switches to the other player
   - Click "New Game" to reset the board

3. **Game Screen (Computer Player):**

   - Computer moves automatically after a brief delay
   - Watch as computer makes strategic moves
   - UI buttons disabled during computer's turn
   - Message shows "Computer is thinking..."

4. **Game Modes:**
   - **Simple:** First player to create SOS wins immediately
   - **General:** Play continues until board is full, most SOSs wins
5. **Winning:**
   - **Simple:** Game ends when first SOS is created
   - **General:** Player with most SOSs when board is full wins
   - Draw if scores are equal (General mode)

## Project Structure

```
SOSGame/
├── SOSGame/                   # Main application
│   ├── Models/
│   │   ├── Board.cs                    # Board state management
│   │   ├── CellValue.cs                # Cell value enumeration
│   │   ├── GameMode.cs                 # Game mode enumeration
│   │   ├── GameState.cs                # Game logic and state
│   │   ├── GameLogic.cs                # Abstract game logic base
│   │   ├── SimpleGameLogic.cs          # Simple mode rules
│   │   ├── GeneralGameLogic.cs         # General mode rules
│   │   ├── Player.cs                   # Player enumeration
│   │   ├── PlayerType.cs               # Player type enumeration
│   │   ├── PlayerController.cs         # Abstract player controller
│   │   ├── HumanPlayerController.cs    # Human player implementation
│   │   ├── ComputerPlayerController.cs # Computer AI implementation
│   │   ├── Move.cs                     # Move data structure
│   │   └── SOSSequence.cs              # SOS sequence representation
│   ├── GameForm.cs                     # Main game window logic
│   ├── GameForm.Designer.cs            # Main game window UI
│   ├── StartForm.cs                    # Start screen logic
│   ├── StartForm.Designer.cs           # Start screen UI
│   └── Program.cs                      # Application entry point
├── SOSGame.Tests/                      # Unit test project
│   ├── BoardTests.cs                   # Board class tests
│   ├── GameStateTests.cs               # GameState tests
│   ├── SimpleGameLogicTests.cs         # Simple mode tests
│   ├── GeneralGameLogicTests.cs        # General mode tests
│   ├── PlayerControllerTests.cs        # Player controller tests
│   └── ComputerPlayerTests.cs          # Computer AI tests
├── SPRINT4_SUBMISSION.md              # Sprint 4 documentation
└── SOSGame.sln                        # Solution file
```

## Architecture

### Design Patterns

- **Strategy Pattern:** PlayerController hierarchy for different player types
- **Template Method:** GameLogic hierarchy for game mode rules
- **Model-View Separation:** Models contain game logic, Forms handle UI
- **Encapsulation:** Private fields with public properties
- **Single Responsibility:** Each class has a focused purpose
- **Dependency Inversion:** High-level modules depend on abstractions

### Key Classes

#### Models

- **Board:** Manages the game board state and cell operations
- **GameState:** Controls game flow, turns, and player management
- **GameLogic:** Abstract base for game mode logic (Template Method)
- **SimpleGameLogic:** Implements Simple mode rules (first SOS wins)
- **GeneralGameLogic:** Implements General mode rules (most SOSs wins)
- **PlayerController:** Abstract base for player types (Strategy Pattern)
- **HumanPlayerController:** Handles human player input via UI
- **ComputerPlayerController:** Implements AI for computer players
- **Move:** Data structure representing a game move
- **SOSSequence:** Represents a detected SOS pattern
- **Enumerations:** CellValue, GameMode, Player, PlayerType

#### Forms

- **StartForm:** Home screen for game setup and configuration
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

**Total: 64 Automated Tests (All Passing)**

The test project includes comprehensive unit tests for:

- **Board Management:** Initialization, move placement, validation (12 tests)
- **Game State:** Turn management, player controllers, game flow (17 tests)
- **Simple Mode:** SOS detection, win conditions (8 tests)
- **General Mode:** Score tracking, winner determination (8 tests)
- **Player Controllers:** Human and Computer implementations (7 tests)
- **Computer AI:** Strategy testing, move validation (10 tests)
- **Integration:** End-to-end gameplay scenarios

See `SOSGame.Tests/` for detailed test implementation.

## Development Notes

### Code Quality

- Clean, readable code following C# conventions
- Proper naming conventions (PascalCase, camelCase)
- XML documentation comments
- No compiler warnings or errors
- SOLID principles applied
- Design patterns properly implemented
- Comprehensive test coverage

### Class Hierarchy

The project demonstrates proper object-oriented design:

- **Abstract base classes:** GameLogic, PlayerController
- **Inheritance:** SimpleGameLogic, GeneralGameLogic extend GameLogic
- **Polymorphism:** PlayerController hierarchy enables runtime player type selection
- **Encapsulation:** Private fields, public properties
- **Abstraction:** Interfaces hide implementation details

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

- Computer AI uses basic strategies (not optimal play)
- No difficulty levels for computer opponent
- No move history or undo functionality
- No game save/load feature
- SOS sequence visual highlighting not yet implemented

## Future Enhancements (Post-Sprint 4)

- Advanced AI with minimax algorithm
- Multiple difficulty levels (Easy, Medium, Hard)
- Hint system for human players
- Move history and replay functionality
- Game statistics tracking
- Network multiplayer support
- Custom board themes
- Sound effects and animations
- Save/load game state
- Tournament mode

## License

This is a course project for educational purposes.

## Contact

For questions or issues, please contact through the course management system.

---

**Build Status:** ✅ Passing (0 errors, 0 warnings)  
**Tests:** ✅ 64/64 tests passing  
**Sprint 4:** ✅ Complete  
**Computer Opponent:** ✅ Fully Functional
