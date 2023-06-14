using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Customers.Commands
{
    public static class DeleteCustomer
    {
        [PublicAPI]
        public class Command : IRequest<bool>
        {
            public Guid Id { get; set; }
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
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var customer = _repository.GetById(request.Id) ?? throw new ArgumentNullException(nameof(request));

                if (customer == null)
                {
                    return Task.FromResult(false);
                }

                _repository.Delete(customer);
                _repository.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}