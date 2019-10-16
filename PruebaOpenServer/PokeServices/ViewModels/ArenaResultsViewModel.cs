using System;
using System.Collections.Generic;
using System.Text;

namespace PokeServices.ViewModels
{
    public class ArenaResultsViewModel
    {
        public int FightCount { get; set; }
        public double ArenaElapsedTime { get; set; }
        public List<PokemonShortInfoViewModel> InitialArenaPosition { get; set; }
        public List<PokemonShortInfoViewModel> FinalArenaPosition { get; set; }
        public List<PokemonArenaBattleViewModel> BattleRecords { get; set; }
    }

    public class PokemonArenaBattleViewModel
    {
        public int Order { get; set; }
        public PokemonShortInfoViewModel ChallengerPokemon { get; set; }
        public PokemonShortInfoViewModel ChallengedPokemon { get; set; }
    }
}
