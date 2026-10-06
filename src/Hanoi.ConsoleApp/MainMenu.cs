namespace Hanoi.ConsoleApp;

public sealed class MainMenu(ConsoleView view)
{
    private const int OptionCount = 3;

    public void Run()
    {
        while (true)
        {
            view.ShowMainMenu();
            var text = view.Prompt("Elige una opción: ");
            if (text is null)
            {
                return; // la entrada se cerró: salir en lugar de pedir la opción indefinidamente
            }

            switch (InputParser.ParseMenuOption(text, OptionCount))
            {
                case 1:
                    new AutoSolveMode(view).Run();
                    break;
                case 2:
                    new ManualPlayMode(view).Run();
                    break;
                case 3:
                    view.ShowMessage("¡Hasta pronto!");
                    return;
                default:
                    view.ShowError($"opción no válida. Ingresa un número entre 1 y {OptionCount}.");
                    break;
            }
        }
    }
}
