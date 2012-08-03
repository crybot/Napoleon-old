using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        internal static readonly UInt64[,] RankAttacks = new UInt64[64, 64]; // square , occupancy
        internal static readonly UInt64[,] FileAttacks = new UInt64[64, 64]; // square , occupancy
        internal static readonly UInt64[,] DiagonalAttacks = new UInt64[64, 64]; // square , occupancy
        internal static readonly UInt64[,] AntiDiagonalAttacks = new UInt64[64, 64]; // square , occupancy

        private static void InitPawnAttacks()
        {         
            for (int sq = 0; sq < 64; sq++)
            {
                WhitePawnAttacks[sq] = CompassRose.OneStepNorthEast(BitBoard.SquareMask(sq)) | CompassRose.OneStepNorthWest(BitBoard.SquareMask(sq));
                BlackPawnAttacks[sq] = CompassRose.OneStepSouthEast(BitBoard.SquareMask(sq)) | CompassRose.OneStepSouthWest(BitBoard.SquareMask(sq));
            }
        }

        private static void InitKnightAttacks()
        {
            // inizializza l'array di mosse precalcolate
            for (int sq = 0; sq < 64; sq++)
            {
                KnightAttacks[sq] = Knight.GetKnightAttacks(BitBoard.SquareMask(sq));
            }
        }
        private static void InitKingAttacks()
        {
            for (int sq = 0; sq < 64; sq++)
            {
                // inizializza l'array di mosse precalcolate
                KingAttacks[sq] = King.GetKingAttacks(BitBoard.SquareMask(sq));
            }
        }
        private static void InitRankAttacks()
        {
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
                        targets |= BitBoard.SquareMask(blocker);
                        if (BitBoard.IsBitSet(occupancy, blocker)) break;

                        blocker++;
                    }

                    blocker = file - 1;
                    while (blocker >= 0)
                    {
                        targets |= BitBoard.SquareMask(blocker);
                        if (BitBoard.IsBitSet(occupancy, blocker)) break;

                        blocker--;
                    }

                    RankAttacks[sq, occ] = targets << (8 * rank);
                }
            }
        }
        private static void InitFileAttacks()
        {
            for (int sq = 0; sq < 64; sq++)
            {
                for (int occ = 0; occ < 64; occ++)
                {
                    UInt64 targets = Constants.Empty;
                    UInt64 rankTargets = RankAttacks[7 - (sq / 8), occ]; // converte la posizione reale in quella scalare RANK 

                    for (int bit = 0; bit < 8; bit++) // accede ai singoli bit della traversa (RANK)
                    {
                        int rank = 7 - bit;
                        int file = Square.GetFileIndex(sq);

                        if (BitBoard.IsBitSet(rankTargets, bit))
                        {
                            targets |= BitBoard.SquareMask(Square.GetSquareIndex(file, rank));
                        }
                    }
                    FileAttacks[sq, occ] = targets;
                }
            }
        }
        private static void InitDiagonalAttacks()
        {
            for (int sq = 0; sq < 64; sq++)
            {
                for (int occ = 0; occ < 64; occ++)
                {
                    int diag = Square.GetRankIndex(sq) - Square.GetFileIndex(sq);
                    UInt64 targets = Constants.Empty;
                    UInt64 rankTargets = diag > 0 ? RankAttacks[sq % 8, occ] : RankAttacks[sq / 8, occ];
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
                                targets |= BitBoard.SquareMask(Square.GetSquareIndex(file, rank));
                            }
                        }
                    }

                    DiagonalAttacks[sq, occ] = targets;
                }
            }
        }
        private static void InitAntiDiagonalAttacks()
        {
            for (int sq = 0; sq < 64; sq++)
            {
                for (int occ = 0; occ < 64; occ++)
                {
                    int diag = Square.GetAntiDiagonalIndex(sq);

                    UInt64 targets = Constants.Empty;
                    UInt64 rankTargets = diag > 7 ? RankAttacks[7 - sq / 8, occ] : RankAttacks[sq % 8, occ];
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
                                targets |= BitBoard.SquareMask(Square.GetSquareIndex(file, rank));
                            }
                        }
                    }

                    AntiDiagonalAttacks[sq, occ] = targets;
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
        }
    }
}
