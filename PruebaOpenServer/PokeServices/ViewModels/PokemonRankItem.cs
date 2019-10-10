using System;
using System.Collections.Generic;
using System.Text;

namespace PokeServices.ViewModels
{
    public class PokemonRankItem
    {
        public int DexNumber { get; set; }
        public string PkmnName { get; set; }
        private int _remainingBattles = 2;

        public bool CanFight { get { return _remainingBattles > 0; } }

        public PokemonRankItem() { }

        public void Fight()
        {
            _remainingBattles--;
        }

        public PokemonRankItem Clone()
        {
            return new PokemonRankItem()
            {
                DexNumber = DexNumber,
                PkmnName = PkmnName,
                _remainingBattles = _remainingBattles
            };
        }
    }
}
