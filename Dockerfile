# -----------------------------
# Stage 1: Build Application
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish app
RUN dotnet publish -c Release -o out


# -----------------------------
# Stage 2: Run Application
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/out .

# Expose port
EXPOSE 80

# Run app
ENTRYPOINT ["dotnet", "EmployeesCrud.dll"]
