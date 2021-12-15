using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WinFormsApp1.Objects
{
    class Krug : BaseObject
    {
        private int SizeSleep = 0;
        public int Size = 40;
        public Action SizeZero;
        public Krug(float x, float y, float angle, int id) : base(x, y, angle, id)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Green), -(Size / 2), -(Size / 2), Size, Size);
            ReducingTheSize();
        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-(Size / 2), -(Size / 2), Size, Size);
            return path;
        }

        public void ReducingTheSize()
        {
            SizeSleep++;
            if (SizeSleep == 7)
            {
                SizeSleep = 0;
                Size--;
            }
            if (Size <= 0)
            {
                if (SizeZero != null)
                {
                    SizeZero();
                }
            }
        }

    }
}
