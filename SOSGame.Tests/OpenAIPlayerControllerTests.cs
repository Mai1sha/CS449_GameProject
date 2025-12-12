using SOSGame.Models;
using Xunit;

namespace SOSGame.Tests
{
    public class OpenAIPlayerControllerTests
    {
        [Fact]
        public void BuildPrompt_IncludesBoardSize()
        {
            var board = new Board(5);
            var gameLogic = new SimpleGameLogic(board);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            string prompt = controller.TestBuildPrompt(board, gameLogic, GameMode.Simple);
            
            Assert.Contains("5x5", prompt);
        }

        [Fact]
        public void BuildPrompt_IncludesGameMode_Simple()
        {
            var board = new Board(5);
            var gameLogic = new SimpleGameLogic(board);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            string prompt = controller.TestBuildPrompt(board, gameLogic, GameMode.Simple);
            
            Assert.Contains("SIMPLE", prompt);
            Assert.Contains("First to create ANY S-O-S wins", prompt);
        }

        [Fact]
        public void BuildPrompt_IncludesGameMode_General()
        {
            var board = new Board(5);
            var gameLogic = new GeneralGameLogic(board);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            string prompt = controller.TestBuildPrompt(board, gameLogic, GameMode.General);
            
            Assert.Contains("GENERAL", prompt);
            Assert.Contains("Most S-O-S sequences wins", prompt);
        }

        [Fact]
        public void BuildPrompt_IncludesScores()
        {
            var board = new Board(5);
            var gameLogic = new SimpleGameLogic(board);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            string prompt = controller.TestBuildPrompt(board, gameLogic, GameMode.Simple);
            
            Assert.Contains("Score:", prompt);
            Assert.Contains("You=0", prompt);
            Assert.Contains("Opponent=0", prompt);
        }

        [Fact]
        public void BuildPrompt_IncludesBoardState()
        {
            var board = new Board(3);
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(1, 1, CellValue.O);
            var gameLogic = new SimpleGameLogic(board);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            string prompt = controller.TestBuildPrompt(board, gameLogic, GameMode.Simple);
            
            Assert.Contains("BOARD STATE", prompt);
            Assert.Contains("0-indexed", prompt);
        }

        [Fact]
        public void BuildPrompt_IncludesPlayerColor()
        {
            var board = new Board(5);
            var gameLogic = new SimpleGameLogic(board);
            var blueController = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            var redController = new TestableOpenAIPlayerController(Player.Red, "test-key");
            
            string bluePrompt = blueController.TestBuildPrompt(board, gameLogic, GameMode.Simple);
            string redPrompt = redController.TestBuildPrompt(board, gameLogic, GameMode.Simple);
            
            Assert.Contains("You: Blue", bluePrompt);
            Assert.Contains("You: Red", redPrompt);
        }

        [Fact]
        public void BuildPrompt_IncludesResponseFormat()
        {
            var board = new Board(5);
            var gameLogic = new SimpleGameLogic(board);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            string prompt = controller.TestBuildPrompt(board, gameLogic, GameMode.Simple);
            
            Assert.Contains("YOUR MOVE", prompt);
            Assert.Contains("Row Col Letter", prompt);
        }

        [Fact]
        public void ParseOpenAIResponse_Format_RowColumnLetter()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("Row 2, Column 3, S", board);
            
            Assert.NotNull(move);
            Assert.Equal(2, move.Row);  // 0-indexed, so 2 stays 2
            Assert.Equal(3, move.Col);  // 0-indexed, so 3 stays 3
            Assert.Equal(CellValue.S, move.Value);
        }

        [Fact]
        public void ParseOpenAIResponse_Format_NumbersWithSpaces()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("2 3 O", board);
            
            Assert.NotNull(move);
            Assert.Equal(2, move.Row);  // 0-indexed
            Assert.Equal(3, move.Col);  // 0-indexed
            Assert.Equal(CellValue.O, move.Value);
        }

        [Fact]
        public void ParseOpenAIResponse_Format_Parentheses()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("(1,4) S", board);
            
            Assert.NotNull(move);
            Assert.Equal(1, move.Row);  // 0-indexed
            Assert.Equal(4, move.Col);  // 0-indexed
            Assert.Equal(CellValue.S, move.Value);
        }

        [Fact]
        public void ParseOpenAIResponse_ZeroIndexed()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("0 0 S", board);
            
            Assert.NotNull(move);
            Assert.Equal(0, move.Row);
            Assert.Equal(0, move.Col);
            Assert.Equal(CellValue.S, move.Value);
        }

        [Fact]
        public void ParseOpenAIResponse_OneIndexed()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("1 1 O", board);
            
            Assert.NotNull(move);
            Assert.Equal(1, move.Row);  // 0-indexed, so 1 stays 1
            Assert.Equal(1, move.Col);  // 0-indexed, so 1 stays 1
            Assert.Equal(CellValue.O, move.Value);
        }

        [Fact]
        public void ParseOpenAIResponse_InvalidFormat_ReturnsNull()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("invalid response", board);
            
            Assert.Null(move);
        }

        [Fact]
        public void ParseOpenAIResponse_EmptyString_ReturnsNull()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("", board);
            
            Assert.Null(move);
        }

        [Fact]
        public void ParseOpenAIResponse_OutOfBounds_ReturnsNull()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("10 10 S", board);
            
            Assert.Null(move);
        }

        [Fact]
        public void ParseOpenAIResponse_InvalidLetter_ReturnsNull()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("2 3 X", board);
            
            Assert.Null(move);
        }

        [Fact]
        public void ParseOpenAIResponse_LowercaseLetter()
        {
            var board = new Board(5);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            var move = controller.TestParseOpenAIResponse("2 3 s", board);
            
            Assert.NotNull(move);
            Assert.Equal(2, move.Row);  // 0-indexed
            Assert.Equal(3, move.Col);  // 0-indexed
            Assert.Equal(CellValue.S, move.Value);
        }
    }

    public class TestableOpenAIPlayerController : OpenAIPlayerController
    {
        public TestableOpenAIPlayerController(Player player, string apiKey) 
            : base(player, apiKey)
        {
        }

        public string TestBuildPrompt(Board board, GameLogic gameLogic, GameMode gameMode)
        {
            return typeof(OpenAIPlayerController)
                .GetMethod("BuildPrompt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(this, new object?[] { board, gameLogic, gameMode, null }) as string ?? string.Empty;
        }

        public Move? TestParseOpenAIResponse(string response, Board board)
        {
            return typeof(OpenAIPlayerController)
                .GetMethod("ParseOpenAIResponse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(this, new object[] { response, board }) as Move;
        }
    }
}
