# ----------------------------------------------------------------------------------------------------------
# README
# ----------------------------------------------------------------------------------------------------------
# Open a CLI
# Navigate to the SecurityTokenService directory
# Change "password" to your desired value
# Run the script
#
# HTTPS should now work for both running the service locally and running the service through docker-compose
# NOTE: This applies to Linux Containers on Docker for Windows.
#
# References:
# https://github.com/aspnet/Docs/issues/6199
# https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-https-development.md
# https://github.com/aspnet/DotNetTools/tree/dev/src/dotnet-dev-certs
# https://blogs.msdn.microsoft.com/webdev/2018/02/27/asp-net-core-2-1-https-improvements/

$password = "password"

# Create the self-signed development cert
dotnet dev-certs https -ep $env:userprofile\.aspnet\https\TradePriceAggregator.pfx -p "$password"

# Trust the cert
dotnet dev-certs https --trust

# Add the cert password to the UserSecrets configs for Kestrel configuration
dotnet user-secrets -p TradePriceAggregator\TradePriceAggregator.csproj set "Kestrel:Certificates:Development:Password" "$password"