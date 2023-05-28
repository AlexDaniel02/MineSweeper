using MineSweeper.Commands;
using MineSweeper.Models;
using MineSweeper.Managers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MineSweeper.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private int _revealedCellsCount;
        private ImageSource _resetImage;
        private bool _isFlagChecked;
        private SerializationManager _serializationManager;
        private Board _board;
        private DispatcherTimer _timer;
        public ICommand SaveGameCommand { get; set; }
        public ICommand OpenAboutCommand { get; set; }
        public ICommand CellClickedCommand { get; set; }
        public ICommand ResetGameCommand { get; set; }
        public GameState GameState { get; set; }       
        public ImageSource ResetImage
        {
            get
            {
                return _resetImage;
            }
            set
            {

                _resetImage = value;
                OnPropertyChanged();
            }
        }
        public int RevealedCellsCount
        {
            get
            {
                return _revealedCellsCount;
            }
            set
            {
                _revealedCellsCount = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan ElapsedTime
        {
            get { return GameState.Timer; }
            set
            {
                GameState.Timer = value;
                OnPropertyChanged();
            }
        }
        public Board Board
        {
            get
            {
                return _board;
            }
            set
            {
                _board = value;
                OnPropertyChanged();
            }
        }
        public bool IsFlagChecked
        {
            get
            {
                return _isFlagChecked;
            }
            set
            {
                _isFlagChecked = value;
                OnPropertyChanged();
            }
        }
        
        public GameViewModel()
        {
            Initialize(0, 0);          
        }
        public GameViewModel(int boardWidth, int boardHeight)
        {
            Initialize(boardWidth, boardHeight);
            GameManager.GenerateMap(Board);
            GameManager.GenerateBombs(Board);
            GameManager.CalculateNumbers(Board);
            StartTimer();
        }
        private void Initialize(int mapWidth, int mapHeight)
        {
            GameState = new GameState();
            Board = new Board()
            {
                Height = mapHeight,
                Width = mapWidth
            };
            _serializationManager = new SerializationManager(this);
            CellClickedCommand = new GenericRelayCommand<int>(CellClicked);
            SaveGameCommand = new RelayCommand(SaveGame);
            OpenAboutCommand = new RelayCommand(OpenAbout);
            ResetGameCommand = new RelayCommand(ResetGame);
            ResetImage = ImagesManager.ImagesList[13];
        }
        public void StartTimer()
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += TimerTick;
            }
            if (!_timer.IsEnabled)
            {
                _timer.Start();
            }
        }
        private void TimerTick(object sender, EventArgs e)
        {
            ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(1));
        }
        private void ResetGame()
        {
            _timer.Start();
            RevealedCellsCount = 0;
            Initialize(Board.Width, Board.Height);
            GameManager.GenerateMap(Board);
            GameManager.GenerateBombs(Board);
            GameManager.CalculateNumbers(Board);
        }
        private void OpenAbout()
        {
            MessageBox.Show("Created by Stoica Alexandru");
        }
        private void SaveGame()
        {
            GameState.RevealedCellsCount = RevealedCellsCount;
            GameState.Board = Board;
            GameState.Timer = ElapsedTime;
            _serializationManager.SaveGame();
            ExitGame();
        }
        private void CheckGameStatus()
        {
            if (RevealedCellsCount >= Board.Width * Board.Height - Board.BombsNumber)
            {
                _timer.Stop();
                ResetImage = ImagesManager.ImagesList[14];
                GameManager.RevealBombs(Board, true);
                MessageBox.Show("Congratulations! You won the game in " + ElapsedTime.ToString(@"mm\:ss") + "!");
                ExitGame();
            }
        }
        private void RevealAdjacentCells(int index)
        {
            Cell adjacentCell;
            int row = index / Board.Width;
            int column = index % Board.Width;
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = column - 1; j <= column + 1; j++)
                {
                    if (i < 0 || i >= Board.Height || j < 0 || j >= Board.Width)
                    {
                        continue;
                    }
                    int adjacentIndex = i * Board.Width + j;
                    adjacentCell = Board.Grid[adjacentIndex];
                    if (adjacentCell.IsRevealed || adjacentCell.IsMine)
                    {
                        continue;
                    }
                    RevealedCellsCount++;
                    adjacentCell.IsRevealed = true;
                    adjacentCell.IsEnabled = false;
                    if (adjacentCell.Number != 0)
                    {
                        adjacentCell.Image = ImagesManager.ImagesList[adjacentCell.Number];
                    }
                    else
                    {
                        adjacentCell.Image = ImagesManager.ImagesList[10];
                        RevealAdjacentCells(adjacentIndex);
                    }
                    CheckGameStatus();
                }
            }
        }
        private void CellClicked(int index)
        {
            if (Board.Grid[index].IsFlagged && !IsFlagChecked)
            {
                return;
            }
            if (IsFlagChecked)
            {
                FlagCell(index);
            }
            else
            {
                RevealCell(index);
            }
        }
        private void FlagCell(int index)
        {
            if (Board.Grid[index].IsFlagged)
            {
                Board.Grid[index].IsFlagged = false;
                Board.Grid[index].Image = ImagesManager.ImagesList[0];
            }
            else
            {
                Board.Grid[index].IsFlagged = true;
                Board.Grid[index].Image = ImagesManager.ImagesList[9];
            }
        }
        private void MineClicked(int index)
        {
            Board.Grid[index].Image = ImagesManager.ImagesList[12];
            _timer.Stop();
            ResetImage = ImagesManager.ImagesList[15];
            GameManager.RevealBombs(Board, false);
            Board.Grid[index].Image = ImagesManager.ImagesList[12];
            MessageBox.Show("Game over! You Lost!");
            ExitGame();
        }
        private void RevealCell(int index)
        {
            Board.Grid[index].IsRevealed = true;
            Board.Grid[index].IsEnabled = false;
            RevealedCellsCount++;
            if (Board.Grid[index].IsMine)
            {
                MineClicked(index);
            }
            else if (Board.Grid[index].Number == 0)
            {
                Board.Grid[index].Image = ImagesManager.ImagesList[10];
                RevealAdjacentCells(index);
            }
            else if (Board.Grid[index].Number != 0)
            {
                Board.Grid[index].Image = ImagesManager.ImagesList[Board.Grid[index].Number];
            }
            CheckGameStatus();
        }
        private void ExitGame()
        {
            var gameWindow = Application.Current.Windows.OfType<Window>()
                        .SingleOrDefault(w => w.DataContext == this);
            gameWindow?.Close();
        }
    }
}
