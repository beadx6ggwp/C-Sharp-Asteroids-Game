using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ship_test1
{
    public class Bullet : Entity
    {
        public Bullet(Animation anim, double x, double y, double r, double speed, double angle) : base(anim,x, y, r, speed, angle)
        {
            this.name = "bullet";
        }

        public override void CheckEdge(int minW, int maxW, int minH, int maxH)
        {
            if (pos.x < minW || pos.x > maxW || pos.y < minH || pos.y > maxH)
                life = 0;
        }
    }
}
