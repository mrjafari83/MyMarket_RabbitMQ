using Application.Context;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;
using Microsoft.AspNetCore.Identity;
using RabbitMQ.EndPoint;
using Application.Facade.Interfaces;
using Application.Facade.Classes;
using Application.Services.GetEntites.GetEntitiesByFilter;
using RabbitMQ.EndPoint.Repositories.GetUsersByFilter;
using Common.Utilities;
using RabbitMQ.EndPoint.Excels;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RabbitBackgroundService>();

        services.AddSingleton<IRecive,Recive>();
        services.AddScoped<IGetExcel, GetExcel>();
        services.AddScoped<IDataBaseContext, DataBaseContext>();
        services.AddScoped<IExcelFacade, ExcelFacade>();
        services.AddScoped<IGetEntitiesByFilterService, GetEntitiesByFilterService>();
        services.AddScoped<IGetUserByFilter,GetUsersByFilter>();
        services.AddScoped<SaveLogInFile>();

        services.AddDbContext<DataBaseContext>(optons =>
            optons.UseSqlServer(@"Data Source= .; Initial Catalog= Market_DB; Integrated Security= False;User Id=sa;Password=123;")
        );

        services.AddIdentityCore<ApplicationUser>(option =>
        {
            option.Password.RequireLowercase = false;
            option.Password.RequireUppercase = false;
            option.Password.RequiredLength = 8;
            option.User.RequireUniqueEmail = true;
            option.Lockout.MaxFailedAccessAttempts = 10;
        }).AddEntityFrameworkStores<DataBaseContext>();

    })
    .Build();

await host.RunAsync();