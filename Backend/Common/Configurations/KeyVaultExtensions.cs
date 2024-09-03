using Azure.Identity;
using Common.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Common.Configurations;

public static class KeyVaultExtensions
{
    public static IHostBuilder ConfigureKeyVault(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var configuration = configurationBuilder.Build();

            var keyVaultConnection = configuration[KeyVaultSecretNames.KEY_VAULT_CONNECTION]
                                     ?? throw new Exception(ErrorStrings.MISSING_KEY_VAULT_CONNECTION);
            
            configurationBuilder.AddAzureKeyVault(new Uri(keyVaultConnection), new DefaultAzureCredential());
        });

        return hostBuilder;
    }
}