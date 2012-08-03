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
        private static int occupancy;
        private static int square;
        private static int rank;
        private static int file;
        private static int diag;
        private static int antiDiag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BitBoard GetAllTargets(PieceColor pieceColor, BitBoard bishops, Board board)
        {
            occupiedSquares = board.GetAllPieces();

            while (bishops != 0)
            {
                targets = Constants.Empty;
                square = BitBoard.BitScanForwardReset(ref bishops);
                rank = Square.GetRankIndex(square);
                file = Square.GetFileIndex(square);
                diag = Square.GetDiagonalIndex(file, rank);
                antiDiag = Square.GetAntiDiagonalIndex(file, rank);

                occupancy = BitBoard.ToInt32((occupiedSquares & Constants.DiagonalMask[diag]) * Constants.DiagonalMagic[diag] >> 56);
                targets |= MovePackHelper.DiagonalAttacks[square, (occupancy >> 1) & 63];

                occupancy = BitBoard.ToInt32((occupiedSquares & Constants.AntiDiagonalMask[antiDiag]) * Constants.AntiDiagonalMagic[antiDiag] >> 56);
                targets |= MovePackHelper.AntiDiagonalAttacks[square, (occupancy >> 1) & 63];
            }

            return targets & ~board.GetPlayerPieces(pieceColor);
        }
    }
}
