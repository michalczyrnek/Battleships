
﻿using BattleshipsWar.UI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BattleshipWarTest")]

namespace BattleshipsWar
{
    public class StartGame
    {
        public CellProperty[,] PlayerOneBoard = new CellProperty[10, 10];
        public CellProperty[,] PlayerTwoBoard = new CellProperty[10, 10];

        public List<Ship> PlayerOneShips = new List<Ship>(); 
        public List<Ship> PlayerTwoShips = new List<Ship>();

        private bool AllShipsPlaced = false;
        private bool NextPlayer = false;

        private int CounterOfShipsPlaced = 0;

        private int[] Coords = { -1, -1 };

        public (CellProperty[,], CellProperty[,]) PlaceShips()
        {
            string placement = "", direction;
            while (AllShipsPlaced == false)
            {
             
                if (NextPlayer == false)
                {
                    UserCommunication(out placement, out direction, "First");
                    PlayerOneBoard = PlaceShipOnBoard(PlayerOneBoard, placement, direction);

                    ActionGameUI.DrawBoard(PlayerOneBoard);

                    if (CounterOfShipsPlaced == 7)
                    {
                        CounterOfShipsPlaced = 0;
                        NextPlayer = true;
                        AnyKeyToContinue();
                        ActionGameUI.DrawBoard(PlayerTwoBoard);
                    }

                }
                else
                {
                    UserCommunication(out placement, out direction, "Second");
                    PlayerTwoBoard = PlaceShipOnBoard(PlayerTwoBoard, placement, direction);
                    ActionGameUI.DrawBoard(PlayerTwoBoard);

                    if (CounterOfShipsPlaced == 7)
                    {
                        AllShipsPlaced = true;
                    }
                }
            }

            AnyKeyToContinue();
            return (PlayerOneBoard, PlayerTwoBoard);
        }

        private static void AnyKeyToContinue()
        {
            Console.Write("Click any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        private static void UserCommunication(out string placement, out string direction, string user)
        {
            Console.Write($"{user} Player, please choose where you want start build ship:");
            placement = Console.ReadLine();
            Console.Write("Choose ship direction (Up, right, Down, Left):");
            direction = Console.ReadLine();
            Console.Clear();
        }

        internal CellProperty[,] PlaceShipOnBoard(CellProperty[,] board, string placement, string direction)
        {
            InputParser check = new InputParser();
            Coords = check.ChangeCordsToIndexes(placement);

            if (Coords[0] == -1 || Coords[1] == -1)
            {
                Console.WriteLine("Wrong coordinates!\n\n");
                return board;
            }

            direction = direction.ToLower();
            Direction userChoice;

            switch (direction)
            {
                case "up":
                    userChoice = Direction.Up;
                    break;
                case "right":
                    userChoice = Direction.Right;
                    break;
                case "down":
                    userChoice = Direction.Down;
                    break;
                case "left":
                    userChoice = Direction.Left;
                    break;
                default:
                    Console.WriteLine("Wrong direction!\n\n");
                    return board;
            }

            switch (CounterOfShipsPlaced)
            {
                case 0:
                    {
                        KindOfShip lengthOfShip = KindOfShip.Six;
                        FillTheBoard(board, userChoice, lengthOfShip);
                        break;
                    }
                case 1:
                case 2:
                    {
                        KindOfShip lengthOfShip = KindOfShip.Four;
                        FillTheBoard(board, userChoice, lengthOfShip);
                        break;
                    }
                case 3:
                case 4:
                    {
                        KindOfShip lengthOfShip = KindOfShip.Three;
                        FillTheBoard(board, userChoice, lengthOfShip);
                        break;
                    }
                case 5:
                case 6:
                    {
                        KindOfShip lengthOfShip = KindOfShip.Two;
                        FillTheBoard(board, userChoice, lengthOfShip);
                        break;
                    }
            }
                    

            CounterOfShipsPlaced++;


            return board;
        }

        private void FillTheBoard(CellProperty[,] board, Direction userChoice, KindOfShip lengthOfShip)
        {
            if (CounterOfShipsPlaced > 0)
            {
                lengthOfShip = KindOfShip.Four;
                if (CounterOfShipsPlaced > 2)
                {
                    lengthOfShip = KindOfShip.Three;
                    if (CounterOfShipsPlaced > 4)
                    {
                        lengthOfShip = KindOfShip.Two;
                    }
                }
            }

            Ship ship = new Ship(lengthOfShip, Coords, userChoice);
            if (NextPlayer == false)
            {
                PlayerOneShips.Add(ship);
            }
            else
            {
                PlayerTwoShips.Add(ship);
            }

            int[] coordsToChange = { -1, -1 };

            int canNotBePlaced = 0;
            bool validCoordinates = false;

            Scanner scan = new Scanner();

            for (int i = 0; i < ship.Coords.Count; i++)
            {
                coordsToChange = ship.Coords[i];

                validCoordinates = scan.CheckCoordinatesCorrectness(coordsToChange, board);

                if (validCoordinates == true)
                {
                    for (int row = -1; row <= 1; row++)
                    {
                        for (int column = -1; column <= 1; column++)
                        {
                            try
                            {
                                if (board[coordsToChange[0] + row, coordsToChange[1] + column] != CellProperty.Empty)
                                {
                                    canNotBePlaced++;
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }

            if (canNotBePlaced <= 0 && validCoordinates == true)
            {
                for (int j = 0; j < ship.Coords.Count; j++)
                {
                    coordsToChange = ship.Coords[j];

                    try
                    {
                        board[coordsToChange[0], coordsToChange[1]] = CellProperty.Occupied;
                    }
                    catch
                    {
                        Console.WriteLine("Ship couldn't be build. It was either: near another ship, on another ship or out of board range.");

                        CounterOfShipsPlaced--;

                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Ship couldn't be build. It was either: near another ship, on another ship or out board range.\n\n");

                CounterOfShipsPlaced--;
            }
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    }
}

