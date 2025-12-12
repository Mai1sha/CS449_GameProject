using SOSGame.Models;
using Xunit;
using Xunit.Abstractions;

namespace SOSGame.Tests
{
    public class OpenAIPromptVerificationTest
    {
        private readonly ITestOutputHelper _output;

        public OpenAIPromptVerificationTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void VerifyPromptFormat()
        {
            var board = new Board(5);
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(1, 1, CellValue.O);
            board.PlaceMove(2, 2, CellValue.S);
            
            var gameLogic = new SimpleGameLogic(board);
            var controller = new TestableOpenAIPlayerController(Player.Blue, "test-key");
            
            string prompt = controller.TestBuildPrompt(board, gameLogic, GameMode.Simple);
            
            _output.WriteLine("Generated Prompt:");
            _output.WriteLine(prompt);
            
            Assert.Contains("5x5", prompt);
            Assert.Contains("SIMPLE", prompt);
            Assert.Contains("First to create ANY S-O-S wins", prompt);
            Assert.Contains("Score:", prompt);
            Assert.Contains("BOARD STATE", prompt);
            Assert.Contains("You: Blue", prompt);
            Assert.Contains("YOUR MOVE", prompt);
        }

        [Fact]
        public void VerifyPromptFormat_General()
        {
            var board = new Board(3);
            var gameLogic = new GeneralGameLogic(board);
            var controller = new TestableOpenAIPlayerController(Player.Red, "test-key");
            
            string prompt = controller.TestBuildPrompt(board, gameLogic, GameMode.General);
            
            _output.WriteLine("Generated Prompt (General Mode):");
            _output.WriteLine(prompt);
            
            Assert.Contains("3x3", prompt);
            Assert.Contains("GENERAL", prompt);
            Assert.Contains("Most S-O-S sequences wins", prompt);
            Assert.Contains("You: Red", prompt);
        }
    }
}
