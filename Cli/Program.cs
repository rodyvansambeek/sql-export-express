using Cli.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();
services.AddDependencies();

var app = new CommandApp(new TypeRegistrar(services));
app.Configure(config =>
{
    config.SetApplicationName("SqlExportExpress");
});
app.SetDefaultCommand<Cli.Commands.ExportCommand>();

return app.Run(args);
