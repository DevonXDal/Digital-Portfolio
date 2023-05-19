# https://hub.docker.com/_/microsoft-dotnet
# From guide: https://medium.com/bb-tutorials-and-thoughts/dockerizing-angular-app-with-net-core-backend-734ea2df84df
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /build

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

# copy csproj and restore as distinct layers
COPY./*.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /build
RUN dotnet publish -c release -o published --no-cache

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /build/published ./
ENTRYPOINT ["dotnet", "Portfolio.dll"]
