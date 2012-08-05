using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal static class Pawn
    {
        /// <summary>
        /// PER QUESTIONI DI PERFORMANCE tutti i metodi riguardanti i pedoni vengono forzati
        /// come INLINE riducendo il tempo di chiamata
        /// </summary>
        /// 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BitBoard GetAllTargets(PieceColor color, BitBoard pawns, Board board)
        {
            BitBoard empty = board.EmptySquares;
            BitBoard enemyPieces = board.GetEnemyPieces(color);

            return GetQuietTargets(color, pawns, empty) | GetAnyAttack(color, pawns, board);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetQuietTargets(PieceColor color, BitBoard pawns, BitBoard empty)
        {
            return GetSinglePushTargets(color, pawns, empty) | GetDoublePushTargets(color, pawns, empty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetSinglePushTargets(PieceColor color, BitBoard pawns, BitBoard empty)
        {
            return color == PieceColor.White ? CompassRose.OneStepNorth(pawns) & empty : CompassRose.OneStepSouth(pawns) & empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetDoublePushTargets(PieceColor color, BitBoard pawns, BitBoard empty)
        {
            BitBoard singlePush = GetSinglePushTargets(color, pawns, empty);

            return color == PieceColor.White 
                ? CompassRose.OneStepNorth(singlePush) & empty & Constants.Ranks.Four
                : CompassRose.OneStepSouth(singlePush) & empty & Constants.Ranks.Five;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetPawnsAbleToSinglePush(PieceColor color, BitBoard pawns, BitBoard empty)
        {
            switch (color)
            {
                case PieceColor.White:
                    return CompassRose.OneStepSouth(empty) & pawns;
                case PieceColor.Black:
                    return CompassRose.OneStepNorth(empty) & pawns;
                default:
                    throw new NotImplementedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetPawnsAbleToDoublePush(PieceColor color, BitBoard pawns, BitBoard empty)
        {
            switch (color)
            {
                case PieceColor.White:
                    {
                        BitBoard emptyRank3 = CompassRose.OneStepSouth(empty & Constants.Ranks.Four) & empty;
                        return GetPawnsAbleToSinglePush(color, pawns, emptyRank3);
                    }
                case PieceColor.Black:
                    {
                        BitBoard emptyRank6 = CompassRose.OneStepNorth(empty & Constants.Ranks.Six) & empty;
                        return GetPawnsAbleToSinglePush(color, pawns, emptyRank6);
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetEastAttacks(PieceColor color, BitBoard pawns)
        {
            return color == PieceColor.White ? CompassRose.OneStepNorthEast(pawns) : CompassRose.OneStepSouthEast(pawns);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetWestAttacks(PieceColor color, BitBoard pawns)
        {
            return color == PieceColor.White ? CompassRose.OneStepNorthWest(pawns) : CompassRose.OneStepSouthWest(pawns);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BitBoard GetAnyAttack(PieceColor color, BitBoard pawns, Board board)
        {
            return (GetEastAttacks(color, pawns) | GetWestAttacks(color, pawns)) & board.GetEnemyPieces(color);
        }
    }
}
