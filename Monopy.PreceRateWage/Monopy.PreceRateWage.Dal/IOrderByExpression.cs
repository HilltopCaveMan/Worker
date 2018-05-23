﻿using System.Linq;

namespace Monopy.PreceRateWage.Dal
{
    public interface IOrderByExpression<TEntity> where TEntity : class
    {
        IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query);

        IOrderedQueryable<TEntity> ApplyThenBy(IOrderedQueryable<TEntity> query);
    }
}