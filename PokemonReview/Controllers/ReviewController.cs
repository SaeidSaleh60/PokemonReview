using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController:Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository , IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200 , Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }


        [HttpGet("{reviewId}")]
        [ProducesResponseType(200 , Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }


        [HttpGet("Review/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]

        public IActionResult GetReviewsOfAnPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewByPokeId(pokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        // Post Data to Database
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId,[FromQuery] int pokemonId, [FromBody] ReviewDto reviewCreate)
        {// we add [FromQuery] as we need int reviewId and pokemonId to Post owner with reviewerCreate
            //to fill data in pokemonId column and reviewId column
            //check if reviewerCreate valid value or null
            if (reviewCreate == null)
                return BadRequest(ModelState);
            // check if review Title Exists in table or not
            var review = _reviewRepository.GetReviews().Where(c => c.Title.Trim().ToUpper()
            == reviewCreate.Title.TrimEnd().ToUpper()).FirstOrDefault();
            if (review != null)
            {
                ModelState.AddModelError("", "Review already Exists");
                return StatusCode(402, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //mapping the entered reviewrCreate from type ReviewrDto to type Review
            var _reviewMap = _mapper.Map<Review>(reviewCreate);
            // important note: as there are an PokemonId and reviewId columns in Table Review we should add this bellow
            // we get the pokemon and reviewer of review
            _reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            _reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);


            // check the review created or not using the method i already made in ReviewRepository
            if (!_reviewRepository.CreateReview(_reviewMap))
            {
                ModelState.AddModelError("", "Something went Wrong while Saving");
                return StatusCode(500, ModelState);
            }

            // if created return : Successfully Created
            return Ok("Successfully Created");
        }

        // Put Data
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);
            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);
            //while testing using swagger :reviewId from body must equal reviewId from query
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(updatedReview);
            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Delete Data
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            var reviewToDelete = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Some Thing went Wromg while deleting Review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
