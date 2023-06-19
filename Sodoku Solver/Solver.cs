using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using Sudoku_Solver;
using System.Windows.Forms;



// demo of this in action found here
// https://www.youtube.com/watch?v=XGkZtz7pf7A



namespace Sudoku_Solver
{
    internal class Solver
    {

        // constants

        const int SCAN_SIZE = 50;

        const int TILE_OFFSET = 3;
        const int GAME_SIZE = 495;
        const int TILE_SIZE = 55;


        // static variables

        static Point gameLocation;

        static List<List<int>> board;

        // colors

        static Color LINE_COLOR = Color.FromArgb(255, 52, 72, 97);
        static Color SELECTED_TILE_COLOR = Color.FromArgb(255, 187, 222, 251);
        static Color SELECTED_NUMBER_COLOR = Color.FromArgb(255, 195, 215, 234);
        static Color RESTRICTED_TILE_COLOR = Color.FromArgb(255, 226, 235, 243);
        static Color TILE_COLOR = Color.FromArgb(255, 255, 255, 255);

        static ScreenReader s;

        static List<List<Point>> NUMBER_RESTRICTIONS;


        // MAIN

        static void Main()
        {
            Console.WriteLine("Running Sudoku Solver");

            Console.WriteLine("Initializing");
            initBoard();
            initNumberRestrictions();

            gameLocation = CursorManager.findColorNearCursor(LINE_COLOR, SCAN_SIZE, SCAN_SIZE);

            Console.WriteLine("Press Any Key To Start");
            Console.ReadKey();

            CursorManager.click(gameLocation.X + 650, gameLocation.Y + 460, "left");
            Thread.Sleep(250);

            CursorManager.click(gameLocation.X + 650, gameLocation.Y + 330, "left");
            Thread.Sleep(250);
            

            Console.WriteLine("Reading Board");
            readBoard();

            Console.WriteLine("Board Read");
            printBoard();

            Console.WriteLine("Solving");
            if (!solve(0))
            {
                Console.WriteLine("No Solution Found");
                Console.WriteLine();
                Console.WriteLine("Finished");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Solution Found");
            printBoard();

            Console.WriteLine("Entering Solution");
            enterSolution();

            Console.WriteLine();
            Console.WriteLine("Finished");
            Console.ReadKey();
            return;
        }



        static void readBoard()
        {
            s = new ScreenReader(gameLocation.X + TILE_OFFSET, gameLocation.Y + TILE_OFFSET, GAME_SIZE, GAME_SIZE);
            s.captureScreenshot();

            readNumbers();
        }



        static void readNumbers()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    parseNumber(i, j);

                }
            }

        }



        static void parseNumber(int i, int j)
        {
            Color c;
            bool valid_number;

            for (int k = 8; k >= 0; k--)
            {
                valid_number = true;

                for (int l = 0; l < NUMBER_RESTRICTIONS[k].Count; l++)
                {
                    c = s.screenshot.GetPixel(i * TILE_SIZE + NUMBER_RESTRICTIONS[k][l].X, j * TILE_SIZE + NUMBER_RESTRICTIONS[k][l].Y);

                    if (c.Equals(SELECTED_TILE_COLOR) ||
                        c.Equals(SELECTED_NUMBER_COLOR) ||
                        c.Equals(RESTRICTED_TILE_COLOR) ||
                        c.Equals(TILE_COLOR))
                    {
                        valid_number = false;
                        break;
                    }
                }

                if (valid_number)
                {
                    board[j][i] = k + 1;
                    return;
                }
            }

            return;
        }


        static void initBoard()
        {
            board = new List<List<int>>();

            for (int i = 0; i < 9; i++)
                board.Add(new List<int>());

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    board[i].Add(0);

            return;
        }



        static void initNumberRestrictions()
        {
            NUMBER_RESTRICTIONS = new List<List<Point>>();

            for (int i = 0; i < 9; i++)
                NUMBER_RESTRICTIONS.Add(new List<Point>());

            NUMBER_RESTRICTIONS[0].Add(new Point(28, 14));
            NUMBER_RESTRICTIONS[0].Add(new Point(28, 20));
            NUMBER_RESTRICTIONS[0].Add(new Point(28, 38));

            NUMBER_RESTRICTIONS[1].Add(new Point(22, 14));
            NUMBER_RESTRICTIONS[1].Add(new Point(22, 34));
            NUMBER_RESTRICTIONS[1].Add(new Point(33, 38));

            NUMBER_RESTRICTIONS[2].Add(new Point(18, 18));
            NUMBER_RESTRICTIONS[2].Add(new Point(18, 35));
            NUMBER_RESTRICTIONS[2].Add(new Point(28, 25));

            NUMBER_RESTRICTIONS[3].Add(new Point(17, 31));
            NUMBER_RESTRICTIONS[3].Add(new Point(28, 14));
            NUMBER_RESTRICTIONS[3].Add(new Point(30, 32));

            NUMBER_RESTRICTIONS[4].Add(new Point(19, 26));
            NUMBER_RESTRICTIONS[4].Add(new Point(20, 13));
            NUMBER_RESTRICTIONS[4].Add(new Point(32, 13));

            NUMBER_RESTRICTIONS[5].Add(new Point(17, 27));
            NUMBER_RESTRICTIONS[5].Add(new Point(24, 23));
            NUMBER_RESTRICTIONS[5].Add(new Point(33, 17));

            NUMBER_RESTRICTIONS[6].Add(new Point(19, 14));
            NUMBER_RESTRICTIONS[6].Add(new Point(23, 35));
            NUMBER_RESTRICTIONS[6].Add(new Point(33, 14));

            NUMBER_RESTRICTIONS[7].Add(new Point(20, 28));
            NUMBER_RESTRICTIONS[7].Add(new Point(20, 23));
            NUMBER_RESTRICTIONS[7].Add(new Point(26, 25));
            NUMBER_RESTRICTIONS[7].Add(new Point(31, 23));

            NUMBER_RESTRICTIONS[8].Add(new Point(19, 26));
            NUMBER_RESTRICTIONS[8].Add(new Point(19, 36));
            NUMBER_RESTRICTIONS[8].Add(new Point(26, 29));
            NUMBER_RESTRICTIONS[8].Add(new Point(32, 34));

            return;
        }



        static void printBoard()
        {
            Console.WriteLine();

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    int n = board[y][x];
                    Console.Write((n == 0 ? " " : n + "") + " " + (x == 2 || x == 5 ? "| " : ""));
                }

                Console.WriteLine(y == 2 || y == 5 ? "\n------+-------+------" : "");
            }

            Console.WriteLine();
            return;
        }



        static bool solve(int t)
        {
            bool b;

            if (t == 81) return true;

            if (board[t / 9][t % 9] != 0)
                return solve(t + 1);

            for (int i = 1; i <= 9; i++)
            {
                if (!validPosition(i, t)) continue;

                board[t / 9][t % 9] = i;

                b = solve(t + 1);

                if (b) return true;
            }

            board[t / 9][t % 9] = 0;
            return false;
        }



        static bool validPosition(int n, int t)
        {
            int x = t % 9;
            int y = t / 9;

            // check column
            for (int i = 0; i < 9; i++)
                if (board[i][x] == n)
                    return false;

            // check row
            for (int i = 0; i < 9; i++)
                if (board[y][i] == n)
                    return false;

            // check box
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[(y / 3) * 3 + i][(x / 3) * 3 + j] == n)
                        return false;

            return true;
        }



        static void enterSolution()
        {
            CursorManager.click(gameLocation.X + 10, gameLocation.Y + 10, "left");

            int d = 1;

            for (int i = 0; i < 81; i++)
            {
                SendKeys.SendWait("{" + board[i / 9][d == 1 ? i % 9 : 8 - i % 9] + "}");

                if (i % 9 == 8)
                {
                    SendKeys.SendWait("{DOWN}");
                    d *= -1;
                }
                else
                    SendKeys.SendWait(d == 1 ? "{RIGHT}" : "{LEFT}");
            }

            return;
        }
    }
}
