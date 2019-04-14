using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SampleWebApp.BLL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        Data.CandidateContext _context;
        DbSet<T> _table = null;

        public Repository()
        {
            this._context = new Data.CandidateContext();
            _table = _context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _table.ToList();

        public T GetById(object id) => _table.Find(id);

        public void Insert(T obj)
        {
            _table.Add(obj);
        }


        public void Update(T obj)
        {
            _table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            T existing = _table.Find(id);
            _table.Remove(existing);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}