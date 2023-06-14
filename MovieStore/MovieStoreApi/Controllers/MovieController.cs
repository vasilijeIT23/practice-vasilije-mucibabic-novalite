using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStoreApi.Movies.Commands;
using MovieStoreApi.Movies.Quieries;
using MovieStoreCore;
using System.Net;

namespace MovieStoreApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/movies")]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("filtered")]
        public IActionResult Find()
        {
            return NoContent();
        }

        [HttpGet("all")]
        [ProducesResponseType(type: typeof(IEnumerable<Movie>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _mediator.Send(new GetAllMovies.Query());

            return Ok(movies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(type: typeof(Movie), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var movie = await _mediator.Send(new GetMovieById.Query { Id = id });

            return movie == null ? NotFound(id) : Ok(movie);
        }

        [HttpPost]
        [ProducesResponseType(type: typeof(Movie), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateMovie.Command request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(type: typeof(Movie), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateMovie.Command request)
        {
            var response = await _mediator.Send(request);

            return response == null ? NoContent() : NotFound();
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromBody] DeleteMovie.Command request)
        {
            var response = await _mediator.Send(request);
            return response ? NoContent() : NotFound();
        }
    }
}
