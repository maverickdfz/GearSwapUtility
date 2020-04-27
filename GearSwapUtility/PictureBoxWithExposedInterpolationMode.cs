using System.Windows.Forms;

public class PictureBoxWithExposedInterpolationMode : PictureBox
{
    public System.Drawing.Drawing2D.InterpolationMode InterpolationMode { get; set; }

    protected override void OnPaint(PaintEventArgs paintEventArgs)
    {
        System.Drawing.Drawing2D.InterpolationMode OldInterpolationMode = paintEventArgs.Graphics.InterpolationMode;
        paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
        base.OnPaint(paintEventArgs);
        paintEventArgs.Graphics.InterpolationMode = OldInterpolationMode;
    }
}