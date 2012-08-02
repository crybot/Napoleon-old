using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    internal struct Move
    {
        private readonly Square fromSquare;
        private readonly Square toSquare;
        private readonly PieceType pieceMoved;

        internal Square FromSquare
        {
            get { return fromSquare; }
        } 
        internal Square ToSquare
        {
            get { return toSquare; }
        }
        internal PieceType PieceMoved
        {
            get { return pieceMoved; }
        }
    
        internal Move(Square fromSquare, Square toSquare, PieceType pieceMoved)
        {
            this.fromSquare = fromSquare;
            this.toSquare = toSquare;
            this.pieceMoved = pieceMoved;
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
                return (this.fromSquare == compare.fromSquare && this.toSquare == compare.toSquare);
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

            algebraic.Append(this.pieceMoved.GetInitial());
            algebraic.Append(this.fromSquare.ToAlgebraic());
            algebraic.Append(this.toSquare.ToAlgebraic());

            return algebraic.ToString();
        }
    }
}
