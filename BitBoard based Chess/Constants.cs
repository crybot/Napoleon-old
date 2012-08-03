using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    internal static class Constants
    {
        /// <summary>
        /// Per questioni di performance non verra` utilizzato il tipo BitBoard per inizializzare le costanti
        /// verra` invece usato il tipo nativo UInt64, alias del tipo ulong
        /// </summary>

        #region MagicMultipliers

        internal static readonly UInt64[] FileMagic = 
        {
            0x8040201008040200, 0x4020100804020100, 0x2010080402010080, 0x1008040201008040,
            0x0804020100804020, 0x0402010080402010, 0x0201008040201008, 0x0100804020100804 
        };

        internal static readonly UInt64[] DiagonalMagic = 
        { 
            0x0000000000000000, 0x0000000000000000, 0x0808080000000000, 0x1010101000000000,
            0x2020202020000000, 0x4040404040400000, 0x8080808080808000, 0x0101010101010100,
            0x0101010101010100, 0x0101010101010100, 0x0101010101010100, 0x0101010101010100,
            0x0101010101010100, 0x0000000000000000, 0x0000000000000000 
        };

        internal static readonly UInt64[] AntiDiagonalMagic =
        {
            0x0000000000000000, 0x0000000000000000, 0x0101010101010100, 0x0101010101010100,
            0x0101010101010100, 0x0101010101010100, 0x0101010101010100, 0x0101010101010100,
            0x0080808080808080, 0x0040404040404040, 0x0020202020202020, 0x0010101010101010,
            0x0008080808080808, 0x0000000000000000, 0x0000000000000000
        };

        #endregion

        #region Masks

        /// <summary>
        /// questi array con esattamente 6 bit attivi per ciascun elemento
        /// saranno utili nella generazione delle mosse delle torri e delle regine
        /// per ottenere i gradi di occupazione a 6 bit di tali pezzi
        /// </summary>

        internal static readonly UInt64[] SixBitRankMask = 
        { 
            0x000000000000007E, 0x0000000000007E00, 0x00000000007E0000,
            0x000000007E000000, 0x0000007E00000000, 0x00007E0000000000,
            0x007E000000000000, 0x7E00000000000000 
        };

        internal static readonly UInt64[] SixBitFileMask = 
        { 
            0x0001010101010100, 0x0002020202020200, 0x0004040404040400,
            0x0008080808080800, 0x0010101010101000, 0x0020202020202000,
            0x0040404040404000, 0x0080808080808000 
        };

        internal static readonly UInt64[] DiagonalMask = 
        { 
            0x0000000000000080, 0x0000000000008040, 0x0000000000804020, 0x0000000080402010,
            0x0000008040201008, 0x0000804020100804, 0x0080402010080402, 0x8040201008040201,
            0x4020100804020100, 0x2010080402010000, 0x1008040201000000, 0x0804020100000000,
            0x0402010000000000, 0x0201000000000000, 0x0100000000000000
        };

        internal static readonly UInt64[] AntiDiagonalMask =
        { 
            0x0000000000000001, 0x0000000000000102, 0x0000000000010204, 0x0000000001020408,
            0x0000000102040810, 0x0000010204081020, 0x0001020408102040, 0x0102040810204080,
            0x0204081020408000, 0x0408102040800000, 0x0810204080000000, 0x1020408000000000,
            0x2040800000000000, 0x4080000000000000, 0x8000000000000000
        };

        #endregion

        #region Constants

        internal const UInt64 Empty = 0x0000000000000000;
        internal const UInt64 Universe = 0xFFFFFFFFFFFFFFFF;

        internal const UInt64 LightSquares = 0x55AA55AA55AA55AA;
        internal const UInt64 DarkSquares = 0xAA55AA55AA55AA55;

        internal const UInt64 NotAFile = ~Files.A;
        internal const UInt64 NotBFile = ~Files.B;
        internal const UInt64 NotCFile = ~Files.C;
        internal const UInt64 NotDFile = ~Files.D;
        internal const UInt64 NotEFile = ~Files.E;
        internal const UInt64 NotFFile = ~Files.F;
        internal const UInt64 NotGFile = ~Files.G;
        internal const UInt64 NotHFile = ~Files.H;
        internal const UInt64 NotABFile = NotAFile | NotBFile;
        internal const UInt64 NotGHFile = NotGFile | NotHFile;

        internal static class Squares
        {
            internal const UInt64 A1 = 0x0000000000000001;
            internal const UInt64 B1 = 0x0000000000000002;
            internal const UInt64 C1 = 0x0000000000000004;
            internal const UInt64 D1 = 0x0000000000000008;
            internal const UInt64 E1 = 0x0000000000000010;
            internal const UInt64 F1 = 0x0000000000000020;
            internal const UInt64 G1 = 0x0000000000000040;
            internal const UInt64 H1 = 0x0000000000000080;

            internal const UInt64 A2 = 0x0000000000000100;
            internal const UInt64 B2 = 0x0000000000000200;
            internal const UInt64 C2 = 0x0000000000000400;
            internal const UInt64 D2 = 0x0000000000000800;
            internal const UInt64 E2 = 0x0000000000001000;
            internal const UInt64 F2 = 0x0000000000002000;
            internal const UInt64 G2 = 0x0000000000004000;
            internal const UInt64 H2 = 0x0000000000008000;

            internal const UInt64 A3 = 0x0000000000010000;
            internal const UInt64 B3 = 0x0000000000020000;
            internal const UInt64 C3 = 0x0000000000040000;
            internal const UInt64 D3 = 0x0000000000080000;
            internal const UInt64 E3 = 0x0000000000100000;
            internal const UInt64 F3 = 0x0000000000200000;
            internal const UInt64 G3 = 0x0000000000400000;
            internal const UInt64 H3 = 0x0000000000800000;

            internal const UInt64 A4 = 0x0000000001000000;
            internal const UInt64 B4 = 0x0000000002000000;
            internal const UInt64 C4 = 0x0000000004000000;
            internal const UInt64 D4 = 0x0000000008000000;
            internal const UInt64 E4 = 0x0000000010000000;
            internal const UInt64 F4 = 0x0000000020000000;
            internal const UInt64 G4 = 0x0000000040000000;
            internal const UInt64 H4 = 0x0000000080000000;

            internal const UInt64 A5 = 0x0000000100000000;
            internal const UInt64 B5 = 0x0000000200000000;
            internal const UInt64 C5 = 0x0000000400000000;
            internal const UInt64 D5 = 0x0000000800000000;
            internal const UInt64 E5 = 0x0000001000000000;
            internal const UInt64 F5 = 0x0000002000000000;
            internal const UInt64 G5 = 0x0000004000000000;
            internal const UInt64 H5 = 0x0000008000000000;

            internal const UInt64 A6 = 0x0000010000000000;
            internal const UInt64 B6 = 0x0000020000000000;
            internal const UInt64 C6 = 0x0000040000000000;
            internal const UInt64 D6 = 0x0000080000000000;
            internal const UInt64 E6 = 0x0000100000000000;
            internal const UInt64 F6 = 0x0000200000000000;
            internal const UInt64 G6 = 0x0000400000000000;
            internal const UInt64 H6 = 0x0000800000000000;

            internal const UInt64 A7 = 0x0001000000000000;
            internal const UInt64 B7 = 0x0002000000000000;
            internal const UInt64 C7 = 0x0004000000000000;
            internal const UInt64 D7 = 0x0008000000000000;
            internal const UInt64 E7 = 0x0010000000000000;
            internal const UInt64 F7 = 0x0020000000000000;
            internal const UInt64 G7 = 0x0040000000000000;
            internal const UInt64 H7 = 0x0080000000000000;

            internal const UInt64 A8 = 0x0100000000000000;
            internal const UInt64 B8 = 0x0200000000000000;
            internal const UInt64 C8 = 0x0400000000000000;
            internal const UInt64 D8 = 0x0800000000000000;
            internal const UInt64 E8 = 0x1000000000000000;
            internal const UInt64 F8 = 0x2000000000000000;
            internal const UInt64 G8 = 0x4000000000000000;
            internal const UInt64 H8 = 0x8000000000000000;

        }
        internal static class Ranks
        {
            internal const UInt64 One = 0x00000000000000FF;
            internal const UInt64 Two = 0x000000000000FF00;
            internal const UInt64 Three = 0x0000000000FF0000;
            internal const UInt64 Four = 0x00000000FF000000;
            internal const UInt64 Five = 0x000000FF00000000;
            internal const UInt64 Six = 0x0000FF0000000000;
            internal const UInt64 Seven = 0x00FF000000000000;
            internal const UInt64 Eight = 0xFF00000000000000;
        }
        internal static class Files
        {
            internal const UInt64 A = 0x0101010101010101;
            internal const UInt64 B = 0x0202020202020202;
            internal const UInt64 C = 0x0404040404040404;
            internal const UInt64 D = 0x0808080808080808;
            internal const UInt64 E = 0x1010101010101010;
            internal const UInt64 F = 0x2020202020202020;
            internal const UInt64 G = 0x4040404040404040;
            internal const UInt64 H = 0x8080808080808080;
        }
        internal static class InitialPositions
        {
            internal const UInt64 WhitePawns = 0x000000000000FF00;
            internal const UInt64 WhiteKnights = 0x0000000000000042;
            internal const UInt64 WhiteBishops = 0x0000000000000024;
            internal const UInt64 WhiteRooks = 0x0000000000000081;
            internal const UInt64 WhiteQueen = 0x0000000000000008;
            internal const UInt64 WhiteKing = 0x0000000000000010;

            internal const UInt64 BlackPawns = 0x00FF000000000000;
            internal const UInt64 BlackKnights = 0x4200000000000000;
            internal const UInt64 BlackBishops = 0x2400000000000000;
            internal const UInt64 BlackRooks = 0x8100000000000000;
            internal const UInt64 BlackQueen = 0x0800000000000000;
            internal const UInt64 BlackKing = 0x1000000000000000;

        }

        internal const UInt64 DeBrujinValue = 0x07EDD5E59A4E28C2;

        #region DeBrujinTable

        internal static readonly int[] DeBrujinTable =
        {
            63,  0, 58,  1, 59, 47, 53,  2,
            60, 39, 48, 27, 54, 33, 42,  3,
            61, 51, 37, 40, 49, 18, 28, 20,
            55, 30, 34, 11, 43, 14, 22,  4,
            62, 57, 46, 52, 38, 26, 32, 41,
            50, 36, 17, 19, 29, 10, 13, 21,
            56, 45, 25, 31, 35, 16,  9, 12,
            44, 24, 15,  8, 23,  7,  6,  5
        };
        #endregion

        #endregion
    }
}
