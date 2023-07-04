using static NavMeshStudio.Utils;

namespace NavMeshStudio;

public partial class NavMeshStudio : Form
{
    public NavMeshStudio()
    {
        InitializeComponent();
        RegisterCharacterEncodings();
        this.RegisterDefaultEvents();
        this.SetVersionString();
    }
}