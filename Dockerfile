FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Install Chrome dependencies
RUN apt-get update \
    && apt-get -y -f install --no-install-recommends \
    && apt-get -y install --no-install-recommends wget gnupg2 apt-utils \
    && rm -rf /var/lib/apt/lists/*

# latest google-chrome-stable can be found at https://www.ubuntuupdates.org/pm/google-chrome-stable
ARG CHROME_VERSION=150.0.7871.114-1
RUN wget --no-verbose -O /tmp/chrome.deb https://dl.google.com/linux/chrome/deb/pool/main/g/google-chrome-stable/google-chrome-stable_${CHROME_VERSION}_amd64.deb \
  && apt-get update \
  && apt-get install -y --no-install-recommends /tmp/chrome.deb \
  && rm /tmp/chrome.deb \
  && rm -rf /var/lib/apt/lists/*

RUN apt-get update \
    && apt-get install -y --no-install-recommends fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf \
    && rm -rf /var/lib/apt/lists/*

ENV PUPPETEER_EXECUTABLE_PATH="/usr/bin/google-chrome-stable"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

ARG CONFIGURATION=Release

WORKDIR /SeaPublicWebsite

# Install NodeJS and NPM
SHELL ["/bin/bash", "-o", "pipefail", "-c"]
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y --no-install-recommends nodejs \
    && rm -rf /var/lib/apt/lists/*

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

# Restore ManagementShell
RUN dotnet restore SeaPublicWebsite.ManagementShell/

# Build and publish a release
RUN dotnet publish -c $CONFIGURATION -o out

# Build ManagementShell
RUN dotnet build SeaPublicWebsite.ManagementShell/ -c $CONFIGURATION --no-restore -o /cli

# Build runtime image
FROM base
USER app

WORKDIR /SeaPublicWebsite
COPY --from=build-env /SeaPublicWebsite/out .
COPY --from=build-env /cli ./cli
EXPOSE 8080
ENTRYPOINT ["dotnet", "SeaPublicWebsite.dll"]
