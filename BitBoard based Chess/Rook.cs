using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    static class Rook
    {
        private static BitBoard occupiedSquares;
        private static BitBoard targets;
        private static int occupancy;
        private static int square;
        private static int rank;
        private static int file;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BitBoard GetAllTargets(PieceColor pieceColor, BitBoard rooks, Board board)
        {
            occupiedSquares = board.GetAllPieces();

            while (rooks != 0)
            {
                targets = Constants.Empty;
                square = BitBoard.BitScanForwardReset(ref rooks);
                rank = Square.GetRankIndex(square);
                file = Square.GetFileIndex(square);

                occupancy = BitBoard.ToInt32((occupiedSquares & Constants.SixBitRankMask[rank]) >> (8 * rank));
                targets |= MovePackHelper.RankAttacks[square, (occupancy >> 1) & 63];

                occupancy = BitBoard.ToInt32((occupiedSquares & Constants.SixBitFileMask[file]) * Constants.FileMagic[file] >> 57);
                targets |= MovePackHelper.FileAttacks[square, occupancy];
            }

            return targets & ~board.GetPlayerPieces(pieceColor);
        }
    }
}
