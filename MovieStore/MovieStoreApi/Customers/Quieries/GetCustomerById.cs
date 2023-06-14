using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Customers.Quieries
{
    public static class GetCustomerById
    {
        [PublicAPI]
        public class Query : IRequest<Customer?>
        {
            public Guid Id { get; set; }
        }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Query, Customer?>
        {
            private readonly IRepository<Customer> _repository;

            public RequestHandler(IRepository<Customer> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public Task<Customer?> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_repository.GetById(request.Id));
            }
        }
    }
}
