using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    internal static class King
    {
        internal static BitBoard GetAllTargets(PieceColor pieceColor, BitBoard king, Board board)
        {
            BitBoard kingMoves = MovePackHelper.KingAttacks[(BitBoard.BitScanForward(king))];
            return kingMoves & ~board.GetPlayerPieces(pieceColor);
        }
        internal static void InitKingAttacks()
        {
            for (int sq = 0; sq < 64; sq++)
            {
                MovePackHelper.KingAttacks[sq] = GetKingAttacks(BitBoard.SquareMask(sq));
            }
        }
        private static BitBoard GetKingAttacks(BitBoard king)
        {
            BitBoard attacks = CompassRose.OneStepEast(king) | CompassRose.OneStepWest(king);
            king |= attacks;
            attacks |= CompassRose.OneStepNorth(king) | CompassRose.OneStepSouth(king);
            return attacks;
        }      
    }
}
