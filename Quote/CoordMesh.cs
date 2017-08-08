using Common.Interfaces;
using System;
using System.Collections.Generic;

namespace Quote
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


    public class CoordMeshSettings
    {
        public readonly double RESOLUTION;
        public readonly int LON_MIN;
        public readonly int LON_MAX;
        public readonly int LAT_MIN;
        public readonly int LAT_MAX;

        public CoordMeshSettings(double resolution, int lonMin, int lonMax, int latMin, int latMax)
        {
            this.RESOLUTION = resolution;
            this.LON_MIN = lonMin;
            this.LON_MAX = lonMax;
            this.LAT_MIN = latMin;
            this.LAT_MAX = latMax;
        }
    }

    public interface ICoord
    {
        int X { get; }
        int Y { get; }

    }

    public class Coord : ICoord, System.IEquatable<ICoord>
    {
        public int X { get; }
        public int Y { get; }

        public Coord(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(ICoord other)
        {
            return other != null && other.X == this.X && other.Y == this.Y;
        }
    }

    public class ItemCoord<T> : Coord
    {
        public T Item { get; }
        public ItemCoord(T item, int x, int y) : base(x, y)
        {
            this.Item = item;
        }
    }

    public class CoordMesh
    {

        protected readonly CoordMeshSettings _settings;

        public CoordMesh(CoordMeshSettings settings)
        {
            this._settings = settings;
        }

        public ICoord FitLonLat(double lon, double lat)
        {
            return new Coord(FitLon(lon), FitLat(lat));
        }

        public int FitLon(double lon)
        {
            return FitToCoord(lon, this._settings.LON_MAX, this._settings.LON_MIN, this._settings.RESOLUTION);
        }

        public int FitLat(double lat)
        {
            return FitToCoord(lat, this._settings.LAT_MAX, this._settings.LAT_MIN, this._settings.RESOLUTION);
        }
        public int FitLonSide()
        {
            return FitSide(this._settings.LON_MAX, this._settings.LON_MIN, this._settings.RESOLUTION);
        }
        public int FitLatSide()
        {
            return FitSide(this._settings.LAT_MAX, this._settings.LAT_MIN, this._settings.RESOLUTION);
        }

        private static int FitToCoord(double point, int max, int min, double res)
        {
            return BoundaryCheck((int)Math.Floor((point - min) / res), max - min);
        }
        private static int FitSide(int max, int min, double resolution)
        {
            return (int)Math.Floor((max - min) / resolution);
        }

        protected static int BoundaryCheck(int coord, int length)
        {
            if (coord >= length)
                coord = length - 1;
            else if (coord < 0)
                coord = 0;

            return coord;
        }
    }
    public class CoordMeshWithItems<T> : CoordMesh where T : class, IHasId
    {
        private readonly IDictionary<int, T>[,] _mesh;

        public CoordMeshWithItems(CoordMeshSettings settings) : base(settings)
        {
            this._mesh = new IDictionary<int, T>[this.FitLonSide(), this.FitLatSide()];
        }

        /// <summary>
        /// Adds item T to coordinate mesh at the specified longitude and latitude. 
        /// All items at each coordinate have a unique key. Override behavior controlled by 'overrideIfExists' parameter
        /// </summary>
        /// <param name="lon">Longitude coordinate to add at</param>
        /// <param name="lat">Latitude coordinate to add at</param>
        /// <param name="item">item with id(key) to add</param>
        /// <param name="overrideIfExists">If true, overrides item if an item with the same key already exists (at coordinate). 
        /// If false, item is not added and mesh is not changed if an item with the same key already exists (at coordinate). </param>
        public void Add(double lon, double lat, T item, bool overrideIfExists = false)
        {
            Add(FitLonLat(lon, lat), item, overrideIfExists);
        }

        public void Add(ICoord coords, T item, bool overrideIfExists = false)
        {
            if (overrideIfExists)
            {
                //adding an item this way, an existing item will be overridden if it exists. If not, a new item with the key will be added
                GetItems(coords)[item.Id] = item;
            }
            else
            {
                try
                {
                    //this way ArgumentException will be thrown if the key exists
                    GetItems(coords).Add(item.Id, item);
                }
                catch (ArgumentException)
                {
                    //TODO: do nothing or indicate somehow that at duplicate add was attempted
                }
            }
        }

        /// <summary>
        // Removes item T from coordinate mesh at the specified longitude and latitude. 
        /// </summary>
        /// <param name="lon">Longitude coordinate to remove from </param>
        /// <param name="lat">Latitude coordinate to remove from </param>
        /// <param name="item">item with id(key) to remove</param>
        /// <returns>True if the item was successfully removed. False otherwise</returns>
        public bool Remove(double lon, double lat, T item)
        {
            return Remove(FitLonLat(lon, lat), item);
        }

        public bool Remove(ICoord coords, T item)
        {
            if (item == default(T))
                return false;
            return this.Remove(coords, item.Id);
        }


        /// <summary>
        // Removes item with the specified key from coordinate mesh at the specified longitude and latitude. 
        /// </summary>
        /// <param name="lon">Longitude coordinate to remove from </param>
        /// <param name="lat">Latitude coordinate to remove from </param>
        /// <param name="key">Key of item to remove</param>
        /// <returns>True if the item was successfully removed. False otherwise</returns>
        public bool Remove(double lon, double lat, int key)
        {
            return Remove(FitLonLat(lon, lat), key);
        }

        public bool Remove(ICoord coords, int key)
        {
            return GetItems(coords).Remove(key);
        }

        public T GetItem(double lon, double lat, int key)
        {
            return GetItem(FitLonLat(lon, lat), key);
        }

        public T GetItem(ICoord coords, int key)
        {
            if (GetItems(coords).TryGetValue(key, out T item))
                return item;
            return default(T);
        }
        public bool TryGetItem(double lon, double lat, int key, out T item)
        {
            return TryGetItem(FitLonLat(lon, lat), key, out item);
        }

        public bool TryGetItem(ICoord coords, int key, out T item)
        {
            return (item = GetItem(coords, key)) != default(T);
        }


        public IDictionary<int, T> GetItems(ICoord coords)
        {
            return this._mesh[coords.X, coords.Y] = this._mesh[coords.X, coords.Y] ?? new Dictionary<int, T>();
        }

        public IDictionary<int, T> GetItems(double lon, double lat)
        {
            return GetItems(FitLonLat(lon, lat));
        }


    }
}
