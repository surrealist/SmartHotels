﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GreatFriends.SmartHoltel.Services
{
  public abstract class ServiceBase<T> : IService<T> where T : class
  {
    protected readonly App app;
    public ServiceBase(App app) => this.app = app;

    public virtual IQueryable<T> Query(Expression<Func<T, bool>> predicate) => app.db.Set<T>().Where(predicate);
    public IQueryable<T> All() => Query(_ => true);
    public virtual T Find(params object[] keys) => app.db.Set<T>().Find(keys);

    public virtual T Add(T item) => app.db.Set<T>().Add(item).Entity;
    public virtual T Update(T item) => app.db.Set<T>().Update(item).Entity;
    public virtual T Remove(T item) => app.db.Set<T>().Remove(item).Entity;
  }
}
