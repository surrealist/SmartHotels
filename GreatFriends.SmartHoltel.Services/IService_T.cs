﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GreatFriends.SmartHoltel.Services
{
  public interface IService<T> where T : class
  {
    IQueryable<T> Query(Expression<Func<T, bool>> predicate);
    IQueryable<T> All();
    T Find(params object[] keys);

    T Add(T item);
    T Update(T item);
    T Remove(T item);
  }
}