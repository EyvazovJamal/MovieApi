using Marten;

namespace Application.Common;


public interface IReadOnlyDocumentStore
{
    IQuerySession QuerySession();
}
