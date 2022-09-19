using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IOwnerRepository
    {
        //Get Data
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonOfAnOwner(int ownerId);
        bool OwnerExists(int ownerId);

        //Add Data
        bool CreateOwner(Owner owner);
        bool Save();

        //Update Data
        bool UpdateOwner(Owner owner);

        //Delete Data
        bool DeleteOwner(Owner owner);
    }
}
