FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ENV ASPNETCORE_URLS http://+:8800
EXPOSE 8800

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore "DevBlog.Api/DevBlog.Api.csproj"
WORKDIR "/src/."
COPY . .
RUN dotnet build "DevBlog.Api/DevBlog.Api.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "DevBlog.Api/DevBlog.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevBlog.Api.dll"]