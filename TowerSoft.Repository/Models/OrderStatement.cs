namespace TowerSoft.Repository {
    /// <summary>
    /// Used to store order by settings from FluentQueryBuilder
    /// </summary>
    public class OrderStatement {
        /// <summary>
        /// Column to order by
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Direction to order by
        /// </summary>
        public bool IsAscending { get; set; }

        internal OrderStatement(string columnName, bool isAscending) {
            ColumnName = columnName;
            IsAscending = isAscending;
        }
    }
}
