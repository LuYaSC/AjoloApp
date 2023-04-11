#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SAP.Service/SAP.Service.csproj", "SAP.Service/"]
COPY ["SAP.Model/SAP.Model.csproj", "SAP.Model/"]
COPY ["SAP.Repository/SAP.Repository.csproj", "SAP.Repository/"]
COPY ["SAP.RuleEngine/SAP.RuleEngine.csproj", "SAP.RuleEngine/"]
COPY ["SAP.Core/SAP.Core.csproj", "SAP.Core/"]
RUN dotnet restore "SAP.Service/SAP.Service.csproj"
COPY . .
WORKDIR "/src/SAP.Service"
RUN dotnet build "SAP.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SAP.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SAP.Service.dll"]