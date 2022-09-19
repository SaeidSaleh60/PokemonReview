using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        bool ReviewerExists(int reviewerId);
        ICollection<Review> GetReviewsByViewer(int reviewerId);

        //Add Data
        bool CreatReviewer(Reviewer reviewer);
        bool Save();

        //Update Data
        bool UpdateReviewer(Reviewer reviewer);

        //Delete Data
        bool DeleteReviewer(Reviewer reviewer);
    }
}
