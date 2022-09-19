using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;
using System.Collections.Generic;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")] // default, and is  written in all api controller
    [ApiController] // default, and is  written in all api controller
    public class PokemonController : Controller // remember inherit from main default Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonRepository pokemonRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            /**
            // create var pokemons to be returned later, Look that
            // I Mapp to PokemonDto to view only List of specific Colomuns
            **/
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId)) // check if the pokemon exists
                return NotFound();
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            /**
            // create var pokemon to be returned later, Look that
            // I Mapp to PokemonDto to view only specific Colomuns
            **/
            if (!ModelState.IsValid) // check if it's valid
                return BadRequest(ModelState);
            return Ok(pokemon); // return the specific pokemon
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();
            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rating);
        }

        // Post Data to Database
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatOwner([FromQuery] int ownerId,[FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreate)
        {// we add [FromQuery] as we need int pokeId and categoryId to Post owner with his CountryId
            //check if pokemonCreate valid value or null
            if (pokemonCreate == null)
                return BadRequest(ModelState);
            // check if pokemon Name Exists in table or not
            var pokemon = _pokemonRepository.GetPokemons().Where(c => c.Name.Trim().ToUpper()
            == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already Exists");
                return StatusCode(402, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //mapping the entered ownerCreate from type OwnerDto to type Country
            var _pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);


            // check the owner created or not using the method i already made in OwnerRepository
            if (!_pokemonRepository.CreatePokemon(_pokemonMap ,categoryId , ownerId))
            {
                ModelState.AddModelError("", "Something went Wrong while Saving");
                return StatusCode(500, ModelState);
            }

            // if created return : Successfully Created
            return Ok("Successfully Created");
        }

        // Put Data
        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner([FromQuery]int ownerId,[FromQuery]int categoryId,int pokeId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);
            if (pokeId != updatedPokemon.Id)
                return BadRequest(ModelState);
            //while testing using swagger :pokeId from body must equal pokeId from query
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);
            if (!_pokemonRepository.UpdatePokemon(pokemonMap, ownerId, categoryId))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Delete Data
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();
            var reviewesToDelete = _reviewRepository.GetReviewByPokeId(pokeId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokeId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // to delete reviews of a Pokemon
            if (!_reviewRepository.DeleteReviews(reviewesToDelete.ToList()))
            {
                ModelState.AddModelError("", "Some Thing went Wromg while deleting Review");
                return StatusCode(500, ModelState);
            }
            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Some Thing went Wromg while deleting Pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
