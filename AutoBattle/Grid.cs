using System;
using System.Collections.Generic;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        public List<Cell> Matrix { get; } = new List<Cell>();

        /// <summary>Height</summary>
        public int Rows;

        /// <summary>Length</summary>
        public int Columns;

        public Grid(int columns, int rows)
        {
            Rows = rows;
            Columns = columns;
            for (int j = 0; j < Rows; j++) {
                for (int i = 0; i < Columns; i++) {
                    Cell newCell = new Cell(i, j, Matrix.Count);
                    Matrix.Add(newCell);
                }
            }
            Console.WriteLine("The battlefield has been created\n");
        }

        // Removed parameters, just use the class'
        public void DrawBattlefield()
        {
            for (int j = 0; j < Rows; j++) {
                for (int i = 0; i < Columns; i++) {
                    Cell currentCell = Matrix[j * Columns + i];
                    if (currentCell.Occupied) {
                        float healthPercentage = currentCell.Occupant.GetHealthPercentage();
                        SetColour(healthPercentage);
                        Console.Write($"[{currentCell.Occupant.Name[0]}]\t");
                    }
                    else {
                        Console.Write("[ ]\t");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }

        // More it colourful
        private static void SetColour(float healthPercentage)
        {
            if (healthPercentage > .80f)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (healthPercentage > .60f)
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            else if (healthPercentage > .40f)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (healthPercentage > .20f)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.DarkRed;
        }
    }
}