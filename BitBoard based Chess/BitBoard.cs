using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

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

        internal static BitBoard ToBitBoard(Int32 toConvert)
        {
            return (UInt64)toConvert;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int32 ToInt32(BitBoard bitBoard)
        {
            return (Int32)bitBoard.value;
        }

        internal static bool IsBitSet(BitBoard bitBoard, Int32 posBit)
        {
            return (bitBoard & ((UInt64)1 << (posBit))) != 0;
        }
        internal static void Display(BitBoard bitBoard)
        {
            for (Int32 r = 7; r >= 0; r--)
            {
                Console.WriteLine("   ------------------------");

                Console.Write(" {0} ", r + 1);

                for (Int32 c = 0; c <= 7; c++)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int32 PopCount(BitBoard bitBoard)
        {
            bitBoard.value -= ((bitBoard.value >> 1) & 0x5555555555555555UL);
            bitBoard.value = ((bitBoard.value >> 2) & 0x3333333333333333UL) + (bitBoard.value & 0x3333333333333333UL);
            bitBoard.value = ((bitBoard.value >> 4) + bitBoard.value) & 0x0F0F0F0F0F0F0F0FUL;
            return (Int32)((bitBoard.value * 0x0101010101010101UL) >> 56);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int32 BitScanForward(BitBoard bitBoard)
        {
            Debug.Assert(bitBoard.value != 0);

            return Constants.DeBrujinTable[((ulong)((long)bitBoard.value & -(long)bitBoard.value) * Constants.DeBrujinValue) >> 58];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int32 BitScanForwardReset(ref BitBoard bitBoard)
        {
            Debug.Assert(bitBoard.value != 0);

            UInt64 bb = bitBoard.value;
            bitBoard.value &= (bitBoard.value - 1);

            return Constants.DeBrujinTable[((ulong)((long)bb & -(long)bb) * Constants.DeBrujinValue) >> 58];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsAttacked(BitBoard target, byte side, Board board)
        {
            BitBoard slidingAttackers;
            BitBoard pawnAttacks;
            BitBoard allPieces = board.OccupiedSquares;
            byte enemyColor = side.GetOpposite();
            Int32 to;

            while (target != 0)
            {
                to = BitBoard.BitScanForwardReset(ref target);
                pawnAttacks = side == PieceColor.White ? MovePackHelper.WhitePawnAttacks[to] : MovePackHelper.BlackPawnAttacks[to];

                if ((board.GetPieceSet(enemyColor, PieceType.Pawn) & pawnAttacks) != 0) return true;
                if ((board.GetPieceSet(enemyColor, PieceType.Knight) & MovePackHelper.KnightAttacks[to]) != 0) return true;
                if ((board.GetPieceSet(enemyColor, PieceType.King) & MovePackHelper.KingAttacks[to]) != 0) return true;

                // file / rank attacks
                slidingAttackers = board.GetPieceSet(enemyColor, PieceType.Queen) | board.GetPieceSet(enemyColor, PieceType.Rook);

                if (slidingAttackers != 0)
                {
                    if ((MovePackHelper.GetRankAttacks(allPieces, to) & slidingAttackers) != 0) return true;
                    if ((MovePackHelper.GetFileAttacks(allPieces, to) & slidingAttackers) != 0) return true;
                }

                // diagonals
                slidingAttackers = board.GetPieceSet(enemyColor, PieceType.Queen) | board.GetPieceSet(enemyColor, PieceType.Bishop);

                if (slidingAttackers != 0)
                {
                    if ((MovePackHelper.GetH1A8DiagonalAttacks(allPieces, to) & slidingAttackers) != 0) return true;
                    if ((MovePackHelper.GetA1H8DiagonalAttacks(allPieces, to) & slidingAttackers) != 0) return true;
                }
            }
            return false;
        }
    }
}
