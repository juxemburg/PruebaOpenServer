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
    }
}
