using DAL.Context;
using DAL.Exceptions;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.DataAccess
{
    public class DataAccess_SQL : IDataAccess
    {
        private readonly ApplicationDbContext _context;

        public DataAccess_SQL(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Entry<T>(entity).State = EntityState.Added;
        }

        public void AddResult(PokemonRankResult model)
        {
            Add(model);
            foreach (var item in model.BattleRecords)
            {
                Add(item);
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Entry<T>(entity).State = EntityState.Deleted;
        }

        public void Edit<T>(T entity) where T : class
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
        }

        public async Task<PokemonRankResult> Find_ResultAsync(int id)
        {
            return await _context.RankResults.Include(r => r.BattleRecords)
                .FirstOrDefaultAsync(item => item.Id == id);
        }
        public async Task<List<PokemonRankResult>> GetAll_ResultsAsync()
        {
            return await _context.RankResults.Include(r => r.BattleRecords).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            try
            {
                return (await _context.SaveChangesAsync()) > 0;
            }
            catch (Exception e)
            {
                throw new DataAccessException("Error while excecuting DAL operation", e);
            }
        }
    }
}
