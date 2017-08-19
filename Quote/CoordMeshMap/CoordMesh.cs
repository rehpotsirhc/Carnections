using Common.Interfaces;
using System;
using System.Collections.Generic;

namespace Quote.CoordMeshMap
{

    //long range: -66.84(E) ==> -124.45(W)   *57.61
    //lat range:  25.13(S) ==> 49.35 (N)    *24.22

    //MESH long range: 64 ==> 126 *62 (extra for padding)
    //MESH lat range: 24 ==> 51   *27 (extra for padding)
    //MESH Unit length: .5, MESH is *124 x *54


    //example indices to lat/long mappping

    //long: 64 => 0; 64.5 => 1;65 => 2; 65.5 => 3; 66 => 4; 67 => 6; 68 => 8; 69 =>  10
    //example calculations:
    //round((64 - 64) / .5) = 0
    //round((65.5 - 64) / .5) = 3
    //round((68.4 - 64) / .5) = 9
    //round((68.5 - 64) / .5) = 9
    //round((68.6 - 64) / .5) = 9
    //round((68.7 - 64) / .5) = 9
    //round((68.8 - 64) / .5) = 10
    //round((69 - 64) / .5) = 10
    //round((69.2 - 64) / .5) = 10
    //round((69.3 - 64) / .5) = 11; summary 68.75 - 69.25
    //round((126 - 64) / .5) = 124

    //floor: [2 ==> 2.5)  / .5 = 4
    //flor:  [2.5 ==> 3) / .5 = 5

    //   [min, max) 

    //62
    //.3
    //206.66667


    //ISSUE: Everything outside boundary gets pushed to the edge of the boundary
    //see BoundaryCheck

   

    //public class ItemCoord<T> : Coord
    //{
    //    public T Item { get; }
    //    public ItemCoord(T item, int x, int y) : base(x, y)
    //    {
    //        this.Item = item;
    //    }
    //}

    public class CoordMesh
    {

        protected readonly CoordMeshSettings _settings;
        private Func<int, int, int> _constrainBoundaryFunc;

        public CoordMesh(CoordMeshSettings settings, Func<int, int, int> ConstrainBoundaryFunc = null)
        {
            this._settings = settings;
            if (_constrainBoundaryFunc != null)
                this._constrainBoundaryFunc = DefaultConstrainBoundary;

        }

        public ICoord FitLonLat(double lon, double lat)
        {
            return new Coord(FitLon(lon), FitLat(lat));
        }

        public int FitLon(double lon)
        {
            return FitToCoord(lon, this._settings.LON_MAX, this._settings.LON_MIN, this._settings.RESOLUTION, this._constrainBoundaryFunc);
        }

        public int FitLat(double lat)
        {
            return FitToCoord(lat, this._settings.LAT_MAX, this._settings.LAT_MIN, this._settings.RESOLUTION, this._constrainBoundaryFunc);
        }
        public int FitLonSide()
        {
            return FitSide(this._settings.LON_MAX, this._settings.LON_MIN, this._settings.RESOLUTION);
        }
        public int FitLatSide()
        {
            return FitSide(this._settings.LAT_MAX, this._settings.LAT_MIN, this._settings.RESOLUTION);
        }
        public static int DefaultConstrainBoundary(int coord, int length)
        {
            if (coord >= length)
                coord = length - 1;
            else if (coord < 0)
                coord = 0;

            return coord;
        }
        private static int FitToCoord(double point, int max, int min, double res, Func<int, int, int> ConstrainBoundaryFunc)
        {
            return ConstrainBoundaryFunc((int)Math.Floor((point - min) / res), (max - min) + 1);
        }
        private static int FitSide(int max, int min, double resolution)
        {
            return (int)Math.Floor((max - min) / resolution);
        }
    }
}
