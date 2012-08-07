using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal struct Square
    {
        internal readonly int File;
        internal readonly int Rank;
        internal readonly int SquareIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Square(int file, int rank)
        {
            this.File = file;
            this.Rank = rank;
            this.SquareIndex = GetSquareIndex(file, rank);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Square(int squareIndex)
        {
            this.File = squareIndex % 8; // & 7
            this.Rank = squareIndex / 8; // >> 3
            this.SquareIndex = squareIndex;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetA1H8DiagonalIndex(int file, int rank)
        {
            return 7 + rank - file;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetA1H8DiagonalIndex(int squareIndex)
        {
            return 7 + GetRankIndex(squareIndex) - GetFileIndex(squareIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetH1A8AntiDiagonalIndex(int file, int rank)
        {
            return rank + file;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetH1A8AntiDiagonalIndex(int squareIndex)
        {
            return GetRankIndex(squareIndex) + GetFileIndex(squareIndex);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetFileIndex(int squareIndex)
        {
            return squareIndex & 7;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetRankIndex(int squareIndex)
        {
            return squareIndex >> 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetSquareIndex(int file, int rank)
        {
            return file + 8 * rank;
        }

        internal static int Parse(string square)
        {
            // converte la notazione algebrica (es. a1) in coordinate decimali
            square = square.ToLower(); // converte la stringa in minuscolo
            int x = (int)(square[0] - 'a');
            int y = (int)(square[1] - '1');
            return GetSquareIndex(x, y);
        }

        internal static string ToAlgebraic(int square)
        {
            string str = string.Empty;
            str += (char)(Square.GetFileIndex(square) + 97);
            str += (int)(Square.GetRankIndex(square) + 1);

            return str;
        }
    }
}
