using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace ship_test1
{
    /*
     ===============================================================
     基本說明
     
     在這個程式中我以timer1_Tick當作game loop
     
     這個程式中，你們真正需要研究的有兩個部分update()、draw()
     
     update()
     - 物體的移動
     - 速度與加速度的使用
     - 了解Vector的使用方法與重要性
     - Vector函式庫可以在方案總管中看見，不知道怎麼使用的話，先去把小山看完
     
     draw()
     - 畫布的平移與旋轉
     - 熟悉各種基本形狀的繪圖方法
     
     希望你們能先搞懂update()與draw()中的內容
     再去慢慢看其他的地方
     
     關於draw()中的方法，我想你們可能沒學過，你們先google研究看看
     這邊我給你們一些關鍵字跟提示
     
     基本關鍵字:
     C# Graphics TranslateTransform
     C# Graphics RotateTransform
     C# Graphics Save
     C# Graphics Restore
     
     其他關鍵字你可以用中文直翻或怎樣的都可以，這些東西資源還算多
     最快的就是在關鍵字後面加個 stackoverflow
     
     看得懂MSDN的就先看MSDN，不行的話就先看其他中文的部落格
     ===============================================================
     
     QA:

     都看不懂，不會用怎麼辦?
     模仿各個教學的範例測試，慢慢就會瞭解了
     
     要怎麼測試?
     開一個新的專案，在paint事件中慢慢測試各種繪圖效果，成功了再拿來用
     
     不會測試?
     可以找我，我會盡量教
     
     懶得花時間測試就想找人要結果?
     把專案砍了，先跳下去再說

━━━━━┒
┓┏┓┏┓┃爽啦!
┛┗┛┗┛┃＼○／
┓┏┓┏┓┃ /
┛┗┛┗┛┃ノ)
┓┏┓┏┓┃
┛┗┛┗┛┃
┓┏┓┏┓┃
┛┗┛┗┛┃
     
     這個專案只是參考，希望你們能了解原理後自己做出來
     2017/9/20 許展維
     */

    // 這邊先送一隻佛祖，保佑你們
    //
    //                       _oo0oo_
    //                      o8888888o
    //                      88" . "88
    //                      (| -_- |)
    //                      0\  =  /0
    //                    ___/`---'\___
    //                  .' \\|     |// '.
    //                 / \\|||  :  |||// \
    //                / _||||| -:- |||||- \
    //               |   | \\\  -  /// |   |
    //               | \_|  ''\---/''  |_/ |
    //               \  .-\__  '-'  ___/-. /
    //             ___'. .'  /--.--\  `. .'___
    //          ."" '<  `.___\_<|>_/___.' >' "".
    //         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
    //         \  \ `_.   \_ __\ /__ _/   .-` /  /
    //     =====`-.____`.___ \_____/___.-`___.-'=====
    //                       `=---='
    //
    //     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //
    //               佛祖保佑         永無bug
    //
    //***************************************************    
    public partial class Form1 : Form
    {
        // FPS計算用
        int lastTime = 0;
        int deltaTime = 0;
        List<double> timeAll = new List<double>();

        readonly int FPS = 60;
        readonly double delteT = 16.666667;
        Graphics g_panel;
        Graphics g_main;
        GraphicsState g_status;
        Bitmap backGround;

        List<Entity> entities = new List<Entity>();
        Ship ship;

        Point[] ship_points = new Point[] { new Point(20, 0), new Point(-10, -15), new Point(-10, 15) };

        Random ran = new Random();

        Animation sBullet, sShip, sShip_go;

        //--------------------[37:left, 38:up, 39:right, 40:down]
        bool[] keys = new bool[] { false, false, false, false };

        public Form1()
        {
            InitializeComponent();
            //animation = new Animation(new SpriteSheet(@"resource\fire_bomb.png", 8, 14), 5, 0, 6);

            main();
        }
        private void main()// Init, 這個main()我是用來當作初始化變數用的
        {
            // 16,39
            this.Width = 800 + 16;
            this.Height = 600 + 39;

            // 建立畫布
            backGround = new Bitmap(panel1.Width, panel1.Height);
            g_main = Graphics.FromImage(backGround);
            g_panel = panel1.CreateGraphics();

            g_main.SmoothingMode = SmoothingMode.AntiAlias;// 開啟反鋸齒

            Init();

            // start
            lastTime = Environment.TickCount;
            timer1.Interval = 1000 / FPS;
            timer1.Start();
        }

        private void Init()
        {
            sBullet = new Animation(new SpriteSheet(@"resource\fire_blue.png", 32, 64), 30, 0, 15);
            sShip = new Animation(new SpriteSheet(@"resource\spaceship.png", 39, 43), 0, 0, 0);
            sShip_go = new Animation(new SpriteSheet(@"resource\spaceship.png", 39, 44), 10, 1, 2);

            // 初始化
            ship = new Ship(new Animation[] { sShip, sShip_go }, panel1.Width / 2, panel1.Height / 2, 20, 0.1);

            entities.Add(ship);
        }


        private void timer1_Tick(object sender, EventArgs e)// game loop
        {
            // 計算FPS
            deltaTime = Environment.TickCount - lastTime;
            timeAll.Add(1000.0 / (deltaTime | 1));
            lastTime = Environment.TickCount;

            //--------------
            update((double)deltaTime / 1000);


            g_main.Clear(Color.White);

            draw(g_main);
            DebugInf();

            g_panel.DrawImageUnscaled(backGround, 0, 0);
            //--------------

            this.Text = $"{panel1.Size.ToString()} {(double)deltaTime / 1000}";
            if (deltaTime >= delteT)
            {
                timer1.Interval = Math.Max((int)(delteT - (deltaTime - delteT)), 1);
            }
        }
        private void update(double deltaTime)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(deltaTime);
                entities[i].CheckEdge(0, Width, 0, Height);
            }


            if (ship.key_space) ship.Fire(entities, sBullet);


            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].life <= 0)
                    entities.RemoveAt(i);
            }
        }
        private void draw(Graphics g)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Render(g);
                //entities[i].Debug(g);
            }
        }

        //------------------鍵盤處理-------------------------
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            ship.setKeys(e.KeyValue, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            ship.setKeys(e.KeyValue, false);
        }


        //---------------下面這段不重要，只是一些執行資料的顯示------------------
        private void DebugInf()
        {
            // 執行資料的顯示，不需要就可以註解掉
            g_status = g_main.Save();
            g_main.TranslateTransform((float)ship.pos.x, (float)ship.pos.y);
            g_main.RotateTransform((float)(ship.velocity.getAngle() * 180 / Math.PI));
            float len = (float)ship.velocity.getLength() / 10;
            if (len > 1)
            {
                g_main.DrawLine(Pens.Orange, 0, 0, len, 0);

                g_main.FillPolygon(Brushes.Orange, new PointF[] {
                    new PointF(len, 0),
                    new PointF(len - 10, 4),
                    new PointF(len - 10, -4) });
            }
            g_main.Restore(g_status);

            double[] fps_arr = timeAll.ToArray();
            Array.Sort(fps_arr);
            int maxFPS = (int)fps_arr[fps_arr.Length - 1];
            int minFPS = (int)fps_arr[0];

            g_main.DrawString($"position : {ship.pos.ToString()}\r\n" +
                         $"velocity : {ship.velocity.ToString()}\r\n" +
                         $"speed : {Math.Round(ship.velocity.getLength(), 2)}\r\n" +
                         $"\r\n" +
                         $"FPS : {Math.Round(1000.0 / deltaTime)}\r\n" +
                         $"Average FPS : {Math.Round(timeAll.Average(), 1)}\r\n" +
                         $"Max FPS : {maxFPS}\r\n" +
                         $"Min FPS : {minFPS}",
                         new Font("consolas", 12), Brushes.Black, 10, 10);
        }
    }
}
