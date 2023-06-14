using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Movies.Commands
{
    public static class DeleteMovie
    {
        [PublicAPI]
        public class Command : IRequest<bool>
        {
            public Guid Id { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<Movie> _repository;

            public RequestHandler(IRepository<Movie> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var movie = _repository.GetById(request.Id) ?? throw new ArgumentNullException(nameof(request));

                if (movie == null)
                {
                    return Task.FromResult(false);
                }

                _repository.Delete(movie);
                _repository.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}
