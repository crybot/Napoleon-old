using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    internal static class Knight
    {
        internal static BitBoard GetAllTargets(PieceColor pieceColor, BitBoard knights, Board board)
        {
            BitBoard targets = Constants.Empty;

            while (knights != 0)
            {
                // accede all'array di mosse precalcolate cercando il primo bit attivo all'interno della bitboard
                targets |= MovePackHelper.KnightAttacks[(BitBoard.BitScanForwardReset(ref knights))];
            }

            return targets & ~board.GetPlayerPieces(pieceColor);
        }
        internal static void InitKnightAttacks()
        {
            // inizializza l'array di mosse precalcolate
            for (int sq = 0; sq < 64; sq++)
            {
                MovePackHelper.KnightAttacks[sq] = GetKnightAttacks(BitBoard.SquareMask(sq));
            }
        }
        private static BitBoard GetKnightAttacks(BitBoard knights)
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
