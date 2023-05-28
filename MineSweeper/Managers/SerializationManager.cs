using Microsoft.Win32;
using MineSweeper.Models;
using MineSweeper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MineSweeper.Managers
{
    public class SerializationManager
    {
        private GameViewModel _gameViewModel;
        public SerializationManager(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
        }
        public void SaveGame()
        {

            string fileName = $"MineSweeper{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xml";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            var serializer = new XmlSerializer(typeof(GameState));
            SerializeToFile(_gameViewModel.GameState, fileName);

        }
        private void SerializeToFile<T>(T data, string fileName)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, data);
            }
        }
        public void OpenSaveFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                Title = "Open Database"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var gameState = DeserializeFromXml<GameState>(openFileDialog.FileName);
                if (gameState != null)
                {
                    _gameViewModel.GameState = gameState;
                    _gameViewModel.RevealedCellsCount = gameState.RevealedCellsCount;
                    _gameViewModel.Board = gameState.Board;
                    GameManager.LoadImages(_gameViewModel.Board);
                    _gameViewModel.ResetImage = ImagesManager.ImagesList[13];
                    _gameViewModel.StartTimer();
                }
            }
        }
        public T DeserializeFromXml<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filePath))
            {
                string xmlContent = reader.ReadToEnd();
                using (StringReader stringReader = new StringReader(xmlContent))
                {
                    return (T)serializer.Deserialize(stringReader);
                }
            }
        }
    }
}
