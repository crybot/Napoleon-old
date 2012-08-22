using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace BitBoard_based_Chess
{
    internal sealed class Board
    {

        internal bool WhiteCanCastleOO;
        internal bool WhiteCanCastleOOO;
        internal bool BlackCanCastleOO;
        internal bool BlackCanCastleOOO;

        internal int EnPassantSquare;

        internal byte SideToMove;

        private readonly int[] kingSquare = new int[2]; // by color
        internal readonly Piece[] pieceSet = new Piece[64];
        private readonly BitBoard[][] bitBoardSet = new BitBoard[2][];

        internal BitBoard WhitePieces = Constants.Empty;
        internal BitBoard BlackPieces = Constants.Empty;
        internal BitBoard OccupiedSquares = Constants.Empty;
        internal BitBoard EmptySquares = Constants.Empty;

        internal Board()
        {
            MovePackHelper.InitAttacks(); //inizializza i vari array di attacchi precalcolati per la generazione delle mosse

            this.bitBoardSet[0] = new BitBoard[6];
            this.bitBoardSet[1] = new BitBoard[6];

            for (int i = 0; i < 6; i++)
            {
                this.bitBoardSet[PieceColor.White][i] = Constants.Empty;
                this.bitBoardSet[PieceColor.Black][i] = Constants.Empty;
            }
        }

        internal void AddPiece(Piece piece, int pos)
        {
            this.pieceSet[pos] = piece;
        }
        internal void RemovePiece()
        {
            ///TODO
            ///TODO
            ///TODO
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void MakeMove(Move move)
        {
            //ARRAY
            this.pieceSet[move.ToSquare] = this.pieceSet[move.FromSquare]; // muove il pezzo
            this.pieceSet[move.FromSquare] = new Piece(PieceColor.None, PieceType.None); // svuota la casella di partenza

            //BITBOARDS
            BitBoard From = Constants.SquareMask[move.FromSquare];
            BitBoard To = Constants.SquareMask[move.ToSquare];
            BitBoard FromTo = From | To;


            this.bitBoardSet[this.SideToMove][move.PieceMoved] ^= FromTo;
            if (this.SideToMove == PieceColor.White)
                this.WhitePieces ^= FromTo;
            else
                this.BlackPieces ^= FromTo;

            if (move.PieceMoved == PieceType.King)
                this.kingSquare[this.SideToMove] = move.ToSquare;

            if (move.IsCapture())
            {
                this.bitBoardSet[this.SideToMove.GetOpposite()][move.PieceCaptured] ^= To;

                //aggiorna i pezzi dell'avversario
                if (this.SideToMove == PieceColor.White)
                    this.BlackPieces ^= To;
                else
                    this.WhitePieces ^= To;

                this.OccupiedSquares ^= From;
                this.EmptySquares ^= From;
            }
            else
            {
                this.OccupiedSquares ^= FromTo;
                this.EmptySquares ^= FromTo;
            }

            this.SideToMove = this.SideToMove.GetOpposite();

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void UndoMove(Move move)
        {
            //ARRAY
            this.pieceSet[move.FromSquare] = this.pieceSet[move.ToSquare]; // muove il pezzo
            this.pieceSet[move.ToSquare] = move.IsCapture() ? new Piece(SideToMove, move.PieceCaptured) : new Piece(PieceColor.None, PieceType.None); // svuota o riempie la casella di partenza 

            //BITBOARDS
            BitBoard From = Constants.SquareMask[move.FromSquare];
            BitBoard To = Constants.SquareMask[move.ToSquare];
            BitBoard FromTo = From | To;

            this.SideToMove = this.SideToMove.GetOpposite();

            // aggiorna la bitboard
            this.bitBoardSet[this.SideToMove][move.PieceMoved] ^= FromTo;
            if (this.SideToMove == PieceColor.White)
                this.WhitePieces ^= FromTo;
            else
                this.BlackPieces ^= FromTo;

            if (move.PieceMoved == PieceType.King)
                this.kingSquare[this.SideToMove] = move.FromSquare;

            if (move.IsCapture())
            {
                this.bitBoardSet[this.SideToMove.GetOpposite()][move.PieceCaptured] ^= To;

                //aggiorna i pezzi dell'avversario
                if (this.SideToMove == PieceColor.White)
                    this.BlackPieces ^= To;
                else
                    this.WhitePieces ^= To;

                this.OccupiedSquares ^= From;
                this.EmptySquares ^= From;
            }
            else
            {
                this.OccupiedSquares ^= FromTo;
                this.EmptySquares ^= FromTo;
            }
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
            return this.bitBoardSet[pieceColor][pieceType];
            //return pieceColor == PieceColor.White ? whiteBitBoardSet[pieceType] : blackBitBoardSet[pieceType];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPlayerPieces()
        {
            return SideToMove == PieceColor.White ? this.WhitePieces : this.BlackPieces;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetEnemyPieces()
        {
            return SideToMove == PieceColor.White ? this.BlackPieces : this.WhitePieces;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BitBoard GetPinnedPieces()
        {
            byte enemy = this.SideToMove.GetOpposite();
            int kingSq = this.kingSquare[this.SideToMove];

            BitBoard playerPieces = this.GetPlayerPieces();
            BitBoard b;
            BitBoard pinned = 0;
            BitBoard pinners = ((this.bitBoardSet[enemy][PieceType.Rook] | this.bitBoardSet[enemy][PieceType.Queen]) & MovePackHelper.PseudoRookAttacks[kingSq])
                | ((this.bitBoardSet[enemy][PieceType.Bishop] | this.bitBoardSet[enemy][PieceType.Queen]) & MovePackHelper.PseudoBishopAttacks[kingSq]);

            while (pinners != 0)
            {
                b = MovePackHelper.InBetween[BitBoard.BitScanForwardReset(ref pinners)][kingSq] & this.OccupiedSquares;

                if ((b != 0) && ((b & (b - 1)) == 0) && ((b & playerPieces) != 0))
                {
                    pinned |= b;
                }
            }

            return pinned;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsMoveLegal(Move move, BitBoard pinned)
        {
            if (move.PieceMoved == PieceType.King)
                return !this.IsAttacked(Constants.SquareMask[move.ToSquare], this.SideToMove);

            if (this.IsAttacked(this.bitBoardSet[SideToMove][PieceType.King], this.SideToMove))
            {
                bool islegal = true;
                this.MakeMove(move);
                islegal = !this.IsAttacked(this.bitBoardSet[SideToMove.GetOpposite()][PieceType.King], this.SideToMove.GetOpposite());
                this.UndoMove(move);

                return islegal;
            }

            return (pinned == 0) || ((pinned & Constants.SquareMask[move.FromSquare]) == 0)
                || MovePackHelper.AreSquareAligned(move.FromSquare, move.ToSquare, this.kingSquare[this.SideToMove]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsAttacked(BitBoard target, byte side)
        {
            BitBoard slidingAttackers;
            BitBoard pawnAttacks;
            BitBoard allPieces = this.OccupiedSquares;
            byte enemyColor = side.GetOpposite();
            int to;

            while (target != 0)
            {
                to = BitBoard.BitScanForwardReset(ref target);
                pawnAttacks = side == PieceColor.White ? MovePackHelper.WhitePawnAttacks[to] : MovePackHelper.BlackPawnAttacks[to];

                if ((this.GetPieceSet(enemyColor, PieceType.Pawn) & pawnAttacks) != 0) return true;
                if ((this.GetPieceSet(enemyColor, PieceType.Knight) & MovePackHelper.KnightAttacks[to]) != 0) return true;
                if ((this.GetPieceSet(enemyColor, PieceType.King) & MovePackHelper.KingAttacks[to]) != 0) return true;

                // file / rank attacks
                slidingAttackers = this.GetPieceSet(enemyColor, PieceType.Queen) | this.GetPieceSet(enemyColor, PieceType.Rook);

                if (slidingAttackers != 0)
                {
                    if ((MovePackHelper.GetRankAttacks(allPieces, to) & slidingAttackers) != 0) return true;
                    if ((MovePackHelper.GetFileAttacks(allPieces, to) & slidingAttackers) != 0) return true;
                }

                // diagonals
                slidingAttackers = this.GetPieceSet(enemyColor, PieceType.Queen) | this.GetPieceSet(enemyColor, PieceType.Bishop);

                if (slidingAttackers != 0)
                {
                    if ((MovePackHelper.GetH1A8DiagonalAttacks(allPieces, to) & slidingAttackers) != 0) return true;
                    if ((MovePackHelper.GetA1H8DiagonalAttacks(allPieces, to) & slidingAttackers) != 0) return true;
                }
            }
            return false;
        }

        internal void Display()
        {
            Piece piece;

            for (int r = 7; r >= 0; r--)
            {
                Console.WriteLine("   ------------------------");

                Console.Write(" {0} ", r + 1);

                for (int c = 0; c <= 7; c++)
                {
                    piece = this.pieceSet[Square.GetSquareIndex(c, r)];
                    Console.Write('[');
                    if (piece.Type != PieceType.None)
                    {
                        Console.ForegroundColor = piece.Color == PieceColor.White ? ConsoleColor.Green : ConsoleColor.DarkRed;
                        Console.Write(this.pieceSet[Square.GetSquareIndex(c, r)].Type.GetInitial());
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(' ');
                    }

                    Console.ResetColor();
                    Console.Write(']');
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n    A  B  C  D  E  F  G  H");
        }


        // INITIALIZE

        private void InitializeBitBoards()
        {
            this.kingSquare[PieceColor.White] = 4;
            this.kingSquare[PieceColor.Black] = 60;

            this.bitBoardSet[PieceColor.White][PieceType.Pawn] = Constants.InitialPositions.WhitePawns;
            this.bitBoardSet[PieceColor.White][PieceType.Knight] = Constants.InitialPositions.WhiteKnights;
            this.bitBoardSet[PieceColor.White][PieceType.Bishop] = Constants.InitialPositions.WhiteBishops;
            this.bitBoardSet[PieceColor.White][PieceType.Rook] = Constants.InitialPositions.WhiteRooks;
            this.bitBoardSet[PieceColor.White][PieceType.Queen] = Constants.InitialPositions.WhiteQueen;
            this.bitBoardSet[PieceColor.White][PieceType.King] = Constants.InitialPositions.WhiteKing;

            this.bitBoardSet[PieceColor.Black][PieceType.Pawn] = Constants.InitialPositions.BlackPawns;
            this.bitBoardSet[PieceColor.Black][PieceType.Knight] = Constants.InitialPositions.BlackKnights;
            this.bitBoardSet[PieceColor.Black][PieceType.Bishop] = Constants.InitialPositions.BlackBishops;
            this.bitBoardSet[PieceColor.Black][PieceType.Rook] = Constants.InitialPositions.BlackRooks;
            this.bitBoardSet[PieceColor.Black][PieceType.Queen] = Constants.InitialPositions.BlackQueen;
            this.bitBoardSet[PieceColor.Black][PieceType.King] = Constants.InitialPositions.BlackKing;

            this.UpdateGenericBitBoards();
        }

        private void InitializeBitBoards(FenString fenString)
        {
            for (int i = 0; i < fenString.PiecePlacement.Length; i++)
            {
                if (fenString.PiecePlacement[i].Type == PieceType.King)
                    this.kingSquare[fenString.PiecePlacement[i].Color] = i;
                if (fenString.PiecePlacement[i].Color != PieceColor.None)
                    this.bitBoardSet[fenString.PiecePlacement[i].Color][fenString.PiecePlacement[i].Type] |= Constants.SquareMask[i];
            }

            this.UpdateGenericBitBoards();
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
            this.EnPassantSquare = Square.Invalid;
        }

        private void InitializeEnPassantSquare(FenString fenString)
        {
            this.EnPassantSquare = fenString.EnPassantSquare;
        }

        private void InitializePieceSet()
        {
            this.ClearPieceSet();

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

        private void ClearPieceSet()
        {
            Enumerable.Repeat<Piece>(new Piece(PieceColor.None, PieceType.None), 64).ToArray().CopyTo(this.pieceSet, 0);
        }

        private void UpdateGenericBitBoards()
        {
            this.WhitePieces =
                  this.bitBoardSet[PieceColor.White][PieceType.Pawn] | this.bitBoardSet[PieceColor.White][PieceType.Knight]
                | this.bitBoardSet[PieceColor.White][PieceType.Bishop] | this.bitBoardSet[PieceColor.White][PieceType.Rook]
                | this.bitBoardSet[PieceColor.White][PieceType.Queen] | this.bitBoardSet[PieceColor.White][PieceType.King];

            this.BlackPieces =
                  this.bitBoardSet[PieceColor.Black][PieceType.Pawn] | this.bitBoardSet[PieceColor.Black][PieceType.Knight]
                | this.bitBoardSet[PieceColor.Black][PieceType.Bishop] | this.bitBoardSet[PieceColor.Black][PieceType.Rook]
                | this.bitBoardSet[PieceColor.Black][PieceType.Queen] | this.bitBoardSet[PieceColor.Black][PieceType.King];

            this.OccupiedSquares = WhitePieces | BlackPieces;
            this.EmptySquares = ~OccupiedSquares;
        }

        internal static void ClearPieceSet(ref Piece[] set)
        {
            set = Enumerable.Repeat<Piece>(new Piece(PieceColor.None, PieceType.None), 64).ToArray();
        }


    }
}
