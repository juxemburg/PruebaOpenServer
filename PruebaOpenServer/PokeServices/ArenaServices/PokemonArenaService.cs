using DAL.DataAccess;
using PokeServices.Exceptions;
using PokeServices.PokedexServices;
using PokeServices.PokemonRankSearchServices;
using PokeServices.ViewModels;
using PokeServices.ViewModels.MappingExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeServices.ArenaServices
{
    public class PokemonArenaService
    {
        private readonly PokemonRankSearchService _rankSearchService;
        private readonly PokedexProfilerService _profilerService;
        private readonly IDataAccess _data;

        public PokemonArenaService(PokemonRankSearchService rankSearchService,
            PokedexProfilerService profilerService,
            IDataAccess data)
        {
            _rankSearchService = rankSearchService;
            _profilerService = profilerService;
            _data = data;
        }

        public async Task<int> CreateArenaAsync(List<string> pkmnNames)
        {
            var arenaResults = await _rankSearchService.FullArenaSearchAsync(pkmnNames);
            var resource = arenaResults.ToEntity();
            _data.AddResult(resource);
            var success = await _data.SaveAllAsync();
            if (!success)
            {
                throw new OperationFailedException("Error al insertar el recurso", OperationErrorStatus.DatabaseError);
            }
            return resource.Id;
        }


        public async Task<ArenaResultsViewModel> GetArenaResultsAsync(int id)
        {
            var model = await _data.Find_ResultAsync(id);
            if (model == null)
            {
                throw new OperationFailedException("El resultado no existe", OperationErrorStatus.ResourceNotFound);
            }

            return _profilerService.GetArenaResultsViewModel(model.ElapsedMiliseconds,
                model.BattleRecords
                .OrderByDescending(record => record.Order)
                .Select(record => $"{record.ChallengerPokemonId}-{record.ChallengedPokemonId}")
                .ToList(),
                model.InitialStateId, model.FinalStateId);
        }

        public async Task<List<ArenaResultsShortInfoViewModel>> GetAllArenaResultsAsync()
        {
            var items = await _data.GetAll_ResultsAsync();
            if (items == null)
            {
                throw new OperationFailedException("El resultado no existe", OperationErrorStatus.ResourceNotFound);
            }

            return items.Select(model => model.ToViewModel()).ToList();
        }

    }
}
