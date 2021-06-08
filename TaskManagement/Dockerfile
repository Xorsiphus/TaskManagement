FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TaskManagement/TaskManagement.csproj", "TaskManagement/"]
RUN dotnet restore "TaskManagement/TaskManagement.csproj"
COPY . .
WORKDIR "/src/TaskManagement"
RUN dotnet build "TaskManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagement.dll"]
