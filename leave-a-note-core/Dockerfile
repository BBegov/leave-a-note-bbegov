#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["leave-a-note-core/leave-a-note-core.csproj", "leave-a-note-core/"]
RUN dotnet restore "leave-a-note-core/leave-a-note-core.csproj"
COPY . .
WORKDIR "/src/leave-a-note-core"
RUN dotnet build "leave-a-note-core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "leave-a-note-core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "leave-a-note-core.dll"]