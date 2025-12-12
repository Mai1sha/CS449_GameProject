using Xunit;
using SOSGame.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SOSGame.Tests
{
    public class OpenAIPromptWithHistoryTest
    {
        [Fact]
        public void BuildPrompt_IncludesMoveHistory_WhenProvided()
        {
            // Arrange
            string fakeApiKey = "test-key-12345";
            var controller = new OpenAIPlayerController(Player.Blue, fakeApiKey);
            var board = new Board(3);
            var gameLogic = new SimpleGameLogic(board);
            
            // Create move history
            var moveHistory = new List<(int row, int col, CellValue value, Player player)>
            {
                (0, 0, CellValue.S, Player.Blue),
                (0, 1, CellValue.O, Player.Red),
                (1, 1, CellValue.S, Player.Blue)
            };

            // Place moves on board to match history
            board.PlaceMove(0, 0, CellValue.S);
            board.PlaceMove(0, 1, CellValue.O);
            board.PlaceMove(1, 1, CellValue.S);

            // Act - Use reflection to call private BuildPrompt method
            var buildPromptMethod = typeof(OpenAIPlayerController).GetMethod(
                "BuildPrompt", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            string prompt = (string)buildPromptMethod.Invoke(
                controller, 
                new object[] { board, gameLogic, GameMode.Simple, moveHistory });

            // Assert
            Console.WriteLine("Generated Prompt with Move History:");
            Console.WriteLine(prompt);
            Console.WriteLine();

            Assert.Contains("RECENT MOVES", prompt);
            Assert.Contains("Blue: (0,0)=S", prompt);
            Assert.Contains("Red: (0,1)=O", prompt);
            Assert.Contains("Blue: (1,1)=S", prompt);
        }

        [Fact]
        public void BuildPrompt_ShowsLast10Moves_WhenHistoryIsLong()
        {
            // Arrange
            string fakeApiKey = "test-key-12345";
            var controller = new OpenAIPlayerController(Player.Blue, fakeApiKey);
            var board = new Board(5);
            var gameLogic = new GeneralGameLogic(board);
            
            // Create 15 moves
            var moveHistory = new List<(int row, int col, CellValue value, Player player)>();
            for (int i = 0; i < 15; i++)
            {
                int row = i / 5;
                int col = i % 5;
                CellValue value = i % 2 == 0 ? CellValue.S : CellValue.O;
                Player player = i % 2 == 0 ? Player.Blue : Player.Red;
                moveHistory.Add((row, col, value, player));
                board.PlaceMove(row, col, value);
            }

            // Act
            var buildPromptMethod = typeof(OpenAIPlayerController).GetMethod(
                "BuildPrompt", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            string prompt = (string)buildPromptMethod.Invoke(
                controller, 
                new object[] { board, gameLogic, GameMode.General, moveHistory });

            // Assert
            Console.WriteLine("Generated Prompt with 15 moves (should show last 10):");
            Console.WriteLine(prompt);
            Console.WriteLine();

            Assert.Contains("RECENT MOVES", prompt);
            // Should show last 5 moves (11-15)
            Assert.Contains("11. Blue: (2,0)=S", prompt);
            Assert.Contains("15. Blue: (2,4)=S", prompt);
            // Should NOT show move 1-10
            Assert.DoesNotContain("1. Blue: (0,0)=S", prompt);
            Assert.DoesNotContain("10. Red: (1,4)=O", prompt);
        }

        [Fact]
        public void BuildPrompt_WorksWithoutMoveHistory()
        {
            // Arrange
            string fakeApiKey = "test-key-12345";
            var controller = new OpenAIPlayerController(Player.Red, fakeApiKey);
            var board = new Board(3);
            var gameLogic = new SimpleGameLogic(board);

            // Act
            var buildPromptMethod = typeof(OpenAIPlayerController).GetMethod(
                "BuildPrompt", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            string prompt = (string)buildPromptMethod.Invoke(
                controller, 
                new object[] { board, gameLogic, GameMode.Simple, null });

            // Assert
            Console.WriteLine("Generated Prompt without Move History:");
            Console.WriteLine(prompt);
            Console.WriteLine();

            Assert.DoesNotContain("RECENT MOVES", prompt);
            Assert.Contains("BOARD STATE", prompt);
            Assert.Contains("YOUR MOVE", prompt);
        }
    }
}
