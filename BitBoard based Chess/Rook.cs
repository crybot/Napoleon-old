using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    static class Rook
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BitBoard GetAllTargets(byte pieceColor, BitBoard rooks, Board board)
        {
            BitBoard occupiedSquares = board.OccupiedSquares;
            BitBoard targets = Constants.Empty;
            int square;

            square = BitBoard.BitScanForward(rooks);

            targets |= MovePackHelper.GetRankAttacks(occupiedSquares, square);
            targets |= MovePackHelper.GetFileAttacks(occupiedSquares, square);

            return targets & ~board.GetPlayerPieces();


        }
    }
}
