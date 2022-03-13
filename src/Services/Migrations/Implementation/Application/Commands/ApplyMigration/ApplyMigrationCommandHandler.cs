using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Migrations.Application.Exceptions;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.Migrations.Infrastructure;
using HelpLine.Services.Migrations.Models;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace HelpLine.Services.Migrations.Application.Commands.ApplyMigration
{
    internal class ApplyMigrationCommandHandler : IRequestHandler<ApplyMigrationCommand, bool>
    {
        private readonly MigrationsMongoContext _mongoContext;
        private readonly ILogger _logger;


        public ApplyMigrationCommandHandler(MigrationsMongoContext mongoContext, ILogger logger)
        {
            _mongoContext = mongoContext;
            _logger = logger;
        }

        public async Task<bool> Handle(ApplyMigrationCommand request, CancellationToken cancellationToken)
        {
            if (request.Descriptor.IsManual && !request.ExecuteManual)
                return false;

            if (await _mongoContext.Applied.Find(x => x.Name == request.Descriptor.Name).AnyAsync())
                throw new MigrationAppliedException(request.Descriptor.Name);

            var migration = request.Descriptor.Instance;
            var applied = new List<IMigration>();
            var executionCtx = new ExecutionContext(request.Params);
            try
            {
                _logger.Information("Run migration {Migration}", request.Descriptor.Name);
                request.Descriptor.Statuses.Add(new MigrationExecutingStatus());
                await Execute(migration, applied, executionCtx);
                await _mongoContext.Applied.InsertOneAsync(new AppliedMigration(request.Descriptor.Name),
                    cancellationToken: cancellationToken);
                request.Descriptor.Statuses.Add(new MigrationAppliedStatus());
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Can't execute migration {Migration}", request.Descriptor.Name);
                request.Descriptor.Statuses.Add(new MigrationErrorStatus(e));
                request.Descriptor.Statuses.Add(new MigrationRollbackStatus());
                applied.Reverse();
                foreach (var appliedMigration in applied)
                    try
                    {
                        await appliedMigration.Down(executionCtx);
                        request.Descriptor.Statuses.Add(new MigrationRollbackSuccessStatus());
                    }
                    catch (Exception exception)
                    {
                        _logger.Error(e, "Can't rollback migration {Migration}, step: {Step}", request.Descriptor.Name,
                            appliedMigration.GetType().FullName);
                        request.Descriptor.Statuses.Add(new MigrationRollbackErrorStatus(exception));
                        break;
                    }

                return false;
            }
        }

        private async Task Execute(IMigrationInstance migrationInstance, List<IMigration> applied,
            ExecutionContext ctx)
        {
            if (migrationInstance is IMigrationInit migrationWithInit)
                await migrationWithInit.Init();

            switch (migrationInstance)
            {
                case ISteppedMigration steppedMigration:
                {
                    foreach (var step in steppedMigration.Steps)
                    {
                        _logger.Debug("Run migration step {Step}", step.GetType().FullName);
                        await Execute(step, applied, ctx);
                    }

                    break;
                }
                case IMigration migration:
                    applied.Add(migration);
                    await migration.Up(ctx);
                    break;
            }
        }
    }
}
