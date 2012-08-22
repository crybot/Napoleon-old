using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    internal static class King
    {
        internal static BitBoard GetAllTargets(byte pieceColor, BitBoard king, Board board)
        {
            BitBoard kingMoves = MovePackHelper.KingAttacks[(BitBoard.BitScanForward(king))];
            return kingMoves & ~board.GetPlayerPieces();
        }

        internal static BitBoard GetKingAttacks(BitBoard king)
        {
            BitBoard attacks = CompassRose.OneStepEast(king) | CompassRose.OneStepWest(king);
            king |= attacks;
            attacks |= CompassRose.OneStepNorth(king) | CompassRose.OneStepSouth(king);
            return attacks;
        }      
    }
}
