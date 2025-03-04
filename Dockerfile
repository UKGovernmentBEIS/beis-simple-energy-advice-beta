FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
# Install Chrome
RUN apt-get update && apt-get -f install && apt-get -y install wget gnupg2 apt-utils
RUN wget -q -O - https://dl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN echo 'deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main' >> /etc/apt/sources.list
RUN apt-get update \
    && apt-get install -y google-chrome-stable --no-install-recommends --allow-downgrades fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf
ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome-stable"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

ARG CONFIGURATION=Release

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
RUN dotnet publish -c $CONFIGURATION -o out

# Build runtime image
FROM base
USER app

WORKDIR /SeaPublicWebsite
COPY --from=build-env /SeaPublicWebsite/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SeaPublicWebsite.dll"]
