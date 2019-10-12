using System;
using System.Collections.Generic;
using System.Text;

namespace PokeServices.ViewModels
{
    public class PokemonInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Types { get; set; }
        public IEnumerable<string> Abilities { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public PokemonSpriteCollectionViewModel Sprites { get; set; }
        public Dictionary<string, int> Stats { get; set; }
    }

    public class PokemonSpriteCollectionViewModel
    {
        public PokemonSpriteViewModel Default { get; set; }
        public PokemonSpriteViewModel Shiny { get; set; }

        public PokemonSpriteCollectionViewModel(string frontDefault, string backDefault,
            string frontShiny, string backShiny)
        {
            Default = new PokemonSpriteViewModel()
            {
                Front = frontDefault,
                Back = backDefault
            };

            Shiny = new PokemonSpriteViewModel()
            {
                Front = frontShiny,
                Back = backShiny
            };
        }
    }

    public class PokemonSpriteViewModel
    {
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
