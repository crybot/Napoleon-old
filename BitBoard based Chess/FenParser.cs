using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    class FenString
    {
        public string Str;

        public Piece[] PiecePlacement { get; set; }
        public PieceColor SideToMove { get; set; }
        public bool CanWhiteShortCastle { get; set; }
        public bool CanWhiteLongCastle { get; set; }
        public bool CanBlackShortCastle { get; set; }
        public bool CanBlackLongCastle { get; set; }
        public Square? EnPassantTargetSquare { get; set; }

        public static implicit operator FenString(string str)
        {
            return new FenString(str);
        }
        public FenString(string fenStr)
        {
            this.Str = fenStr;
            PiecePlacement = new Piece[64];
        }

        public FenString Parse()
        {
            FenString fen = new FenString(this.Str);
            string input = this.Str;
            string[] fields = input.Split(' ');

            string piecePlacement = fields[0];
            string sideToMove = fields[1];
            string castling = fields[2];
            string enPassant = fields[3];
            string halfMove = fields[4];
            string fullMove = fields[5];

            //PIECE PLACEMENT
            #region PiecePlacement

            string[] ranks = piecePlacement.Split('/');
            PieceFactory factory = new PieceFactory();

            for (int i = 0; i < ranks.Length; i++)
            {
                int empty = 0;
                for (int l = 0; l < ranks[i].Length; l++)
                {
                    #region pieceCreation

                    switch (ranks[i][l])
                    {
                        case 'P':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Pawn, PieceColor.White);
                            break;
                        case 'p':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Pawn, PieceColor.Black);
                            break;
                        case 'N':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Knight, PieceColor.White);
                            break;
                        case 'n':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Knight, PieceColor.Black);
                            break;
                        case 'B':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Bishop, PieceColor.White);
                            break;
                        case 'b':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Bishop, PieceColor.Black);
                            break;
                        case 'R':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Rook, PieceColor.White);
                            break;
                        case 'r':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Rook, PieceColor.Black);
                            break;
                        case 'Q':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Queen, PieceColor.White);
                            break;
                        case 'q':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Queen, PieceColor.Black);
                            break;
                        case 'K':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.King, PieceColor.White);
                            break;
                        case 'k':
                            fen.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.King, PieceColor.Black);
                            break;
                        default:
                            empty += (int)char.GetNumericValue(ranks[i][l]) - 1;
                            break;
                    }

                    #endregion
                }
            }
            #endregion

            //SIDE TO MOVE
            #region SideToMove

            switch (sideToMove[0])
            {
                case 'w':
                    fen.SideToMove = PieceColor.White;
                    break;
                case 'b':
                    fen.SideToMove = PieceColor.Black;
                    break;
            }

            #endregion

            //CASTLING ABILITY
            #region CastlingAbility

            fen.CanWhiteShortCastle = false;
            fen.CanWhiteLongCastle = false;
            fen.CanBlackShortCastle = false;
            fen.CanBlackLongCastle = false;

            //switch (castling[0])
            //{
            //    case '-':
            //        break;
            //}

            if (castling.Length > 1)
            {
                for (int i = 0; i < castling.Length; i++)
                {
                    switch (castling[i])
                    {
                        case 'K':
                            fen.CanWhiteShortCastle = true;
                            break;
                        case 'k':
                            fen.CanBlackShortCastle = true;
                            break;
                        case 'Q':
                            fen.CanWhiteLongCastle = true;
                            break;
                        case 'q':
                            fen.CanBlackLongCastle = true;
                            break;
                    }
                }
            }
            else
            {
                switch (castling[0])
                {
                    case 'K':
                        fen.CanWhiteShortCastle = true;
                        break;
                    case 'k':
                        fen.CanBlackShortCastle = true;
                        break;
                    case 'Q':
                        fen.CanWhiteLongCastle = true;
                        break;
                    case 'q':
                        fen.CanBlackLongCastle = true;
                        break;
                    default:
                        break;
                }
            }

            #endregion

            //EN-PASSANT
            #region EnPassant

            if (enPassant.Length == 1)
            {
                if (enPassant[0] == '-')
                    fen.EnPassantTargetSquare = null;
            }
            else
            {
                fen.EnPassantTargetSquare = enPassant;
            }

            #endregion

            return fen;
        }
    }
}
