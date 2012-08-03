using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    static class Bishop
    {
        private static BitBoard occupiedSquares;
        private static BitBoard targets;
        private static int square;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BitBoard GetAllTargets(PieceColor pieceColor, BitBoard bishops, Board board)
        {
            occupiedSquares = board.GetAllPieces();
            targets = Constants.Empty;

            while (bishops != 0)
            {
                square = BitBoard.BitScanForwardReset(ref bishops);

                targets |= MovePackHelper.GetA1H8DiagonalAttacks(occupiedSquares, square);
                targets |= MovePackHelper.GetH1A8DiagonalAttacks(occupiedSquares, square);
            }

            return targets & ~board.GetPlayerPieces(pieceColor);
        }
    }
}
