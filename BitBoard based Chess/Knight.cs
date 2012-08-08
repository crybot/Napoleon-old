using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    internal static class Knight
    {
        internal static BitBoard GetAllTargets(byte pieceColor, BitBoard knights, Board board)
        {
            BitBoard targets = Constants.Empty;

            targets |= MovePackHelper.KnightAttacks[(BitBoard.BitScanForward(knights))];

            return targets & ~board.GetPlayerPieces(pieceColor);
        }

        internal static BitBoard GetKnightAttacks(BitBoard knights)
        {
            BitBoard west, east, attacks;
            east = CompassRose.OneStepEast(knights);
            west = CompassRose.OneStepWest(knights);
            attacks = (east | west) << 16;
            attacks |= (east | west) >> 16;
            east = CompassRose.OneStepEast(east);
            west = CompassRose.OneStepWest(west);
            attacks |= (east | west) << 8;
            attacks |= (east | west) >> 8;

            return attacks;
        }
    }
}
