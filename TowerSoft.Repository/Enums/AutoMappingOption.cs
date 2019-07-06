using System;
using System.Collections.Generic;
using System.Text;

namespace TowerSoft.Repository {
    public enum AutoMappingOption {
        /// <summary>
        /// Default option. No maps will automatically be generated.
        /// </summary>
        None = 1,
        /// <summary>
        /// Automatically generates default mapping based on the names of all of the
        /// public non-virtual properties with public getters and setters. These can be 
        /// overriden by the fluent maps
        /// </summary>
        UseDefaultPropertyMaps
    }
}
