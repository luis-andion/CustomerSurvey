using CustomerSurvey.Repository.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CustomerSurvey.Service.Services
{
    public abstract class GenericService<T> : IGenericService<T> where T : class
    {
        protected readonly IGenericRepository<T> dataRepository;
        public GenericService(IGenericRepository<T> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("The Repository cannot be null");
            }

            dataRepository = repository;
        }

        public void Add(T entity)
        {
            dataRepository.Add(entity);
        }

        public void Delete(T entity)
        {
            dataRepository.Delete(entity);
        }

        public void Edit(T entity)
        {
            dataRepository.Edit(entity);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dataRepository.FindBy(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return dataRepository.GetAll();
        }

        public void Save()
        {
            dataRepository.Save();
        }
    }
}
