using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeaPublicWebsite.Data;

namespace SeaPublicWebsite.ManagementShell;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var contextOptions = new DbContextOptionsBuilder<SeaDbContext>()
            .UseNpgsql(
                Environment.GetEnvironmentVariable("ConnectionStrings__PostgreSQLConnection") ??
                @"UserId=postgres;Password=postgres;Server=localhost;Port=5432;Database=seadev;Include Error Detail=true;Pooling=true")
            .Options;

        using var context = new SeaDbContext(contextOptions);
        var outputProvider = new OutputProvider();
        var dataAccessProvider = new DataAccessProvider(context);
        var commandHandler = new CommandHandler(outputProvider, dataAccessProvider);

        Subcommand command;

        try
        {
            command = Enum.Parse<Subcommand>(args[0], true);
        }
        catch (Exception)
        {
            var allSubcommands = string.Join(", ", Enum.GetValues<Subcommand>());
            outputProvider.Output(
                $"Please specify a valid subcommand - available options are: {allSubcommands}");
            return;
        }

        switch (command)
        {
            case Subcommand.SetEmergencyMaintenanceState:
                await commandHandler.TrySetMaintenanceState(args.Skip(1).ToArray());
                break;
            default:
                outputProvider.Output("Invalid terminal command entered. Please refer to the documentation");
                return;
        }
    }

    private enum Subcommand
    {
        SetEmergencyMaintenanceState
    }
}