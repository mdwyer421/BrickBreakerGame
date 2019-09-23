//Matthew Dwyer and William Marciello

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Timers;

// observable collections
using System.Collections.ObjectModel;

// debug output
using System.Diagnostics;

// timer, sleep
using System.Threading;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

// hi res timer
using PrecisionTimers;

// Rectangle
// Must update References manually
using System.Drawing;

// INotifyPropertyChanged
using System.ComponentModel;


namespace BouncingBall
{
    public partial class Model : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private static UInt32 _numBalls = 1;
        private UInt32[] _buttonPresses = new UInt32[_numBalls];
        Random _randomNumber = new Random();
        private TimerQueueTimer.WaitOrTimerDelegate _ballTimerCallbackDelegate;
        private TimerQueueTimer.WaitOrTimerDelegate _paddleTimerCallbackDelegate;

        private TimerQueueTimer.WaitOrTimerDelegate _timerAllDelegate;

        private TimerQueueTimer _ballHiResTimer;
        private TimerQueueTimer _paddleHiResTimer;

        private TimerQueueTimer _timerAll;
        public double _timePassed = 0;
        public int ticks = 0;

        private double _ballXMove = 1;
        private double _ballYMove = 1;
        System.Drawing.Rectangle _ballRectangle;
        System.Drawing.Rectangle _paddleRectangle;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        int numRow = 5;
        int numCol = 15;

       
        public ObservableCollection<Brick> BrickCollection;
        private static UInt32 _numBricks = 24;
        Rectangle[] _brickRectangles = new Rectangle[_numBricks];
        int _brickHeight = 25;
        int _brickWidth = 50; //make bricks bigger, ball smaller
        System.Windows.Media.Brush FillColorRed;



        //hanlding of score

        private int _score = 0; // to parse the int to a string
        public int score
        {
            get { return _score; }
            set { _score = value; }
        }


        private String _scored = ""; //handles the scoring
        public String Scored
        {
            get { return _scored; }
            set {
                _scored = value;
                OnPropertyChanged("Scored");
                                         }
            
        }


        //handling of the time

        private int _gameTime = 0;// to parse the int to a string
        public int GameTime
        {
            get { return _gameTime; }
            set { _gameTime = value; }

        }


        private String _GameTimed = "";// handles the gametime
        public String Gametimed
        {
            get { return _GameTimed; }
            set
            {
                _GameTimed = value;
                OnPropertyChanged("Gametimed");
            }

        }
          



        private bool _moveBall = false;
        public bool MoveBall
        {
            get { return _moveBall; }
            set { _moveBall = value; }
        }

        private double _windowHeight = 100;
        public double WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

