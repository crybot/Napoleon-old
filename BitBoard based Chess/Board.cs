using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.CompilerServices;

namespace BitBoard_based_Chess
{
    /// <summary>
    /// ho implementato due metodi per gestire il gruppo di bitboard
    /// non ho ancora deciso pero` quale utilizzare
    /// </summary>

    internal sealed class Board
    {
        internal Piece[] pieceSet = new Piece[64];

        private BitBoard whitePawns;
        private BitBoard whiteKnights;
        private BitBoard whiteBishops;
        private BitBoard whiteRooks;
        private BitBoard whiteQueens;
        private BitBoard whiteKing;

        private BitBoard blackPawns;
        private BitBoard blackKnights;
        private BitBoard blackBishops;
        private BitBoard blackRooks;
        private BitBoard blackQueens;
        private BitBoard blackKing;

        //#region BitBoardsDictionaries

        //private BitBoard[] whiteBitBoardSet = new BitBoard[6];
        //private BitBoard[] blackBitBoardSet = new BitBoard[6];
        //#endregion

        #region Generic BitBoards

        internal BitBoard WhitePieces = Constants.Empty;
        internal BitBoard BlackPieces = Constants.Empty;
        internal BitBoard AllPieces = Constants.Empty;
        internal BitBoard EmptySquares = Constants.Empty;

        #endregion

        internal Board()
        {
            MovePackHelper.InitAttacks(); //inizializza i vari array di attacchi precalcolati per la generazione delle mosse

            //for (int i = 0; i < 6; i++)
            //{
            //    this.whiteBitBoardSet[i] = Constants.Empty;
            //    this.blackBitBoardSet[i] = Constants.Empty;
            //}

            this.whitePawns = Constants.Empty;
            this.whiteKnights = Constants.Empty;
            this.whiteBishops = Constants.Empty;
            this.whiteRooks = Constants.Empty;
            this.whiteQueens = Constants.Empty;
            this.whiteKing = Constants.Empty;

            this.blackPawns = Constants.Empty;
            this.blackKnights = Constants.Empty;
            this.blackBishops = Constants.Empty;
            this.blackRooks = Constants.Empty;
            this.blackQueens = Constants.Empty;
            this.blackKing = Constants.Empty;

            Board.InitializePieceSet(ref pieceSet);
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
            //this.whiteBitBoardSet[PieceType.Pawn] = Constants.InitialPositions.WhitePawns;
            //this.whiteBitBoardSet[PieceType.Knight] = Constants.InitialPositions.WhiteKnights;
            //this.whiteBitBoardSet[PieceType.Bishop] = Constants.InitialPositions.WhiteBishops;
            //this.whiteBitBoardSet[PieceType.Rook] = Constants.InitialPositions.WhiteRooks;
            //this.whiteBitBoardSet[PieceType.Queen] = Constants.InitialPositions.WhiteQueen;
            //this.whiteBitBoardSet[PieceType.King] = Constants.InitialPositions.WhiteKing;

            //this.blackBitBoardSet[PieceType.Pawn] = Constants.InitialPositions.BlackPawns;
            //this.blackBitBoardSet[PieceType.Knight] = Constants.InitialPositions.BlackKnights;
            //this.blackBitBoardSet[PieceType.Bishop] = Constants.InitialPositions.BlackBishops;
            //this.blackBitBoardSet[PieceType.Rook] = Constants.InitialPositions.BlackRooks;
            //this.blackBitBoardSet[PieceType.Queen] = Constants.InitialPositions.BlackQueen;
            //this.blackBitBoardSet[PieceType.King] = Constants.InitialPositions.BlackKing;

            this.whitePawns = Constants.InitialPositions.WhitePawns;
            this.whiteKnights = Constants.InitialPositions.WhiteKnights;
            this.whiteBishops = Constants.InitialPositions.WhiteBishops;
            this.whiteRooks = Constants.InitialPositions.WhiteRooks;
            this.whiteQueens = Constants.InitialPositions.WhiteQueen;
            this.whiteKing = Constants.InitialPositions.WhiteKing;

            this.blackPawns = Constants.InitialPositions.BlackPawns;
            this.blackKnights = Constants.InitialPositions.BlackKnights;
            this.blackBishops = Constants.InitialPositions.BlackBishops;
            this.blackRooks = Constants.InitialPositions.BlackRooks;
            this.blackQueens = Constants.InitialPositions.BlackQueen;
            this.blackKing = Constants.InitialPositions.BlackKing;

            InitializeBitBoards();

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
                if (pieceSet[i].Color == PieceColor.White)
                {
                    switch (pieceSet[i].Type)
                    {
                        case PieceType.Pawn:
                            this.whitePawns |= Constants.SquareMask[i];
                            break;
                        case PieceType.Knight:
                            this.whiteKnights |= Constants.SquareMask[i];
                            break;
                        case PieceType.Bishop:
                            this.whiteBishops |= Constants.SquareMask[i];
                            break;
                        case PieceType.Rook:
                            this.whiteRooks |= Constants.SquareMask[i];
                            break;
                        case PieceType.Queen:
                            this.whiteQueens |= Constants.SquareMask[i];
                            break;
                        case PieceType.King:
                            this.whiteKing |= Constants.SquareMask[i];
                            break;
                    }
                    //whiteBitBoardSet[pieceSet[i].PieceType] |= Constants.SquareMask[i];
                }
                else if (pieceSet[i].Color == PieceColor.Black)
                {
                    switch (pieceSet[i].Type)
                    {
                        case PieceType.Pawn:
                            this.blackPawns |= Constants.SquareMask[i];
                            break;
                        case PieceType.Knight:
                            this.blackKnights |= Constants.SquareMask[i];
                            break;
                        case PieceType.Bishop:
                            this.blackBishops |= Constants.SquareMask[i];
                            break;
                        case PieceType.Rook:
                            this.blackRooks |= Constants.SquareMask[i];
                            break;
                        case PieceType.Queen:
                            this.blackQueens |= Constants.SquareMask[i];
                            break;
                        case PieceType.King:
                            this.blackKing |= Constants.SquareMask[i];
                            break;
                    }
                    //blackBitBoardSet[pieceSet[i].PieceType] |= Constants.SquareMask[i];
                }
            }
            #endregion

