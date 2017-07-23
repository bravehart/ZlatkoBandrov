namespace ZlatkoBandrov.DataAccess.Repositories
{
    public interface IUnitOfWork
    {
        void SaveChanges();

        void Rollback();
    }
}
