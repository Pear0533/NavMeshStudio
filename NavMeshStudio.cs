using static NavMeshStudio.Utils;

namespace NavMeshStudio;

public partial class NavMeshStudio : Form
{
    public NavMeshStudio()
    {
        InitializeComponent();
        CenterToScreen();
        RegisterCharacterEncodings();
        this.RegisterDefaultEvents();
        this.SetVersionString();
    }
}