            InitializeBitBoards();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPieceSet(PieceColor pieceColor, byte pieceType)
        {
            if (pieceColor == PieceColor.White)
            {
                switch (pieceType)
                {
                    case PieceType.Pawn:
                        return this.whitePawns;
                    case PieceType.Knight:
                        return this.whiteKnights;
                    case PieceType.Bishop:
                        return this.whiteBishops;
                    case PieceType.Rook:
                        return this.whiteRooks;
                    case PieceType.Queen:
                        return this.whiteQueens;
                    case PieceType.King:
                        return this.whiteKing;
                }
            }
            else
            {
                switch (pieceType)
                {
                    case PieceType.Pawn:
                        return this.blackPawns;
                    case PieceType.Knight:
                        return this.blackKnights;
                    case PieceType.Bishop:
                        return this.blackBishops;
                    case PieceType.Rook:
                        return this.blackRooks;
                    case PieceType.Queen:
                        return this.blackQueens;
                    case PieceType.King:
                        return this.blackKing;
                }
            }

            throw new NotImplementedException();
            //return pieceColor == PieceColor.White ? whiteBitBoardSet[pieceType] : blackBitBoardSet[pieceType];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPlayerPieces(PieceColor color)
        {
            return color == PieceColor.White ? this.WhitePieces : this.BlackPieces;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetEnemyPieces(PieceColor color)
        {
            return color == PieceColor.White ? this.BlackPieces : this.WhitePieces;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //internal BitBoard GetEmptySquares()
        //{
        //    return this.emptySquares;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //internal BitBoard GetAllPieces()
        //{
        //    return this.allPieces;
        //}

        private void InitializeBitBoards()
        {
            this.WhitePieces =
                  this.whitePawns | this.whiteKnights | this.whiteBishops
                | this.whiteRooks | this.whiteQueens | this.whiteKing;

            this.BlackPieces =
                  this.blackPawns | this.blackKnights | this.blackBishops
                | this.blackRooks | this.blackQueens | this.blackKing;

            this.AllPieces = WhitePieces | BlackPieces;
            this.EmptySquares = ~AllPieces;

            //this.WhitePieces =
            //      this.whiteBitBoardSet[PieceType.Pawn] | this.whiteBitBoardSet[PieceType.Knight]
            //    | this.whiteBitBoardSet[PieceType.Bishop] | this.whiteBitBoardSet[PieceType.Rook]
            //    | this.whiteBitBoardSet[PieceType.Queen] | this.whiteBitBoardSet[PieceType.King];

            //this.BlackPieces =
            //      this.blackBitBoardSet[PieceType.Pawn] | this.blackBitBoardSet[PieceType.Knight]
            //    | this.blackBitBoardSet[PieceType.Bishop] | this.blackBitBoardSet[PieceType.Rook]
            //    | this.blackBitBoardSet[PieceType.Queen] | this.blackBitBoardSet[PieceType.King];

            //this.AllPieces = WhitePieces | BlackPieces;
            //this.EmptySquares = ~AllPieces;
        }

        internal static void InitializePieceSet(ref Piece[] set)
        {
           set =  Enumerable.Repeat<Piece>(new Piece(PieceColor.None, PieceType.None), 64).ToArray();
        }

    }

}
