using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{

    internal struct BitBoard
    {
        private UInt64 value;

        private BitBoard(UInt64 board)
        {
            this.value = board;
        }

        public static implicit operator BitBoard(UInt64 board)
        {
            return new BitBoard(board);
        }
        public static implicit operator UInt64(BitBoard board)
        {
            return board.value;
        }

        internal static BitBoard ToBitBoard(int toConvert)
        {
            return (UInt64)toConvert;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int32 ToInt32(BitBoard bitBoard)
        {
            return (Int32)bitBoard.value;
        }

        internal static bool IsBitSet(BitBoard bitBoard, int posBit)
        {
            return (bitBoard & ((UInt64)1 << (posBit))) != 0;
        }
        internal static void Display(BitBoard bitBoard)
        {
            for (int r = 7; r >= 0; r--)
            {
                Console.WriteLine("   ------------------------");

                Console.Write(" {0} ", r + 1);

                for (int c = 0; c <= 7; c++)
                {
                    Console.Write('[');
                    if (IsBitSet(bitBoard, Square.GetSquareIndex(c, r)))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('1');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write('0');
                    }

                    Console.ResetColor();
                    Console.Write(']');
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n    A  B  C  D  E  F  G  H");
        }
        internal static int PopCount(BitBoard bitBoard)
        {
            UInt64 M1 = 0x5555555555555555;  // 1 zero,  1 one ...
            UInt64 M2 = 0x3333333333333333;  // 2 zeros,  2 ones ...
            UInt64 M4 = 0x0f0f0f0f0f0f0f0f;  // 4 zeros,  4 ones ...
            UInt64 M8 = 0x00ff00ff00ff00ff;  // 8 zeros,  8 ones ...
            UInt64 M16 = 0x0000ffff0000ffff;  // 16 zeros, 16 ones ...
            UInt64 M32 = 0x00000000ffffffff;  // 32 zeros, 32 ones

            bitBoard.value = (bitBoard.value & M1) + ((bitBoard.value >> 1) & M1);   //put count of each  2 bits into those  2 bits
            bitBoard.value = (bitBoard.value & M2) + ((bitBoard.value >> 2) & M2);   //put count of each  4 bits into those  4 bits
            bitBoard.value = (bitBoard.value & M4) + ((bitBoard.value >> 4) & M4);   //put count of each  8 bits into those  8 bits
            bitBoard.value = (bitBoard.value & M8) + ((bitBoard.value >> 8) & M8);   //put count of each 16 bits into those 16 bits
            bitBoard.value = (bitBoard.value & M16) + ((bitBoard.value >> 16) & M16);   //put count of each 32 bits into those 32 bits
            bitBoard.value = (bitBoard.value & M32) + ((bitBoard.value >> 32) & M32);   //put count of each 64 bits into those 64 bits
            return (int)bitBoard.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int BitScanForward(BitBoard bitBoard)
        {
            return Constants.DeBrujinTable[((ulong)((long)bitBoard.value & -(long)bitBoard.value) * Constants.DeBrujinValue) >> 58];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int BitScanForwardReset(ref BitBoard bitBoard)
        {
            int bitIndex = Constants.DeBrujinTable[((ulong)((long)bitBoard.value & -(long)bitBoard.value) * Constants.DeBrujinValue) >> 58];
            bitBoard.value &= bitBoard.value - 1;
            return bitIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsAttacked(BitBoard target, PieceColor side, Board board)
        {
            BitBoard slidingAttackers;
            BitBoard pawnAttacks;
            BitBoard allPieces = board.GetAllPieces();
            PieceColor enemy = side.GetOpposite();
            int to;

            while (target != 0)
            {
                to = BitBoard.BitScanForward(target);
                pawnAttacks = side == PieceColor.White ? MovePackHelper.WhitePawnAttacks[to] : MovePackHelper.BlackPawnAttacks[to];

                if ((board.GetPieceSet(enemy, PieceType.Pawn) & pawnAttacks) != 0)
                    return true;
                if ((board.GetPieceSet(enemy, PieceType.Knight) & MovePackHelper.KnightAttacks[to]) != 0) return true;
                if ((board.GetPieceSet(enemy, PieceType.King) & MovePackHelper.KingAttacks[to]) != 0) return true;

                // file / rank attacks
                slidingAttackers = board.GetPieceSet(enemy, PieceType.Queen) | board.GetPieceSet(enemy, PieceType.Rook);

                if (slidingAttackers != 0)
                {
                    if ((MovePackHelper.GetRankAttacks(allPieces, to) & slidingAttackers) != 0)
                        return true;
                    if ((MovePackHelper.GetFileAttacks(allPieces, to) & slidingAttackers) != 0)
                        return true;
                }

                // diagonals
                slidingAttackers = board.GetPieceSet(enemy, PieceType.Queen) | board.GetPieceSet(enemy, PieceType.Bishop);

                if (slidingAttackers != 0)
                {
                    if ((MovePackHelper.GetH1A8DiagonalAttacks(allPieces, to) & slidingAttackers) != 0)
                        return true;
                    if ((MovePackHelper.GetA1H8DiagonalAttacks(allPieces, to) & slidingAttackers) != 0)
                        return true;
                }

                target.value &= target.value - 1;
            }

            return false;
        }
    }
}
