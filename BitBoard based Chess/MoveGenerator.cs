using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal static class MoveGenerator
    {
        /// <summary>
        /// Questo delegato e` usato per generalizzare la serializzazione (o estrazione) delle mosse
        /// riducendo di molto il codice duplicato, ma non compromettendo le prestazioni.
        /// Il metodo che andra` a sostituire il delegato deve restituire una BitBoard che rappresenta
        /// le mosse possibili del pezzo sottoposto alla verifica e deve accettare gli stessi parametri
        /// del delegato
        /// </summary>

        private delegate BitBoard GetTargetsDelegate(PieceColor color, BitBoard pieces, Board board);

        private static List<Move> moveList = new List<Move>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<Move> ExtractMoves(PieceColor color, byte type, BitBoard pieces, Board board, GetTargetsDelegate getTargets)
        {
            BitBoard targets;
            int fromIndex;
            int toIndex;

            moveList.Clear();

            while (pieces != 0)
            {
                fromIndex = BitBoard.BitScanForwardReset(ref pieces); // search for LS1B and then reset it
                targets = getTargets(color, Constants.SquareMask[fromIndex], board);

                while (targets != 0)
                {
                    toIndex = BitBoard.BitScanForwardReset(ref targets); // search for LS1B and then reset it
                    moveList.Add(new Move(fromIndex, toIndex, type, board.pieceSet[toIndex].PieceType, 0));
                }
            }
            return moveList;
        }

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
            return ExtractMoves(color, PieceType.Pawn, pawns, board, Pawn.GetAllTargets);
        }
        internal static List<Move> GetKingMoves(PieceColor color, BitBoard king, Board board)
        {
            return ExtractMoves(color, PieceType.King, king, board, King.GetAllTargets);
        }
        internal static List<Move> GetKnightMoves(PieceColor color, BitBoard knights, Board board)
        {
            return ExtractMoves(color, PieceType.Knight, knights, board, Knight.GetAllTargets);
        }
        internal static List<Move> GetRookMoves(PieceColor color, BitBoard rooks, Board board)
        {
            return ExtractMoves(color, PieceType.Rook, rooks, board, Rook.GetAllTargets);
        }
        internal static List<Move> GetBishopMoves(PieceColor color, BitBoard bishops, Board board)
        {
            return ExtractMoves(color, PieceType.Bishop, bishops, board, Bishop.GetAllTargets);
        }
        internal static List<Move> GetQueenMoves(PieceColor color, BitBoard queens, Board board)
        {
            return ExtractMoves(color, PieceType.Queen, queens, board, Queen.GetAllTargets);
        }

    }
}
