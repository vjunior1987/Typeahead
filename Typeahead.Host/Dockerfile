#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Typeahead.Host/Typeahead.Host.csproj", "Typeahead.Host/"]
COPY ["Typeahead.DAL/Typeahead.DAL.csproj", "Typeahead.DAL/"]
COPY ["Typeahead.Service/Typeahead.Service.csproj", "Typeahead.Service/"]
COPY ["Typeahead.Tests/Typeahead.Tests.csproj", "Typeahead.Tests/"]
RUN dotnet restore "Typeahead.Host/Typeahead.Host.csproj"
COPY . .
WORKDIR "/src/Typeahead.Host"
RUN dotnet build "Typeahead.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Typeahead.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Typeahead.Host.dll"]