using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneCalculator
{
    class ConstantCalculator
    {
        public int numberOfElems;
        public float _x;
        public float _y;
        public float xy;
        public float x_sqr;

        public ConstantCalculator()
        {
            numberOfElems = 0;
            _x = 0;
            _y = 0;
            xy = 0;
            x_sqr = 0;
        }

        public void IncreaseConstants(float x, float y)
        {
            numberOfElems++;
            Increase_x(x);
            Increase_y(y);
            Increasexy(x,y);
            IncreaseXsqr(x);
        }

        public void FinalizeConstants()
        {
            _x *= (float)1 / numberOfElems;
            _y *= (float)1 / numberOfElems;
            x_sqr *= (float)1 / numberOfElems;
        }

        public float CalculateB1()
        {
            float top = (float)numberOfElems * _x * _y;
            float bottom = (float)numberOfElems * (_x * _x);
            //Console.WriteLine(top+" "+bottom);
            return (xy - top) / (x_sqr - bottom);
        }

        public float CalculateB0(float b1)
        {
            return _y-(b1*_x);
        }

        private void Increase_x(float x)
        {
            _x += x;
        }

        private void Increase_y(float y)
        {
            _y += y;
        }

        private void Increasexy(float x, float y)
        {
            xy += (x * y);
        }

        private void IncreaseXsqr(float x)
        {
            x_sqr += (x * x);
        }

        public override string ToString()
        {
            return "N:"+numberOfElems+" _X: " + _x + " _Y: " + _y + " XY: " + xy + " X^2: " + x_sqr;
        }
    }
}
