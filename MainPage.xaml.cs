using FlappyBirdMaui.Game;
using FlappyBirdMaui.Views;

namespace FlappyBirdMaui;

/// <summary>
/// Code-behind da página principal. Liga a interface (XAML) ao motor
/// do jogo (GameEngine): inicia/para o loop de 60 FPS, traduz toques
/// do utilizador em chamadas ao GameEngine, e atualiza a UI com o
/// estado atual (score, game over, etc).
/// </summary>
public partial class MainPage : ContentPage
{
    private const int TargetFps = 60;
    private static readonly TimeSpan FrameInterval = TimeSpan.FromMilliseconds(1000.0 / TargetFps);

    private readonly GameEngine _engine = new();
    private GameDrawable? _drawable;
    private IDispatcherTimer? _gameLoopTimer;

    private int _bestScore;
    private bool _isCanvasReady;

    public MainPage()
    {
        InitializeComponent();
        SetupGameLoop();
    }

    private void SetupGameLoop()
    {
        _gameLoopTimer = Dispatcher.CreateTimer();
        _gameLoopTimer.Interval = FrameInterval;
        _gameLoopTimer.Tick += OnGameLoopTick;
    }

    private void OnGameCanvasSizeChanged(object? sender, EventArgs e)
    {
        if (GameCanvas.Width <= 0 || GameCanvas.Height <= 0)
            return;

        _engine.Initialize((float)GameCanvas.Width, (float)GameCanvas.Height);

        if (!_isCanvasReady)
        {
            _drawable = new GameDrawable(_engine);
            GameCanvas.Drawable = _drawable;
            _isCanvasReady = true;
        }

        GameCanvas.Invalidate();
    }

    private void OnScreenTapped(object? sender, TappedEventArgs e)
    {
        if (!_isCanvasReady) return;

        if (_engine.IsGameOver)
            return;

        bool wasStarted = _engine.HasStarted;
        _engine.OnTap();

        if (!wasStarted)
        {
            TapToStartLabel.IsVisible = false;
            StartGameLoop();
        }
    }

    private void StartGameLoop()
    {
        _gameLoopTimer?.Start();
    }

    private void StopGameLoop()
    {
        _gameLoopTimer?.Stop();
    }

    private void OnGameLoopTick(object? sender, EventArgs e)
    {
        _engine.Update();

        ScoreLabel.Text = _engine.Score.ToString();
        GameCanvas.Invalidate();

        if (_engine.IsGameOver)
        {
            HandleGameOver();
        }
    }

    private void HandleGameOver()
    {
        StopGameLoop();

        if (_engine.Score > _bestScore)
            _bestScore = _engine.Score;

        FinalScoreLabel.Text = $"Pontuação: {_engine.Score}";
        BestScoreLabel.Text = $"Melhor: {_bestScore}";
        GameOverPanel.IsVisible = true;
    }

    private void OnRestartClicked(object? sender, EventArgs e)
    {
        GameOverPanel.IsVisible = false;
        TapToStartLabel.IsVisible = true;
        ScoreLabel.Text = "0";

        _engine.Initialize((float)GameCanvas.Width, (float)GameCanvas.Height);
        GameCanvas.Invalidate();
    }
}
