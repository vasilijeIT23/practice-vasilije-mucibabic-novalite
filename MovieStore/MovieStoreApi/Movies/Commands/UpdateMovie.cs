using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Exceptions;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Movies.Commands
{
    public static class UpdateMovie
    {
        [PublicAPI]
        public class Command : IRequest<Movie>
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public LicencingType LicencingType { get; set; }
            public string Description { get; set; } = string.Empty;
        }

        [UsedImplicitly]
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Movie>();
            }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, Movie>
        {
            private readonly IRepository<Movie> _repository;
            private readonly IMapper _mapper;

            public RequestHandler(IRepository<Movie> repository, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }
            public Task<Movie> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new InvalidInputValueException();
                }

                var movie = _repository.GetById(request.Id);

                if (movie == null)
                {
                    throw new MovieDoesntExistException();
                }

                _mapper.Map(request, movie);
                _repository.SaveChanges();

                return Task.FromResult(movie);
            }
        }
    }
}
