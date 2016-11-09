using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace WindowsFormsApplication3
{


    public partial class Form1 : Form
    {
        string last_font = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
    "gbk.txt",
    System.IO.FileMode.Open,
    System.IO.FileAccess.Read);
            //ファイルを読み込むバイト型配列を作成する
            byte[] bs = new byte[fs.Length];
            //ファイルの内容をすべて読み込む
            fs.Read(bs, 0, bs.Length);
            //閉じる
            fs.Close();
            StringBuilder sb = new StringBuilder();

            byte[] cp = new byte[2];
            for (int i = 0; i < bs.Length; i += 2)
            {
                cp[0] = bs[i];
                cp[1] = bs[i + 1];

                sb.Append("0x");
                sb.Append(cp[0].ToString("X2"));
                sb.Append(cp[1].ToString("X2"));
                sb.Append("\t");
                sb.AppendLine(Encoding.GetEncoding(936).GetString(cp));




            }

            textBox1.Text = sb.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            System.IO.FileStream fs = new System.IO.FileStream(
    "fs.bin",
    System.IO.FileMode.Open,
    System.IO.FileAccess.Read);
            //ファイルを読み込むバイト型配列を作成する
            byte[] bs = new byte[fs.Length];
            //ファイルの内容をすべて読み込む
            fs.Read(bs, 0, bs.Length);
            //閉じる
            fs.Close();



            //ランダムな色の点をランダムな位置にうちまくる
            Color c = Color.FromArgb(0, 0, 0);
            int f = 0;
            int f2 = 0;
            int len = 0;
            int a = 0;
            int b = 0;
            int max = 32 * 10000;

            if (max > bs.Length)
            {

                max = bs.Length;
            }

            //200x100サイズのImageオブジェクトを作成する
            Bitmap img = new Bitmap(256, max / 32 * 2);
            int k = 0;
            for (int i = 0; i < max; i = i + 2)
            {
                f = bs[i];
                f2 = bs[i + 1];
                len = i >> 1;
                a = ((len / 16) * 16) % 256;
                b = (k % 16) + (len / 256) * 16;
                k++;
                if (k == 16) { k = 0; }
                if ((f & 1) != 0)
                {
                    img.SetPixel(7 + a, b, c);
                }
                if ((f & 2) != 0)
                {
                    img.SetPixel(6 + a, b, c);
                }
                if ((f & 4) != 0)
                {
                    img.SetPixel(5 + a, b, c);
                }
                if ((f & 8) != 0)
                {
                    img.SetPixel(4 + a, b, c);
                }
                if ((f & 16) != 0)
                {
                    img.SetPixel(3 + a, b, c);
                }
                if ((f & 32) != 0)
                {
                    img.SetPixel(2 + a, b, c);
                }
                if ((f & 64) != 0)
                {
                    img.SetPixel(1 + a, b, c);
                }
                if ((f & 128) != 0)
                {
                    img.SetPixel(0 + a, b, c);
                }



                if ((f2 & 1) != 0)
                {
                    img.SetPixel(15 + a, b, c);
                }
                if ((f2 & 2) != 0)
                {
                    img.SetPixel(14 + a, b, c);
                }
                if ((f2 & 4) != 0)
                {
                    img.SetPixel(13 + a, b, c);
                }
                if ((f2 & 8) != 0)
                {
                    img.SetPixel(12 + a, b, c);
                }
                if ((f2 & 16) != 0)
                {
                    img.SetPixel(11 + a, b, c);
                }
                if ((f2 & 32) != 0)
                {
                    img.SetPixel(10 + a, b, c);
                }
                if ((f2 & 64) != 0)
                {
                    img.SetPixel(9 + a, b, c);
                }
                if ((f2 & 128) != 0)
                {
                    img.SetPixel(8 + a, b, c);
                }

            }

            //作成した画像を表示する
            pictureBox1.Image = img;
            img.Save("FONT.bmp", ImageFormat.Bmp);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "FONTX2ファイル(*.FNT;*.TLF;)|*.FNT;*.TLF|すべてのファイル(*.*)|*.*";
            ofd.Title = "開くファイルを選択してください";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {



                System.IO.FileStream fs = new System.IO.FileStream(ofd.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] bs = new byte[18];
                fs.Read(bs, 0, 18);

                string fontx2 = Encoding.GetEncoding(932).GetString(bs).Substring(0, 6);
                if (fontx2.Contains("FONTX2") == false)
                {
                    MessageBox.Show("fontx2以外のフォントは変換できません");
                    return;
                }
                if ((bs[14] != 8 || bs[14] != 16) && bs[15] != 16)
                {
                    MessageBox.Show("8x16,16x16以外のフォントは変換できません");
                    return;
                }
                if (bs[16] > 1)
                {

                    MessageBox.Show("ASCII,SHIFT-JIS以外のフォントは変換できません");
                    return;
                }

                if (bs[14] == 8 && bs[16] == 0)
                {
                    byte[] asciis = new byte[1520 * 2];
                    fs.Read(asciis, 0, 1520 * 2);

                    System.IO.FileStream afs = new System.IO.FileStream("ASCII.bin", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    //バイト型配列の内容をすべて書き込む
                    afs.Write(asciis, 32 * 16 - 1, 1520);
                    //閉じる
                    afs.Close();
                    fs.Close();
                    MessageBox.Show("半角フォントASCII.binが作成されました");

                    //3C 42 A5 81 A5 99 42 3C から　- 0x610,

                    return;
                }

                int encode = bs[16];
                int tablelen = bs[17];

                byte[] tbl = new byte[tablelen * 4];
                fs.Read(tbl, 0, tbl.Length);
                byte[] font = new byte[fs.Length - 18 - tablelen * 4];
                fs.Read(font, 0, font.Length);


                fs.Close();

                int[] cp = new int[font.Length / 32];

                int big = 0;
                int big2 = 0;
                int ct = 0;
                for (var i = 0; i < tbl.Length; i = i + 4)
                {
                    big = tbl[i] + tbl[i + 1] * 256;
                    big2 = tbl[i + 2] + tbl[i + 3] * 256;
                    for (var k = 0; big + k <= big2; k++)
                    {
                        cp[ct] = big + k;
                        ct++;
                    }

                }
                int pos = 0;
                byte[] sjis = new byte[2];
                byte[] euc = new byte[2];
                byte[] gohan = new byte[font.Length];
                string s = "";

                for (var i = 0; i < cp.Length; i++)
                {
                    sjis[0] = Convert.ToByte(cp[i] / 256);
                    sjis[1] = Convert.ToByte(cp[i] & 255);
                    s = Encoding.GetEncoding(932).GetString(sjis);
                    euc = Encoding.GetEncoding(51932).GetBytes(s);
                    pos = (euc[0] - 0xa1) * 94 * 32 + (euc[1] - 0xa1) * 32;
                    if (pos > 0 && pos < font.Length - 32)
                    {
                        Array.ConstrainedCopy(font, i * 32, gohan, pos, 32);
                    }
                }
                byte[] n = new byte[8];

                Array.Resize(ref gohan, 268736 - 0x5f0 - 8-8);//267,208 ばいと



                System.IO.FileStream ffs = new System.IO.FileStream("FS.bin", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                //バイト型配列の内容をすべて書き込む
                ffs.Write(gohan, 0, gohan.Length);
                ffs.Write(n, 0, 8);
                //閉じる
                ffs.Close();
                MessageBox.Show("全角EUC-JPフォント FS.bin が作成されました、 bmpみるでプレビューできます");
                last_font = Encoding.GetEncoding(932).GetString(bs).Substring(6, 8).Trim();

                return;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "suprxファイル(*.suprx)|*.suprx|すべてのファイル(*.*)|*.*";
            ofd.Title = "開くファイルを選択してください";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                
                System.IO.FileStream fs = new System.IO.FileStream(ofd.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                fs.Close();
                int patchpos = 0;
                for (int i = 0; i < bs.Length; i++) {
                    if (bs[i] == 0x3C) {
                        //3C 42 A5 81 A5 99 42 3C から　- 0x610,
                        if(bs[i+1]==0x42 && bs[i + 2] == 0xa5 && bs[i + 3] == 0x81 && bs[i + 4] == 0xa5 && bs[i + 5] == 0x99 && bs[i + 6] == 0x42 && bs[i + 7] == 0x3c)
                        {
                            patchpos = i;
                            break;


                        }
                    }
                }
                if (patchpos == 0) {
                    MessageBox.Show("パッチ箇所が見つかりませんでした");
                }

               string exe= System.Windows.Forms.Application.StartupPath;

                if (File.Exists(CombinePaths(exe,"ASCII.bin"))==true) {

                    System.IO.FileStream afs = new System.IO.FileStream("ASCII.bin", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    byte[] ascii = new byte[afs.Length];
                    afs.Read(ascii, 0, ascii.Length);
                    afs.Close();
                    Array.ConstrainedCopy(ascii, 0, bs, patchpos - 0x610, ascii.Length);
                }

                if (File.Exists(CombinePaths(exe, "FS.bin"))==true)
                {
                    System.IO.FileStream zfs = new System.IO.FileStream("FS.bin", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    byte[] zenkaku = new byte[zfs.Length];
                    zfs.Read(zenkaku, 0, zenkaku.Length);
                    zfs.Close();
                    Array.ConstrainedCopy( zenkaku, 0, bs, patchpos - 0x610 - 267216+8, zenkaku.Length);                    
                }



                System.IO.FileStream ffs = new System.IO.FileStream(last_font+".suprx", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                ffs.Write(bs, 0, bs.Length);
                ffs.Close();
                MessageBox.Show("全角EUC-JPフォントがぱっちされました");



            }
        }


        public static string CombinePaths(string path1, string path2)
        {
            path1 = path1.TrimEnd(System.IO.Path.DirectorySeparatorChar);
            path2 = path2.TrimStart(System.IO.Path.DirectorySeparatorChar);
            return path1 + System.IO.Path.DirectorySeparatorChar + path2;
        }
    }
}
