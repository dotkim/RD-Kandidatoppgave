using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace WebApplication1
{
    public class AppHostCommon
    {
        protected Container Container;
        public AppHostCommon(ServiceStackHost self)
        {
            Container = self.Container;
        }

        public void Init()
        {
            SetupDatabase();
        }

        private void SetupDatabase()
        {
            var connectionFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            Container.Register<IDbConnectionFactory>(c => connectionFactory);

            var db = new Database();
            db.CreateTablesAndTestData(connectionFactory);
            Container.RegisterAutoWiredAs<Database, IDatabase>();
        }

    }
}
