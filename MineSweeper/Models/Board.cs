using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Models
{
    public class Board : BaseModel
    {
        private ObservableCollection<Cell> _grid;
        private int _bombsNumber;
        public ObservableCollection<Cell> Grid
        {
            get
            {
                return _grid;
            }
            set
            {
                _grid = value;
                OnPropertyChanged();
            }
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public int BombsNumber { get; set; }
        public Board()
        {
            Grid = new ObservableCollection<Cell>();
        }
    }
    public enum BoardDifficulty
    {
        Beginner,
        Intermediate,
        Expert
    }


}
