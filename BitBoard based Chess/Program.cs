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

            //Move move1 = new Move(12, 28, PieceType.Pawn, board.pieceSet[28].Type, PieceType.None);
            //Move move2 = new Move(51, 35, PieceType.Pawn, board.pieceSet[35].Type, PieceType.None);
            //Move move3 = new Move(28, 35, PieceType.Pawn, board.pieceSet[35].Type, PieceType.None);


            //BitBoard.Display(board.GetPieceSet(board.SideToMove.GetOpposite(), PieceType.Pawn));

            //Console.ReadKey();
            //Console.Clear();

            //move1 = new Move(23, 38, PieceType.Knight, board.pieceSet[38].Type, PieceType.None);
            //board.MakeMove(move1);

            //BitBoard.Display(board.GetPieceSet(board.SideToMove, PieceType.Pawn));

            //Console.ReadKey();
            //Console.Clear();

            //board.Display();
            //Console.ReadKey();
            //Console.Clear();

            //board.UndoMove(move1);
            //board.Display();

            //Console.ReadKey();
            //Console.Clear();

            //move2 = new Move(51, 35, PieceType.Pawn, board.pieceSet[35].Type, PieceType.None);
            //board.MakeMove(move2);

            //board.Display();
            //Console.ReadKey();
            //Console.Clear();

            //move3 = new Move(28, 35, PieceType.Pawn, board.pieceSet[35].Type, PieceType.None);
            //board.MakeMove(move3);

            //board.Display();
            //Console.ReadKey();
            //Console.Clear();


            //board.UndoMove(move3);

            //board.Display();
            //Console.ReadKey();
            //Console.Clear();

            //board.UndoMove(move2);

            //board.Display();
            //Console.ReadKey();
            //Console.Clear();

            //board.UndoMove(move1);

            //board.Display();
            //Console.ReadKey();
            //Console.Clear();

            Stopwatch watch = Stopwatch.StartNew();

            //Divide(board.SideToMove, 1, board);
            Console.WriteLine(Perft(PieceColor.White, 5, board));

            watch.Stop();
            Console.WriteLine("Catture: {0}", cap);
            Console.WriteLine("Tempo Impiegato: {0}", watch.ElapsedMilliseconds);
            Console.ReadKey();

            //Console.ReadKey();
        }


        private static int cap = 0;

        private static int Perft(byte color, int depth, Board board)
        {
            MoveList moves = new MoveList();
            MoveGenerator.GetLegalMoves(color, moves.moves, ref moves.Length, board);

            int nodes = 0;

            if (depth == 1)
            {
                return moves.Length;
            }

            if (depth == 0)
                return 1;

            //if (moves.Length == 0)
            //    return 1;

            for (int i = 0; i < moves.Length; i++)
            {
                //if (moves.moves[i].IsCapture())
                //    cap++;
                board.MakeMove(moves.moves[i]);
                nodes += Perft(color.GetOpposite(), depth - 1, board);
                board.UndoMove(moves.moves[i]);
            }

            return nodes;

        }

        private static void Divide(byte color, int depth, Board board)
        {
            int total = 0;
            int temp = 0;
            int NumMoves = 0;

            MoveList moves = new MoveList();
            MoveGenerator.GetLegalMoves(color, moves.moves, ref moves.Length, board);

            Console.WriteLine("Move\tNodes");

            for (int i = 0; i < moves.Length; i++)
            {
                board.MakeMove(moves.moves[i]);
                temp = Perft(color.GetOpposite(), depth - 1, board);
                total += temp;
                Console.WriteLine("{0} {1} ", moves.moves[i].ToAlgebraic(), temp);
                board.UndoMove(moves.moves[i]);
                NumMoves++;
            }

            Console.WriteLine("Total Nodes: {0}", total);
            Console.WriteLine("Moves: {0}", NumMoves);

        }

        private static void Test(Board board)
        {
            //MoveList moves = MoveGenerator.GetAllMoves(PieceColor.White, board);

            //short i, k, l, m, n, p;

            ////for (i = 0; i < moves.Lenght; i++)
            ////{
            ////    Console.WriteLine(moves.moves[i].ToAlgebraic());
            ////}
            ////Console.WriteLine(i);

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
