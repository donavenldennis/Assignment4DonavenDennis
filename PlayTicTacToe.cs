using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Assignment4DonavenDennis
{
  

    internal class PlayTicTacToe
    {
        #region PossableGridValues
        /// <summary>
        /// This enumeration provides strict possible game grid values
        /// </summary>
       
        #endregion

        #region PlayTicTacToeGlobalVariables
        /// <summary>
        /// This Region will contain the values which are used by the whole
        /// PLayTicTacToe Class
        /// </summary>
        public PlayBoxStatus[,] GameGrid = {
            {0, 0, 0},
            {0, 0, 0},
            {0, 0, 0}
            };
        int[,,] WinConditions = {
            {{0, 0}, {0, 1}, {0, 2}},
            {{1, 0}, {1, 1}, {1, 2}},
            {{2, 0}, {2, 1}, {2, 2}},
            {{0, 0}, {1, 0}, {2, 0}},
            {{0, 1}, {1, 1}, {2, 1}},
            {{0, 2}, {1, 2}, {2, 2}},
            {{0, 0}, {1, 1}, {2, 2}},
            {{0, 2}, {1, 1}, {2, 0}},
            };
        public enum PlayBoxStatus { N = 0, X = 1, O = 2 }
        #endregion

        #region PrivateGetSet
        /// <summary>
        /// PrivateGetters and Setters for the Box Statuses
        /// </summary>
        private PlayBoxStatus N { get; set; }
        private PlayBoxStatus X { get; set; }
        private PlayBoxStatus O { get; set; }
        #endregion

        #region StartGame
        /// <summary>
        /// This region is for reseting the game board upon a game mode selection
        /// </summary>
        public void StartGameAndResetBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    GameGrid[i, j] = PlayBoxStatus.N;
                }
            }
        }
        #endregion

        #region PlayerTurn
        /// <summary>
        /// Take Turns Will Handle the logic for Changing the Back end Game Board Logic
        /// </summary>       
        public void PlayerTurn (bool IsXTurn, int MoveA, int MoveB)
        {
            if(IsXTurn)
            {
                GameGrid[MoveA, MoveB] = PlayBoxStatus.X;
            }
            else
            {
                GameGrid[MoveA, MoveB] = PlayBoxStatus.O;
            }

        }
        #endregion

        #region CheckForWinner
        /// <summary>
        /// This take a bool value representing the current players turn and check against each win
        /// option to see if prior move won the game
        /// </summary> 
        public int[,] CheckForWinner(bool IsXTurn)
        {
            PlayBoxStatus PlayerChecking = PlayBoxStatus.X; //set player to default
            if(!IsXTurn) PlayerChecking = PlayBoxStatus.O; //Change player if not default
            int[,] WinnerGrid = {{-1, -1}, {-1, -1}, {-1, -1}}; //set WinnerGrid to default
            int i = 0; //initialize outer counter
            int j = 0; //initialize inner counter
            int BoxCorrect = 0; //initialize correct box counter
            bool IsWinner = false; //initialize that no winner has been found yet
            while (!IsWinner && i < 8) //repeat until outer counter passes max or winner is found
            { 
                //if box matches winner
                if(GameGrid[WinConditions[i,j,0], WinConditions[i, j, 1]] == PlayerChecking)
                {
                    BoxCorrect++; //increment correct counter
                    WinnerGrid[j, 0] = WinConditions[i, j, 0]; //set Winner Grid to correct row
                    WinnerGrid[j, 1] = WinConditions[i, j, 1]; //set Winner Grid to correct column
                    if(BoxCorrect == 3) IsWinner = true; //if all three correct boxes found set IsWinner to true
                }
                j++;
                j = j % 3; //reset inner counter if reaches max
                if (j == 0) 
                {
                    BoxCorrect = 0; //reset correct counter
                    i++; //iterate outer counter
                }
            }
            if (!IsWinner) //if no winner found mark first value in winner grid -1 to signal no winner
            {
                WinnerGrid[0, 0] = -1;
            }
            return WinnerGrid; //return winner grid
        }
        #endregion

        #region ComputerPlayer
        /// <summary>
        /// Will first Check if there is a move than Computer can make that will lead to a win.
        /// If it exists the computer will return said move
        /// 
        /// If no such move exists Computer Will then check if there is a move that the player can make to
        /// win. If it exists the computer will return it to block player
        /// 
        /// If no such move exists, computer will return a random empty space
        /// </summary>
        /// <returns></returns>
        public int[] ComputerPlayer() 
        {
            int[] ComputerCanWin = IfCanWin(PlayBoxStatus.O);
            if (ComputerCanWin[0] != -1) return ComputerCanWin;

            int[] PlayerCanWin = IfCanWin(PlayBoxStatus.X);
            if (PlayerCanWin[0] != -1) return PlayerCanWin;

            int a = -1;
            int b = -1;
            do
            {
                Random rnd = new Random();
                a = rnd.Next(3);
                b = rnd.Next(3);
            }
            while (GameGrid[a, b] != PlayBoxStatus.N);
            return [a, b];
        }
        #endregion

        #region IfCanWin
        /// <summary>
        /// Will take a given player and check if they can win in the next move
        /// </summary>
        /// <param name="Player"></param>
        /// <returns></returns>
        private int[] IfCanWin(PlayBoxStatus Player)
        {
            int[] WinMove= { -1, -1 };
            int PlayerOwned = 0;
            for (int i = 0; i < 8 && PlayerOwned < 2 ; i++) 
            {
                WinMove[0] = -1;
                WinMove[1] = -1;
                PlayerOwned = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (GameGrid[WinConditions[i, j, 0], WinConditions[i, j, 1]] == PlayBoxStatus.N)
                    {
                        WinMove[0] = WinConditions[i, j, 0];
                        WinMove[1] = WinConditions[i, j, 1];
                    }
                    else if (GameGrid[WinConditions[i, j, 0], WinConditions[i, j, 1]] == Player)
                    {
                        PlayerOwned++;
                    }
                    else
                    {
                        PlayerOwned--;
                    }
                }
            }
            if(PlayerOwned != 2) WinMove[0] = -1;
            return WinMove;
        }
        #endregion
    }
}
