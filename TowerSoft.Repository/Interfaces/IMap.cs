namespace TowerSoft.Repository {
    /// <summary>
    /// Database map used to map a column to a property
    /// </summary>
    public interface IMap {
        /// <summary>
        /// Name of the property on the object
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Name of the column in the table
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Returns the value of the property for the supplied entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        object GetValue(object entity);

        /// <summary>
        /// Sets the value of the property on the supplied entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="value"></param>
        void SetValue(object entity, object value);
    }
}
