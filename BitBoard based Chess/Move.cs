using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal struct Move
    {
        private readonly int FromSquare;
        private readonly int ToSquare;
        private readonly byte PieceMoved;
        private readonly byte PieceCaptured;
        private readonly byte PiecePromoted;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Move(int fromSquare, int toSquare, byte pieceMoved, byte pieceCaptured, byte piecePromoted)
        {
            this.FromSquare = fromSquare;
            this.ToSquare = toSquare;
            this.PieceMoved = pieceMoved;
            this.PieceCaptured = pieceCaptured;
            this.PiecePromoted = piecePromoted;
        }

        public static bool operator ==(Move a, Move b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Move a, Move b)
        {
            return !a.Equals(b);
        }
        public override bool Equals(object other)
        {
            if (other is Move)
            {
                Move compare = (Move)other;
                return (this.FromSquare == compare.FromSquare && this.ToSquare == compare.ToSquare);
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal string ToAlgebraic()
        {
            StringBuilder algebraic = new StringBuilder();

            algebraic.Append(this.PieceMoved.GetInitial());
            algebraic.Append(new Square(this.FromSquare).ToAlgebraic()); // TODO
            algebraic.Append(new Square(this.ToSquare).ToAlgebraic()); // TODO

            return algebraic.ToString();
        }
    }
}
