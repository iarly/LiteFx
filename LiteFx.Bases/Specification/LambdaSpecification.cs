﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace LiteFx.Bases.Specification
{
    /// <summary>
    /// Specification pattern implementation using lambda expressions.
    /// </summary>
    /// <typeparam name="T">Type that will be evaluated.</typeparam>
    public class LambdaSpecification<T> : ILambdaSpecification<T>
    {
        /// <summary>
        /// The predicated expression.
        /// </summary>
        public Expression<Func<T, bool>> Predicate { get; private set; }

        /// <summary>
        /// Cached compiled predicate.
        /// </summary>
        private Func<T, bool> predicateCompiledCache;

        /// <summary>
        /// Cached compiled predicate.
        /// </summary>
        protected Func<T, bool> PredicateCompiledCache
        {
            get { return predicateCompiledCache ?? (predicateCompiledCache = Predicate.Compile()); }
        }

        /// <summary>
        /// Lambda specification constructor.
        /// </summary>
        /// <param name="predicate">The predicated expression that will be used in IsSatisfiedBy method.</param>
        public LambdaSpecification(Expression<Func<T, bool>> predicate)
        {
            Predicate = predicate;
        }

        /// <summary>
        /// Combine two specifications using the AndAlso (&&) operator.
        /// </summary>
        /// <param name="leftSide">Specification that will be in the left side of the operation.</param>
        /// <param name="rightSide">Specification that will be in the left side of the operation.</param>
        /// <returns>The new combined specification.</returns>
        public static LambdaSpecification<T> operator &(LambdaSpecification<T> leftSide, LambdaSpecification<T> rightSide)
        {
            var rightInvoke = Expression.Invoke(rightSide.Predicate, leftSide.Predicate.Parameters.Cast<Expression>());
            var newExpression = Expression.MakeBinary(ExpressionType.AndAlso, leftSide.Predicate.Body, rightInvoke);

            return new LambdaSpecification<T>(Expression.Lambda<Func<T, bool>>(newExpression, leftSide.Predicate.Parameters));
        }

        /// <summary>
        /// Combine two specifications using the OrElse (||) operator.
        /// </summary>
        /// <param name="leftSide">Specification that will be in the left side of the operation.</param>
        /// <param name="rightSide">Specification that will be in the left side of the operation.</param>
        /// <returns>The new combined specification.</returns>
        public static LambdaSpecification<T> operator |(LambdaSpecification<T> leftSide, LambdaSpecification<T> rightSide)
        {
            var rightInvoke = Expression.Invoke(rightSide.Predicate, leftSide.Predicate.Parameters.Cast<Expression>());
            var newExpression = Expression.MakeBinary(ExpressionType.OrElse, leftSide.Predicate.Body, rightInvoke);

            return new LambdaSpecification<T>(Expression.Lambda<Func<T, bool>>(newExpression, leftSide.Predicate.Parameters));
        }

        #region ISpecification<T> Members

        /// <summary>
        /// Verifies the entity over the predicate.
        /// </summary>
        /// <param name="entity">Entity to be verified.</param>
        /// <returns>True if the specification is satisfied and false if it is not.</returns>
        public bool IsSatisfiedBy(T entity)
        {
            return PredicateCompiledCache(entity);
        }

        #endregion
    }
}