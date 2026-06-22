namespace FlappyBirdMaui;

/// <summary>
/// Ponto de entrada da aplicação. Como este jogo só tem uma página,
/// definimos a MainPage diretamente como janela principal — sem
/// necessidade de Shell ou navegação.
/// </summary>
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage())
        {
            Title = "Flappy Bird MAUI"
        };
    }
}
