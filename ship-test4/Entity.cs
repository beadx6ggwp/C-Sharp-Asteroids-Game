using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ship_test1
{
    public class Entity
    {
        public string name = "";

        public int life = 1;

        public Vector pos;// 本體位置
        public Vector velocity;// 速度
        public Vector acc;// 加速度
        public double friction = 1;// 摩擦係數

        public double angle = 0;// 方向角
        public double radius = 20;// 半徑
        public double acc_power = 0;

        public Animation anim;

        public Entity(double x, double y, double r, double speed, double angle)
        {
            pos = new Vector(x, y);
            this.angle = angle;
            radius = r;

            velocity = new Vector(0, 0);
            velocity.setLength(speed);
            velocity.setAngle(angle);

            acc = new Vector(0, 0);
        }

        public virtual void Update(double deltaTime)
        {
            acc.setAngle(angle);// 根據當前的方向來決定推進的方向

            velocity.add(acc);// 將當前向量加上加速度
            velocity.multiplyScalar(friction);// 乘上阻力

            Vector curr_vel = new Vector(velocity.x, velocity.y);
            if (velocity.getLength() > acc_power * friction)// 簡易靜摩擦力
                pos.add(curr_vel.multiplyScalar(deltaTime));// 更新位置

            if (anim != null)
                anim.Update();
        }
        public virtual void Render(Graphics g)
        {
            GraphicsState gs = g.Save();

            g.TranslateTransform((float)(pos.x), (float)pos.y);
            g.RotateTransform((float)(angle * 180 / Math.PI));

            if (anim != null)
                anim.Render(g, -anim.spriteSheet.frameWidth / 2, -anim.spriteSheet.frameHeight / 2, 0);

            g.Restore(gs);
        }
        public void Debug(Graphics g)
        {
            GraphicsState gs = g.Save();

            g.TranslateTransform((float)(pos.x), (float)pos.y);
            g.RotateTransform((float)(angle * 180 / Math.PI));

            g.DrawArc(Pens.Black, -(float)radius, -(float)radius, (float)radius * 2, (float)radius * 2, 0, 360);
            g.DrawLine(Pens.Red, 0, 0, (float)radius, 0);

            g.Restore(gs);
        }

        public virtual void CheckEdge(int minW, int maxW, int minH, int maxH)
        {
            if (pos.x < minW) pos.x = maxW;
            if (pos.x > maxW) pos.x = minW;
            if (pos.y < minH) pos.y = maxH;
            if (pos.y > maxH) pos.y = minH;
        }

        public static bool isCollide(Entity e1, Entity e2)
        {
            double dx = e1.pos.x - e2.pos.x;
            double dy = e1.pos.y - e2.pos.y;
            double dr = e1.radius + e2.radius;
            return dx * dx + dy * dy < dr * dr;
        }
    }
}
