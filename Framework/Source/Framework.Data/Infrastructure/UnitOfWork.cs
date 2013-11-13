using Framework.Data.Context;

namespace Framework.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;

        private AppContext _appContext;
        protected AppContext AppContext
        {
            get { return _appContext ?? (_appContext = _databaseFactory.Get()); }
        }

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        public void Commit()
        {
            AppContext.Commit();
        }
    }
}
