namespace IdentitySystem.Database
{
    public static class SqlServerTypes
    {
        public static string String { get; private set; } = "NVARCHAR";

        public static string Boolean { get; private set; } = "BIT";

        public static string Guid { get; private set; } = "UNIQUEIDENTIFIER";
    }
}