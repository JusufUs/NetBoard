﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ClashN.Tool
{
    public static class QueryableExtension
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
        {
            return OrderBy<T>(query, propertyName, false);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName)
        {
            return OrderBy<T>(query, propertyName, true);
        }

        private static IOrderedQueryable<T> OrderBy<T>(IQueryable<T> query, string propertyName, bool isDesc)
        {
            string methodname = (isDesc) ? "OrderByDescendingInternal" : "OrderByInternal";

            var memberProp = typeof(T).GetProperty(propertyName);

            var method = typeof(QueryableExtension).GetMethod(methodname)
                                       .MakeGenericMethod(typeof(T), memberProp.PropertyType);

            return (IOrderedQueryable<T>)method.Invoke(null, new object[] { query, memberProp });
        }

        public static IOrderedQueryable<T> OrderByInternal<T, TProp>(IQueryable<T> query, PropertyInfo memberProperty)
        {
            return query.OrderBy(GetLambda<T, TProp>(memberProperty));
        }

        public static IOrderedQueryable<T> OrderByDescendingInternal<T, TProp>(IQueryable<T> query, PropertyInfo memberProperty)
        {
            return query.OrderByDescending(GetLambda<T, TProp>(memberProperty));
        }

        private static Expression<Func<T, TProp>> GetLambda<T, TProp>(PropertyInfo memberProperty)
        {
            if (memberProperty.PropertyType != typeof(TProp))
                throw new Exception();

            var thisArg = Expression.Parameter(typeof(T));
            var lamba = Expression.Lambda<Func<T, TProp>>(Expression.Property(thisArg, memberProperty), thisArg);

            return lamba;
        }
    }
}