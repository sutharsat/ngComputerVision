namespace ngComputerVision.Contracts
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ClaimsCollectionName { get; set; } = null!;
        public string EntityResultCollectionName { get; set; } = null!;
        public string CredentialCollectionName { get; set; } = null!;
    }
}