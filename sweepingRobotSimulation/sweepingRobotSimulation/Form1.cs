using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace sweepingRobotSimulation
{
    public partial class Form1 : Form
    {
        Label tips;
        string baseText = "扫地机器人仿真 By GuessEver";
        public string basePath = "../../../../src/";
        public string gccStr(string filename)
        {
            filename = basePath + filename;
            return "g++ " + filename + ".cpp -o " + filename;
        }
        public string runStr(string filename)
        {
            return "cd " + basePath + " && " + filename + ".exe";
        }
        public bool runCommand(string command)
        {
            // MessageBox.Show(command);
            Process process = new Process();//创建进程对象  
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";//设定需要执行的命令  
            startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出  
            startInfo.UseShellExecute = false;//不使用系统外壳程序启动
            startInfo.RedirectStandardInput = false;//不重定向输入
            startInfo.RedirectStandardOutput = false; //重定向输出
            startInfo.CreateNoWindow = true;//不创建窗口
            process.StartInfo = startInfo;
            try
            {
                if (process.Start())
                {
                    process.WaitForExit();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public TextBox W_input, H_input, N_input, B_input, A_input;
        public bool getReady()
        {
            string W_str = W_input.Text;
            string H_str = H_input.Text;
            string N_str = N_input.Text;
            string B_str = B_input.Text;
            string A_str = A_input.Text;
            string parameter = W_str + " " + H_str + " " + N_str + " " + B_str + " " + A_str;
            return this.runCommand(this.gccStr("mapGenerator"))
                && this.runCommand(this.gccStr("routeSolver"))
                && this.runCommand(this.runStr("mapGenerator " + parameter))
                && this.runCommand(this.runStr("routeSolver"));
        }
        public int baseLeft = 150, baseTop = 10;
        public int W = 0, H = 0, Sx, Sy, A;
        public int[][] cap;
        public void readParameters()
        {
            StreamReader sr = new StreamReader(basePath + "map.txt", Encoding.Default);
            string[] tmp = sr.ReadLine().Split(new char[] { ' ' });
            W = int.Parse(tmp[0]);
            H = int.Parse(tmp[1]);
            cap = new int[1000][];
            for (int i = 0; i < 1000; i++)
            {
                cap[i] = new int[1000];
            }
            for (int j = 0; j < H; j++)
            {
                tmp = sr.ReadLine().Split(new char[] { ' ' });
                for (int i = 0; i < W; i++)
                {
                    cap[i][j] = int.Parse(tmp[i]);
                }
            }
            tmp = sr.ReadLine().Split(new char[] { ' ' });
            A = int.Parse(tmp[0]); Sx = int.Parse(tmp[1]); Sy = int.Parse(tmp[2]);
            sr.Close();
        }
        public void fillColor(int x, int y, SolidBrush brush)
        {
            Point[] p = new Point[4];
            p[0] = new Point(x, y);
            p[1] = new Point(x, y + 1);
            p[2] = new Point(x + 1, y + 1);
            p[3] = new Point(x + 1, y);
            Graphics g = this.CreateGraphics();
            g.FillPolygon(brush, p);
        }
        public void printMap()
        {
            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < H; j++)
                {
                    int x = i + baseLeft, y = j + baseTop;
                    this.fillColor(x, y, new SolidBrush(Color.White));
                }
            }
            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < H; j++)
                    if(cap[i][j] == -1)
                    {
                        int x = i + baseLeft, y = j + baseTop;
                        this.fillColor(x, y, new SolidBrush(Color.Red));
                    }
            }
            paintRobot(Sx, Sy, A, new SolidBrush(Color.Black), 1);
        }
        private void generateMap(object sender, EventArgs e)
        {
            tips.Text = "Compiling and rendering...";
            tips.Refresh();
            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < H; j++)
                {
                    int x = i + baseLeft, y = j + baseTop;
                    this.fillColor(x, y, new SolidBrush(BackColor));
                }
            }
            this.getReady();
            this.readParameters();
            this.printMap();
            tips.Text = baseText;
            tips.Refresh();
        }
        private void renderMap(object sender, EventArgs e)
        {
            tips.Text = "Rendering...";
            tips.Refresh();
            this.readParameters();
            this.printMap();
            tips.Text = baseText;
            tips.Refresh();
        }
        private void paintRobot(int currentX, int currentY, int size, SolidBrush brush, int addVal)
        {
            for (int i = currentX; i < currentX + A; i++)
            {
                for (int j = currentY; j < currentY + A; j++)
                {
                    int x = i + baseLeft, y = j + baseTop;
                    this.fillColor(x, y, brush);
                    cap[i][j] += addVal;
                }
            }
        }
        private void runMap(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(basePath + "route.txt", Encoding.Default);
            string route = sr.ReadLine();
            sr.Close();

            if (route == null)
            {
                return;
            }
            int currentX = Sx, currentY = Sy;
            for (int idx = 0; idx < route.Length; idx++)
            {
                SolidBrush brush = new SolidBrush(Color.LightBlue);
                paintRobot(currentX, currentY, A, brush, 0);
                tips.Text = idx + " / " + route.Length;
                tips.Refresh();
                switch (route[idx])
                {
                    case 'N':
                        --currentY;
                        break;
                    case 'E':
                        ++currentX;
                        break;
                    case 'S':
                        ++currentY;
                        break;
                    case 'W':
                        --currentX;
                        break;
                    default:
                        MessageBox.Show("Route Error! Not in set [NESW]");
                        break;
                }
                // MessageBox.Show("(" + currentX + "," + currentY + ") - " + route[idx]);
                paintRobot(currentX, currentY, A, new SolidBrush(Color.Black), 1);
            }
        }
        public Form1()
        {
            InitializeComponent();
            this.Width = 650; this.Height = 400;
            this.Text = baseText;

            Label W_label = new Label();
            W_label.Text = "宽";
            W_label.Width = 100;
            W_label.Left = 10;
            W_label.Top = 10;
            this.Controls.Add(W_label);
            W_input = new TextBox();
            W_input.Text = "430";
            W_input.Width = 100;
            W_input.Left = 10;
            W_input.Top = 30;
            this.Controls.Add(W_input);

            Label H_label = new Label();
            H_label.Text = "高";
            H_label.Width = 100;
            H_label.Left = 10;
            H_label.Top = 70;
            this.Controls.Add(H_label);
            H_input = new TextBox();
            H_input.Text = "300";
            H_input.Width = 100;
            H_input.Left = 10;
            H_input.Top = 90;
            this.Controls.Add(H_input);

            Label N_label = new Label();
            N_label.Text = "障碍物个数";
            N_label.Width = 100;
            N_label.Left = 10;
            N_label.Top = 130;
            this.Controls.Add(N_label);
            N_input = new TextBox();
            N_input.Text = "100";
            N_input.Width = 100;
            N_input.Left = 10;
            N_input.Top = 150;
            this.Controls.Add(N_input);

            Label B_label = new Label();
            B_label.Text = "障碍物大小";
            B_label.Width = 100;
            B_label.Left = 10;
            B_label.Top = 190;
            this.Controls.Add(B_label);
            B_input = new TextBox();
            B_input.Text = "7";
            B_input.Width = 100;
            B_input.Left = 10;
            B_input.Top = 210;
            this.Controls.Add(B_input);

            Label A_label = new Label();
            A_label.Text = "机器人大小";
            A_label.Width = 100;
            A_label.Left = 10;
            A_label.Top = 250;
            this.Controls.Add(A_label);
            A_input = new TextBox();
            A_input.Text = "10";
            A_input.Width = 100;
            A_input.Left = 10;
            A_input.Top = 270;
            this.Controls.Add(A_input);

            Button generator = new Button();
            generator.Text = "Generate ＆ Render Map";
            generator.Left = 10;
            generator.Top = 320;
            generator.Width = 150;
            generator.Click += new System.EventHandler(this.generateMap);
            this.Controls.Add(generator);

            Button render = new Button();
            render.Text = "Render Map";
            render.Left = baseLeft + 20;
            render.Top = 320;
            render.Width = 100;
            render.Click += new System.EventHandler(this.renderMap);
            this.Controls.Add(render);

            Button runner = new Button();
            runner.Text = "Run it!";
            runner.Left = baseLeft + 130;
            runner.Top = 320;
            runner.Width = 100;
            runner.Click += new System.EventHandler(this.runMap);
            this.Controls.Add(runner);

            tips = new Label();
            tips.Width = 400;
            tips.Left = baseLeft + 250;
            tips.Top = 325;
            tips.Text = baseText;
            this.Controls.Add(tips);
        }
    }
}
