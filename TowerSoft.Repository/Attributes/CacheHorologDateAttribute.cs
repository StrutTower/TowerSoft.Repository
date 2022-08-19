using System;

namespace TowerSoft.Repository.Attributes {
    /// <summary>
    /// Only use on Intersystems Caché databases.
    /// Uses the $HOROLOG date format for this column
    /// with connecting to the database.
    /// </summary>
    public class CacheHorologDateAttribute : Attribute {
        public string FunctionName { get; set; }
    }
}
