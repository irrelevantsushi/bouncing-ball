using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;
using SkiaSharp.Views.Desktop;


namespace SkiaApp
{

    struct Ball // State of the ball
    {
        public SKPoint pos;
        public SKPoint vel;
        public int r;
        public SKColor color;

    }

    public partial class Form1 : Form
    {
        // Place instance variables here

        const int N = 3; // Number of balls
        Ball[] balls = new Ball[N]; // says the size of the array is whatever N is, happens to be number of balls

        static Random rnd = new Random(); // random number generator that is created here

        public static SKColor GetRandomColor() // Sets the possible colors of the balls
        {
            int hue = rnd.Next(360);
            SKColor color = SKColor.FromHsv((hue * 1f), 100f, 100f);
            return color;
        }

        public Form1()
        {
            InitializeComponent();
            
            for (int i = 0; i < N; ++i)
            {
                ref var ball = ref balls[i]; // .r is radius of ball
                ball.r = rnd.Next(30, 75); // Sets bounds of radii
                
                ball.pos = new SKPoint {
                    X = rnd.Next(ball.r, skControl1.Width - ball.r),
                    Y = rnd.Next(ball.r, skControl1.Height - ball.r)
                };

                ball.vel = new SKPoint(
                    rnd.Next(-6, 6), 
                    rnd.Next(-6, 6)
                );
                // example of two different initialization styles, they do the same thing!!: they give value to x and y in the struct
                
                ball.color = GetRandomColor();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)  // Works within time, so balls can move
        {
            for (int i = 0; i < N; ++i)
            {
                ref Ball b = ref balls[i]; // Creates a reference to Ball, so it's easier to write out

                b.pos += b.vel; // adds dx to x, integration

                if (b.pos.X-b.r <0) // For bouncing off of left side
                {
                    b.vel.X *= -1;
                }
                if (b.pos.X + b.r > skControl1.Width) // bouncing off of right side
                {
                    b.vel.X *= -1;
                }
                
                if (b.pos.Y - b.r < 0) // bouncing off of top
                {
                    b.vel.Y *= -1;
                }
                if (b.pos.Y + b.r > skControl1.Height) // bouncing off of bottom
                {
                    b.vel.Y*= -1;
                }
            }
            skControl1.Refresh();
        }

        private void skControl1_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            // need to have this line in order to place things on canvas


            canvas.Clear(SKColors.White);

            SKPaint p = new SKPaint();
            p.IsAntialias = true; // Creates smoother lines, less pixally


            foreach (var ball in balls)
            {
                p.Color = ball.color;
                canvas.DrawCircle(ball.pos, ball.r, p);
               
            }
        }
        
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Can create more forms, the windows where the balls show up
            if (false)
            {
                // var otherForm = new Form1(); // these are the same thing, just two different ways of writing it
                Form1 otherForm = new Form1();
                otherForm.Show();
            }
            Application.Run(new Form1());
        }
    }
}
