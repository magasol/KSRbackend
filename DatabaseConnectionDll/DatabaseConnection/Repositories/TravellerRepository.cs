using System.Linq;
using DatabaseConnection.entities;

namespace DatabaseConnection.Repositories
{
    public class TravellerRepository : Repository<Traveller>, ITravellerRepository
    {
        public Traveller FindUserByEmail(string email)
        {
            using (var context = new GenericContext<Traveller>())
            {
                var item = context.Entity.Where<Traveller>(t => t.email == email).SingleOrDefault();
                if (item == null)
                {
                    return null;
                }
                return item;
            }
        }

        public Traveller FindUserByLogin(string login)
        {
            using (var context = new GenericContext<Traveller>())
            {
                var item = context.Entity.Where<Traveller>(t => t.login == login).SingleOrDefault();
                if (item == null)
                {
                    return null;
                }
                return item;
            }
        }
    }
}
