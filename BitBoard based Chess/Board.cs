﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    internal sealed class Board
    {
        internal Piece[] pieceSet = new Piece[64];

        #region BitBoardsDictionaries

        private Dictionary<byte, BitBoard> whiteBitBoardSet = new Dictionary<byte, BitBoard>();
        private Dictionary<byte, BitBoard> blackBitBoardSet = new Dictionary<byte, BitBoard>();
        #endregion

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
                    whiteBitBoardSet[pieceSet[i].PieceType] |= Constants.SquareMask[i];
                }
                else if (pieceSet[i].PieceColor == PieceColor.Black)
                {
                    blackBitBoardSet[pieceSet[i].PieceType] |= Constants.SquareMask[i];
                }
            }
            #endregion

            InitBitBoards();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPieceSet(PieceColor pieceColor, byte pieceType)
        {
            return pieceColor == PieceColor.White ? whiteBitBoardSet[pieceType] : blackBitBoardSet[pieceType];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPlayerPieces(PieceColor color)
        {
            return color == PieceColor.White ? this.whitePieces : this.blackPieces;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetEnemyPieces(PieceColor color)
        {
            return color == PieceColor.White ? this.blackPieces : this.whitePieces;
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

    }

}
