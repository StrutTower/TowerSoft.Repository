namespace TowerSoft.Repository {
    /// <summary>
    /// Options for how the EntityMap is processed
    /// </summary>
    public enum AutoMappingOption {
        /// <summary>
        /// Default option. No maps will automatically be generated.
        /// </summary>
        None = 1,
        /// <summary>
        /// Automatically generates default mapping based on the names of all of the
        /// public properties with public getters and setters. These can be
        /// overridden by the fluent maps.
        /// </summary>
        UseDefaultPropertyMaps,
        /// <summary>
        /// Automatically generates default mapping based on the names of all of the
        /// public properties with public getters and setters except properties that
        /// end with _Object or _Objects. These can be overridden by the fluent maps.
        /// </summary>
        UseNonObjectPropertyMaps
    }
}
