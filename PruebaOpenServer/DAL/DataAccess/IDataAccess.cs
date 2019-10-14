using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataAccess
{
    public interface IDataAccess
    {
        // Operaciones Básicas
        void Add<T>(T entity) where T : class;
        void Edit<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAllAsync();

        //Estudiantes
        void AddResult(PokemonRankResult model);
        Task<PokemonRankResult> Find_ResultAsync(int id);

        Task<List<PokemonRankResult>> GetAll_ResultsAsync();
    }
}
