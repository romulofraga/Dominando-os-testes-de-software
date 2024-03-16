namespace NerdStore.Core;

public interface IUnitOfWork
{
  Task<bool> Commit();
}
