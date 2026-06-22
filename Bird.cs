using Microsoft.Maui.Graphics;

namespace FlappyBirdMaui.Models;

/// <summary>
/// Representa o pássaro controlado pelo jogador.
/// Contém apenas dados e a física básica (gravidade, salto).
/// Não sabe nada sobre desenho — isso é feito pelo GraphicsView.
/// </summary>
public class Bird
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Velocity { get; set; }
    public float Radius { get; set; } = 18f;
    public float Rotation { get; set; }

    public const float Gravity = 0.6f;
    public const float JumpForce = -10f;

    public void Reset(float startX, float startY)
    {
        X = startX;
        Y = startY;
        Velocity = 0f;
        Rotation = 0f;
    }

    public void Update()
    {
        Velocity += Gravity;
        Y += Velocity;
        Rotation = Math.Clamp(Velocity * 3f, -30f, 90f);
    }

    public void Jump()
    {
        Velocity = JumpForce;
    }

    public RectF GetBounds()
    {
        return new RectF(X - Radius, Y - Radius, Radius * 2, Radius * 2);
    }
}
