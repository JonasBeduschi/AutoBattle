/******************************************************************/
// Kokku Software Engineer Applicant Test
//
// 2021/12/20
//
// Jonas de Oliveira Beduschi
// https://www.linkedin.com/in/jonas-beduschi/
// https://jonasbeduschi.github.io/
//
// Extra Feature: Oct~Nov
// Add an effect for each class that can somehow paralyze other characters (random chance)
//
/******************************************************************/

using System;
using System.Collections.Generic;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Program
    {
        private static bool gameRunning = true;

        // Use a single Random instance for everything
        private static Random random = new Random();

        private static Grid grid = new Grid(12, 8);
        private static Character playerCharacter;
        private static Character enemyCharacter;
        private static List<Character> allPlayers = new List<Character>();
        private static int currentTurn = 0;

        private static void Main(string[] args)
        {
            Setup();
            StartGame();

            while (gameRunning) {
                WaitForPlayerInput();
                UpdateRound();
            }
            Console.ReadLine();
        }

        private static void WaitForPlayerInput()
        {
            Console.WriteLine("\nClick on any key to start the next turn...\n\n");
            Console.ReadKey();
        }

        private static void Setup()
        {
            ShowInstructions();
            int playerChoice = ReadPlayerChoice();
            while (playerChoice < 1 || playerChoice > 4) {
                Console.WriteLine("\nInvalid number. Please, try again.\n");
                ShowInstructions();
                playerChoice = ReadPlayerChoice();
            }
            Console.WriteLine();
            // These methods shouldn't have been nested in each other
            // Simplified character creation
            playerCharacter = CreateCharacter("Player", playerChoice, 0);
            enemyCharacter = CreateCharacter("Enemy", random.Next(1, 5), 1);
        }

        private static void StartGame()
        {
            playerCharacter.Target = enemyCharacter;
            enemyCharacter.Target = playerCharacter;

            allPlayers.Add(playerCharacter);
            allPlayers.Add(enemyCharacter);

            PositionCharacter(playerCharacter);
            PositionCharacter(enemyCharacter);

            grid.DrawBattlefield();
        }

        private static bool OnlyOnePlayerAlive()
        {
            int alive = 0;
            foreach (Character character in allPlayers) {
                if (character.Status != StatusEffect.Dead)
                    alive++;
            }
            return alive == 1;
        }

        private static void EndGame()
        {
            gameRunning = false;
            foreach (Character character in allPlayers) {
                if (character.Status == StatusEffect.Dead) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{character.Name} has lost the game!");
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{character.Name} has won the game!");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to quit...");
        }

        private static void UpdateRound()
        {
            if (currentTurn == 0) {
                allPlayers.Sort();
                // Flip a coin to see who goes first
                if (random.Next(2) == 0)
                    allPlayers.Reverse();
            }

            if (OnlyOnePlayerAlive()) {
                EndGame();
                return;
            }

            foreach (Character character in allPlayers)
                character.StartTurn(grid);

            grid.DrawBattlefield();
            currentTurn++;
        }

        private static void PositionCharacter(Character character)
        {
            int index;
            Cell randomLocation;
            do {
                index = random.Next(0, grid.Matrix.Count);
                randomLocation = grid.Matrix[index];
            } while (randomLocation.Occupied);
            Console.WriteLine($"{character.Name} spawned in cell {index}");

            randomLocation.Occupant = character;
            grid.Matrix[index] = randomLocation;
            character.CurrentCell = grid.Matrix[index];
        }

        private static Character CreateCharacter(string name, int classIndex, int index)
        {
            CharacterClass characterClass = (CharacterClass)classIndex;
            Console.WriteLine($"{name} class choice: {characterClass}");
            return new Character(name, index, characterClass);
        }

        private static void ShowInstructions()
        {
            Console.WriteLine("Choose between one of these classes:"); // Fixed text
            Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
        }

        private static int ReadPlayerChoice()
        {
            string choice = Console.ReadLine();
            bool isNumber = int.TryParse(choice, out int value);
            if (isNumber == false)
                return -1;
            return value;
        }
    }
}