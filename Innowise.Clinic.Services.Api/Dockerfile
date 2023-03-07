FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Innowise.Clinic.Services.Api/NuGet.Config", "./"]
COPY ["Innowise.Clinic.Services.Api/Innowise.Clinic.Services.Api.csproj", "Innowise.Clinic.Services.Api/"]
COPY ["Innowise.Clinic.Services.Dto/Innowise.Clinic.Services.Dto.csproj", "Innowise.Clinic.Services.Dto/"]
COPY ["Innowise.Clinic.Services.Configuration/Innowise.Clinic.Services.Configuration.csproj", "Innowise.Clinic.Services.Configuration/"]
COPY ["Innowise.Clinic.Services.Services/Innowise.Clinic.Services.Services.csproj", "Innowise.Clinic.Services.Services/"]
RUN dotnet restore "Innowise.Clinic.Services.Api/Innowise.Clinic.Services.Api.csproj"
COPY . .
WORKDIR "/src/Innowise.Clinic.Services.Api"
RUN dotnet build "Innowise.Clinic.Services.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innowise.Clinic.Services.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innowise.Clinic.Services.Api.dll"]
