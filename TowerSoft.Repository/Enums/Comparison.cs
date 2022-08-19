namespace TowerSoft.Repository {
    /// <summary>
    /// Set the comparison that will be used when generating the SQL where statement
    /// </summary>
    public enum Comparison {
        /// <summary>
        /// Column is equal to the value
        /// </summary>
        Equals,
        /// <summary>
        /// Column is not equal to the value
        /// </summary>
        NotEquals,
        /// <summary>
        /// Column is greater than the value
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Column is greater or equal to the value
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// Column is less than the value
        /// </summary>
        LessThan,
        /// <summary>
        /// Column is less than or equal to the value
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// Column LIKE value with a wildcard added to the left side of the value
        /// </summary>
        LikeLeftSideWildcard,
        /// <summary>
        /// Column LIKE value with a wildcard added to the right side of the value
        /// </summary>
        LikeRightSideWildcard,
        /// <summary>
        /// Column LIKE value with a wildcard added to both sides of the value
        /// </summary>
        LikeBothSidesWildcard
    }
}
