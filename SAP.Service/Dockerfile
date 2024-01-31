#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AjoloApp.Service/AjoloApp.Service.csproj", "AjoloApp.Service/"]
COPY ["AjoloApp.Model/AjoloApp.Model.csproj", "AjoloApp.Model/"]
COPY ["AjoloApp.Repository/AjoloApp.Repository.csproj", "AjoloApp.Repository/"]
COPY ["AjoloApp.RuleEngine/AjoloApp.RuleEngine.csproj", "AjoloApp.RuleEngine/"]
COPY ["AjoloApp.Core/AjoloApp.Core.csproj", "AjoloApp.Core/"]
RUN dotnet restore "AjoloApp.Service/AjoloApp.Service.csproj"
COPY . .
WORKDIR "/src/AjoloApp.Service"
RUN dotnet build "AjoloApp.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AjoloApp.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AjoloApp.Service.dll"]