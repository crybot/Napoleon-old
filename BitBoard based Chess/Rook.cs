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
        internal static BitBoard GetAllTargets(PieceColor pieceColor, BitBoard rooks, Board board)
        {
            BitBoard occupiedSquares = board.GetAllPieces();
            BitBoard targets = Constants.Empty;
            int square;

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
