namespace Cli.Commands.Options;

public interface ITextOption
{
    abstract static string TryGetPrompt(string? value);
}