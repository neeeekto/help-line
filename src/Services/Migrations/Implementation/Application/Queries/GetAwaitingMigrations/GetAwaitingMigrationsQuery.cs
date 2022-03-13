using System.Collections.Generic;
using HelpLine.Services.Migrations.Application.Views;
using HelpLine.Services.Migrations.Contracts;

namespace HelpLine.Services.Migrations.Application.Queries.GetAwaitingMigrations
{
    public class GetAwaitingMigrationsQuery : ICommand<IEnumerable<AwaitingMigrationView>>
    {

    }
}
