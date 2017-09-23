using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ship_test1
{
    public class Animation
    {
        public SpriteSheet spriteSheet;
        public List<int> animationSequence = new List<int>();
        public int currentFrame;
        public double frameSleep;
        public int lastTime;
        public Animation(SpriteSheet spriteSheet, double frameSpeedPerSecond, int startFrame, int endFrame)
        {
            this.spriteSheet = spriteSheet;

            currentFrame = 0;

            frameSleep = frameSpeedPerSecond != 0 ? 1000.0 / frameSpeedPerSecond : 0;
            lastTime = Environment.TickCount;

            for (int index = startFrame; index <= endFrame; index++)
                animationSequence.Add(index);
        }

        public void Update()
        {
            if (Environment.TickCount - lastTime > frameSleep && frameSleep != 0)
            {
                currentFrame = (currentFrame + 1) % animationSequence.Count;
                lastTime = Environment.TickCount;
            }
        }
        public void Render(Graphics g, double x, double y, double angle)
        {
            int index = animationSequence[currentFrame];
            int framePerRow = spriteSheet.framePerRow;

            int col = index % framePerRow;
            int row = index / framePerRow;

            int sw = spriteSheet.frameWidth;
            int sh = spriteSheet.frameHeight;
            int sx = col * sw;
            int sy = row * sh;
            int dw = sw;
            int dh = sh;

            RectangleF destRect = new RectangleF((float)x, (float)y, dw, dh);
            RectangleF srcRect = new RectangleF(sx, sy, sw, sh);

            float ang = (float)(angle * 180 / Math.PI);
            g.RotateTransform(90 + ang);
            g.DrawImage(spriteSheet.img, destRect, srcRect, GraphicsUnit.Pixel);
            g.RotateTransform(-90 - ang);


        }
        public void Render(Graphics g, int x, int y, int width, int height, double angle)
        {
            int index = animationSequence[currentFrame];
            int framePerRow = spriteSheet.framePerRow;

            int col = index % framePerRow;
            int row = index / framePerRow;

            int sw = spriteSheet.frameWidth;
            int sh = spriteSheet.frameHeight;
            int sx = col * sw;
            int sy = row * sh;
            int dw = sw;
            int dh = sh;

            Rectangle destRect = new Rectangle(x, y, width, height);
            Rectangle srcRect = new Rectangle(sx, sy, sw, sh);

            float ang = (float)(angle * 180 / Math.PI);
            g.RotateTransform(90 + ang);
            g.DrawImage(spriteSheet.img, destRect, srcRect, GraphicsUnit.Pixel);
            g.RotateTransform(-90 - ang);
        }
    }

    public class SpriteSheet
    {
        public Image img;
        public int frameWidth;
        public int frameHeight;
        public int framePerRow;
        public SpriteSheet(string fileName, int frameWidth, int frameHeight)
        {
            img = Image.FromFile(@fileName);
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            framePerRow = img.Width / frameWidth;
        }
    }


}
