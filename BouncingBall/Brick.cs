//William Marciello and Matthew Dwyer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// Brushes
using System.Windows.Media;
using System.Drawing;

namespace BouncingBall
{
    public class Brick : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _brickName;
        public string BrickName
        {
            get { return _brickName; }
            set
            {
                _brickName = value;
                OnPropertyChanged("BrickName");
            }
        }

        private string _brickLabel;
        public string BrickLabel
        {
            get { return _brickLabel; }
            set
            {
                _brickLabel = value;
                OnPropertyChanged("BrickLabel");
            }
        }

        private System.Windows.Media.Brush _brickBrush;
        public System.Windows.Media.Brush BrickBrush
        {
            get { return _brickBrush; }
            set
            {
                _brickBrush = value;
                OnPropertyChanged("BrickBrush");
            }
        }

        private System.Windows.Media.Brush _brickBackground = System.Windows.Media.Brushes.Coral;
        public System.Windows.Media.Brush BrickBackground
        {
            get { return _brickBackground; }
            set
            {
                _brickBackground = value;
                OnPropertyChanged("BrickBackground");
            }
        }

        private int _brickHeight;
        public int BrickHeight
        {
            get { return _brickHeight; }
            set
            {
                _brickHeight = value;
                OnPropertyChanged("BrickHeight");
            }
        }

        private int _brickWidth;
        public int BrickWidth
        {
            get { return _brickWidth; }
            set
            {
                _brickWidth = value;
                OnPropertyChanged("BrickWidth");
            }
        }

        private System.Windows.Visibility _brickVisible;
        public System.Windows.Visibility BrickVisible
        {
            get { return _brickVisible; }
            set
            {
                _brickVisible = value;
                OnPropertyChanged("BrickVisible");
            }
        }

        private Rectangle _brickRectangle;
        public Rectangle BrickRectangle
        {
            get { return _brickRectangle; }
            set { _brickRectangle = value; }
        }

        private double _brickCanvasTop;
        public double BrickCanvasTop
        {
            get { return _brickCanvasTop; }
            set
            {
                _brickCanvasTop = value;
                OnPropertyChanged("BrickCanvasTop");
            }
        }

        private double _brickCanvasLeft;
        public double BrickCanvasLeft
        {
            get { return _brickCanvasLeft; }
            set
            {
                _brickCanvasLeft = value;
                OnPropertyChanged("BrickCanvasLeft");
            }
        }

        private System.Windows.Media.Brush _brickFill;
        public System.Windows.Media.Brush BrickFill
        {
            get { return _brickFill; }
            set
            {
                _brickFill = value;
                OnPropertyChanged("BrickFill");
            }
        }
        



    }
}
