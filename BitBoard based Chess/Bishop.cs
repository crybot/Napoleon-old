using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    static class Bishop
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BitBoard GetAllTargets(byte pieceColor, BitBoard bishops, Board board)
        {
            BitBoard occupiedSquares = board.OccupiedSquares;
            BitBoard targets = Constants.Empty;
            int square;

            square = BitBoard.BitScanForward(bishops);

            targets |= MovePackHelper.GetA1H8DiagonalAttacks(occupiedSquares, square);
            targets |= MovePackHelper.GetH1A8DiagonalAttacks(occupiedSquares, square);

            return targets & ~board.GetPlayerPieces();
        }
    }
}
