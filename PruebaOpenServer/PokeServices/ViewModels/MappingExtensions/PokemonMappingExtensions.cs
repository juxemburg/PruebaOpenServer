using Entities;
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

        public static PokemonRankResult ToEntity(this ArenaResultsViewModel viewModel)
            => new PokemonRankResult()
            {
                Date = DateTime.Now,
                ElapsedMiliseconds = viewModel.ArenaElapsedTime,
                StepCount = viewModel.FightCount,
                InitialStateId = viewModel.InitialArenaPosition.Aggregate("", (acc, current) => $"{acc}-{current.DexNum}"),
                FinalStateId = viewModel.FinalArenaPosition.Aggregate("", (acc, current) => $"{acc}-{current.DexNum}"),
                BattleRecords = viewModel.BattleRecords.Select(rec => rec.ToEntity()).ToList()
            };

        public static PokemonBattleRecord ToEntity(this PokemonArenaBattleViewModel viewModel)
            => new PokemonBattleRecord()
            {
                ChallengedPokemonId = viewModel.ChallengedPokemon.DexNum,
                ChallengerPokemonId = viewModel.ChallengerPokemon.DexNum,
                Order = viewModel.Order
            };
        public static ArenaResultsShortInfoViewModel ToViewModel(this PokemonRankResult model)
            => new ArenaResultsShortInfoViewModel()
            {
                Id = model.Id,
                Date = model.Date,
                ElapsedMiliseconds = model.ElapsedMiliseconds,
                StepCount = model.StepCount
            };
    }
}
