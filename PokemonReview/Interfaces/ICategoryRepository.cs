using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface ICategoryRepository
    {
        // Get Data From Database
        ICollection<Category> GetCategories();// to get all Categories
        Category GetCategory(int id);// to get specific category
        ICollection<Pokemon> GetPokemonByCategory(int Id);// to get a Specific Pokemon by CategoryId
        bool CategoryExists(int id);// Check if a specific Category Exists

        // Adding Data to Database
        bool CreatCategory(Category category);
        bool Save();
       
        // Update Data
        bool UpdateCategory(Category category);

        //Delete Data
        bool DeleteCategory(Category category);
    }
}
