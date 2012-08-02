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
            BitBoard empty = board.GetEmptySquares();
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
            switch (color)
            {
                case PieceColor.White:
                    {
                        return CompassRose.OneStepNorth(pawns) & empty;
                    }
                case PieceColor.Black:
                    {
                        return CompassRose.OneStepSouth(pawns) & empty;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetDoublePushTargets(PieceColor color, BitBoard pawns, BitBoard empty)
        {
            switch (color)
            {
                case PieceColor.White:
                    {
                        BitBoard singlePush = GetSinglePushTargets(color, pawns, empty);
                        return CompassRose.OneStepNorth(singlePush) & empty & Constants.Ranks.Four;
                    }
                case PieceColor.Black:
                    {
                        BitBoard singlePush = GetSinglePushTargets(color, pawns, empty);
                        return CompassRose.OneStepSouth(singlePush) & empty & Constants.Ranks.Five;
                    }
                default:
                    throw new NotImplementedException();
            }
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
            switch (color)
            {
                case PieceColor.White:
                    return CompassRose.OneStepNorthEast(pawns);
                case PieceColor.Black:
                    return CompassRose.OneStepSouthEast(pawns);
                default:
                    throw new NotImplementedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetWestAttacks(PieceColor color, BitBoard pawns)
        {
            switch (color)
            {
                case PieceColor.White:
                    return CompassRose.OneStepNorthWest(pawns);
                case PieceColor.Black:
                    return CompassRose.OneStepSouthWest(pawns);
                default:
                    throw new NotImplementedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BitBoard GetAnyAttack(PieceColor color, BitBoard pawns, Board board)
        {
            return (GetEastAttacks(color, pawns) | GetWestAttacks(color, pawns)) & board.GetEnemyPieces(color);
        }
    }
}
