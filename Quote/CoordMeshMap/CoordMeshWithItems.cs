using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quote.CoordMeshMap
{
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
