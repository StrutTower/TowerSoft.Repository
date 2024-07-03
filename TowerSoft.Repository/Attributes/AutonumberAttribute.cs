using System;

namespace TowerSoft.Repository.Attributes {
    /// <summary>
    /// Defines the properties as an autonumber map
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutonumberAttribute : Attribute {
        /// <summary>
        /// Name of the function used to display the value
        /// </summary>
        public string FunctionName { get; set; }
    }
}
