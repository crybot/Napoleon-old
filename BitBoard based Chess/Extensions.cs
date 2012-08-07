using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal static class Extensions
    {
        internal static char GetInitial(this byte type)
        {
            switch (type)
            {
                case PieceType.Bishop:
                    return 'B';
                case PieceType.King:
                    return 'K';
                case PieceType.Knight:
                    return 'N';
                case PieceType.Pawn:
                    return 'P';
                case PieceType.Queen:
                    return 'Q';
                case PieceType.Rook:
                    return 'R';
                default:
                    return ' ';
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte GetOpposite(this byte color)
        {
            return color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        }
    }
}
