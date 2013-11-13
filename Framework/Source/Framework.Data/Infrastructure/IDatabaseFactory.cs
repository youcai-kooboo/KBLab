using Framework.Data.Context;

namespace Framework.Data.Infrastructure
{
    public interface IDatabaseFactory
    {
        AppContext Get();
    }
}
