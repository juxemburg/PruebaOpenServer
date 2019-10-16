using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class PokemonBattleRecord
    {
        public int Id { get; set; }

        public int PokemonRankResultId { get; set; }
        public PokemonRankResult PokemonRankResult { get; set; }

        public int Order { get; set; }

        public int ChallengerPokemonId { get; set; }
        public int ChallengedPokemonId { get; set; }
    }
}
