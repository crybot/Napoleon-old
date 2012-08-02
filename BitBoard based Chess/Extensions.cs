using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    internal static class Extensions
    {
        internal static char GetInitial(this PieceType type)
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
        internal static PieceColor GetOpposite(this PieceColor color)
        {
            if (color == PieceColor.White)
                return PieceColor.Black;
            if (color == PieceColor.Black)
                return PieceColor.White;
            else
                return PieceColor.None;
        }
    }
}
