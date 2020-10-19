using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Transactions;
using System.Xml;

namespace Maze
{
    class Shit
    {
        public static void WriteASentence(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void GameSettings()
        {
            WriteASentence(ConsoleColor.Cyan, "Здравствуйте,вас приветствует игра Maze ");
            WriteASentence(ConsoleColor.Cyan, "Управление на WASD ");
            WriteASentence(ConsoleColor.Cyan, "У нас есть рулетка,при 10% вы можете выиграть ");
            WriteASentence(ConsoleColor.Cyan, "В остальном случае вы проиграете");
            WriteASentence(ConsoleColor.Cyan, "Рулетка работает при вводе 'f' ");
        }
        public static void Input(char symbol)
        {
                if (symbol == 'W' || symbol == 'w') Move.dY = -1;
                if (symbol == 'S' || symbol == 's') Move.dY = +1;
                if (symbol == 'D' || symbol == 'd') Move.dX = +1;
                if (symbol == 'A' || symbol == 'a') Move.dX = -1;
        }
    }
    class Game
    {
        static bool isGameEnded()
        { 
            return Move.reached;
        }
        public static void Generate()
        {
            MazeGeneration.GenerateField();
            Player.PlaceDog();
        }

        static void Main(string[] args)
        {
            Shit.GameSettings();
            Generate();
            MazeGeneration.Draw();
            while (!isGameEnded())
            {  
                Move.GetInput();
                Move.Logic();
                MazeGeneration.Draw();  
            }
        }
    }
    class Move
    {
        public static int dX = 0, dY = 0;
        public static bool reached;
        public static void Logic()
        {
            Shit.GameSettings();
            TryToGo(Player.dogX + dX, Player.dogY + dY);     
            Finish();
        }
        public static (int, int) GetInput()
        {
            (dX, dY) = (0, 0);
            char symbol = char.Parse(Console.ReadLine());
            Shit.Input(symbol);
            Console.Clear();
            if (symbol == 'F'|| symbol == 'f') Roulette();
            return (dX, dY);
        }
        static void Roulette()
        {
            Random rand = new Random();
            if (rand.Next(0, 101) >= 90)
            {
                Player.dogX = MazeGeneration.finishX;
                Player.dogY = MazeGeneration.finishY;
            }
            else
            {
                Shit.WriteASentence(ConsoleColor.Red, "Поздравляем,вы проиграли!");
                reached = true;
            }
        }
        static void Finish()
        {
            if (Player.dogX == MazeGeneration.finishX && Player.dogY == MazeGeneration.finishY)
            {
                Shit.WriteASentence(ConsoleColor.Yellow, "Поздравляем,вы прошли!");
                reached = true;
            }
        }
        static void TryToGo(int newX, int newY)
        {
            if (CanGoTo(newX, newY))
                GoTo(newX, newY);
        }
        static bool IsWalkable(int X, int Y)
        {
            if (MazeGeneration.field[Y, X] == '#')
            {
                Shit.WriteASentence(ConsoleColor.Red, "СТЕНА");
                return false;
            }
            return true;
        }
        static bool CanGoTo(int newX, int newY)
        {
            if (newX < 0 || newY < 0 || newX >= MazeGeneration.width || newY >= MazeGeneration.height)
                return false;
            if (!IsWalkable(newX, newY))
                return false;
            return true;
        }
        static void GoTo(int newX, int newY)
        {
            (Player.dogX, Player.dogY) = (newX, newY);
        }
    }
    class Player
    { 
        public static char dog = '@';
        public static int dogX = 0, dogY = 0;

        public static void PlaceDog()
        {
            Random rand = new Random();
            dogX = rand.Next(0, MazeGeneration.width - 1);
            dogY = rand.Next(0, MazeGeneration.height - 1); ;
        }
    }
    public class MazeGeneration
    {
         public static int width = 10;
         public static int height = 10;
         public static char[,] field = new char[height, width];
         static int block_freq = 30;
         static char symbol;
         public static int finishX, finishY;
         
        public static void GenerateField()
        {
            for (int i = 0; i < height; i++)
            {
                field[i, 0] = ' ';
                for (int j = 0; j < width; j++)
                {
                    Random randd = new Random();
                    
                    if(randd.Next(0, 101) < block_freq)
                    {
                        symbol = '#';
                    }
                    else
                    {
                        symbol = '.';
                    }
                    field[i, j] = symbol;
                }
            }
            Random rand = new Random();
            finishX = rand.Next(0, width - 1);
            finishY = rand.Next(0, height - 1);
            field[finishY, finishX] = 'O';
        }
        public static void Draw()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if(i == Player.dogY && j == Player.dogX)
                        symbol = Player.dog;
                    else
                        symbol = field[i, j];
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }
    }
}
