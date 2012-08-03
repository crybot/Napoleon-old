using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal struct Square
    {
        private readonly int file;
        private readonly int rank;

        internal int File
        {
            get { return this.file; }
        }
        internal int Rank
        {
            get { return this.rank; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Square(int file, int rank)
        {
            this.file = file;
            this.rank = rank;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Square(int squareIndex)
        {
            this.file = squareIndex % 8; // & 7
            this.rank = squareIndex / 8; // >> 3
        }

        public static implicit operator Square(string str)
        {
            // converte la notazione algebrica (es. a1) in coordinate decimali
            str = str.ToLower(); // converte la stringa in minuscolo
            int x = (int)(str[0] - 'a');
            int y = (int)(str[1] - '1');

            return new Square(x, y);
        }
        public static bool operator ==(Square a, Square b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Square a, Square b)
        {
            return !(a == b);
        }

        public override bool Equals(object other)
        {
            if (other is Move)
            {
                Square compare = (Square)other;
                return (this.File == compare.File && this.Rank == compare.Rank);
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

        internal static int GetA1H8DiagonalIndex(int file, int rank)
        {
            return 7 + rank - file;
        }
        internal static int GetA1H8DiagonalIndex(int squareIndex)
        {
            return 7 + GetRankIndex(squareIndex) - GetFileIndex(squareIndex);
        }

        internal static int GetH1A8AntiDiagonalIndex(int file, int rank)
        {
            return rank + file;
        }
        internal static int GetH1A8AntiDiagonalIndex(int squareIndex)
        {
            return GetRankIndex(squareIndex) + GetFileIndex(squareIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetFileIndex(int squareIndex)
        {
            return squareIndex % 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetRankIndex(int squareIndex)
        {
            return squareIndex / 8;
        }

        internal static int GetSquareIndex(int file, int rank)
        {
            return file + 8 * rank;
        }

        internal string ToAlgebraic()
        {
            string str = "";
            str += (char)(this.File + 97);
            str += (this.Rank + 1).ToString();

            return str;
        }
    }
}
