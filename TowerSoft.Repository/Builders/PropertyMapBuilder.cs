﻿using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository.Maps;

namespace TowerSoft.Repository.Builders {
    public partial class PropertyMapBuilder : IEquatable<PropertyMapBuilder> {
        private string PropertyName { get; }
        private bool IsAutonumber { get; set; }
        private bool IsID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public PropertyMapBuilder(string propertyName) {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Maps the property to a column name
        /// </summary>
        /// <param name="columnName">Column name in the database</param>
        /// <returns></returns>
        public IMap To(string columnName) {
            return GetReturnMap(columnName);
        }

        /// <summary>
        /// Maps the property to a column with the same name
        /// </summary>
        /// <returns></returns>
        public IMap ToSameName() {
            return GetReturnMap(null);
        }

        /// <summary>
        /// Marks the property as not mapped. Use this to overide a map generated by the auto mapping option.
        /// </summary>
        /// <returns></returns>
        public IMap NotMapped() {
            return new Map(PropertyName, null);
        }

        /// <summary>
        /// Marks the map as an autonumber map. Use this for primary keys that auto increment
        /// </summary>
        /// <returns></returns>
        public PropertyMapBuilder AsAutonumber() {
            IsAutonumber = true;
            return this;
        }

        /// <summary>
        /// Marls the map as an ID map. Use this for primary keys that do not auto increment
        /// </summary>
        /// <returns></returns>
        public PropertyMapBuilder AsID() {
            IsID = true;
            return this;
        }

        private IMap GetReturnMap(string columnName = null) {
            if (IsAutonumber)
                return new AutonumberMap(PropertyName, columnName ?? PropertyName);
            else if (IsID)
                return new IDMap(PropertyName, columnName ?? PropertyName);
            return new Map(PropertyName, columnName ?? PropertyName);
        }

        public override int GetHashCode() {
            return PropertyName.ToLower().GetHashCode();
        }

        public bool Equals(PropertyMapBuilder other) {
            return other != null && PropertyName.Equals(other.PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}