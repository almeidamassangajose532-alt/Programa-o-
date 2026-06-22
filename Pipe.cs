using Microsoft.Maui.Graphics;

namespace FlappyBirdMaui.Models;

/// <summary>
/// Representa um par de tubos (um em cima, um em baixo) com um espaço
/// (gap) no meio por onde o pássaro deve passar.
/// </summary>
public class Pipe
{
    public float X { get; set; }
    public float Width { get; set; } = 60f;
    public float GapTop { get; set; }
    public float GapHeight { get; set; }
    public bool Scored { get; set; }
    public float GapBottom => GapTop + GapHeight;

    public void Reset(float startX, float screenHeight, float groundHeight, float gapHeight, Random rng)
    {
        X = startX;
        GapHeight = gapHeight;
        Scored = false;

        float minTop = 40f;
        float maxTop = screenHeight - groundHeight - gapHeight - 40f;
        if (maxTop < minTop) maxTop = minTop;

        GapTop = (float)(rng.NextDouble() * (maxTop - minTop) + minTop);
    }

    public void Update(float speed)
    {
        X -= speed;
    }

    public RectF GetTopBounds()
    {
        return new RectF(X, 0, Width, GapTop);
    }

    public RectF GetBottomBounds(float screenHeight, float groundHeight)
    {
        float bottomY = GapBottom;
        float bottomHeight = (screenHeight - groundHeight) - bottomY;
        return new RectF(X, bottomY, Width, bottomHeight);
    }
}
