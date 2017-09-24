using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ship_test1
{
    public class Asteroid : Entity
    {
        public Asteroid(Animation anim, double x, double y, double r, double speed, double angle) : base(anim, x, y, r, speed, angle)
        {
            this.name = "asteroid";
        }
    }
}
