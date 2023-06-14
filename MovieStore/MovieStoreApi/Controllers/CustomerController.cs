using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStoreApi.Customers.Commands;
using MovieStoreApi.Customers.Quieries;
using MovieStoreApi.Extensions;
using MovieStoreCore;
using MovieStoreInfrastructure;
using System.Net;

namespace MovieStoreApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("filtered")]
        public ActionResult<IEnumerable<Customer>> Find()
        {
            return NoContent();
        }

        [HttpGet("all")]
        [ProducesResponseType(type: typeof(IEnumerable<Customer>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _mediator.Send(new GetAllCustomers.Query());

            return Ok(customers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(type: typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerById.Query { Id = id });

            return customer == null ? NotFound() : Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(type: typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create()
        {

            var userEmail = UserExtenisons.GetUserEmail(User);
            var userName = UserExtenisons.GetUserName(User);

            var result = await _mediator.Send(new CreateCustomer.Command
            {
                Email = userEmail,
                Name = userName
            });
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(type: typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateCustomer.Command request)
        {
            var response = await _mediator.Send(request);
            //todo add notfound somehow

            return Ok(response);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromBody] DeleteCustomer.Command request)
        {
            var response = await _mediator.Send(request);
            return response ? NoContent() : NotFound();
        }

        [HttpPost("purchase/{movieId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PurchaseMovie([FromRoute] Guid movieId)
        {
            var userEmail = UserExtenisons.GetUserEmail(User);
            var response = await _mediator.Send(new PurchaseMovie.Command { MovieId = movieId, CustomerEmail = userEmail });
            return Ok(response);
        }

        [ProducesResponseType(type: typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPut("promote")]
        public async Task<IActionResult> Promote()
        {
            try
            {
                var userEmail = UserExtenisons.GetUserEmail(User);
                var response = await _mediator.Send(new PromoteCustomer.Command { Email = userEmail });

                return response ? Ok(response) : BadRequest();
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
