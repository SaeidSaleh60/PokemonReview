using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IReviewRepository
    {
        // Get Data
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewByPokeId(int pokeId);
        bool ReviewExists(int reviewId);

        //Add Data
        bool CreateReview(Review review);
        bool Save();

        //Update Data
        bool UpdateReview(Review review);

        //Delete Data
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);

    }
}
