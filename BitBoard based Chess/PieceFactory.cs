using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    class PieceFactory
    {
        public Piece Create(PieceType pieceType, PieceColor pieceColor)
        {
            return new Piece(pieceColor, pieceType);
        }
    }
}
