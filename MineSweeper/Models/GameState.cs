using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MineSweeper.Models
{
    public class GameState
    {
        public Board Board { get; set; }
        public TimeSpan Timer { get; set; }
        public int RevealedCellsCount { get; set; }
        public GameState()
        {
            Board = new Board();
        }
    }
}
