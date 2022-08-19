using System;

namespace TowerSoft.Repository.Attributes {
    /// <summary>
    /// Only use on Intersystems Caché databases.
    /// Uses the Fileman logical date format for this column with
    /// connecting to the database.
    /// </summary>
    public class CacheFilemanDateAttribute : Attribute {
        public string FunctionName { get; set; }
    }
}
