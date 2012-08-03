using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal sealed class Board
    {
        #region BitBoardsDictionaries

        private Dictionary<PieceType, BitBoard> whiteBitBoardSet = new Dictionary<PieceType, BitBoard>();
        private Dictionary<PieceType, BitBoard> blackBitBoardSet = new Dictionary<PieceType, BitBoard>();
        #endregion

        private Piece[] pieceSet = new Piece[64];

        #region Generic BitBoards

        private BitBoard whitePieces = Constants.Empty;
        private BitBoard blackPieces = Constants.Empty;
        private BitBoard allPieces = Constants.Empty;
        private BitBoard emptySquares = Constants.Empty;
        #endregion

        internal Board()
        {
            MovePackHelper.InitAttacks(); //inizializza i vari array di attacchi precalcolati per la generazione delle mosse

            whiteBitBoardSet.Add(PieceType.Pawn, Constants.Empty);
            whiteBitBoardSet.Add(PieceType.Knight, Constants.Empty);
            whiteBitBoardSet.Add(PieceType.Bishop, Constants.Empty);
            whiteBitBoardSet.Add(PieceType.Rook, Constants.Empty);
            whiteBitBoardSet.Add(PieceType.Queen, Constants.Empty);
            whiteBitBoardSet.Add(PieceType.King, Constants.Empty);

            blackBitBoardSet.Add(PieceType.Pawn, Constants.Empty);
            blackBitBoardSet.Add(PieceType.Knight, Constants.Empty);
            blackBitBoardSet.Add(PieceType.Bishop, Constants.Empty);
            blackBitBoardSet.Add(PieceType.Rook, Constants.Empty);
            blackBitBoardSet.Add(PieceType.Queen, Constants.Empty);
            blackBitBoardSet.Add(PieceType.King, Constants.Empty);

            pieceSet = Enumerable.Repeat<Piece>(new Piece(PieceColor.None, PieceType.None), 64).ToArray();
        }

        internal void AddPiece()
        {
            ///TODO
            ///TODO
            ///TODO
        }
        internal void RemovePiece()
        {
            ///TODO
            ///TODO
            ///TODO
        }

        internal void Equip()
        {
            this.whiteBitBoardSet[PieceType.Pawn] = Constants.InitialPositions.WhitePawns;
            this.whiteBitBoardSet[PieceType.Knight] = Constants.InitialPositions.WhiteKnights;
            this.whiteBitBoardSet[PieceType.Bishop] = Constants.InitialPositions.WhiteBishops;
            this.whiteBitBoardSet[PieceType.Rook] = Constants.InitialPositions.WhiteRooks;
            this.whiteBitBoardSet[PieceType.Queen] = Constants.InitialPositions.WhiteQueen;
            this.whiteBitBoardSet[PieceType.King] = Constants.InitialPositions.WhiteKing;

            this.blackBitBoardSet[PieceType.Pawn] = Constants.InitialPositions.BlackPawns;
            this.blackBitBoardSet[PieceType.Knight] = Constants.InitialPositions.BlackKnights;
            this.blackBitBoardSet[PieceType.Bishop] = Constants.InitialPositions.BlackBishops;
            this.blackBitBoardSet[PieceType.Rook] = Constants.InitialPositions.BlackRooks;
            this.blackBitBoardSet[PieceType.Queen] = Constants.InitialPositions.BlackQueen;
            this.blackBitBoardSet[PieceType.King] = Constants.InitialPositions.BlackKing;

            InitBitBoards();

        }
        internal void Clear()
        {

        }
        internal void LoadGame(FenString fenString)
        {
            pieceSet = fenString.PiecePlacement;

            #region Bitboards Setting

            for (int i = 0; i < pieceSet.Length; i++)
            {
                if (pieceSet[i].PieceColor == PieceColor.White)
                {
                    whiteBitBoardSet[pieceSet[i].PieceType] |= BitBoard.SquareMask(i);
                }
                else if (pieceSet[i].PieceColor == PieceColor.Black)
                {
                    blackBitBoardSet[pieceSet[i].PieceType] |= BitBoard.SquareMask(i);
                }
            }
            #endregion

            InitBitBoards();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPieceSet(PieceColor pieceColor, PieceType pieceType)
        {
            switch (pieceColor)
            {
                case PieceColor.White:
                    {
                        return whiteBitBoardSet[pieceType];
                    }
                case PieceColor.Black:
                    {
                        return blackBitBoardSet[pieceType];
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPlayerPieces(PieceColor color)
        {
            switch (color)
            {
                case PieceColor.White:
                    return this.whitePieces;
                case PieceColor.Black:
                    return this.blackPieces;
                default:
                    throw new NotImplementedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetEnemyPieces(PieceColor color)
        {
            switch (color)
            {
                case PieceColor.White:
                    return this.blackPieces;
                case PieceColor.Black:
                    return this.whitePieces;
                default:
                    throw new NotImplementedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetEmptySquares()
        {
            return this.emptySquares;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetAllPieces()
        {
            return this.allPieces;
        }

        private void InitBitBoards()
        {
            this.whitePieces =
                  this.whiteBitBoardSet[PieceType.Pawn] | this.whiteBitBoardSet[PieceType.Knight]
                | this.whiteBitBoardSet[PieceType.Bishop] | this.whiteBitBoardSet[PieceType.Rook]
                | this.whiteBitBoardSet[PieceType.Queen] | this.whiteBitBoardSet[PieceType.King];

            this.blackPieces =
                  this.blackBitBoardSet[PieceType.Pawn] | this.blackBitBoardSet[PieceType.Knight]
                | this.blackBitBoardSet[PieceType.Bishop] | this.blackBitBoardSet[PieceType.Rook]
                | this.blackBitBoardSet[PieceType.Queen] | this.blackBitBoardSet[PieceType.King];

            this.allPieces = whitePieces | blackPieces;
            this.emptySquares = ~allPieces;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool isAttacked(BitBoard target, PieceColor side)
        {
            BitBoard slidingAttackers;
            BitBoard pawnAttacks;
            BitBoard allPieces = this.GetAllPieces();
            PieceColor enemy = side.GetOpposite();

            int to, rank, file, diag, antiDiag, occupancy;

            while (target != 0)
            {
                to = BitBoard.BitScanForward(target);
                rank = Square.GetRankIndex(to);
                file = Square.GetFileIndex(to);
                diag = Square.GetDiagonalIndex(file,rank);
                antiDiag = Square.GetAntiDiagonalIndex(file, rank);
                pawnAttacks = side == PieceColor.White ? MovePackHelper.WhitePawnAttacks[to] : MovePackHelper.BlackPawnAttacks[to];

                if ((this.GetPieceSet(enemy, PieceType.Pawn) & pawnAttacks) != 0)
                    return true;
                if ((this.GetPieceSet(enemy, PieceType.Knight) & MovePackHelper.KnightAttacks[to]) != 0) return true;
                if ((this.GetPieceSet(enemy, PieceType.King) & MovePackHelper.KingAttacks[to]) != 0) return true;

                // file / rank attacks
                slidingAttackers = this.GetPieceSet(enemy, PieceType.Queen) | this.GetPieceSet(enemy, PieceType.Rook);

                if (slidingAttackers != 0)
                {
                    occupancy = BitBoard.ToInt32((allPieces & Constants.SixBitRankMask[rank]) >> (8 * rank));
                    if ((MovePackHelper.RankAttacks[to, (occupancy >> 1) & 63] & slidingAttackers) != 0)
                        return true;

                    occupancy = BitBoard.ToInt32((allPieces & Constants.SixBitFileMask[file]) * Constants.FileMagic[file] >> 57);
                    if ((MovePackHelper.FileAttacks[to, occupancy] & slidingAttackers) != 0)
                        return true;
                }

                // diagonals
                slidingAttackers = this.GetPieceSet(enemy, PieceType.Queen) | this.GetPieceSet(enemy, PieceType.Bishop);

                if (slidingAttackers != 0)
                {
                    occupancy = BitBoard.ToInt32((allPieces & Constants.AntiDiagonalMask[antiDiag]) * Constants.AntiDiagonalMagic[antiDiag] >> 56);
                    if ((MovePackHelper.AntiDiagonalAttacks[to, (occupancy >> 1) & 63] & slidingAttackers) != 0)
                        return true;

                    occupancy = BitBoard.ToInt32((allPieces & Constants.DiagonalMask[diag]) * Constants.DiagonalMagic[diag] >> 56);
                    if ((MovePackHelper.DiagonalAttacks[to, (occupancy >> 1) & 63] & slidingAttackers) != 0)
                        return true;
                }

                target &= target - 1;
            }

            return false;
        }



    }
}
