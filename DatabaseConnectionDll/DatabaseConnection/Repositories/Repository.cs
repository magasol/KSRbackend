﻿using DatabaseConnection.entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseConnection.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        public IList<T> GetAll()
        {
            using (var context = new GenericContext<T>())
            {
                var all = context.Entity.OrderBy(x => x.id).ToList();
                return all;
            }
        }

        public T Get(int id)
        {
            using (var context = new GenericContext<T>())
            {
                var item = context.Entity.Find(id);
                if (item == null)
                {
                    return null;
                }
                return item;
            }
        }

        public void Add(T entity)
        {
            using (var context = new GenericContext<T>())
            {
                context.Entity.Add(entity);
                context.SaveChanges();
            }
        }

        public void AddRange(List<T> entities)
        {
            using (var context = new GenericContext<T>())
            {
                context.Entity.AddRange(entities);
                context.SaveChanges();
            }
        }

        public void Delete(T entity)
        {
            using (var context = new GenericContext<T>())
            {
                context.Remove(entity);
                context.SaveChanges();
            }
        }

        public void DeleteRange(List<T> entities)
        {
            using (var context = new GenericContext<T>())
            {
                context.RemoveRange(entities);
                context.SaveChanges();
            }
        }

        public int NextId()
        {
            using (var context = new GenericContext<T>())
            {
                return context.Entity.Max(x => x.id) + 1;
            }
        }

        public bool Update(T changedEntity)
        {
            using (var context = new GenericContext<T>())
            {
                var entity = context.Entity.Find(changedEntity.id);
                if (entity == null)
                {
                    return false;
                }
                context.Entry(entity).CurrentValues.SetValues(changedEntity);
                context.SaveChanges();
                return true;
            }
        }
    }
}

