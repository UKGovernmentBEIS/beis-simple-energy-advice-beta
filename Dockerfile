FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /SeaPublicWebsite

# Copy everything
COPY . ./

# Install NodeJS and NPM
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - &&\
apt-get install -y nodejs

# Build node assets
WORKDIR /SeaPublicWebsite/SeaPublicWebsite
RUN npm install
RUN npm run build

# Add Sources
WORKDIR /SeaPublicWebsite
RUN dotnet nuget add source /SeaPublicWebsite/Lib --name Local

# Restore as distinct layers
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /SeaPublicWebsite
COPY --from=build-env /SeaPublicWebsite/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "SeaPublicWebsite.dll"]
