using System;

namespace TowerSoft.Repository.Attributes {
    /// <summary>
    /// Sets a property as not mapped to any database column
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SkipMappingAttribute : Attribute { }
}
