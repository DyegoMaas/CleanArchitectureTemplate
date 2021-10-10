FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /publish
COPY src .
RUN dotnet restore ./CleanArchitectureTemplate.Presentation/CleanArchitectureTemplate.Presentation.csproj
RUN dotnet publish ./CleanArchitectureTemplate.Presentation/CleanArchitectureTemplate.Presentation.csproj -c Release --output ./out 

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal as release
WORKDIR /app
COPY --from=build /publish/out .
ENV ASPNETCORE_URLS "http://0.0.0.0:5000/"
ENTRYPOINT ["dotnet", "CleanArchitectureTemplate.Presentation.dll"]