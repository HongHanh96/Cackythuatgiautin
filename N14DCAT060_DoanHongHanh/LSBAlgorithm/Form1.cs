using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LSBAlgorithm
{
    public partial class LSBAlgorithm : Form
    {

        OpenFileDialog ofd = new OpenFileDialog();
        SaveFileDialog sfd = new SaveFileDialog();
        Bitmap encode_img, decode_img;

        public LSBAlgorithm()
        {
            InitializeComponent();
        }

        private void encodeOpenFile_Click(object sender, EventArgs e)
        {
            ofd.Filter = " Image Files (*.png; *.bmg) | *.png; *.bmg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                encode_img = new Bitmap(ofd.FileName);
                encodeImage1.Image = encode_img;
                encodeInputImage.Text = ofd.FileName;
            }
        }

        private void Encode_Click(object sender, EventArgs e)
        {
            if (encode_img == null || encodeMessage.Text == "")
                return;
            string msg = string.Empty;
            foreach (char ch in encodeMessage.Text)
                msg += Convert.ToString((int)ch, 2).PadLeft(8, '0');
            int z = 0;
            for (int i = 0; i < encode_img.Width; i++)
                for (int j = 0; j < encode_img.Height; j++)
                {
                    Color pxl = encode_img.GetPixel(i, j);
                    encode_img.SetPixel(i, j, Color.FromArgb(change8thbit(pxl.R, msg, ref z), change8thbit(pxl.G, msg, ref z), change8thbit(pxl.B, msg, ref z)));
                }
            encodeImage2.Image = encode_img;
            sfd.Filter = " Image Files (*.png; *.bmg) | *.png; *.bmg";
            if (sfd.ShowDialog() == DialogResult.OK)
                encode_img.Save(sfd.FileName);
        }

        private byte change8thbit(byte b, string msg, ref int z)
        {
            string tmp = Convert.ToString(b, 2).PadLeft(8, '0');
            if (z < msg.Length)
            {
                tmp = tmp.Substring(0, 7) + msg.Substring(z, 1);
                z++;
            }
            else
                tmp = tmp.Substring(0, 7) + "0";
            return Convert.ToByte(tmp, 2);
        }

        private void decodeOpenFile_Click(object sender, EventArgs e)
        {
            ofd.Filter = " Image Files (*.png; *.jpg) | *.png; *.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                decode_img = new Bitmap(ofd.FileName);
                decodeImage.Image = decode_img;
                decodeInputImage.Text = ofd.FileName;
            }
        }

        private void decode_Click(object sender, EventArgs e)
        {
            if (decode_img == null)
                return;
            string result = string.Empty, tmp = string.Empty;
            for (int i = 0; i < decode_img.Width; i++)
                for (int j = 0; j < decode_img.Height; j++)
                {
                    Color pxl = decode_img.GetPixel(i, j);
                    tmp += get8thbit(pxl.R) + get8thbit(pxl.G) + get8thbit(pxl.B);
                    if (tmp.Length >= 8)
                    {
                        string t = tmp.Substring(0, 8);
                        if (t != "00000000")
                            result += Convert.ToChar(Convert.ToByte(t, 2));
                        else
                        {
                            decodeMessage.Text = result;
                            return;
                        }
                        tmp = tmp.Substring(8, tmp.Length - 8);
                    }
                }
            decodeMessage.Text = result;
        }

        private void LSBAlgorithm_Load(object sender, EventArgs e)
        {

        }

        private void encodeTab_Click(object sender, EventArgs e)
        {

        }

        private string get8thbit(byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8,'0').Substring(7, 1);
        }
    }
}
