using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ship_test1
{
    public class Vector
    {
        public double x, y;
        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // 暫時不需要
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y);
        }
        public static Vector operator *(Vector v1, Vector v2)
        {
            return new Vector(v1.x * v2.x, v1.y * v2.y);
        }
        public static Vector operator /(Vector v1, Vector v2)
        {
            return new Vector(v1.x / v2.x, v1.y / v2.y);
        }        
        
        public override string ToString()
        {
            return $"{Math.Round(x, 2)}, {Math.Round(y, 2)}";
        }
        public Vector add(Vector v2)
        {
            x += v2.x;
            y += v2.y;
            return this;
        }
        public Vector addScalar(double Scalar)
        {
            x += Scalar;
            y += Scalar;
            return this;
        }

        public Vector subtract(Vector v2)
        {
            x -= v2.x;
            y -= v2.y;
            return this;
        }
        public Vector subtractScalar(double Scalar)
        {
            x -= Scalar;
            y -= Scalar;
            return this;
        }

        public Vector multiply(Vector v2)
        {
            x *= v2.x;
            y *= v2.y;
            return this;
        }
        public Vector multiplyScalar(double Scalar)
        {
            x *= Scalar;
            y *= Scalar;
            return this;
        }

        public Vector divide(Vector v2)
        {
            x /= v2.x;
            y /= v2.y;
            return this;
        }
        public Vector divideScalar(double Scalar)
        {
            x /= Scalar;
            y /= Scalar;
            return this;
        }

        public Vector setAngle(double angle)
        {
            double len = getLength();
            x = Math.Cos(angle) * len;
            y = Math.Sin(angle) * len;
            return this;
        }
        public Vector setLength(double len)
        {
            double angle = getAngle();
            x = Math.Cos(angle) * len;
            y = Math.Sin(angle) * len;
            return this;
        }

        public double getLength()
        {
            return Math.Sqrt(x * x + y * y);
        }
        public double getAngle()
        {
            return Math.Atan2(y, x); ;
        }

        public Vector norm()
        {
            double len = getLength();
            x /= len;
            y /= len;
            return this;
        }
    }
}
