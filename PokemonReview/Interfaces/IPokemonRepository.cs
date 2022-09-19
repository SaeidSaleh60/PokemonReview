using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        // ICollection<Pokemon> : return type (List of Pokemon, as we know every class is a type, here type of Pokemon)
        // Pokemon : Class Name
        // GetPokemons : method function

        Pokemon GetPokemon(int id); // to get specific pokemon by it's Id
        Pokemon GetPokemon(string name);// to get specific pokemon by it's name
        decimal GetPokemonRating(int pokeId); // to get specific pokemon Rating by it's Id
        bool PokemonExists(int pokeId);// to check if specific pokemon is Existed by it's Id

        //Add Data

        // because of many to many relationships between Pokemon table and both: Owner and Category Table
        // we will Add both CategoryId and OwnerId of the Joining Tables here when Posting a new Pokemon
        bool CreatePokemon(Pokemon pokemon , int categoryId , int ownerId);
        bool Save();

        //Update Data
        bool UpdatePokemon(Pokemon pokemon, int categoryId, int ownerId);

        //Delete Data
        bool DeletePokemon(Pokemon pokemon);
    }
}
