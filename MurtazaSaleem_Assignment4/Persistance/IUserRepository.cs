
using MurtazaSaleem_Assignment4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurtazaSaleem_Assignment4.Persistance
{
    /// <summary>
    /// Defines the operations required to manage users for the application. Currently
    /// a very thin interface. Additional methods would be required to add, update and delete
    /// users in a full application implementation.An application of of the interface seggregation 
    /// principle (ISP). Derives from IRepository to inherit all general methods such as SaveChanges.
    /// </summary>
    public interface IUserRepository : IRepository
    {
        /// <summary>
        /// Returns an enumerator of users saved in the repository
        /// </summary>
        IEnumerable<User> Users { get; }

        /// <summary>
        /// Registers a user that can log into the application
        /// </summary>
        /// <param name="user"></param>
        void AddUser(User user);
    }
}
