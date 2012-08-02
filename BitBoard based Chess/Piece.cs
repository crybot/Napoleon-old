using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    internal enum PieceType { None, Pawn, Knight, Bishop, Rook, Queen, King }
    internal enum PieceColor { None, White, Black }

    internal struct Piece
    {
        private readonly PieceType pieceType;
        private readonly PieceColor pieceColor;

        internal PieceType PieceType
        {
            get { return pieceType; }
        }
        internal PieceColor PieceColor
        {
            get { return pieceColor; }
        } 

        internal Piece(PieceColor pieceColor, PieceType pieceType)
        {
            this.pieceColor = pieceColor;
            this.pieceType = pieceType;
        }
    }
}
