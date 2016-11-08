using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
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
            for(int i = 0; i < bs.Length; i += 2)
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
            int max = 32*10000;

            if (max > bs.Length) {

                max = bs.Length;
            }

            //200x100サイズのImageオブジェクトを作成する
            Bitmap img = new Bitmap(256,  max/32*2);
            int k = 0;
            for (int i = 0; i < max; i=i+2)
            {
                f = bs[i];
                f2 = bs[i+1];
                len = i >> 1;
                a = ((len/16)*16)%256;
                b = (k % 16)+(len / 256)*16;
                k++;
                if (k == 16) { k = 0; }
                if ((f & 1)!=0){ 
                img.SetPixel(7 + a, b, c);
                }
                if ((f & 2) != 0)
                {
                    img.SetPixel(6 + a,  b, c);
                }
                if ((f & 4) != 0)
                {
                    img.SetPixel(5 + a,  b, c);
                }
                if ((f & 8) != 0)
                {
                    img.SetPixel(4 + a,   b, c);
                }
                if ((f & 16) != 0)
                {
                    img.SetPixel(3 + a, b, c);
                }
                if ((f & 32) != 0)
                {
                    img.SetPixel(2 + a,   b, c);
                }
                if ((f & 64) != 0)
                {
                    img.SetPixel(1 + a,   b, c);
                }
                if ((f & 128) != 0)
                {
                    img.SetPixel(0 + a,   b, c);
                }



                if ((f2 & 1) != 0)
                {
                    img.SetPixel(15 + a,   b, c);
                }
                if ((f2 & 2) != 0)
                {
                    img.SetPixel(14 + a,  b, c);
                }
                if ((f2 & 4) != 0)
                {
                    img.SetPixel( 13 + a,  b, c);
                }
                if ((f2 & 8) != 0)
                {
                    img.SetPixel( 12 + a,  b, c);
                }
                if ((f2 & 16) != 0)
                {
                    img.SetPixel( 11 + a,  b, c);
                }
                if ((f2 & 32) != 0)
                {
                    img.SetPixel( 10 + a,  b, c);
                }
                if ((f2 & 64) != 0)
                {
                    img.SetPixel( 9 + a,  b, c);
                }
                if ((f2 & 128) != 0)
                {
                    img.SetPixel( 8 + a,  b, c);
                }

            }

            //作成した画像を表示する
            pictureBox1.Image = img;
            img.Save("FONT.bmp", ImageFormat.Bmp);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
             ofd.Filter = "FONTX2ファイル(*.FNT;*.TLF;*.BIN)|*.FNT;*.TLF;*.BIN|すべてのファイル(*.*)|*.*";
            ofd.Title = "開くファイルを選択してください";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {



                System.IO.FileStream fs = new System.IO.FileStream( ofd.FileName, System.IO.FileMode.Open,System.IO.FileAccess.Read);
                byte[] bs = new byte[18];
                fs.Read(bs, 0, 18);

                string fontx2 = Encoding.GetEncoding(932).GetString(bs).Substring(0,6);
                if (fontx2.Contains("FONTX2") == false) {
                    MessageBox.Show("fontx2以外のフォントは変換できません");
                    return;
                }
                if (bs[14] != 16 || bs[15] != 16) {
                    MessageBox.Show("16x16以外のフォントは変換できません");
                    return;
                }
                if (bs[16] != 1) {

                    MessageBox.Show("SHIFT-JIS以外のフォントは変換できません");
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

                Array.Resize(ref gohan, 268736 - 0x5f0-8);//267,216 ばいと



                System.IO.FileStream ffs = new System.IO.FileStream("FS.bin", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                //バイト型配列の内容をすべて書き込む
                ffs.Write(gohan, 0, gohan.Length);
                ffs.Write(n, 0, 8);
                //閉じる
                ffs.Close();
                

                return;
            }

        }
    }
}
