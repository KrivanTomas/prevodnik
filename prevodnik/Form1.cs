using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace prevodnik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string binarymask = "00000000\\.00000000\\.00000000\\.00000000";
        string ipv4mask = "990\\.990\\.990\\.990";
        string ipv6mask = "aaaA\\:\\:aaaA\\:aaaA\\:aaaA\\:aaaA\\%90";

        private string decToBin(string dec)
        {
            int value = Convert.ToInt32(dec);
            int larg = 1;
            for(int i = 1; i < value; i *= 2)
            {
                larg = i;
            }
            string bin = "";
            if (value == 0) return "0";
            for (int i = larg; i >= 1; i /= 2)
            {
                bin += value / i == 1 ? "1" : "0";
                value %= i;
            }
            return bin;
        }
        private string binToDec(string bin)
        {
            string defBin = deFormatBin(bin);
            if(defBin.Length == 0) return "0";
            int larg = (int)Math.Pow(2, defBin.Length - 1);
            int dec = 0;
            foreach(char c in defBin)
            {
                dec += c == '1' ? larg : 0;
                larg /= 2;
            }
            return dec.ToString();
        }

        private string formatBin(string bin)
        {
            while (bin.Length < 8)
            {
                bin = "0" + bin;
            }
            return bin;
        }
        private string deFormatBin(string bin)
        {
            while (bin[0] == '0')
            {
                bin = bin.Substring(1,bin.Length - 1);
                if (bin.Length == 0) return "0";
            }
            return bin;
        }

        private string deEmpty(string value)
        {
            value = value.Replace(" ", "");
            if (value.Length == 0) return "0";
            return value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex) {
                case 0:
                    maskedTextBox1.Mask = binarymask;
                    break;
                case 1:
                    maskedTextBox1.Mask = ipv4mask;
                    break;
                case 2:
                    maskedTextBox1.Mask = ipv6mask;
                    break;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            string text = maskedTextBox1.Text;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    { // BIN
                        Console.WriteLine("a");
                        string[] split = text.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        split = split.Select(x => deEmpty(x)).ToArray();
                        label1.Text = String.Join(".", split.Select(x => binToDec(x)));
                        label2.Text = String.Join(".", split);
                        break;
                    }
                case 1:
                    { // DEC
                        string[] split = text.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        label2.Text = String.Join(".", split.Select(x => formatBin(decToBin(x))));

                        break;
                    }
                case 2:
                    { // HEX
                        string[] split = text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    }
            }
        }
    }
}