using MineSweeper.Commands;
using MineSweeper.Managers;
using MineSweeper.Models;
using MineSweeper.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MineSweeper.ViewModels
{
    public class GameMenuViewModel : BaseViewModel
    {
        private List<ImageSource> _imagesList;
        private int _boardWidth;
        private int _boardHeight;
        private GameViewModel _gameViewModel;
        private BoardDifficulty _selectedDifficulty;
        private SerializationManager _serializationManager;
        public ICommand SelectDifficultyCommand { get; set; }
        public ICommand LoadGameCommand {   get; set; }
        public ICommand StartGameCommand { get; set; }
        public BoardDifficulty BoardDifficulty { get; set; }
        public int MapHeight { get; set; }
        
        public List<ImageSource> ImagesList
        {
            get
            {
                return _imagesList;
            }
            set
            {
                _imagesList = value;
                OnPropertyChanged();
            }
        }
        public BoardDifficulty SelectedDifficulty
        {
            get {
                return _selectedDifficulty;
            }
            set {
                _selectedDifficulty = value;
                OnPropertyChanged();
            }
        }
        
        public GameMenuViewModel()
        {
            SelectDifficultyCommand = new RelayCommand(SelectDifficulty);
            StartGameCommand = new RelayCommand(StartGame);
            LoadGameCommand = new RelayCommand(OpenSaveFile);
            ImagesManager.LoadImages();
            ImagesList = ImagesManager.ImagesList;
            _gameViewModel = new GameViewModel();
            _serializationManager = new SerializationManager(_gameViewModel);
        }
        public void SelectDifficulty ()
        {
            switch (SelectedDifficulty)
            { 
                case BoardDifficulty.Beginner:
                    _boardWidth = 9;
                    _boardHeight = 9;
                    break;
                case BoardDifficulty.Intermediate:
                    _boardWidth = 16;
                    _boardHeight = 16;
                    break;
                case BoardDifficulty.Expert:
                    _boardHeight = 16;
                    _boardWidth = 30;
                    break;
            }
        }
        private void StartGame()
        {
            SelectDifficulty();
            if (_boardWidth != 0)
            {
                GameViewModel gameViewModel = new GameViewModel(_boardWidth,_boardHeight);
                new GameView { DataContext = gameViewModel }.Show();
            }
            else
            {
                MessageBox.Show("You need to select a board size!");
            }
        }
        private void OpenSaveFile()
        {
            _serializationManager.OpenSaveFile();            
            new GameView { DataContext = _gameViewModel }.Show();
        }
    }
}
