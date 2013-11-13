using Framework.Data.Context;

namespace Framework.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private AppContext _appContext;
        public AppContext Get()
        {
            return _appContext ?? (_appContext = new AppContext());
        }

        protected override void DisposeCore()
        {
            if (_appContext != null)
            {
                _appContext.Dispose();
            }
        }
    }
}
