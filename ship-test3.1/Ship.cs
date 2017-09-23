using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ship_test1
{
    public class Ship : Entity
    {
        public bool key_up, key_down, key_left, key_right, key_space;

        public Animation static_anim;
        public Animation move_anim;

        public Point[] ship_points = new Point[] { new Point(20, 0), new Point(-10, -15), new Point(-10, 15) };

        public int lastFireTime = 0;
        public int fireDelay = 200;
        public int bulletSpeed = 400;
        public int bulletSize = 15;
        public Ship(Animation[] anim_arr, double x, double y, double r, double thrust_power) : base(x, y, r, 0, 0)
        {
            this.name = "ship";

            static_anim = anim_arr[0];
            move_anim = anim_arr[1];

            acc = new Vector(0, 0);
            acc_power = thrust_power;

            friction = 0.99;
        }
        public override void Update(double deltaTime)
        {

            // 如果有按UP就讓加速度大小為thrust_power，否則歸零
            if (key_up) acc.setLength(acc_power);
            else acc.setLength(0);

            anim = acc.getLength() > 0 ? move_anim : static_anim;

            // 左右轉向
            if (key_left) angle -= 0.1;
            if (key_right) angle += 0.1;

            base.Update(deltaTime);
        }

        public override void Render(Graphics g)
        {
            base.Render(g);
        }
        public void setKeys(int keyValue, bool status)
        {
            switch (keyValue)
            {
                case 37: key_left = status; break;
                case 38: key_up = status; break;
                case 39: key_right = status; break;
                case 40: key_down = status; break;
                case 32: key_space = status; break;
            }
        }

        public void Fire(List<Entity> entities, Animation bullet_anim)
        {
            if (Environment.TickCount - lastFireTime > fireDelay)
            {
                entities.Add(new Bullet(bullet_anim, pos.x, pos.y, bulletSize, bulletSpeed + velocity.getLength(), angle));
                lastFireTime = Environment.TickCount;
            }
        }
    }
}
