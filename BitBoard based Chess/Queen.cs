using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    static class Queen
    {
        internal static BitBoard GetAllTargets(byte pieceColor, BitBoard queens, Board board)
        {
            return Rook.GetAllTargets(pieceColor, queens, board) | Bishop.GetAllTargets(pieceColor, queens, board);
        }
    }
}
