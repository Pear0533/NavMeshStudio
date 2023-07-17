namespace NavMeshStudio;

public class Console
{
    private readonly RichTextBox View = new();

    public Console() { }

    public Console(NavMeshStudio studio)
    {
        View = studio.consoleTextBox;
    }

    public void Write(string message)
    {
        View.AppendText((View.TextLength == 0 ? "" : '\n') + message);
    }
}