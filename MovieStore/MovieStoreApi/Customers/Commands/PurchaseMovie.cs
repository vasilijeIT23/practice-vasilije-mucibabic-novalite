using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Customers.Commands
{
    public static class PurchaseMovie
    {
        [PublicAPI]
        public class Command : IRequest<bool>
        {
            public string? CustomerEmail { get; set; }
            public Guid MovieId { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<Customer> _customerRepository;
            private readonly IRepository<Movie> _movieRepository;
            public RequestHandler(IRepository<Customer> customerRepository, IRepository<Movie> movieRepository)
            {
                _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
                _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            }

            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }


                var customer = _customerRepository.Find(x => x.Email == request.CustomerEmail).SingleOrDefault();

                var movie = _movieRepository.GetById(request.MovieId);

                if (customer == null || movie == null)
                {
                    return Task.FromResult(false);
                }

                var result = customer.PurchaseMovie(movie);

                if (result.IsFailed)
                {
                    return Task.FromResult(false);
                }

                _customerRepository.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}
