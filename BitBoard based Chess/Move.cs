using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal sealed class MoveList
    {
        internal readonly Move[] moves = new Move[Constants.MaxMoves + 2];
        internal Int32 Lenght = 0;

        internal void Clear()
        {
            if (this.Lenght != 0)
            {
                Array.Clear(moves, 0, this.Lenght);
                this.Lenght = 0;
            }
        }
    }

    internal struct Move
    {
        internal readonly byte FromSquare;
        internal readonly byte ToSquare;
        internal readonly byte PieceMoved; // overloaded to manage castle (KING)
        internal readonly byte PieceCaptured;
        internal readonly byte PiecePromoted;// overloaded to manage castle (ROOK) // overloaded to manage en-passant (PAWN)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Move(byte fromSquare, byte toSquare, byte pieceMoved, byte pieceCaptured, byte piecePromoted)
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
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }


        internal bool IsNull()
        {
            return (this.FromSquare == this.ToSquare);
        }
        internal bool IsCapture()
        {
            return (this.PieceCaptured != PieceType.None);
        }
        internal bool IsCastle()
        {
            return (PieceMoved == PieceType.King && PiecePromoted == PieceType.Rook);
        }
        internal bool IsCastleOO()
        {
            return (FromSquare == 60 && ToSquare == 62) || (FromSquare == 4 && ToSquare == 6);
        }
        internal bool IsCastleOOO()
        {
            return (FromSquare == 60 && ToSquare == 58) || (FromSquare == 4 && ToSquare == 2);
        }
        internal bool IsPromotion()
        {
            return (this.PieceMoved == PieceType.Pawn && this.PiecePromoted != PieceType.None && this.PiecePromoted != PieceType.Pawn);
        }
        internal bool IsEnPassant()
        {
            return (this.PieceMoved == PieceType.Pawn && this.PiecePromoted == PieceType.Pawn);
        }

        internal string ToAlgebraic()
        {
            StringBuilder algebraic = new StringBuilder();

            if (this.IsCastle())
            {
                if (this.IsCastleOO())
                    algebraic.Append("O-O");

                else if (this.IsCastleOOO())
                    algebraic.Append("O-O-O");
            }

            else
            {
                //algebraic.Append(this.PieceMoved.GetInitial());
                algebraic.Append(Square.ToAlgebraic(this.FromSquare)); // TODO
                if (this.IsCapture()) algebraic.Append("x");
                algebraic.Append(Square.ToAlgebraic(this.ToSquare)); // TODO
                if (this.IsPromotion()) algebraic.Append(this.PiecePromoted.GetInitial());
                else if (this.IsEnPassant()) algebraic.Append("e.p.");
            }

            return algebraic.ToString();
        }
    }
}
