using MineSweeper.Models;
using System;
using System.Windows.Media;

namespace MineSweeper.Managers
{
    public static class GameManager
    {
        private static Random _random = new Random();
        public static void GenerateMap(Board board)
        {
            CalculateBombsNumber(board);
            for (int i = 0; i < board.Width * board.Height; i++)
            {
                board.Grid.Add(new Cell() { Index = i });
            }
        }
        public static void CalculateBombsNumber(Board board)
        {
            switch (board.Width)
            {
                case 9:
                    board.BombsNumber = 10;
                    break;
                case 16:
                    board.BombsNumber = 40;
                    break;
                case 30:
                    board.BombsNumber = 99;
                    break;
            }
        }
        public static void GenerateBombs(Board board)
        {
            int bombsPlaced = 0;
            while (bombsPlaced < board.BombsNumber)
            {
                int index = _random.Next(board.Grid.Count);

                if (!board.Grid[index].IsMine)
                {
                    board.Grid[index].IsMine = true;
                    bombsPlaced++;
                }
            }
        }
        public static void CalculateNumbers(Board board)
        {
            for (int i = 0; i < board.Grid.Count; i++)
            {
                if (!board.Grid[i].IsMine)
                {
                    int count = GetAdjacentBombCount(board, i);
                    board.Grid[i].Number = count;
                }
            }
        }
        public static int GetAdjacentBombCount(Board board, int index)
        {
            int count = 0;
            int row = index / board.Width;
            int column = index % board.Width;

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = column - 1; j <= column + 1; j++)
                {
                    if (!(i >= 0 && i < board.Height && j >= 0 && j < board.Width))
                    {
                        continue;
                    }
                    int adjacentIndex = i * board.Width + j;
                    if (board.Grid[adjacentIndex].IsMine)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public static int CountRevealedCells(Board board)
        {
            int count = 0;
            foreach (var cell in board.Grid)
            {
                if (cell.IsRevealed)
                {
                    count++;
                }
            }
            return count;
        }
        public static void LoadImages(Board board)
        {
            foreach (var cell in board.Grid)
            {
                if (cell.IsFlagged)
                {
                    cell.Image = ImagesManager.ImagesList[9];
                    continue;
                }
                if (cell.IsRevealed == false)
                {
                    continue;
                }
                if (cell.IsMine)
                {
                    continue;
                }

                if (cell.Number == 0)
                {
                    cell.Image = ImagesManager.ImagesList[10];
                }
                else
                {
                    cell.Image = ImagesManager.ImagesList[cell.Number];
                }
            }
        }
        public static void RevealBombs(Board board, bool gameWon)
        {

            ImageSource image;
            if (gameWon)
            {
                image = ImagesManager.ImagesList[9];
            }
            else
            {
                image = ImagesManager.ImagesList[11];
            }
            foreach (var cell in board.Grid)
            {
                if (cell.IsMine == true)
                {
                    cell.IsRevealed = true;
                    cell.IsEnabled = false;
                    cell.Image = image;
                }
            }
        }
    }
}
