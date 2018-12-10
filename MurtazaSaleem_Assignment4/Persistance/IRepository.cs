namespace MurtazaSaleem_Assignment4.Persistance
{
    /// <summary>
    /// Defines the general operations that need to be supported by a repository. An application
    /// of the interface seggregation principle (ISP)
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Saves the changes performed on the repository. Must be called when repository items
        /// have been added, updated or deleted
        /// </summary>
        void SaveChanges();
    }
}