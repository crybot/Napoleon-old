using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading.Tasks;
using Performance.FrameWork;

namespace BitBoard_based_Chess
{
    internal sealed class Program
    {
        //TESTER
        static void Main(string[] args)
        {
            Board board = new Board();
            board.Equip();

            //BitBoard square;
            //BitBoard targets = Constants.Empty;

            //for (int i = 0; i < 64; i++)
            //{
            //    square = BitBoard.SquareMask(i);

            //    if (board.isAttacked(square, PieceColor.Black))
            //    {
            //        targets |= square;
            //    }
            //}

            //BitBoard.Display(targets);
            //Console.ReadKey();

            Console.Write("Tempo Impiegato: {0}", PerformanceTester.TestPerformance(() => { Test(board); }));

            Console.ReadKey();
        }

        private static void Test(Board board)
        {
            List<Move> moves = MoveGenerator.GetAllMoves(PieceColor.White, board);
            byte i, k, l, m, n, p;
            BitBoard square;

            for (i = 0; i < moves.Count; i++)
            {
                square = Constants.SquareMask[i];
                for (k = 0; k < 18; k++)
                {
                    for (l = 0; l < 18; l++)
                    {
                        for (m = 0; m < 18; m++)
                        {
                            for (n = 0; n < 18; n++)
                            {
                                MoveGenerator.GetAllMoves(PieceColor.White, board);
                                for (p = 0; p < 18; p++)
                                {
                                    //if (BitBoard.IsAttacked(square, PieceColor.Black, board))
                                    //{

                                    //}
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
