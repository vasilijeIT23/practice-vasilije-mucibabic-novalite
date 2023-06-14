using MovieStoreCore;
using MovieStoreInfrastructure;

namespace MovieStoreApi.Repositories
{
    public class MovieRepository : Repository<Movie>
    {
        public MovieRepository(MovieStoreContext context) : base(context) { }
    }
}
