FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Install Chrome
RUN apt-get update && apt-get -f install && apt-get -y install wget gnupg2 apt-utils

# latest google-chrome-stable can be found at https://www.ubuntuupdates.org/pm/google-chrome-stable
ARG CHROME_VERSION=139.0.7258.138-1
RUN wget --no-verbose -O /tmp/chrome.deb https://dl.google.com/linux/chrome/deb/pool/main/g/google-chrome-stable/google-chrome-stable_${CHROME_VERSION}_amd64.deb \
  && apt install -y /tmp/chrome.deb --no-install-recommends \
  && rm /tmp/chrome.deb

RUN apt-get update \
    && apt-get install -y --no-install-recommends fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf

ENV PUPPETEER_EXECUTABLE_PATH="/usr/bin/google-chrome-stable"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

ARG CONFIGURATION=Release

WORKDIR /SeaPublicWebsite

# Install NodeJS and NPM
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - &&\
apt-get install -y nodejs

# Copy everything
COPY . ./

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
