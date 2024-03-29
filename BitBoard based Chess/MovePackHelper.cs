﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal static class MovePackHelper
    {
        /// <summary>
        /// per questioni di performance non verra` utilizzato il wrapper BitBoard per inizializzare le costanti
        /// verra` invece usato il tipo nativo UInt64, alias del tipo ulong
        /// </summary>

        internal static readonly UInt64[] WhitePawnAttacks = new UInt64[64]; // square
        internal static readonly UInt64[] BlackPawnAttacks = new UInt64[64]; // square
        internal static readonly UInt64[] KingAttacks = new UInt64[64]; // square
        internal static readonly UInt64[] KnightAttacks = new UInt64[64]; // square

        internal static readonly UInt64[] PseudoRookAttacks = new UInt64[64]; // square
        internal static readonly UInt64[] PseudoBishopAttacks = new UInt64[64]; // square

        private static readonly UInt64[][] RankAttacks = new UInt64[64][]; // square , occupancy
        private static readonly UInt64[][] FileAttacks = new UInt64[64][]; // square , occupancy
        private static readonly UInt64[][] A1H8DiagonalAttacks = new UInt64[64][]; // square , occupancy
        private static readonly UInt64[][] H1A8DiagonalAttacks = new UInt64[64][]; // square , occupancy

        internal static readonly UInt64[][] InBetween = new UInt64[64][]; // square , square

        private static void InitPawnAttacks()
        {
            for (int sq = 0; sq < 64; sq++)
            {
                WhitePawnAttacks[sq] = CompassRose.OneStepNorthEast(Constants.SquareMask[sq]) | CompassRose.OneStepNorthWest(Constants.SquareMask[sq]);
                BlackPawnAttacks[sq] = CompassRose.OneStepSouthEast(Constants.SquareMask[sq]) | CompassRose.OneStepSouthWest(Constants.SquareMask[sq]);
            }
        }

        private static void InitKnightAttacks()
        {
            // inizializza l'array di mosse precalcolate
            for (int sq = 0; sq < 64; sq++)
            {
                KnightAttacks[sq] = Knight.GetKnightAttacks(Constants.SquareMask[sq]);
            }
        }
        private static void InitKingAttacks()
        {
            for (int sq = 0; sq < 64; sq++)
            {
                // inizializza l'array di mosse precalcolate
                KingAttacks[sq] = King.GetKingAttacks(Constants.SquareMask[sq]);
            }
        }
        private static void InitRankAttacks()
        {
            for (int i = 0; i < 64; i++)
            {
                RankAttacks[i] = new UInt64[64];
            }

            for (int sq = 0; sq < 64; sq++)
            {
                for (int occ = 0; occ < 64; occ++)
                {
                    int rank = Square.GetRankIndex(sq);
                    int file = Square.GetFileIndex(sq);

                    UInt64 occupancy = BitBoard.ToBitBoard(occ << 1);
                    UInt64 targets = Constants.Empty;

                    int blocker = file + 1;
                    while (blocker <= 7)
                    {
                        targets |= Constants.SquareMask[blocker];
                        if (BitBoard.IsBitSet(occupancy, blocker)) break;

                        blocker++;
                    }

                    blocker = file - 1;
                    while (blocker >= 0)
                    {
                        targets |= Constants.SquareMask[blocker];
                        if (BitBoard.IsBitSet(occupancy, blocker)) break;

                        blocker--;
                    }

                    RankAttacks[sq][occ] = targets << (8 * rank);
                }
            }
        }
        private static void InitFileAttacks()
        {
            for (int i = 0; i < 64; i++)
            {
                FileAttacks[i] = new UInt64[64];
            }

            for (int sq = 0; sq < 64; sq++)
            {
                for (int occ = 0; occ < 64; occ++)
                {
                    UInt64 targets = Constants.Empty;
                    UInt64 rankTargets = RankAttacks[7 - (sq / 8)][occ]; // converte la posizione reale in quella scalare RANK 

                    for (int bit = 0; bit < 8; bit++) // accede ai singoli bit della traversa (RANK)
                    {
                        int rank = 7 - bit;
                        int file = Square.GetFileIndex(sq);

                        if (BitBoard.IsBitSet(rankTargets, bit))
                        {
                            targets |= Constants.SquareMask[Square.GetSquareIndex(file, rank)];
                        }
                    }
                    FileAttacks[sq][occ] = targets;
                }
            }
        }
        private static void InitDiagonalAttacks()
        {
            for (int i = 0; i < 64; i++)
            {
                A1H8DiagonalAttacks[i] = new UInt64[64];
            }

            for (int sq = 0; sq < 64; sq++)
            {
                for (int occ = 0; occ < 64; occ++)
                {
                    int diag = Square.GetRankIndex(sq) - Square.GetFileIndex(sq);
                    UInt64 targets = Constants.Empty;
                    UInt64 rankTargets = diag > 0 ? RankAttacks[sq % 8][occ] : RankAttacks[sq / 8][occ];
                    // converte la posizione reale in quella scalare RANK //

                    for (int bit = 0; bit < 8; bit++) // accede ai singoli bit della traversa (RANK)
                    {
                        int rank;
                        int file;

                        if (BitBoard.IsBitSet(rankTargets, bit))
                        {
                            if (diag >= 0)
                            {
                                rank = diag + bit;
                                file = bit;
                            }
                            else
                            {
                                file = bit - diag;
                                rank = bit;
                            }
                            if ((file >= 0) && (file <= 7) && (rank >= 0) && (rank <= 7))
                            {
                                targets |= Constants.SquareMask[Square.GetSquareIndex(file, rank)];
                            }
                        }
                    }

                    A1H8DiagonalAttacks[sq][occ] = targets;
                }
            }
        }
        private static void InitAntiDiagonalAttacks()
        {
            for (int i = 0; i < 64; i++)
            {
                H1A8DiagonalAttacks[i] = new UInt64[64];
            }

            for (int sq = 0; sq < 64; sq++)
            {
                for (int occ = 0; occ < 64; occ++)
                {
                    int diag = Square.GetH1A8AntiDiagonalIndex(sq);

                    UInt64 targets = Constants.Empty;
                    UInt64 rankTargets = diag > 7 ? RankAttacks[7 - sq / 8][occ] : RankAttacks[sq % 8][occ];
                    // converte la posizione reale in quella scalare RANK //

                    for (int bit = 0; bit < 8; bit++) // accede ai singoli bit della traversa (RANK)
                    {
                        int rank;
                        int file;

                        if (BitBoard.IsBitSet(rankTargets, bit))
                        {
                            if (diag >= 7)
                            {
                                rank = 7 - bit;
                                file = (diag - 7) + bit;
                            }
                            else
                            {
                                rank = diag - bit;
                                file = bit;
                            }
                            if ((file >= 0) && (file <= 7) && (rank >= 0) && (rank <= 7))
                            {
                                targets |= Constants.SquareMask[Square.GetSquareIndex(file, rank)];
                            }
                        }
                    }

                    H1A8DiagonalAttacks[sq][occ] = targets;
                }
            }
        }
        private static void InitPseudoAttacks()
        {
            for (int i = 0; i < 64; i++)
            {
                PseudoRookAttacks[i] = RankAttacks[i][0] | FileAttacks[i][0];
                PseudoBishopAttacks[i] = A1H8DiagonalAttacks[i][0] | H1A8DiagonalAttacks[i][0];
            }
        }
        private static void InitInBetweenTable()
        {
            const UInt64 m1 = unchecked((ulong)(-(long)1));
            const UInt64 a2a7 = 0x0001010101010100;
            const UInt64 b2g7 = 0x0040201008040200;
            const UInt64 h1b7 = 0x0002040810204080; // Thanks Dustin, g2b7 did not work for c1-a3
            UInt64 btwn, line, rank, file;

            for (int sq1 = 0; sq1 < 64; sq1++)
            {
                InBetween[sq1] = new UInt64[64];
                for (int sq2 = 0; sq2 < 64; sq2++)
                {
                    btwn = (m1 << sq1) ^ (m1 << sq2);
                    file = (ulong) ((sq2 & 7) - (sq1 & 7));
                    rank =  (ulong) (((sq2 | 7) - sq1) >> 3);
                    line = ((file & 0xff) - 1) & a2a7; // a2a7 if same file
                    line += 2 * (((rank & 0xff) - 1) >> 58); // b1g1 if same rank
                    line += (((rank - file) & 0xff) - 1) & b2g7; // b2g7 if same diagonal
                    line += (((rank + file) & 0xff) - 1) & h1b7; // h1b7 if same antidiag
                    line *= (ulong)((long)btwn & -(long)btwn); // mul acts like shift by smaller square
                    InBetween[sq1][sq2] = line & btwn;
                }
            }
        }

        internal static void InitAttacks()
        {
            MovePackHelper.InitRankAttacks();
            MovePackHelper.InitFileAttacks();
            MovePackHelper.InitDiagonalAttacks();
            MovePackHelper.InitAntiDiagonalAttacks();
            MovePackHelper.InitPawnAttacks();
            MovePackHelper.InitKingAttacks();
            MovePackHelper.InitKnightAttacks();
            MovePackHelper.InitPseudoAttacks();
            MovePackHelper.InitInBetweenTable();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 GetRankAttacks(BitBoard occupiedSquares, int square)
        {
            int rank = Square.GetRankIndex(square);
            int occupancy = BitBoard.ToInt32((occupiedSquares & Constants.SixBitRankMask[rank]) >> (8 * rank));
            return RankAttacks[square][(occupancy >> 1) & 63];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 GetFileAttacks(BitBoard occupiedSquares, int square)
        {
            int file = Square.GetFileIndex(square);
            int occupancy = BitBoard.ToInt32((occupiedSquares & Constants.SixBitFileMask[file]) * Constants.FileMagic[file] >> 56);
            return FileAttacks[square][(occupancy >> 1) & 63];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 GetA1H8DiagonalAttacks(BitBoard occupiedSquares, int square)
        {
            int diag = Square.GetA1H8DiagonalIndex(square);
            int occupancy = BitBoard.ToInt32((occupiedSquares & Constants.A1H8DiagonalMask[diag]) * Constants.A1H8DiagonalMagic[diag] >> 56);
            return A1H8DiagonalAttacks[square][(occupancy >> 1) & 63];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 GetH1A8DiagonalAttacks(BitBoard occupiedSquares, int square)
        {
            int diag = Square.GetH1A8AntiDiagonalIndex(square);
            int occupancy = BitBoard.ToInt32((occupiedSquares & Constants.H1A8DiagonalMask[diag]) * Constants.H1A8DiagonalMagic[diag] >> 56);
            return H1A8DiagonalAttacks[square][(occupancy >> 1) & 63];
        }

         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool AreSquareAligned(int s1, int s2, int s3)
        {
            return ((InBetween[s1][s2] | InBetween[s1][s3] | InBetween[s2][s3])
                  & (Constants.SquareMask[s1] | Constants.SquareMask[s2] | Constants.SquareMask[s3])) != 0;
        }
    }
}
