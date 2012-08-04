using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    //internal enum PieceType : byte { None = 0x0, Pawn = 0x2, Knight = 0x4, Bishop = 0x8, Rook = 0x10, Queen = 0x20, King = 0x40 }
    internal enum PieceColor { None, White, Black }

    /// <summary>
    /// Viene utilizzata questa Pseudo-Enumerazione poiche` concede un notevole
    /// aumento di performance rispetto all'utilizzo di un enum tradizionale
    /// </summary>
    internal static class PieceType
    {
        internal const byte None = 0;
        internal const byte Pawn = 1;
        internal const byte Knight = 2;
        internal const byte Bishop = 3;
        internal const byte Rook = 4;
        internal const byte Queen = 5;
        internal const byte King = 6;
    }

    //internal static class PieceColor
    //{
    //    internal const byte None = 0;
    //    internal const byte White = 1;
    //    internal const byte Black = 2;
    //}

    internal struct Piece
    {
        internal readonly byte PieceType;
        internal readonly PieceColor PieceColor;

        internal Piece(PieceColor pieceColor, byte pieceType)
        {
            this.PieceColor = pieceColor;
            this.PieceType = pieceType;
        }
    }
}
