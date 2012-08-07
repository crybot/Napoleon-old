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
            board.LoadGame("8/6bb/8/8/R1pP2k1/4P3/P7/K7 b - d3 0 1");

            //BitBoard pawns = board.GetPieceSet(PieceColor.White, PieceType.Pawn);

            //BitBoard.Display(pawns);
            //Console.ReadKey();
            //Console.Clear();

            //board.MakeMove(new Move(8, 16, PieceType.Pawn, PieceType.None, PieceType.None));

            //pawns = board.GetPieceSet(PieceColor.White, PieceType.Pawn);

            //BitBoard.Display(pawns);
            //Console.ReadKey();
            //Console.Clear();

            //Console.ReadKey();

            Console.Write("Tempo Impiegato: {0}", PerformanceTester.TestPerformance(() => { Test(board); }));

            Console.ReadKey();
        }

        private static void Test(Board board)
        {
            MoveList moves = MoveGenerator.GetAllMoves(PieceColor.Black, board);

            short i, k, l, m, n, p;

            for (i = 0; i < moves.Lenght; i++)
            {
                Console.WriteLine(moves.moves[i].ToAlgebraic());
            }
            Console.WriteLine(i);

            //for (i = 0; i < moves.Lenght; i++)
            //{
            //    for (k = 0; k < moves.Lenght; k++)
            //    {
            //        for (l = 0; l < moves.Lenght; l++)
            //        {
            //            for (m = 0; m < moves.Lenght; m++)
            //            {
            //                for (n = 0; n < moves.Lenght; n++)
            //                {
            //                    MoveGenerator.GetAllMoves(PieceColor.White, board);

            //                    for (p = 0; p < moves.Lenght; p++)
            //                    {
            //                        //if (BitBoard.IsAttacked(square, PieceColor.Black, board))
            //                        //{

            //                        //}

            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

        }
    }
}
