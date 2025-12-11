# SOS Game Project

A Windows Forms application implementing the SOS board game with both Simple and General game modes, featuring an intelligent computer opponent.


## Project Information

**Course:** CS449 - Foundation of Sotware Engineering  
**Current Sprint:** Sprint 5

## Sprint-5 Deliverables - Record & Replay 

The sprint-5 gives the chance to record the game in a text file and enables the replay option.

## 8 new (14-21) User-Stories & Acceptance Criteria implemented


## Previous Sprints(0,1,2,3,4) implemented perfectly


## Features Implemented

--Game Modes-Completed ✅
--Player Types (New in Sprint 4)-Completed ✅
--Computer AI-Completed ✅
--Game Functionality-Completed ✅
--User Interface-Completed ✅


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

```


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



###F Future implemenmtation
I am looking forward to implement the feature of Artificial Inteligence in project.
