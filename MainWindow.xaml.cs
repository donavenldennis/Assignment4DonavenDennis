using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Assignment4DonavenDennis.PlayTicTacToe;

namespace Assignment4DonavenDennis
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region AppGlobalVarables
        /// <summary>
        /// This region will contain values used by the whole
        /// Application
        /// </summary>
        public bool InGame = false; //denotes that is in current game
        public bool PlayerVSComputer = false; //denotes that it is P vs C
        public bool IsXTurn = true; //Tracks Turns
        public bool XIsWinner = true;//Tracks who is winner if there is one
        //Will be used to denote boxes of winning move
        public int TurnsTaken = 0;
        public string[] WinningBoxesButtons = { "Button", "Button", "Button" }; 
        private PlayTicTacToe Game = new PlayTicTacToe();//Initialize Game Logic Object
        #endregion

        #region InisalizeTicTacToeApp
        /// <summary>
        /// Will initialize Application and print starting directions
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            GameStatus.Text = "Please Select a Game Mode";
        }
        #endregion

        #region Button_Click        
        /// <summary>
        /// This Will Handle all Events for the Application by using the 
        /// sending button to call appropriate function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (InGame && ((Button)sender).Content == "") // if InGame is set to true GameBoard is available
            {
                PlayerTakesTurn(sender); //calls function when player takes their turn
                return;
            }
            //calls for game selection
            if(sender == PlayerVsPlayerButtton || sender == PlayerVsComputerButton) GameSelect(sender);
        }
        #endregion

        #region GameSelect
        /// <summary>
        /// This function will call the appropriate functions to reset the game board to prepare for
        /// a new game. It will also set PlayerVSCompueter if said button is sender
        /// </summary>
        /// <param name="sender"></param>
        public void GameSelect(object sender)
        {
            if (sender == PlayerVsComputerButton)
            { 
                PlayerVSComputer = true;
            }
            StartNewGame(); // run processes to start game
        }
        #endregion

        #region StartNewGame
        /// <summary>
        /// This will reset the game board for new game and disable game select buttons
        /// </summary>
        public void StartNewGame()
        {
            Dis_EnableButton(PlayerVsPlayerButtton, false);
            Dis_EnableButton(PlayerVsComputerButton, false);
            ResetAllGameButtons();
            InGame = true;
            WinningBoxesButtons[0] = "Button";
            WinningBoxesButtons[1] = "Button";
            WinningBoxesButtons[2] = "Button";
            TurnsTaken = 0;
            Game.StartGameAndResetBoard();
        }
        #endregion

        #region TurnTaken 
        /// <summary>
        /// Will call function to update sender appearance to reflect player action with a X or O
        /// and background color change in sending grid box and store the move in the PlayTicTacToe
        /// Class Game Object
        /// 
        /// Then check if there is a winner. If there, InGame status will be changed to false
        /// and function will be called to adjust appearance of board to reflect winning move
        /// and Print message.
        /// 
        /// If no winner found it will check if there are any more open spaces. if there are
        /// not function to changes appearance of board will be called to reflect a tie and InGame
        /// Status will be changed to false
        /// 
        /// if move available and game is PVP, it will return to falling function so that 
        /// next move can be made by the next player
        /// 
        /// If game is vs a computer a function will be called to preform computers turn
        /// </summary>
        /// <param name="sender"></param>
        public void PlayerTakesTurn(Object sender)
        {
            TurnsTaken++;
            PlacePlayerSymbol(sender);//Change Box Appearance
            EncodeMove(sender);//Store Player Move
            int [,] WinningMove = Game.CheckForWinner(IsXTurn);//Check For Winner and store move if won
            if (WinningMove[0,0] != -1) InGame = false; //End Game if won
            if (!InGame)
            {
                XIsWinner = IsXTurn;
                IfWin(WinningMove); //EndGame
                return;
            }
            if(TurnsTaken == 9)
            {
                InGame = false;
                IfTie();
                return;
            }
            IsXTurn = !IsXTurn;
            if(PlayerVSComputer && !IsXTurn)    
            {
                int [] ComputerMoveCode = Game.ComputerPlayer();
                PlayerTakesTurn(DecodeComputerMove(ComputerMoveCode));
                if (!InGame)
                { 
                    IsXTurn = true;
                    return; 
                }
            }
            if (IsXTurn)
            {
                GameStatus.Text = "It is now X's Turn";
            }
            else
            {
                GameStatus.Text = "It is now O's Turn";
            }
        }
        #endregion

        #region PlacePlayerSymble
        /// <summary>
        /// while update appearance of sender for the given players turn
        /// </summary>
        /// <param name="sender"></param>
        public void PlacePlayerSymbol(Object sender)
        {
            if (IsXTurn) //update sender if X's turn, then return
            {
                ((Button)sender).Background = Brushes.LightBlue;
                ((Button)sender).Foreground = Brushes.Blue;
                ((Button)sender).Content = "X";
                return;
            }
            //update sender if O's (not X's turn)
            ((Button)sender).Background = Brushes.Pink;
            ((Button)sender).Foreground = Brushes.Red;
            ((Button)sender).Content = "O";            
        }

        #endregion

        #region EncodeMove
        /// <summary>
        /// This will call the PlayTicTacToe.PlayerTurn with the appropriate arguments
        /// </summary>
        /// <param name="sender"></param>
        public void EncodeMove(Object sender)
        {
            switch (((Button)sender).Name)
            {
                case "Button00":
                    Game.PlayerTurn(IsXTurn, 0, 0);
                    break;
                case "Button01":
                    Game.PlayerTurn(IsXTurn, 0, 1);
                    break;
                case "Button02":
                    Game.PlayerTurn(IsXTurn, 0, 2);
                    break;
                case "Button10":
                    Game.PlayerTurn(IsXTurn, 1, 0);
                    break;
                case "Button11":
                    Game.PlayerTurn(IsXTurn, 1, 1);
                    break;
                case "Button12":
                    Game.PlayerTurn(IsXTurn, 1, 2);
                    break;
                case "Button20":
                    Game.PlayerTurn(IsXTurn, 2, 0);
                    break;
                case "Button21":
                    Game.PlayerTurn(IsXTurn, 2, 1);
                    break;
                case "Button22":
                    Game.PlayerTurn(IsXTurn, 2, 2);
                    break;
            }
        }
        #endregion

        #region ResetAllGameButtons
        /// <summary>
        /// Resets all game buttons
        /// </summary>
        public void ResetAllGameButtons()
        {
            ResetBox(Button00);
            ResetBox(Button01);
            ResetBox(Button02);
            ResetBox(Button10);
            ResetBox(Button11);
            ResetBox(Button12);
            ResetBox(Button20);
            ResetBox(Button21);
            ResetBox(Button22);
        }
        #endregion

        #region ResetBox
        /// <summary>
        /// Will reset sender appearance for new game
        /// </summary>
        /// <param name="box"></param>
        private void ResetBox(Object sender) 
        {
            if (!InGame)
            {
                ((Button)sender).Background = null;
                ((Button)sender).Content = "";
                Dis_EnableButton(sender, true);
                return;
            }
        }
        #endregion

        #region Dis_EnableButton
        /// <summary>
        /// Will except bool and change button IsEnabled Game to provided bool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="enable"></param>
        public void Dis_EnableButton(Object sender, bool enable)
        { 
            if(enable)
            {
                ((Button)sender).IsEnabled = true;
                return;
            }
            ((Button)sender).IsEnabled = false;
        }
        #endregion

        #region IfWin
        /// <summary>
        /// This Method will handle the processes in the case of a win for any player
        /// </summary>
        /// <param name="WinningMove"></param>
        public void IfWin(int[,] WinningMove)
        {
            int[] NotWinningBoxes = DecodeWin(WinningMove);
            DisableAllNonWinGameButton(NotWinningBoxes);
            Dis_EnableButton(PlayerVsPlayerButtton, true);
            Dis_EnableButton(PlayerVsComputerButton, true);
            ShowWinningMove();
            if (XIsWinner == true)
            {
                PlayerXWinCounter.Text = (Int32.Parse(PlayerXWinCounter.Text) + 1).ToString();
                GameStatus.Text = "X's Win! \nPlease Select a Game Mode to play again";
                return;
            }
            PlayerOWinCounter.Text = (Int32.Parse(PlayerOWinCounter.Text) + 1).ToString();
            GameStatus.Text = "O's Win! \nPlease Select a Game Mode to play again";  
            if(PlayerVSComputer) PlayerVSComputer = !PlayerVSComputer;
        }
        #endregion

        #region DecodeWin
        /// <summary>
        /// This will create an array of the boxes that are not part of the winning move
        /// and change the WinningBoxesButtons to the names of the winning boxes
        /// it will then return the array with the winning moves changed to -1
        /// </summary>
        /// <param name="WinningMove"></param>
        /// <returns></returns>
        public int[] DecodeWin(int [,] WinningMove)
        {
            int[] NotWinningBoxes = {0, 1, 2, 3, 4, 5, 6, 7, 8};
            int move = 0;
            int next = (WinningMove[move, 0] * 3) + WinningMove[move, 1];
            foreach(int i in NotWinningBoxes)
            {
                if (i == next) 
                {
                    NotWinningBoxes[i] = -1;
                    WinningBoxesButtons[move] = WinningBoxesButtons[move] 
                        + WinningMove[move, 0].ToString() 
                        + WinningMove[move, 1].ToString();
                    move++;
                    if(move < 3) next = (WinningMove[move, 0] * 3) + WinningMove[move, 1];
                }
            }
            return NotWinningBoxes;
        }
        #endregion

        #region DisableAllNonWinGameButton
        /// <summary>
        /// Will disable all game buttons that are not wining buttons
        /// </summary>
        public void DisableAllNonWinGameButton(int[] NonWin)
        {
            foreach (int i in NonWin)
            {
                switch (i)
                {
                    case 0:
                        Dis_EnableButton(Button00, false);
                        break;
                    case 1:
                        Dis_EnableButton(Button01, false);
                        break;
                    case 2:
                        Dis_EnableButton(Button02, false);
                        break;
                    case 3:
                        Dis_EnableButton(Button10, false);
                        break;
                    case 4:
                        Dis_EnableButton(Button11, false);
                        break;
                    case 5:
                        Dis_EnableButton(Button12, false);
                        break;
                    case 6:
                        Dis_EnableButton(Button20, false);
                        break;
                    case 7:
                        Dis_EnableButton(Button21, false);
                        break;
                    case 8:
                        Dis_EnableButton(Button22, false);
                        break;
                    default: break;
                }
            }
        }
        #endregion

        #region ShowWinningMove
        /// <summary>
        /// Function will change background of winning move
        /// </summary>
        public void ShowWinningMove()
        {
            foreach(string box in WinningBoxesButtons)
            {
                switch(box) 
                {
                    case "Button00":
                        ((Button)Button00).Background = Brushes.Green;
                        break;
                    case "Button01":
                        ((Button)Button01).Background = Brushes.Green;
                        break;
                    case "Button02":
                        ((Button)Button02).Background = Brushes.Green;
                        break;
                    case "Button10":
                        ((Button)Button10).Background = Brushes.Green;
                        break;
                    case "Button11":
                        ((Button)Button11).Background = Brushes.Green;
                        break;
                    case "Button12":
                        ((Button)Button12).Background = Brushes.Green;
                        break;
                    case "Button20":
                        ((Button)Button20).Background = Brushes.Green;
                        break;
                    case "Button21":
                        ((Button)Button21).Background = Brushes.Green;
                        break;
                    case "Button22":
                        ((Button)Button22).Background = Brushes.Green;
                        break;
                }
            }
        }
        #endregion

        #region IfTie
        /// <summary>
        /// Will handle tie Cases
        /// </summary>
        public void IfTie()
        {
            TieCounter.Text = (Int32.Parse(TieCounter.Text) + 1).ToString();
            GameStatus.Text = "Oh, it's a tie. \nPlease Select a Game Mode to play again";
            Dis_EnableButton(PlayerVsPlayerButtton, true);
            Dis_EnableButton(PlayerVsComputerButton, true);
        }
        #endregion

        #region DecodeComputerMove
        /// <summary>
        /// Will decode Computer Move and return the corresponding object
        /// </summary>
        /// <param name="moveCode"></param>
        /// <returns></returns>
        public object DecodeComputerMove(int[] moveCode)
        {
            object box;
            string move = "Button" + moveCode[0].ToString() + moveCode[1].ToString();
            switch(move) 
            {
                case "Button00":
                    box = Button00;
                    break;
                case "Button01":
                    box = Button01;
                    break;
                case "Button02":
                    box = Button02;
                    break;
                case "Button10":
                    box = Button10;
                    break;
                case "Button11":
                    box = Button11;
                    break;
                case "Button12":
                    box = Button12;
                    break;
                case "Button20":
                    box = Button20;
                    break;
                case "Button21":
                    box = Button21;
                    break;
                default:
                    box = Button22;
                    break;
            }
            return box;
        }
        #endregion
    }
}