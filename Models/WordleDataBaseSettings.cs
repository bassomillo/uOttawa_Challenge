namespace WordleBackEndApi.Models;

public class WordleDataBaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string WordCollectionName { get; set; } = null!;

    public string HistoryCollectionName { get; set; } = null!;

}