using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{

    internal static class MoveGenerator
    {
        private static readonly MoveList allMoves = new MoveList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MoveList GetAllMoves(byte color, Board board)
        {
            allMoves.Clear();

            GetPawnMoves(color, board.GetPieceSet(color, PieceType.Pawn), board, allMoves);
            GetKnightMoves(color, board.GetPieceSet(color, PieceType.Knight), board, allMoves);
            GetKingMoves(color, board.GetPieceSet(color, PieceType.King), board, allMoves);
            GetBishopMoves(color, board.GetPieceSet(color, PieceType.Bishop), board, allMoves);
            GetRookMoves(color, board.GetPieceSet(color, PieceType.Rook), board, allMoves);
            GetQueenMoves(color, board.GetPieceSet(color, PieceType.Queen), board, allMoves);
            GetCastleMoves(color, board, allMoves);

            return allMoves;
        }

        internal static void GetPawnMoves(byte color, BitBoard pawns, Board board, MoveList moveList)
        {
            BitBoard targets;
            BitBoard epTargets;
            byte fromIndex;
            byte toIndex;

            while (pawns != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref pawns); // search for LS1B and then reset it
                targets = Pawn.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it

                    // en passant
                    if (board.EnPassantSquare != Square.Invalid)
                    {
                        epTargets = color == PieceColor.White ? MovePackHelper.WhitePawnAttacks[fromIndex] : MovePackHelper.BlackPawnAttacks[fromIndex];

                        if ((epTargets & Constants.SquareMask[board.EnPassantSquare]) != 0)
                            moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Pawn, PieceType.Pawn, PieceType.Pawn);
                    }

                    // promotions
                    if (Square.GetRankIndex(toIndex) == 7 && color == PieceColor.White || Square.GetRankIndex(toIndex) == 0 && color == PieceColor.Black)
                    {
                        moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.Queen);
                        moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.Rook);
                        moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.Bishop);
                        moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.Knight);
                    }
                    else
                        moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.None); // no promotions
                }
            }
        }
        internal static void GetKingMoves(byte color, BitBoard king, Board board, MoveList moveList)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            while (king != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref king); // search for LS1B and then reset it
                targets = King.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.King, board.pieceSet[toIndex].Type, PieceType.None);
                }
            }
        }
        internal static void GetKnightMoves(byte color, BitBoard knights, Board board, MoveList moveList)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            while (knights != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref knights); // search for LS1B and then reset it
                targets = Knight.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Knight, board.pieceSet[toIndex].Type, PieceType.None);
                }
            }
        }
        internal static void GetRookMoves(byte color, BitBoard rooks, Board board, MoveList moveList)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            while (rooks != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref rooks); // search for LS1B and then reset it
                targets = Rook.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Rook, board.pieceSet[toIndex].Type, PieceType.None);
                }
            }
        }
        internal static void GetBishopMoves(byte color, BitBoard bishops, Board board, MoveList moveList)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            while (bishops != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref bishops); // search for LS1B and then reset it
                targets = Bishop.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Bishop, board.pieceSet[toIndex].Type, PieceType.None);
                }
            }
        }
        internal static void GetQueenMoves(byte color, BitBoard queens, Board board, MoveList moveList)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            while (queens != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref queens); // search for LS1B and then reset it
                targets = Queen.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.moves[moveList.Lenght++] = new Move(fromIndex, toIndex, PieceType.Queen, board.pieceSet[toIndex].Type, PieceType.None);
                }
            }
        }

        internal static void GetCastleMoves(byte color, Board board, MoveList moveList)
        {
            if (color == PieceColor.White)
            {
                if (board.WhiteCanCastleOO)
                {
                    if ((Constants.WhiteCastleMaskOO & board.OccupiedSquares) == 0)
                        moveList.moves[moveList.Lenght++] = Constants.WhiteCastlingOO;

                }
                if (board.WhiteCanCastleOOO)
                {
                    if ((Constants.WhiteCastleMaskOOO & board.OccupiedSquares) == 0)
                        moveList.moves[moveList.Lenght++] = Constants.WhiteCastlingOOO;
                }
            }

            else if (color == PieceColor.Black)
            {
                if (board.BlackCanCastleOO)
                {
                    if ((Constants.BlackCastleMaskOO & board.OccupiedSquares) == 0)
                        moveList.moves[moveList.Lenght++] = Constants.BlackCastlingOO;
                }
                if (board.BlackCanCastleOOO)
                {
                    if ((Constants.BlackCastleMaskOOO & board.OccupiedSquares) == 0)
                        moveList.moves[moveList.Lenght++] = Constants.BlackCastlingOOO;
                }
            }
        }


    }
}
