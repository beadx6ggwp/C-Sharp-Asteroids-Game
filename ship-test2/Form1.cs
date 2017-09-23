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
    /* 
    #                                                    __----~~~~~~~~~~~------___
    #                                   .  .   ~~//====......          __--~ ~~
    #                   -.            \_|//     |||\\  ~~~~~~::::... /~
    #                ___-==_       _-~o~  \/    |||  \\            _/~~-
    #        __---~~~.==~||\=_    -_--~/_-~|-   |\\   \\        _/~
    #    _-~~     .=~    |  \\-_    '-~7  /-   /  ||    \      /
    #  .~       .~       |   \\ -_    /  /-   /   ||      \   /
    # /  ____  /         |     \\ ~-_/  /|- _/   .||       \ /
    # |~~    ~~|--~~~~--_ \     ~==-/   | \~--===~~        .\
    #          '         ~-|      /|    |-~\~~       __--~~
    #                      |-~~-_/ |    |   ~\_   _-~            /\
    #                           /  \     \__   \/~                \__
    #                       _--~ _/ | .-~~____--~-/                  ~~==.
    #                      ((->/~   '.|||' -_|    ~~-/ ,              . _||
    #                                 -_     ~\      ~~---l__i__i__i--~~_/
    #                                 _-~-__   ~)  \--______________--~~
    #                               //.-~~~-~_--~- |-------~~~~~~~~
    #                                      //.-~~~--\
    #                  神獸保佑
    #                程式永無BUG!
    */
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

        Vector ship;// 本體位置(在ship中Vec是當作座標，而不是向量)
        Vector thrust;// 推進力(加速度)
        Vector velocity;// 速度
        double friction = 0.99;// 摩擦係數
        double thrust_power = 0.1;// 加速度大小
        double angle = 0;// 飛行船的方向角
        int radius = 20;// 半徑

        Point[] ship_points = new Point[] { new Point(20, 0), new Point(-10, -15), new Point(-10, 15) };

        Random ran = new Random();

        //--------------------[37:left, 38:up, 39:right, 40:down]
        bool[] keys = new bool[] { false, false, false, false };

        public Form1()
        {
            InitializeComponent();
            main();
        }
        private void main()// Init, 這個main()我是用來當作初始化變數用的
        {
            // 建立畫布
            backGround = new Bitmap(panel1.Width, panel1.Height);
            g_main = Graphics.FromImage(backGround);
            g_panel = panel1.CreateGraphics();
            g_main.SmoothingMode = SmoothingMode.AntiAlias;// 開啟反鋸齒

            // 初始化玩家座標、速度向量、加速度向量
            ship = new Vector(panel1.Width / 2, panel1.Height / 2);
            velocity = new Vector(0, 0);
            thrust = new Vector(0, 0);

            // start
            lastTime = Environment.TickCount;
            timer1.Interval = 1000 / FPS;
            timer1.Start();
        }


        private void timer1_Tick(object sender, EventArgs e)// game loop
        {
            // 計算FPS
            deltaTime = Environment.TickCount - lastTime;
            timeAll.Add(1000.0 / (deltaTime | 1));
            lastTime = Environment.TickCount;

            update();

            g_main.Clear(Color.White);

            draw();

            DebugInf();

            g_panel.DrawImageUnscaled(backGround, 0, 0);

            this.Text = $"Interval : {timer1.Interval.ToString()}";
            if (deltaTime >= delteT)
            {
                timer1.Interval = Math.Max((int)(delteT - (deltaTime - delteT)), 1);
            }
        }
        private void update()
        {
            // 如果有按UP就讓加速度大小為thrust_power，否則歸零
            if (keys[1])
                thrust.setLength(thrust_power);
            else
                thrust.setLength(0);

            // 左右轉向
            if (keys[0]) angle -= 0.1;
            if (keys[2]) angle += 0.1;

            thrust.setAngle(angle);// 根據當前的方向來決定推進的方向

            velocity.add(thrust);// 將當前向量加上加速度
            velocity.multiplyScalar(friction);// 乘上阻力

            if (velocity.getLength() < thrust_power * friction)// 簡易的模擬靜摩擦力
                velocity.setLength(0);

            ship.add(velocity);// 更新位置



            // 超出邊界的處理
            if (ship.x < 0) ship.x = panel1.Width;
            if (ship.x > panel1.Width) ship.x = 0;
            if (ship.y < 0) ship.y = panel1.Height;
            if (ship.y > panel1.Height) ship.y = 0;
        }
        private void draw()
        {

            g_status = g_main.Save();

            g_main.TranslateTransform((float)(ship.x), (float)ship.y);
            g_main.RotateTransform((float)(angle * 180 / Math.PI));

            //draw here

            /* 
            g.DrawArc(Pens.Black, -radius, -radius, radius * 2, radius * 2, 0, 360);
            g.DrawLine(Pens.Red, 0, 0, radius, 0);
            */

            g_main.DrawPolygon(Pens.Black, ship_points);// 如果覺得三角形有困難的話，可以先註解這行，用上面的圓形

            if (keys[1]) g_main.DrawLine(Pens.Red, -10, 0, -20, ran.Next(-8, 9));// 加速時的小特效

            //end here
            g_main.Restore(g_status);
        }

        //------------------鍵盤處理-------------------------
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            setKeys(e.KeyValue, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            setKeys(e.KeyValue, false);
        }
        private void setKeys(int keyValue, bool status)
        {
            if (keyValue < 37 || keyValue > 40) return;
            keys[keyValue - 37] = status;
        }


        //---------------下面這段不重要，只是一些執行資料的顯示------------------
        private void DebugInf()
        {
            // 執行資料的顯示，不需要就可以註解掉
            g_status = g_main.Save();
            g_main.TranslateTransform((float)ship.x, (float)ship.y);
            g_main.RotateTransform((float)(velocity.getAngle() * 180 / Math.PI));
            float len = (float)velocity.getLength() * 10;
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

            g_main.DrawString($"position : {ship.ToString()}\r\n" +
                         $"velocity : {velocity.ToString()}\r\n" +
                         $"speed : {Math.Round(velocity.getLength(), 2)}\r\n" +
                         $"\r\n" +
                         $"FPS : {Math.Round(1000.0 / deltaTime)}\r\n" +
                         $"Average FPS : {Math.Round(timeAll.Average(), 1)}\r\n" +
                         $"Max FPS : {maxFPS}\r\n" +
                         $"Min FPS : {minFPS}",
                         new Font("consolas", 12), Brushes.Black, 10, 10);
        }

    }
}
