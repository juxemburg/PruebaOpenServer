using PokeApiNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokeServices.ViewModels.MappingExtensions
{
    public static class PokemonMappingExtensions
    {
        public static PokemonInfoViewModel ToViewModel(this Pokemon pokemon)
            => new PokemonInfoViewModel()
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Types = pokemon.Types.Select(type => type.Type.Name),
                Abilities = pokemon.Abilities.Select(ability => ability.Ability.Name),
                Height = (double)pokemon.Height / 10,
                Weight = (double)pokemon.Weight / 10,
                Stats = pokemon.Stats.ToDictionary(stat => stat.Stat.Name, stat => stat.BaseStat),
                Sprites = new PokemonSpriteCollectionViewModel(pokemon.Sprites.FrontDefault, 
                    pokemon.Sprites.BackDefault, pokemon.Sprites.FrontShiny, pokemon.Sprites.BackShiny)
            };
    }
}
