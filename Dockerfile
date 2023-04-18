FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /SeaPublicWebsite

# Copy everything
COPY . ./
# Restore as distinct layers

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /SeaPublicWebsite
COPY --from=build-env /SeaPublicWebsite/out .
ENTRYPOINT ["dotnet", "SeaPublicWebsite.dll"]