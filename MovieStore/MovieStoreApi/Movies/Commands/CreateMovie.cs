using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Exceptions;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Movies.Commands
{
    public class CreateMovie
    {
        [PublicAPI]
        public class Command : IRequest<Movie>
        {
            public string Title { get; set; } = string.Empty;
            public LicencingType LicencingType { get; set; }

            public string Description { get; set; } = string.Empty;
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, Movie>
        {
            private readonly IRepository<Movie> _repository;

            public RequestHandler(IRepository<Movie> repository)
            {
                _repository = repository;
            }
            public Task<Movie> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new InvalidInputValueException();
                }

                Movie movie = request.LicencingType == LicencingType.TwoDay
                    ? new TwoDayMovie
                    {
                        Title = request.Title,
                        LicencingType = request.LicencingType,
                        Description = request.Description
                    }
                    : new LifelongMovie
                    {
                        Title = request.Title,
                        LicencingType = request.LicencingType,
                        Description = request.Description
                    };

                _repository.Create(movie);
                _repository.SaveChanges();

                return Task.FromResult(movie);
            }
        }
    }
}