using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Movies.Quieries
{
    public static class GetAllMovies
    {
        [PublicAPI]
        public class Query : IRequest<IEnumerable<Movie>> { }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Query, IEnumerable<Movie>>
        {
            private readonly IRepository<Movie> _repository;

            public RequestHandler(IRepository<Movie> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public Task<IEnumerable<Movie>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_repository.GetAll());
            }
        }
    }
}
