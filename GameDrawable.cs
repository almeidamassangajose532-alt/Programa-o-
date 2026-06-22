using FlappyBirdMaui.Game;
using Microsoft.Maui.Graphics;

namespace FlappyBirdMaui.Views;

/// <summary>
/// Responsável apenas por DESENHAR o estado atual do jogo no GraphicsView.
/// Não contém nenhuma lógica de jogo — apenas lê o GameEngine e pinta.
/// </summary>
public class GameDrawable : IDrawable
{
    private readonly GameEngine _engine;

    public GameDrawable(GameEngine engine)
    {
        _engine = engine;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        DrawBackground(canvas, dirtyRect);
        DrawPipes(canvas);
        DrawGround(canvas, dirtyRect);
        DrawBird(canvas);
    }

    private void DrawBackground(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        var gradient = new LinearGradientPaint
        {
            StartColor = Color.FromArgb("#4EC0F2"),
            EndColor = Color.FromArgb("#A6E3F2"),
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1)
        };
        canvas.SetFillPaint(gradient, rect);
        canvas.FillRectangle(rect);
        canvas.RestoreState();

        canvas.FillColor = Color.FromArgb("#FFFFFF").WithAlpha(0.5f);
        canvas.FillEllipse(40, 60, 60, 30);
        canvas.FillEllipse(rect.Width - 100, 120, 70, 34);
    }

    private void DrawGround(ICanvas canvas, RectF rect)
    {
        float groundY = _engine.ScreenHeight - GameEngine.GroundHeight;

        canvas.FillColor = Color.FromArgb("#DEB887");
        canvas.FillRectangle(0, groundY, rect.Width, GameEngine.GroundHeight);

        canvas.FillColor = Color.FromArgb("#6FBF45");
        canvas.FillRectangle(0, groundY, rect.Width, 12);
    }

    private void DrawPipes(ICanvas canvas)
    {
        canvas.FillColor = Color.FromArgb("#4CAF50");
        canvas.StrokeColor = Color.FromArgb("#2E7D32");
        canvas.StrokeSize = 3;

        foreach (var pipe in _engine.Pipes)
        {
            var top = pipe.GetTopBounds();
            canvas.FillRectangle(top);
            canvas.DrawRectangle(top);
            DrawPipeCap(canvas, pipe.X - 4, top.Bottom - 18, pipe.Width + 8, 18);

            var bottom = pipe.GetBottomBounds(_engine.ScreenHeight, GameEngine.GroundHeight);
            canvas.FillRectangle(bottom);
            canvas.DrawRectangle(bottom);
            DrawPipeCap(canvas, pipe.X - 4, bottom.Top, pipe.Width + 8, 18);
        }
    }

    private void DrawPipeCap(ICanvas canvas, float x, float y, float w, float h)
    {
        canvas.FillColor = Color.FromArgb("#388E3C");
        canvas.FillRectangle(x, y, w, h);
        canvas.DrawRectangle(x, y, w, h);
        canvas.FillColor = Color.FromArgb("#4CAF50");
    }

    private void DrawBird(ICanvas canvas)
    {
        var bird = _engine.Bird;

        canvas.SaveState();
        canvas.Translate(bird.X, bird.Y);
        canvas.Rotate(bird.Rotation);

        canvas.FillColor = Color.FromArgb("#FFD23F");
        canvas.FillEllipse(-bird.Radius, -bird.Radius, bird.Radius * 2, bird.Radius * 2);

        canvas.StrokeColor = Color.FromArgb("#E0A800");
        canvas.StrokeSize = 2;
        canvas.DrawEllipse(-bird.Radius, -bird.Radius, bird.Radius * 2, bird.Radius * 2);

        canvas.FillColor = Colors.White;
        canvas.FillEllipse(4, -bird.Radius * 0.4f, 10, 10);
        canvas.FillColor = Colors.Black;
        canvas.FillEllipse(7, -bird.Radius * 0.3f, 5, 5);

        canvas.FillColor = Color.FromArgb("#FF8C00");
        var beak = new PathF();
        beak.MoveTo(bird.Radius - 2, -4);
        beak.LineTo(bird.Radius + 10, 0);
        beak.LineTo(bird.Radius - 2, 6);
        beak.Close();
        canvas.FillPath(beak);

        canvas.FillColor = Color.FromArgb("#F2B705");
        canvas.FillEllipse(-bird.Radius * 0.6f, -2, bird.Radius * 0.9f, bird.Radius * 0.7f);

        canvas.RestoreState();
    }
}
