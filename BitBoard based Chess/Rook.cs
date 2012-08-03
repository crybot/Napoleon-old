using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    static class Rook
    {
        private static BitBoard occupiedSquares;
        private static BitBoard targets;
        private static int square;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BitBoard GetAllTargets(PieceColor pieceColor, BitBoard rooks, Board board)
        {
            occupiedSquares = board.GetAllPieces();
            targets = Constants.Empty;

            while (rooks != 0)
            {
                square = BitBoard.BitScanForwardReset(ref rooks);

                targets |= MovePackHelper.GetRankAttacks(occupiedSquares, square);
                targets |= MovePackHelper.GetFileAttacks(occupiedSquares, square);
            }

            return targets & ~board.GetPlayerPieces(pieceColor);
        }
    }
}
