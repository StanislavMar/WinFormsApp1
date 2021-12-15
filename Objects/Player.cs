using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WinFormsApp1.Objects
{
    class Player : BaseObject
    {
        public Action<Krug> OnSerclOverlap;
        public Action<Marker> OnMarkerOverlap;
        public float vX, vY;
        public Player (float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render (Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Red) ,
                -15, -15,
                30, 30
                );

            g.DrawRectangle(new Pen(Color.Black, 2) ,
                -15, -15,
                30, 30
                );

            g.FillRectangle(new SolidBrush(Color.Blue), 10, 8, 20, 7);
            g.FillRectangle(new SolidBrush(Color.Orange), -5, -5, 10, 10);

            g.FillRectangle(new SolidBrush(Color.Blue), 10, -15, 20, 7);


            g.DrawRectangle(new Pen(Color.Black, 2), 10, 8, 20, 7);
            g.DrawRectangle(new Pen(Color.Black, 2), -5, -5, 10, 10);
            g.DrawRectangle(new Pen(Color.Black, 2), 10, -15, 20, 7);
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
            
            if (obj is Marker)
            {
                OnMarkerOverlap(obj as Marker);

            }
            if (obj is Krug)
            {
                OnSerclOverlap(obj as Krug);
            }
        }
    }
}