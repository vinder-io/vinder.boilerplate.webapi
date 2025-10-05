global using System.Diagnostics.CodeAnalysis;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using Vinder.Sample.Domain.Repositories;

global using Vinder.Sample.Application.Payloads.Product;
global using Vinder.Sample.Application.Validators.Product;
global using Vinder.Sample.Application.Handlers.Product;

global using Vinder.Sample.Infrastructure.Repositories;
global using Vinder.Sample.CrossCutting.Configurations;
global using Vinder.Sample.CrossCutting.Exceptions;

global using Vinder.Internal.Essentials.Contracts;
global using Vinder.Internal.Infrastructure.Persistence.Repositories;

global using MongoDB.Driver;
global using FluentValidation;