using FlappyBirdMaui.Models;

namespace FlappyBirdMaui.Game;

/// <summary>
/// Motor do jogo: contém toda a lógica do Flappy Bird (física, tubos,
/// colisões e pontuação). Não depende de XAML, GraphicsView ou qualquer
/// elemento de interface — pode ser testado isoladamente.
/// </summary>
public class GameEngine
{
    public const float PipeSpeed = 5f;
    public const float PipeGapHeight = 180f;
    public const float PipeSpacing = 250f;
    public const int PipeCount = 3;
    public const float GroundHeight = 80f;

    private readonly Random _rng = new();

    public float ScreenWidth { get; private set; }
    public float ScreenHeight { get; private set; }

    public Bird Bird { get; } = new();
    public List<Pipe> Pipes { get; } = new();

    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool HasStarted { get; private set; }

    public void Initialize(float screenWidth, float screenHeight)
    {
        ScreenWidth = screenWidth;
        ScreenHeight = screenHeight;

        Score = 0;
        IsGameOver = false;
        HasStarted = false;

        Bird.Reset(startX: screenWidth * 0.3f, startY: screenHeight * 0.4f);

        Pipes.Clear();
        for (int i = 0; i < PipeCount; i++)
        {
            var pipe = new Pipe();
            float startX = screenWidth + i * PipeSpacing;
            pipe.Reset(startX, screenHeight, GroundHeight, PipeGapHeight, _rng);
            Pipes.Add(pipe);
        }
    }

    public void OnTap()
    {
        if (IsGameOver) return;

        HasStarted = true;
        Bird.Jump();
    }

    public void Update()
    {
        if (IsGameOver || !HasStarted)
            return;

        Bird.Update();
        UpdatePipes();
        CheckScoring();
        CheckCollisions();
    }

    private void UpdatePipes()
    {
        foreach (var pipe in Pipes)
        {
            pipe.Update(PipeSpeed);

            if (pipe.X + pipe.Width < 0)
            {
                float maxX = Pipes.Max(p => p.X);
                pipe.Reset(maxX + PipeSpacing, ScreenHeight, GroundHeight, PipeGapHeight, _rng);
            }
        }
    }

    private void CheckScoring()
    {
        foreach (var pipe in Pipes)
        {
            if (!pipe.Scored && pipe.X + pipe.Width < Bird.X)
            {
                pipe.Scored = true;
                Score++;
            }
        }
    }

    private void CheckCollisions()
    {
        var birdBounds = Bird.GetBounds();

        float groundY = ScreenHeight - GroundHeight;
        if (birdBounds.Bottom >= groundY)
        {
            Bird.Y = groundY - Bird.Radius;
            EndGame();
            return;
        }

        if (birdBounds.Top <= 0)
        {
            Bird.Y = Bird.Radius;
            Bird.Velocity = 0;
            return;
        }

        foreach (var pipe in Pipes)
        {
            var topBounds = pipe.GetTopBounds();
            var bottomBounds = pipe.GetBottomBounds(ScreenHeight, GroundHeight);

            if (birdBounds.IntersectsWith(topBounds) || birdBounds.IntersectsWith(bottomBounds))
            {
                EndGame();
                return;
            }
        }
    }

    private void EndGame()
    {
        IsGameOver = true;
    }
}
