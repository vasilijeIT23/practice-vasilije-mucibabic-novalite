using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Movies.Quieries
{
    public static class GetMovieById
    {
        [PublicAPI]
        public class Query : IRequest<Movie?>
        {
            public Guid Id { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Query, Movie?>
        {
            private readonly IRepository<Movie> _repository;

            public RequestHandler(IRepository<Movie> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public Task<Movie?> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_repository.GetById(request.Id));
            }
        }
    }
}