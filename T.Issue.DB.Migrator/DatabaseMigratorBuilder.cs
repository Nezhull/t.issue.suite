using T.Issue.DB.Migrator.Impl;

namespace T.Issue.DB.Migrator
{
    public static class DatabaseMigratorBuilder
    {
        public static IDatabaseMigrator Build(IMigratorConfiguration configuration) => new DatabaseMigratorImpl(configuration);
        public static IDatabaseMigrator Build(IMigratorConfiguration configuration, IDbAccessFacade dbAccessFacade) => new DatabaseMigratorImpl(configuration, dbAccessFacade);
    }
}
