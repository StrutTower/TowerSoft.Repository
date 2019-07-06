namespace TowerSoft.Repository {
    /// <summary>
    /// Object used to build simple SQL statments
    /// </summary>
    public class WhereCondition {
        /// <summary>
        /// Column name
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Value to compare to
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Comparison type
        /// </summary>
        public Comparison Comparison { get; private set; }

        /// <summary>
        /// Creates a new WhereCondition object that will be used to build a SQL statement
        /// </summary>
        /// <param name="columnName">Column name to compare to</param>
        /// <param name="value">Value to compare to</param>
        /// <param name="comparison">Comparision type. Default is Equals</param>
        public WhereCondition(string columnName, object value, Comparison comparison = Comparison.Equals) {
            ColumnName = columnName;
            Value = value;
            Comparison = comparison;
        }

        /// <summary>
        /// Returns the SQL where statement for this object
        /// </summary>
        /// <returns></returns>
        public string GetComparisonString() {
            string s = "";
            if (Comparison == Comparison.Equals && Value == null) {
                s = "IS";
            } else if (Comparison == Comparison.NotEquals && Value == null) {
                s = "IS NOT";
            } else {
                switch (Comparison) {
                    case Comparison.Equals:
                        s = "=";
                        break;
                    case Comparison.NotEquals:
                        s = "<>";
                        break;
                    case Comparison.GreaterThan:
                        s = ">";
                        break;
                    case Comparison.GreaterThanOrEqual:
                        s = ">=";
                        break;
                    case Comparison.LessThan:
                        s = "<";
                        break;
                    case Comparison.LessThanOrEqual:
                        s = "<=";
                        break;
                    case Comparison.LikeBothSidesWildcard:
                    case Comparison.LikeLeftSideWildcard:
                    case Comparison.LikeRightSideWildcard:
                        s = "LIKE";
                        break;
                }
            }
            return s;
        }
        
        /// <summary>
        /// Returns the parameter value
        /// </summary>
        /// <returns></returns>
        public object GetParameterValue() {
            if (IsNullEqualsOrNotEquals()) {
                Value = "NULL";
            }
            switch (Comparison) {
                case Comparison.LikeRightSideWildcard:
                    return Value + "%";
                case Comparison.LikeLeftSideWildcard:
                    return "%" + Value;
                case Comparison.LikeBothSidesWildcard:
                    return "%" + Value + "%";
                default:
                    return Value;
            }
        }

        /// <summary>
        /// Check if the SQL statment should be convert to a null where statement
        /// </summary>
        /// <returns></returns>
        public bool IsNullEqualsOrNotEquals() {
            return Value == null && Comparison == Comparison.Equals || Comparison == Comparison.NotEquals;
        }
    }
}