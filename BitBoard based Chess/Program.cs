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

            UInt64 all = board.GetAllPieces();

            Console.Write("Tempo Impiegato: {0}", PerformanceTester.TestPerformance(() => { Test(board); }));

            Console.ReadKey();
        }

        private static void Test(Board board)
        {
            List<Move> moves = MoveGenerator.GetAllMoves(PieceColor.White, board);
            byte i, k, l, m, n, p;

            for (i = 0; i < moves.Count; i++)
            {
                for (k = 0; k < moves.Count; k++)
                {
                    for (l = 0; l < moves.Count; l++)
                    {
                        for (m = 0; m < moves.Count; m++)
                        {
                            for (n = 0; n < moves.Count; n++)
                            {
                                MoveGenerator.GetAllMoves(PieceColor.White, board);
                                for (p = 0; p < moves.Count; p++)
                                {
                                    
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
