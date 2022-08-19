using System;

namespace TowerSoft.Repository.Attributes {
    /// <summary>
    /// Defines the properties as an autonumber map
    /// </summary>
    public class AutonumberAttribute : Attribute {
        public string FunctionName { get; set; }
    }
}
