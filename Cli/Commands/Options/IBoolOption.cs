namespace Cli.Commands.Options;

public interface IBoolOption
{
    abstract static bool TryGetPrompt(bool? value);
}