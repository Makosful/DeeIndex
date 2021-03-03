namespace DeepIndex.Hoster.Indexer.Logic.Abstractions
{
    public interface ICrawler
    {
        bool CrawlFile(string path);
    }
}