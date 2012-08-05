using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal static class MoveGenerator
    {
        private static List<Move> moveList = new List<Move>();

        //private delegate BitBoard GetTargetsDelegate(PieceColor color, BitBoard pieces, Board board);


        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static List<Move> ExtractMoves(PieceColor color, byte type, BitBoard pieces, Board board, GetTargetsDelegate getTargets)
        //{
        //    BitBoard targets;
        //    byte fromIndex;
        //    byte toIndex;

        //    moveList.Clear();

        //    while (pieces != 0)
        //    {
        //        fromIndex = (byte)BitBoard.BitScanForwardReset(ref pieces); // search for LS1B and then reset it
        //        targets = getTargets(color, Constants.SquareMask[fromIndex], board);

        //        while (targets != 0)
        //        {
        //            toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
        //            moveList.Add(new Move(fromIndex, toIndex, type, board.pieceSet[toIndex].PieceType, 0));
        //        }
        //    }
        //    return moveList;
        //}

        internal static List<Move> GetAllMoves(PieceColor color, Board board)
        {
            List<Move> allMoves = new List<Move>(50);
            allMoves.AddRange(GetPawnMoves(color, board.GetPieceSet(color, PieceType.Pawn), board));
            allMoves.AddRange(GetKnightMoves(color, board.GetPieceSet(color, PieceType.Knight), board));
            allMoves.AddRange(GetKingMoves(color, board.GetPieceSet(color, PieceType.King), board));
            allMoves.AddRange(GetBishopMoves(color, board.GetPieceSet(color, PieceType.Bishop), board));
            allMoves.AddRange(GetRookMoves(color, board.GetPieceSet(color, PieceType.Rook), board));
            allMoves.AddRange(GetQueenMoves(color, board.GetPieceSet(color, PieceType.Queen), board));
            return allMoves;
        }
        internal static List<Move> GetPawnMoves(PieceColor color, BitBoard pawns, Board board)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            moveList.Clear();

            while (pawns != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref pawns); // search for LS1B and then reset it
                targets = Pawn.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it

                    // promotions
                    if (Square.GetRankIndex(toIndex) == 8)
                    {
                        moveList.Add(new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.Queen));
                        moveList.Add(new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.Rook));
                        moveList.Add(new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.Bishop));
                        moveList.Add(new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.Knight));
                    }
                    else
                        moveList.Add(new Move(fromIndex, toIndex, PieceType.Pawn, board.pieceSet[toIndex].Type, PieceType.None)); // no promotions
                }
            }

            return moveList;
        }
        internal static List<Move> GetKingMoves(PieceColor color, BitBoard king, Board board)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            moveList.Clear();

            while (king != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref king); // search for LS1B and then reset it
                targets = King.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.Add(new Move(fromIndex, toIndex, PieceType.King, board.pieceSet[toIndex].Type, PieceType.None));
                }
            }

            return moveList;
        }
        internal static List<Move> GetKnightMoves(PieceColor color, BitBoard knights, Board board)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            moveList.Clear();

            while (knights != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref knights); // search for LS1B and then reset it
                targets = Knight.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.Add(new Move(fromIndex, toIndex, PieceType.Knight, board.pieceSet[toIndex].Type, PieceType.None));
                }
            }

            return moveList;
        }
        internal static List<Move> GetRookMoves(PieceColor color, BitBoard rooks, Board board)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            moveList.Clear();

            while (rooks != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref rooks); // search for LS1B and then reset it
                targets = Rook.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.Add(new Move(fromIndex, toIndex, PieceType.Rook, board.pieceSet[toIndex].Type, PieceType.None));
                }
            }

            return moveList;
        }
        internal static List<Move> GetBishopMoves(PieceColor color, BitBoard bishops, Board board)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            moveList.Clear();

            while (bishops != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref bishops); // search for LS1B and then reset it
                targets = Bishop.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.Add(new Move(fromIndex, toIndex, PieceType.Bishop, board.pieceSet[toIndex].Type, PieceType.None));
                }
            }

            return moveList;
        }
        internal static List<Move> GetQueenMoves(PieceColor color, BitBoard queens, Board board)
        {
            BitBoard targets;
            byte fromIndex;
            byte toIndex;

            moveList.Clear();

            while (queens != 0)
            {
                fromIndex = (byte)BitBoard.BitScanForwardReset(ref queens); // search for LS1B and then reset it
                targets = Queen.GetAllTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = (byte)BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.Add(new Move(fromIndex, toIndex, PieceType.Queen, board.pieceSet[toIndex].Type, PieceType.None));
                }
            }

            return moveList;
        }

    }
}
