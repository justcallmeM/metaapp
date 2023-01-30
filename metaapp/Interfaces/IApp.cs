namespace metaapp.Interfaces
{
    public interface IApp
    {
        Task PeriodicWeatherDataRetrieval(string consoleArguments, TimeSpan interval);
    }
}
