FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Assessment.HackerNewsBestStories.API/Assessment.HackerNewsBestStories.API.csproj", "src/Assessment.HackerNewsBestStories.API/"]
RUN dotnet restore "src/Assessment.HackerNewsBestStories.API/Assessment.HackerNewsBestStories.API.csproj"
COPY . .
WORKDIR "/src/src/Assessment.HackerNewsBestStories.API"
RUN dotnet build "Assessment.HackerNewsBestStories.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Assessment.HackerNewsBestStories.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Assessment.HackerNewsBestStories.API.dll"]
