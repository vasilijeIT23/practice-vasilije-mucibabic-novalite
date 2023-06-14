using MovieStoreCore;
using MovieStoreInfrastructure;

namespace MovieStoreApi.Repositories
{
    public class PurchasedMovieRepository : Repository<PurchasedMovie>
    {
        public PurchasedMovieRepository(MovieStoreContext context) : base(context) { }
    }


}
