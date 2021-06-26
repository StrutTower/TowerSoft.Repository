using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TowerSoft.Repository.Maps;

namespace TowerSoft.Repository.Builders {
    public class FluentQueryBuilder<T> {
        private readonly MappingModel<T> mappingModel;

        internal List<WhereCondition> WhereConditions { get; set; } = new List<WhereCondition>();

        internal List<OrderStatement> OrderStatements { get; set; }

        internal int? Limit { get; set; }

        internal int? Offset { get; set; }

        internal FluentQueryBuilder(MappingModel<T> mappingModel) {
            this.mappingModel = mappingModel;
        }

        public FluentQueryBuilder<T> WhereEqual<TProperty>(Expression<Func<T, TProperty>> propertyExpression, object value) {
            AddWhereCondition(propertyExpression, Comparison.Equals, value);
            return this;
        }

        public FluentQueryBuilder<T> Where<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Comparison comparison, object value) {
            AddWhereCondition(propertyExpression, comparison, value);
            return this;
        }

        public FluentQueryBuilder<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> propertyExpression) {
            if (OrderStatements == null) OrderStatements = new List<OrderStatement>();
            OrderStatements.Add(new OrderStatement(GetColumnName(propertyExpression), isAscending: true));
            return this;
        }

        public FluentQueryBuilder<T> OrderByDescending<TProperty>(Expression<Func<T, TProperty>> propertyExpression) {
            if (OrderStatements == null) OrderStatements = new List<OrderStatement>();
            OrderStatements.Add(new OrderStatement(GetColumnName(propertyExpression), isAscending: false));
            return this;
        }

        /// <summary>
        /// LIMIT and OFFSET are not supported in Caché
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public FluentQueryBuilder<T> LimitTo(int limit) {
            Limit = limit;
            return this;
        }

        /// <summary>
        /// LIMIT and OFFSET are not supported in Caché
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public FluentQueryBuilder<T> OffsetBy(int offset) {
            Offset = offset;
            return this;
        }

        internal void AddWhereCondition<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Comparison comparison, object value) {
            WhereConditions.Add(new WhereCondition(GetColumnName(propertyExpression), value, comparison));
        }

        internal string GetColumnName<TProperty>(Expression<Func<T, TProperty>> propertyExpression) {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            if (!mappingModel.AllMapsDictionary.ContainsKey(memberExpression.Member.Name)) {
                throw new Exception("Unable to find a map for the property '" + memberExpression.Member.Name + "'.");
            }
            return mappingModel.AllMapsDictionary[memberExpression.Member.Name];
        }
    }
}
