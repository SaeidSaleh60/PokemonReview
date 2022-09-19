using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface ICountryRepository
    {
        // Get Data From Database
        ICollection<Country> GetCountries();
        Country GetCountry(int countryId); 
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersFromACountry(int countryId);
        bool CountryExists(int countryId);

        // Adding Data to Database
        bool CreatCountry(Country country);
        bool Save();

        // Update Data
        bool UpdateCountry(Country country);

        //Delete Data
        bool DeleteCountry(Country country);
    }
}
