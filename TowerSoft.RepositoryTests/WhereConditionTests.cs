using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TowerSoft.Repository;

namespace TowerSoft.RepositoryTests {
    [TestClass]
    public class WhereConditionTests {
        #region GetComparisonString
        [TestMethod]
        public void GetComparisonString_Equals_ShouldReturnEquals() {
            WhereCondition whereCondition = new WhereCondition("ColumeName", "Value");

            string result = whereCondition.GetComparisonString();

            Assert.AreEqual("=", result);
        }

        [TestMethod]
        public void GetComparisonString_NotEquals_ShouldReturnSqlNotEquals() {
            WhereCondition whereCondition = new WhereCondition("ColumnName", "Value", Comparison.NotEquals);

            string result = whereCondition.GetComparisonString();

            Assert.AreEqual("<>", result);
        }

        [TestMethod]
        public void GetComparisonString_GreaterTHAN_ShouldReturnGreaterThan() {
            WhereCondition whereCondition = new WhereCondition("ColumnName", "Value", Comparison.GreaterThan);

            string result = whereCondition.GetComparisonString();

            Assert.AreEqual(">", result);
        }

        [TestMethod]
        public void GetComparisonString_GreaterThenOrEqual_ShouldReturnGreaterThanOrEqual() {
            WhereCondition whereCondition = new WhereCondition("ColumnName", "Value", Comparison.GreaterThanOrEqual);

            string result = whereCondition.GetComparisonString();

            Assert.AreEqual(">=", result);
        }

        [TestMethod]
        public void GetComparisonString_LessThan_ShouldReturnLessThan() {
            WhereCondition whereCondition = new WhereCondition("ColumnName", "Value", Comparison.LessThan);

            string result = whereCondition.GetComparisonString();

            Assert.AreEqual("<", result);
        }

        [TestMethod]
        public void GetComparisonString_LessThanOrEqual_ShouldReturnLessThanOrEqual() {
            WhereCondition whereCondition = new WhereCondition("ColumnName", "Value", Comparison.LessThanOrEqual);

            string result = whereCondition.GetComparisonString();

            Assert.AreEqual("<=", result);
        }

        [TestMethod]
        public void GetComparisonString_Like_ShouldReturnLike() {
            WhereCondition whereCondition1 = new WhereCondition("ColumnName", "Value", Comparison.LikeBothSidesWildcard);
            WhereCondition whereCondition2 = new WhereCondition("ColumnName", "Value", Comparison.LikeLeftSideWildcard);
            WhereCondition whereCondition3 = new WhereCondition("ColumnName", "Value", Comparison.LikeRightSideWildcard);

            string result1 = whereCondition1.GetComparisonString();
            string result2 = whereCondition2.GetComparisonString();
            string result3 = whereCondition3.GetComparisonString();

            Assert.AreEqual("LIKE", result1);
            Assert.AreEqual("LIKE", result2);
            Assert.AreEqual("LIKE", result3);
        }

        [TestMethod]
        public void GetComparisonString_EqualsNull_ShouldReturnIs() {
            WhereCondition whereCondition = new WhereCondition("ColumnName", null);

            string result = whereCondition.GetComparisonString();

            Assert.AreEqual("IS", result);
        }

        [TestMethod]
        public void GetComparisonString_NotEqualsNull_ShouldReturnIsNot() {
            WhereCondition whereCondition = new WhereCondition("ColumnName", null, Comparison.NotEquals);

            string result = whereCondition.GetComparisonString();

            Assert.AreEqual("IS NOT", result);
        }
        #endregion

        [TestMethod]
        public void GetParameterValue_EqualsNull_ShouldReturnNull() {
            WhereCondition whereCondition = new WhereCondition("ColumnName", null);

            object result = whereCondition.GetParameterValue();

            Assert.AreEqual("NULL", result.ToString());
        }
    }
}
