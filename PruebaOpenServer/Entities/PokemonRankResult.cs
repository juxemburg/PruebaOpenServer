using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class PokemonRankResult
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double ElapsedMiliseconds { get; set; }
        public int StepCount { get; set; }
        public string InitialStateId { get; set; }
        public string FinalStateId { get; set; }

        public virtual List<PokemonBattleRecord> BattleRecords { get; set; }
    }
}
