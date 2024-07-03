using System;

namespace TowerSoft.Repository.Attributes {
    /// <summary>
    /// Sets a property as a primary key
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyMapAttribute : Attribute { }
}
