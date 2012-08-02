using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitBoard_based_Chess
{
    internal static class CompassRose
    {
        
            //northwest    north   northeast
            //noWe         nort         noEa
            //        +7    +8    +9
            //            \  |  /
            //west    -1 <-  0 -> +1    east
            //            /  |  \
            //        -9    -8    -7
            //soWe         sout         soEa
            //southwest    south   southeast

        internal static UInt64 OneStepSouth(UInt64 bitBoard) { return bitBoard >> 8; }
        internal static UInt64 OneStepNorth(UInt64 bitBoard) { return bitBoard << 8; }
        internal static UInt64 OneStepWest(UInt64 bitBoard) { return bitBoard >> 1 & Constants.NotHFile; }
        internal static UInt64 OneStepEast(UInt64 bitBoard) { return bitBoard << 1 & Constants.NotAFile; }

        internal static UInt64 OneStepNorthEast(UInt64 bitBoard) { return bitBoard << 9 & Constants.NotAFile; }
        internal static UInt64 OneStepNorthWest(UInt64 bitBoard) { return bitBoard << 7 & Constants.NotHFile; }
        internal static UInt64 OneStepSouthEast(UInt64 bitBoard) { return bitBoard >> 7 & Constants.NotAFile; }
        internal static UInt64 OneStepSouthWest(UInt64 bitBoard) { return bitBoard >> 9 & Constants.NotHFile; }


        //internal static class Knight
        //{
        //    //        noNoWe    noNoEa
        //    //            +15  +17
        //    //             |     |
        //    //noWeWe  +6 __|     |__+10  noEaEa
        //    //              \   /
        //    //               >0<
        //    //           __ /   \ __
        //    //soWeWe -10   |     |   -6  soEaEa
        //    //             |     |
        //    //            -17  -15
        //    //        soSoWe    soSoEa

        //    internal static UInt64 OneStepNorthNorthEast(UInt64 knight) { return knight << 17 & BoardHelper.NotAFile; }
        //    internal static UInt64 OneStepNorthEastEast(UInt64 knight) { return knight << 10 & BoardHelper.NotABFile; }
        //    internal static UInt64 OneStepSouthEastEast(UInt64 knight) { return knight >> 6 & BoardHelper.NotABFile; }
        //    internal static UInt64 OneStepSouthSouthEast(UInt64 knight) { return knight >> 15 & BoardHelper.NotAFile; }
        //    internal static UInt64 OneStepNorthNorthWest(UInt64 knight) { return knight << 15 & BoardHelper.NotHFile; }
        //    internal static UInt64 OneStepNorthWestWest(UInt64 knight) { return knight << 6 & BoardHelper.NotGHFile; }
        //    internal static UInt64 OneStepSouthWestWest(UInt64 knight) { return knight >> 10 & BoardHelper.NotGHFile; }
        //    internal static UInt64 OneStepSouthSouthWest(UInt64 knight) { return knight >> 17 & BoardHelper.NotHFile; }
        //}

    }
}
