using JetBrains.Annotations;
using MediatR;
using MovieStoreApi.Repositories;
using MovieStoreCore;

namespace MovieStoreApi.Customers.Quieries
{
    public static class GetAllCustomers
    {
        [PublicAPI]
        public class Query : IRequest<IEnumerable<Customer>> { }

        [UsedImplicitly]
        public class RequestHandler : IRequestHandler<Query, IEnumerable<Customer>>
        {
            private readonly IRepository<Customer> _repository;

            public RequestHandler(IRepository<Customer> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public Task<IEnumerable<Customer>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_repository.GetAll());
            }
        }
    }
}
