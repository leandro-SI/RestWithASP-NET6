using Microsoft.EntityFrameworkCore;
using RestWithASPNET.API.Data;
using RestWithASPNET.API.models.Base;
using RestWithASPNET.API.Repositories.Interfaces;

namespace RestWithASPNET.API.Repositories.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {

        private readonly ProjetoContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(ProjetoContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public List<T> FindAll()
        {
            return _dbSet.ToList();
        }

        public T FindById(int id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public T Create(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();

            return item;
        }

        public void Delete(int id)
        {
            var item = _dbSet.FirstOrDefault(x => x.Id == id);

            _dbSet.Remove(item);
            _context.SaveChanges();
        }

        public T Update(T item)
        {
            _dbSet.Update(item);
            _context.SaveChanges();

            return item;
        }
    }
}
