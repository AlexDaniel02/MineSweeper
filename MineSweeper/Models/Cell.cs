using MineSweeper.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace MineSweeper.Models
{
    public class Cell:BaseModel
    {
        public int Number { get; set; }
        private bool _isMine;
        public bool IsFlagged { get; set; }
        private bool _isEnabled;
        public bool IsRevealed;
        [XmlIgnore]
        private ImageSource _image;
        public int Index { get; set; }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                    _isEnabled = value;
                    OnPropertyChanged();               
            }
        }
        public bool IsMine
        {
            get { return _isMine; }
            set
            {
                _isMine = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public ImageSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }
        public Cell()
        {
            IsEnabled = true;
            IsRevealed = false;
            IsMine = false;
            IsFlagged = false;
            Image = ImagesManager.ImagesList[0];
        }
    }
}
  