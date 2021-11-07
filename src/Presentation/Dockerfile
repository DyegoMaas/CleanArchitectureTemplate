FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CleanArchitectureTemplate.Presentation/CleanArchitectureTemplate.Presentation.csproj", "CleanArchitectureTemplate.Presentation/"]
RUN dotnet restore "src/CleanArchitectureTemplate.Presentation/CleanArchitectureTemplate.Presentation.csproj"
COPY . .
WORKDIR "/src/CleanArchitectureTemplate.Presentation"
RUN dotnet build "CleanArchitectureTemplate.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanArchitectureTemplate.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitectureTemplate.Presentation.dll"]