        private double _windowWidth = 100;
        public double WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
            SolidColorBrush mySolidColorBrushRed = new SolidColorBrush();
            mySolidColorBrushRed.Color = System.Windows.Media.Color.FromRgb(255, 0, 0);
            FillColorRed = mySolidColorBrushRed;
        }

        public void InitModel()
        {
            // this delegate is needed for the multi media timer defined 
            // in the TimerQueueTimer class.
            _ballTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(BallMMTimerCallback);
            _paddleTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(paddleMMTimerCallback);

         
            _timerAllDelegate = new TimerQueueTimer.WaitOrTimerDelegate(allTimeMMTimerCallback); //---------------------------------

           
            _timerAll = new TimerQueueTimer(); // we need to show the runtime for the game
            try
            {
                _timerAll.Create(5, 5, _ballTimerCallbackDelegate);
               // tick++;
                //if (tick > 1000) {
                //    GameTime++;
                //    Gametimed = GameTime.ToString(); }                
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create Ball timer. Error from GetLastError = {0}", ex.Error);
            }


            // create our multi-media timers
            _ballHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _ballHiResTimer.Create(5, 5, _ballTimerCallbackDelegate);
                
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create Ball timer. Error from GetLastError = {0}", ex.Error);
            }



            _paddleHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _paddleHiResTimer.Create(2, 2, _paddleTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create paddle timer. Error from GetLastError = {0}", ex.Error);
            }

            BrickCollection = new ObservableCollection<Brick>();
            for (int i = 0; i < numCol; i++)
            {
                for (int j = 0; j < numRow; j++)
                {
                    BrickCollection.Add(new Brick()
                    {
                        //BrickRectangle
                        BrickHeight = _brickHeight,
                        BrickFill = FillColorRed,
                        BrickWidth = _brickWidth,
                        BrickName = i.ToString(),
                        BrickVisible = System.Windows.Visibility.Visible,
                        BrickBackground = System.Windows.Media.Brushes.Red,


                        BrickCanvasLeft = i * _brickWidth,
                        BrickCanvasTop = j * _brickHeight, // offset the bricks from the top of the screen by a bitg

                        BrickRectangle = new Rectangle(i * _brickWidth, j * _brickHeight, _brickWidth - 1, _brickHeight - 1)
                    });
                }




            }
            UpdateRects();



        }



        private void CheckPush() //----------------------------------------------------------------------------
        {
            for (int brick = 0; brick < numCol*numRow; brick++)
            {
                if (BrickCollection[brick].BrickVisible == Visibility.Hidden) continue;

                InterectSide whichSide = IntersectsAt(BrickCollection[brick].BrickRectangle, _ballRectangle);
                switch (whichSide)
                {
                    case InterectSide.NONE:
                      //  BrickCollection[brick].BrickVisible = Visibility.Hidden;
                      //  score++;
                        break;

                    case InterectSide.TOP:
                        BrickCollection[brick].BrickVisible = Visibility.Hidden;
                        _ballYMove *= -1;  
                        score++;
                        Scored = score.ToString();
                        break;

                    case InterectSide.BOTTOM:
                        BrickCollection[brick].BrickVisible = Visibility.Hidden;
                        _ballYMove *= -1;                   
                        score++;
                        Scored = score.ToString();
                        break;

                    case InterectSide.LEFT:
                        BrickCollection[brick].BrickVisible = Visibility.Hidden;
                        _ballXMove *= -1;
                        score++;
                        Scored = score.ToString();
                        break;

                    case InterectSide.RIGHT:
                        BrickCollection[brick].BrickVisible = Visibility.Hidden;
                        _ballXMove *= -1;
                        score++;
                        Scored = score.ToString();
                        break;
                }
            }
        }

        public void resetBrickVisibility()
        {
            for (int brick = 0; brick < numCol * numRow; brick++)
            {
                BrickCollection[brick].BrickVisible = Visibility.Visible;
            }
        }

        enum InterectSide { NONE, LEFT, RIGHT, TOP, BOTTOM };
        private InterectSide IntersectsAt(Rectangle brick, Rectangle ball)
        {
            if (brick.IntersectsWith(ball) == false)

                return InterectSide.NONE;

            Rectangle r = Rectangle.Intersect(brick, ball);

            // did we hit the top of the brick
            if (ball.Top + ball.Height - 1 == r.Top &&
                r.Height == 1)
                return InterectSide.TOP;

            if (ball.Top == r.Top &&
                r.Height == 1)
                return InterectSide.BOTTOM;

            if (ball.Left == r.Left &&
                r.Width == 1)
                return InterectSide.RIGHT;

            if (ball.Left + ball.Width - 1 == r.Left &&
                r.Width == 1)
                return InterectSide.LEFT;

            return InterectSide.NONE;
        }
        
       private void UpdateRects()
        {
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
            for (int brick = 0; brick < _numBricks; brick++)
               BrickCollection[brick].BrickRectangle = new System.Drawing.Rectangle((int)BrickCollection[brick].BrickCanvasLeft,
                (int)BrickCollection[brick].BrickCanvasTop, (int)BrickCollection[brick].BrickWidth, (int)BrickCollection[brick].BrickHeight);
        }


        public void CleanUp()
        {
            _ballHiResTimer.Delete();
            _paddleHiResTimer.Delete();
        }


        public void SetStartPosition()
        {
            
            BallHeight = 25;
            BallWidth = 25;
            paddleWidth = 120;
            paddleHeight = 10;

            GameTime = 0;
            Gametimed = GameTime.ToString();
            score = 0;
            Scored = score.ToString();
            resetBrickVisibility();

            ballCanvasLeft = _windowWidth/2 - BallWidth/2;
            ballCanvasTop = _windowHeight/2;
           
            _moveBall = false;

            paddleCanvasLeft = _windowWidth / 2 - paddleWidth / 2;
            paddleCanvasTop = _windowHeight - paddleHeight;
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
        }

        public void MoveLeft(bool move)
        {
            _movepaddleLeft = move;
        }

        public void MoveRight(bool move)
        {
            _movepaddleRight = move;
        }

      


        private void BallMMTimerCallback(IntPtr pWhat, bool success)
        {

            if (!_moveBall)
                return;

             ticks++;
            if (ticks > 400) {
                GameTime++;
                ticks = 0;
                Gametimed = GameTime.ToString(); }   


            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_ballHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }


            ballCanvasLeft += _ballXMove;
            ballCanvasTop += _ballYMove;


            // check to see if ball has it the left or right side of the drawing element
            if ((ballCanvasLeft + BallWidth >= _windowWidth) ||
                (ballCanvasLeft <= 0))
                _ballXMove = -_ballXMove;


            // check to see if ball has it the top of the drawing element
            if ( ballCanvasTop <= 0) 
                _ballYMove = -_ballYMove;

            if (ballCanvasTop + BallWidth >= _windowHeight)
            {
                // we hit bottom. stop moving the ball
                _moveBall = false;
            }

            // see if we hit the paddle
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
            if (_ballRectangle.IntersectsWith(_paddleRectangle))
            {
                // hit paddle. reverse direction in Y direction
                _ballYMove = -_ballYMove;

                // move the ball away from the paddle so we don't intersect next time around and
                // get stick in a loop where the ball is bouncing repeatedly on the paddle
                ballCanvasTop += 2*_ballYMove;

                // add move the ball in X some small random value so that ball is not traveling in the same 
                // pattern
                ballCanvasLeft += _randomNumber.Next(5);
            }

            CheckPush();
            // 1 seconnd timer for game runtime       
            //UpdateRects();
            // done in callback. OK to delete timer
            _ballHiResTimer.DoneExecutingCallback();
        }

        private void paddleMMTimerCallback(IntPtr pWhat, bool success)
        {

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_paddleHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            if (_movepaddleLeft && paddleCanvasLeft > 0)
                paddleCanvasLeft -= 2;
            else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                paddleCanvasLeft += 2;
            
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);


            // done in callback. OK to delete timer
            _paddleHiResTimer.DoneExecutingCallback();

        }


 /*       private void homeMadeTimer() {
            while (_moveBall)
            {
                if (ticks > 1000)
                {
                    GameTime++;
                    Gametimed = GameTime.ToString();
                }

            }


        } */


        private void allTimeMMTimerCallback(IntPtr pWhat, bool success)
        {

            if (!_moveBall)
                return;
            if(_moveBall) { ticks++; }


             if (ticks > 1000) {
                GameTime++;
                Gametimed = GameTime.ToString(); }  


            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_timerAll.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

         
            if (ballCanvasTop + BallWidth >= _windowHeight)
            {
                // we hit bottom. stop moving the ball reset the timer
                ticks = 0;
            }




            _timerAll.DoneExecutingCallback();
        }

  

    }
}
