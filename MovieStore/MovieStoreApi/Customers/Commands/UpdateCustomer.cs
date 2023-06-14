using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Exceptions;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Customers.Commands
{
    public static class UpdateCustomer
    {
        [PublicAPI]
        public class Command : IRequest<Customer>
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public Role Role { get; set; }
        }

        [UsedImplicitly]
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Customer>();
            }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, Customer>
        {
            private readonly IRepository<Customer> _repository;
            private readonly IMapper _mapper;

            public RequestHandler(IRepository<Customer> repository, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }
            public Task<Customer> Handle(Command request, CancellationToken cancellationToken)
            {
                //BadRequest
                if (request == null)
                {
                    throw new InvalidInputValueException();
                }

                var customer = _repository.GetById(request.Id);
                //CustomerDoesntExist
                if (customer == null)
                {
                    throw new CustomerDoesntExistException();
                }

                _mapper.Map(request, customer);
                _repository.SaveChanges();

                return Task.FromResult(customer);
            }
        }
    }
}