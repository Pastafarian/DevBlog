#Use your choice of image as base. Mine is alpine! 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
ENV ASPNETCORE_URLS http://+:8800
EXPOSE 8800

WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .

RUN dotnet restore "StephenAdam.io.Api/StephenAdam.io.Api.csproj"
WORKDIR "/src/."
COPY . .
RUN dotnet build "StephenAdam.io.Api/StephenAdam.io.Api.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "StephenAdam.io.Api/StephenAdam.io.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StephenAdam.io.Api.dll"]