﻿using Domain.Interfaces;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, string? dbConnectionString)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(dbConnectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }
}
