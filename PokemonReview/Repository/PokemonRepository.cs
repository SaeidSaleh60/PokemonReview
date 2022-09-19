using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class PokemonRepository : IPokemonRepository 
    {
        /** 
        // remember inherit from Interface
        // (remeber how to implement from Interface)
        **/
        private readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context=context;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList();
            /**
            //_context to access all DataContext contents to manipulate database or get all data from database
            // Pokemon in the return statement is the table Pokemon
            // I ask the GetPokemons method with return type ICollection<Pokemon> to return:
            // all the data in table Pokemon ordered by id , in a form of list
            // if we don't write ToList() it will cause error **/
        }

        public Pokemon GetPokemon(int id)
        { // search by Id
            return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
            /**
            // it means use _context to access database and go to Pokemon table and where Id==id return the
            // first row with this id
            // we use where for filterring 
            // we use FirstOrDefault() to return only one Pokemon row
            **/
        }

        public Pokemon GetPokemon(string name)
        {// search by Name
            return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);
            if (review.Count() <= 0) // if ther aren't any Reviews data at this ID return 0
            {
                return 0;
            }
            return ((decimal)review.Sum(r => r.Rating) / review.Count()); // to get the average rating   
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon.Any(p => p.Id == pokeId); // check if a specific Pokemon Exists by it's Id
        }

       

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreatePokemon(Pokemon pokemon, int categoryId, int ownerId)
        {
            // here we will get owner and category of the pokemon to add them
            var ownerOfPokemon = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var categoryOfPokemon = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = ownerOfPokemon,
                Pokemon = pokemon
            };
            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = categoryOfPokemon,
                Pokemon = pokemon
            };
            _context.Add(pokemonCategory);
            
            // then we add the pokemon here
            _context.Add(pokemon);
            return Save();


        }

        public bool UpdatePokemon(Pokemon pokemon, int categoryId, int ownerId)
        {
            _context.Update(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }
    }
}
