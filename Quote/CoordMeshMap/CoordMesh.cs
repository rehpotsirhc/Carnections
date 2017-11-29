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
        private BoundaryHandler.BoundaryHandleFunc _handleBoundaryFunc;

        public CoordMesh(CoordMeshSettings settings, BoundaryHandler.BoundaryHandleFunc handleBoundaryFunc = null)
        {
            this._settings = settings;
            if (handleBoundaryFunc != null)
                this._handleBoundaryFunc = handleBoundaryFunc;
            else
                this._handleBoundaryFunc = BoundaryHandler.HandleBoundary_ConstrainOutOfBounds;
        }

        public ICoord FitLonLat(PosNumber lon, PosNumber lat)
        {
            var coord1 = FitLon(lon);
            var coord2 = FitLat(lat);

            if (!coord1.Ok || !coord1.Ok)
            {
                //TODO: throw exception? log?
            }
            return new Coord(coord1.Coord, coord2.Coord);
        }

        public BoundaryHandler FitLon(PosNumber lon)
        {
            return FitToCoord(lon, this._settings.LON_MAX, this._settings.LON_MIN, this._settings.RESOLUTION, this._handleBoundaryFunc);
        }

        public BoundaryHandler FitLat(PosNumber lat)
        {
            return FitToCoord(lat, this._settings.LAT_MAX, this._settings.LAT_MIN, this._settings.RESOLUTION, this._handleBoundaryFunc);
        }
        public PosInt FitLonSide()
        {
            return FitSide(this._settings.LON_MAX, this._settings.LON_MIN, this._settings.RESOLUTION);
        }
        public PosInt FitLatSide()
        {
            return FitSide(this._settings.LAT_MAX, this._settings.LAT_MIN, this._settings.RESOLUTION);
        }
        private static BoundaryHandler FitToCoord(PosNumber point, PosInt max, PosInt min, PosNumber resolution, BoundaryHandler.BoundaryHandleFunc handleBoundaryFunc)
        {
            return handleBoundaryFunc((int)Math.Floor((point.AsDouble - min.AsInt) / resolution), CalculateLength(max, min, resolution));
        }
        private static PosInt FitSide(PosInt max, PosInt min, PosNumber resolution)
        {
            return CalculateLength(max, min, resolution);
        }
        private static PosInt CalculateSide(PosInt max, PosInt min)
        {
            return new PosInt((int)Math.Ceiling(max.AsDouble - min.AsDouble), zeroAllowed: false);
        }
        private static PosInt CalculateLength(PosInt max, PosInt min, PosNumber resolution)
        {
            return new PosInt((int)Math.Ceiling(CalculateSide(max, min).AsInt * resolution.AsDouble), zeroAllowed: false);
        }
    }
}
