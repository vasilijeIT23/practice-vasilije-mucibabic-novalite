using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Exceptions;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Customers.Commands
{
    public static class PromoteCustomer
    {
        [PublicAPI]
        public class Command : IRequest<bool>
        {
            public string Email { get; set; } = string.Empty;
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<Customer> _repository;

            public RequestHandler(IRepository<Customer> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                //InvalidInputValue
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var customer = _repository.Find(x => x.Email == request.Email).SingleOrDefault();

                if (customer == null)
                {
                    throw new CustomerDoesntExistException();
                }

                var result = customer.PromoteCustomer();

                if (result.IsFailed)
                {
                    return Task.FromResult(false);
                }

                _repository.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}
