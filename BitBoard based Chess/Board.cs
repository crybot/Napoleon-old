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

        internal bool WhiteCanCastleOO;
        internal bool WhiteCanCastleOOO;
        internal bool BlackCanCastleOO;
        internal bool BlackCanCastleOOO;

        internal int? EnPassantSquare;

        internal byte SideToMove;

        internal Piece[] pieceSet = new Piece[64];
        internal readonly BitBoard[][] BitBoardSet = new BitBoard[2][];

        internal BitBoard WhitePieces = Constants.Empty;
        internal BitBoard BlackPieces = Constants.Empty;
        internal BitBoard OccupiedSquares = Constants.Empty;
        internal BitBoard EmptySquares = Constants.Empty;

        internal Board()
        {
            MovePackHelper.InitAttacks(); //inizializza i vari array di attacchi precalcolati per la generazione delle mosse

            this.BitBoardSet[0] = new BitBoard[6];
            this.BitBoardSet[1] = new BitBoard[6];

            for (int i = 0; i < 6; i++)
            {
                this.BitBoardSet[PieceColor.White][i] = Constants.Empty;
                this.BitBoardSet[PieceColor.Black][i] = Constants.Empty;
            }
        }

        internal void AddPiece(Piece piece, Int32 pos)
        {
            this.pieceSet[pos] = piece;
        }
        internal void RemovePiece()
        {
            ///TODO
            ///TODO
            ///TODO
        }

        internal void MakeMove(Move move)
        {
            //this.pieceSet[move.ToSquare] = this.pieceSet[move.FromSquare]; // muove il pezzo
            //this.pieceSet[move.ToSquare] = new Piece(PieceColor.None, PieceType.None); // svuota la casella di partenza

            //switch (this.SideToMove)
            //{
            //    case PieceColor.White:
            //        {
            //            this.whiteBitBoardSet[move.PieceMoved] ^= Constants.SquareMask[move.ToSquare] | Constants.SquareMask[move.FromSquare];
            //            break;
            //        }
            //    case PieceColor.Black:
            //        {
            //            this.blackBitBoardSet[move.PieceMoved] ^= Constants.SquareMask[move.ToSquare] | Constants.SquareMask[move.FromSquare];
            //            break;
            //        }
            //}

        }

        internal void Equip()
        {
            this.InitializePieceSet();
            this.InitializeCastlingStatus();
            this.InitializeSideToMove();
            this.InitializeEnPassantSquare();
            this.InitializeBitBoards();
        }
        internal void Clear()
        {

        }
        internal void LoadGame(FenString fenString)
        {
            this.InitializeCastlingStatus(fenString);
            this.InitializeSideToMove(fenString);
            this.InitializePieceSet(fenString);
            this.InitializeEnPassantSquare(fenString);
            this.InitializeBitBoards(fenString);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPieceSet(byte pieceColor, byte pieceType)
        {
            return this.BitBoardSet[pieceColor][pieceType];
            //return pieceColor == PieceColor.White ? whiteBitBoardSet[pieceType] : blackBitBoardSet[pieceType];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPlayerPieces(byte color)
        {
            return color == PieceColor.White ? this.WhitePieces : this.BlackPieces;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetEnemyPieces(byte color)
        {
            return color == PieceColor.White ? this.BlackPieces : this.WhitePieces;
        }

        // INITIALIZE

        private void InitializeBitBoards()
        {
            this.BitBoardSet[PieceColor.White][PieceType.Pawn] = Constants.InitialPositions.WhitePawns;
            this.BitBoardSet[PieceColor.White][PieceType.Knight] = Constants.InitialPositions.WhiteKnights;
            this.BitBoardSet[PieceColor.White][PieceType.Bishop] = Constants.InitialPositions.WhiteBishops;
            this.BitBoardSet[PieceColor.White][PieceType.Rook] = Constants.InitialPositions.WhiteRooks;
            this.BitBoardSet[PieceColor.White][PieceType.Queen] = Constants.InitialPositions.WhiteQueen;
            this.BitBoardSet[PieceColor.White][PieceType.King] = Constants.InitialPositions.WhiteKing;

            this.BitBoardSet[PieceColor.Black][PieceType.Pawn] = Constants.InitialPositions.BlackPawns;
            this.BitBoardSet[PieceColor.Black][PieceType.Knight] = Constants.InitialPositions.BlackKnights;
            this.BitBoardSet[PieceColor.Black][PieceType.Bishop] = Constants.InitialPositions.BlackBishops;
            this.BitBoardSet[PieceColor.Black][PieceType.Rook] = Constants.InitialPositions.BlackRooks;
            this.BitBoardSet[PieceColor.Black][PieceType.Queen] = Constants.InitialPositions.BlackQueen;
            this.BitBoardSet[PieceColor.Black][PieceType.King] = Constants.InitialPositions.BlackKing;

            this.InitializeGenericBitBoards();
        }

        private void InitializeBitBoards(FenString fenString)
        {
            for (int i = 0; i < fenString.PiecePlacement.Length; i++)
            {
                if (fenString.PiecePlacement[i].Color != PieceColor.None)
                    this.BitBoardSet[fenString.PiecePlacement[i].Color][fenString.PiecePlacement[i].Type] |= Constants.SquareMask[i];
            }

            this.InitializeGenericBitBoards();
        }

        private void InitializeGenericBitBoards()
        {
            this.WhitePieces =
                  this.BitBoardSet[PieceColor.White][PieceType.Pawn] | this.BitBoardSet[PieceColor.White][PieceType.Knight]
                | this.BitBoardSet[PieceColor.White][PieceType.Bishop] | this.BitBoardSet[PieceColor.White][PieceType.Rook]
                | this.BitBoardSet[PieceColor.White][PieceType.Queen] | this.BitBoardSet[PieceColor.White][PieceType.King];

            this.BlackPieces =
                  this.BitBoardSet[PieceColor.Black][PieceType.Pawn] | this.BitBoardSet[PieceColor.Black][PieceType.Knight]
                | this.BitBoardSet[PieceColor.Black][PieceType.Bishop] | this.BitBoardSet[PieceColor.Black][PieceType.Rook]
                | this.BitBoardSet[PieceColor.Black][PieceType.Queen] | this.BitBoardSet[PieceColor.Black][PieceType.King];

            this.OccupiedSquares = WhitePieces | BlackPieces;
            this.EmptySquares = ~OccupiedSquares;
        }

        private void InitializeSideToMove()
        {
            this.SideToMove = PieceColor.White;
        }

        private void InitializeSideToMove(FenString fenString)
        {
            this.SideToMove = fenString.SideToMove;
        }

        private void InitializeCastlingStatus()
        {
            this.WhiteCanCastleOO = true;
            this.WhiteCanCastleOOO = true;
            this.BlackCanCastleOO = true;
            this.BlackCanCastleOOO = true;
        }

        private void InitializeCastlingStatus(FenString fenString)
        {
            this.WhiteCanCastleOO = fenString.CanWhiteShortCastle;
            this.WhiteCanCastleOOO = fenString.CanWhiteLongCastle;
            this.BlackCanCastleOO = fenString.CanBlackShortCastle;
            this.BlackCanCastleOOO = fenString.CanBlackLongCastle;
        }

        private void InitializeEnPassantSquare()
        {
            this.EnPassantSquare = null;
        }

        private void InitializeEnPassantSquare(FenString fenString)
        {
            this.EnPassantSquare = fenString.EnPassantSquare;
        }

        private void InitializePieceSet()
        {
            Board.ClearPieceSet(ref this.pieceSet);
            PieceFactory pieceFactory = new PieceFactory();
            /*PEDONI*/
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.White), 8);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.White), 9);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.White), 10);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.White), 11);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.White), 12);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.White), 13);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.White), 14);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.White), 15);
            //
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.Black), 48);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.Black), 49);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.Black), 50);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.Black), 51);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.Black), 52);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.Black), 53);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.Black), 54);
            this.AddPiece(pieceFactory.Create(PieceType.Pawn, PieceColor.Black), 55);

            /*TORRI*/
            this.AddPiece(pieceFactory.Create(PieceType.Rook, PieceColor.White), 0);
            this.AddPiece(pieceFactory.Create(PieceType.Rook, PieceColor.White), 7);
            //
            this.AddPiece(pieceFactory.Create(PieceType.Rook, PieceColor.Black), 56);
            this.AddPiece(pieceFactory.Create(PieceType.Rook, PieceColor.Black), 63);

            /*CAVALLI*/
            this.AddPiece(pieceFactory.Create(PieceType.Knight, PieceColor.White), 1);
            this.AddPiece(pieceFactory.Create(PieceType.Knight, PieceColor.White), 6);
            //
            this.AddPiece(pieceFactory.Create(PieceType.Knight, PieceColor.Black), 57);
            this.AddPiece(pieceFactory.Create(PieceType.Knight, PieceColor.Black), 62);

            /*ALFIERI*/
            this.AddPiece(pieceFactory.Create(PieceType.Bishop, PieceColor.White), 2);
            this.AddPiece(pieceFactory.Create(PieceType.Bishop, PieceColor.White), 5);
            //
            this.AddPiece(pieceFactory.Create(PieceType.Bishop, PieceColor.Black), 58);
            this.AddPiece(pieceFactory.Create(PieceType.Bishop, PieceColor.Black), 61);

            /*RE*/
            this.AddPiece(pieceFactory.Create(PieceType.King, PieceColor.White), 4);
            //
            this.AddPiece(pieceFactory.Create(PieceType.King, PieceColor.Black), 60);

            /*REGINE*/
            this.AddPiece(pieceFactory.Create(PieceType.Queen, PieceColor.White), 3);
            //
            this.AddPiece(pieceFactory.Create(PieceType.Queen, PieceColor.Black), 59);
        }

        private void InitializePieceSet(FenString fenString)
        {
            fenString.PiecePlacement.CopyTo(this.pieceSet, 0);
        }

        internal static void ClearPieceSet(ref Piece[] set)
        {
            set = Enumerable.Repeat<Piece>(new Piece(PieceColor.None, PieceType.None), 64).ToArray();
        }


    }
}
