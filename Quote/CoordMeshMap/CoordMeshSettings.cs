using System;
using System.Collections.Generic;
using System.Text;

namespace Quote.CoordMeshMap
{
    public class CoordMeshSettings
    {
        public readonly PosNumber RESOLUTION;
        public readonly PosInt LON_MIN;
        public readonly PosInt LON_MAX;
        public readonly PosInt LAT_MIN;
        public readonly PosInt LAT_MAX;

        public CoordMeshSettings(double resolution, int lonMin, int lonMax, int latMin, int latMax)
        {
            this.RESOLUTION = resolution;
            this.LON_MIN = lonMin;
            this.LON_MAX = lonMax;
            this.LAT_MIN = latMin;
            this.LAT_MAX = latMax;

            if (!SettingsAreValid())
                throw new ArgumentException("The coord mesh settings are invalid. The max's have to be >= to the min's");
        }
        public bool SettingsAreValid()
        {
            return this.LON_MAX >= this.LON_MIN &&
                this.LAT_MAX >= this.LAT_MIN;
        }
    }
}
