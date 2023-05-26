using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.SetApplicationName("SqlExportExpress");
});
app.SetDefaultCommand<Cli.Commands.ExportCommand>();

return app.Run(args);
