using PokeApiNet;
using PokeApiNet.Models;
using PokeServices.Exceptions;
using PokeServices.ViewModels;
using PokeServices.ViewModels.MappingExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeServices.PokemonServices
{
    public class PokemonService
    {
        private readonly PokeApiClient _pokeClient = new PokeApiClient();
        public PokemonService() {}


        public async Task<PokemonInfoViewModel> GetPokemonInfoAsync(int dexNumber)
        {
            var pokemon = await _pokeClient.GetResourceAsync<Pokemon>(dexNumber);

            if(pokemon == null)
            {
                throw new OperationFailedException("El Pokémon no existe. ☹", OperationErrorStatus.ResourceNotFound);
            }

            return pokemon.ToViewModel();
        }

    }
}
