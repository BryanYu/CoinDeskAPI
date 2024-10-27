namespace CoinDesk.API.Extension;

public static class WebHostEnvironmentExtension
{
    public static bool IsDocker(this IWebHostEnvironment env)
    {
        return env.IsEnvironment("Docker");
    }
}