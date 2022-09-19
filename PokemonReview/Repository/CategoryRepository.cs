using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private  DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id); // return true or false
        }

        public bool CreatCategory(Category category)
        {
            /**
            // the Learner remove this part in the video
            // change Tracker : beacuse we start a new operation like the next step under(add, Updating, Modifying)
            // add, Updating, Modifying
            // connected vs disconnected
            // EntityState.Added
            **/
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList(); // return them in a list Without ordering them as they are only 3
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(e => e.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int Id)
        {
            return _context.PokemonCategories.Where(p=> p.CategoryId == Id).Select(c=>c.Pokemon).ToList();
            /**
            // be carefull with this : i tell the _context to search in PokemonCategories Table
            // By provuded categoryId , then select Collection of Pokemon regard to the categoryId and put in a List
            **/
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            /**
            // save changes mean entityFramework will convert this to sql commands and
            // take all the data you entered and send it to the database
            **/
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
