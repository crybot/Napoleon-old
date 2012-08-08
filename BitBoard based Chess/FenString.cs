using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    class FenString
    {
        public string FullString { get; set; }

        public Piece[] PiecePlacement = new Piece[64];
        public byte SideToMove { get; set; }
        public bool CanWhiteShortCastle { get; set; }
        public bool CanWhiteLongCastle { get; set; }
        public bool CanBlackShortCastle { get; set; }
        public bool CanBlackLongCastle { get; set; }
        public int EnPassantSquare { get; set; }

        public static implicit operator FenString(string str)
        {
            return new FenString(str);
        }
        public FenString(string str)
        {
            this.FullString = str;
            this.Parse();
        }

        public void Parse()
        {
            string[] fields = this.FullString.Split(' ');

            string piecePlacement = fields[0];
            string sideToMove = fields[1];
            string castling = fields[2];
            string enPassant = fields[3];
            string halfMove = fields[4];
            string fullMove = fields[5];

            this.ParsePiecePlacement(piecePlacement);
            this.ParseSideToMove(sideToMove);
            this.ParseCastling(castling);
            this.ParseEnPassant(enPassant);
        }

        private void ParsePiecePlacement(string field)
        {
            Board.ClearPieceSet(ref this.PiecePlacement);

            string[] ranks = field.Split('/');
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
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Pawn, PieceColor.White);
                            break;
                        case 'p':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Pawn, PieceColor.Black);
                            break;
                        case 'N':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Knight, PieceColor.White);
                            break;
                        case 'n':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Knight, PieceColor.Black);
                            break;
                        case 'B':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Bishop, PieceColor.White);
                            break;
                        case 'b':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Bishop, PieceColor.Black);
                            break;
                        case 'R':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Rook, PieceColor.White);
                            break;
                        case 'r':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Rook, PieceColor.Black);
                            break;
                        case 'Q':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Queen, PieceColor.White);
                            break;
                        case 'q':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.Queen, PieceColor.Black);
                            break;
                        case 'K':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.King, PieceColor.White);
                            break;
                        case 'k':
                            this.PiecePlacement[Square.GetSquareIndex(l + empty, 7 - i)] = factory.Create(PieceType.King, PieceColor.Black);
                            break;
                        default:
                            empty += (int)char.GetNumericValue(ranks[i][l]) - 1;
                            break;
                    }

                    #endregion
                }
            }
        }
        private void ParseSideToMove(string field)
        {
            switch (field[0])
            {
                case 'w':
                    this.SideToMove = PieceColor.White;
                    break;
                case 'b':
                    this.SideToMove = PieceColor.Black;
                    break;
            }
        }
        private void ParseCastling(string field)
        {
            this.CanWhiteShortCastle = false;
            this.CanWhiteLongCastle = false;
            this.CanBlackShortCastle = false;
            this.CanBlackLongCastle = false;

            for (int i = 0; i < field.Length; i++)
            {
                switch (field[i])
                {
                    case 'K':
                        this.CanWhiteShortCastle = true;
                        break;
                    case 'k':
                        this.CanBlackShortCastle = true;
                        break;
                    case 'Q':
                        this.CanWhiteLongCastle = true;
                        break;
                    case 'q':
                        this.CanBlackLongCastle = true;
                        break;
                }
            }
        }
        private void ParseEnPassant(string field)
        {
            if (field.Length == 1)
            {
                if (field[0] == '-')
                    this.EnPassantSquare = Square.Invalid;
            }
            else
            {
                this.EnPassantSquare = Square.Parse(field);
            }
        }

    }
}
