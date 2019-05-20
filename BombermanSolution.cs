
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] field = new string[6];
            field[0] = ".......";
            field[1] = "...O...";
            field[2] = "....O..";
            field[3] = ".......";
            field[4] = "OO.....";
            field[5] = "OO.....";

            var watch = System.Diagnostics.Stopwatch.StartNew();
            field = Play(3, field);
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }

        private static string[] Play(int turns, string[] field)
        {
            int rows = field.Length;
            int columns = field[0].Length;
            int[][] playField;
            int[][] initialField = null;
            int numberCycles = 0;
            turns = turns % 32;

            playField = ConvertStringFieldToNumberField(field, rows);
            initialField = ConvertStringFieldToNumberField(field, rows);

            for (int turn = 2; turn <= turns; turn++)
            {
                numberCycles++;
                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        if (turn % 2 == 1)
                        {
                            if (playField[row][column] == 2)
                            {
                                ExplodeChain(playField, row, column);
                            }
                        }
                        else
                        {
                            playField[row][column]++;
                        }
                    }
                }
                if (Match(playField, initialField))
                {
                    turns = turns % numberCycles;
                    turn = 0;
                    numberCycles = 0;
                }
            }
            return ConvertNumberFieldToStringField(playField);
        }

        private static void PlantAll(int[][] playField)
        {
            for (int i = 0; i < playField.Length; i++)
                for (int j = 0; j < playField[j].Length; j++)
                    playField[i][j] = 1;
        }

        private static bool Match(int[][] playField, int[][] initialField)
        {
            bool match = false;
            for (int i = 0; i < playField.Length; i++)
            {
                if (Enumerable.SequenceEqual(playField[i], initialField[i]))
                {
                    match = true;
                }
                else
                {
                    return false;
                }
            }
            return match;

        }

        private static string[] ConvertNumberFieldToStringField(int[][] playField)
        {
            List<string> playFieldString = new List<string>();
            for (int row = 0; row < playField.Length; row++)
            {
                playFieldString.Add(ConvertNumberToStringLine(playField[row]));
            }
            return playFieldString.ToArray();
        }

        private static string ConvertNumberToStringLine(int[] line)
        {
            StringBuilder lineString = new StringBuilder();
            for (int iteration = 0; iteration < line.Length; iteration++)
            {
                lineString.Append(line[iteration] == 0 ? '.' : 'O');
            }
            return lineString.ToString();
        }

        private static void ExplodeChain(int[][] playField, int row, int column)
        {
            if (playField[row][column] == 2)
            {
                playField[row][column] = 0;
                if (row - 1 >= 0)
                    Explode(playField, row - 1, column);
                if (row + 1 < playField.Length)
                    Explode(playField, row + 1, column);

                if (column - 1 >= 0)
                    Explode(playField, row, column - 1);
                if (column + 1 < playField[0].Length)
                    Explode(playField, row, column + 1);
            }
        }

        private static void Explode(int[][] playField, int row, int column)
        {
            if (playField[row][column] == 1)
            {
                playField[row][column] = 0;
            }
        }

        private static int[][] ConvertStringFieldToNumberField(string[] field, int rows)
        {
            List<int[]> newPlayField = new List<int[]>();
            for (int iteration = 0; iteration < field.Length; iteration++)
            {
                newPlayField.Add(ConvertStringToNumberLine(field[iteration]));
            }
            return newPlayField.ToArray(); ;
        }

        private static int[] ConvertStringToNumberLine(string line)
        {
            List<int> newLine = new List<int>();
            for (int iteration = 0; iteration < line.Length; iteration++)
            {
                newLine.Add(line[iteration] == 'O' ? 1 : 0);
            }
            return newLine.ToArray();
        }
    }
}