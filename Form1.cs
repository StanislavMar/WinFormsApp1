using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Objects;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        
        List<BaseObject> objects = new();
        public static Random rnd = new Random();
        Player player;
        Marker marker;
        Krug circleOne, circleTwo;
        int score = 0;
        public Form1()
        {
            InitializeComponent();

            label1.Text = "Счет: " + score;
            
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            // добавил реакцию на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnSerclOverlap += (s) =>
            {
                score++;
                if (s.Id == 0) circleOne = null;
                else if (s.Id == 1) circleTwo = null;
                objects.Remove(s);
                s = null;
            };

            // остальное не трогаем
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            circleOne = new Krug(rnd.Next() % pbMain.Width, rnd.Next() % pbMain.Height, 0, 0);
            circleTwo = new Krug(rnd.Next() % pbMain.Width, rnd.Next() % pbMain.Height, 0, 1);

            circleOne.SizeZero += () =>
            {
                objects.Remove(circleOne);
                circleOne = null;
            };
            circleTwo.SizeZero += () =>
            {
                objects.Remove(circleTwo);
                circleTwo = null;
            };

            objects.Add(marker);
            objects.Add(player);
            objects.Add(circleOne);
            objects.Add(circleTwo);
 
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);

            UpdateCircles();
            updatePlayer();

            // пересчитываем пересечения
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            // рендерим объекты
            foreach (var obj in objects.ToList())
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
            label1.Text = "Счет: " + score;
        }

        private void updatePlayer()
        {
            // тут добавляем проверку на marker не нулевой
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;

            }

            // тормозящий момент,
            // нужен чтобы, когда игрок достигнет маркера произошло постепенное замедление
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // пересчет позиция игрока с помощью вектора скорости
            player.X += player.vX;
            player.Y += player.vY;

            // запрашиваем обновление pbMain
            // это вызовет метод pbMain_Paint по новой

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // тут добавил создание маркера по клику если он еще не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }

            // а это так и остается
            marker.X = e.X;
            marker.Y = e.Y;
        }
        public void UpdateCircles()
        {
            if (circleOne == null)
            {
                circleOne = new Krug(rnd.Next(pbMain.Width), rnd.Next(pbMain.Height), 0, 0);
                circleOne.SizeZero += () =>
                {
                    objects.Remove(circleOne);
                    circleOne = null;
                };
                objects.Add(circleOne);
            }
            else
            {
                circleOne.ReducingTheSize();
            }

            if (circleTwo == null)
            {
                circleTwo = new Krug(rnd.Next(pbMain.Width), rnd.Next(pbMain.Height), 0, 1);
                circleTwo.SizeZero += () =>
                {
                    objects.Remove(circleTwo);
                    circleTwo = null;
                };
                objects.Add(circleTwo);
            }
            else
            {
                circleTwo.ReducingTheSize();
            }
        }
    }
}
