﻿using System;

namespace TowerSoft.Repository.Attributes {
    /// <summary>
    /// Only use on Intersystems Caché databases.
    /// Uses the $HOROLOG date format for this column
    /// with connecting to the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CacheHorologDateAttribute : Attribute {
        /// <summary>
        /// Name of the function used to display the value
        /// </summary>
        public string FunctionName { get; set; }
    }
}
