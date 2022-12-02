using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace prevodnik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            maskedTextBox1.Text = "";
            label1.Text = "0.0.0.0";
            label2.Text = "0000000.0000000.0000000.00000000";
            label3.Text = "00.00.00.00";
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

        private string binToHex(string bin)
        {
            string hex = "";
            while(bin.Length != 0)
            {
                switch(Convert.ToInt32(binToDec(bin.Substring(0, bin.Length < 4 ? bin.Length : 4)))){
                    case int i when (i < 10):
                        {
                            hex += i.ToString();
                            break;
                        }
                    case 10:
                        hex += "a";
                        break;
                    case 11:
                        hex += "b";
                        break;
                    case 12:
                        hex += "c";
                        break;
                    case 13:
                        hex += "d";
                        break;
                    case 14:
                        hex += "e";
                        break;
                    case 15:
                        hex += "f";
                        break;
                }
                bin = bin.Remove(0, bin.Length < 4 ? bin.Length : 4);
            }
            return hex;
        }

        private string hexToBin(string hex)
        {
            string bin = "";
            foreach(char c in hex)
            {
                switch ((int)c)
                {
                    case int i when (i >= 48 && i <= 57):
                        {
                            bin += formatBin(decToBin((i - 48).ToString()), 4);
                            break;
                        }
                    case int i when (i >= 97 && i <= 102):
                        {
                            bin += formatBin(decToBin((i - 87).ToString()), 4);
                            break;
                        }
                }
            }
            return bin;
        }

        private string formatBin(string bin)
        {
            while (bin.Length < 8)
            {
                bin = "0" + bin;
            }
            return bin;
        }

        private string formatBin(string bin, int length)
        {
            while (bin.Length < length)
            {
                bin = "0" + bin;
            }
            return bin;
        }

        private string deFormatBin(string bin)
        {
            if (bin.Length == 0) return "0";
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
            maskedTextBox1.Text = "";
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
                        if (Regex.IsMatch(text, @"[^01\. ]")) label8.Visible = true;
                        else label8.Visible = false;
                        string[] split = text.Split(new char[] { '.' });
                        split = split.Select(x => deEmpty(x)).ToArray();
                        label1.Text = String.Join(".", split.Select(x => binToDec(x)));
                        label2.Text = String.Join(".", split.Select(x => formatBin(x)));
                        label3.Text = String.Join(".", split.Select(x => binToHex(x)));
                        break;
                    }
                case 1:
                    { // DEC
                        string[] split = text.Split(new char[] { '.' });
                        if(split.Any(x => Convert.ToInt32(deEmpty(x)) > 255)) label8.Visible = true;
                        else label8.Visible = false;
                        label1.Text = String.Join(".", split.Select(x => formatBin(decToBin(deEmpty(x)))));
                        label2.Text = String.Join(".", split.Select(x => deEmpty(x)));
                        label3.Text = String.Join(".", split.Select(x => binToHex(formatBin(decToBin(deEmpty(x))))));
                        break;
                    }
                case 2:
                    { // HEX
                        if (Regex.IsMatch(text, @"[^0-9a-f\.:% ]")) label8.Visible = true;
                        else label8.Visible = false;
                        string[] split = text.Split(new char[] { ':', '%' });
                        string end = split[6];
                        split = split.Take(6).ToArray();
                        //split[5] = Regex.Replace(split[5], @"%\w+", "");
                        Regex rgx = new Regex(@":\w+:");
                        label1.Text = rgx.Replace(String.Join(":", split.Select(x => formatBin(hexToBin(deEmpty(x)), 16))), "::", 1) + "%" + deEmpty(end);
                        label2.Text = rgx.Replace(String.Join(":", split.Select(x => binToDec(hexToBin(deEmpty(x))))), "::", 1) + "%" + deEmpty(end);
                        label3.Text = rgx.Replace(String.Join(":", split.Select(x => deEmpty(x))), "::", 1) + "%" + deEmpty(end);
                        break;
                    }
            }
        }
    }
}