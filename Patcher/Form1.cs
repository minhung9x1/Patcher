using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Patcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.strPath;
            textBox2.Text = Properties.Settings.Default.strHex;
            textBox3.Text = Properties.Settings.Default.strReplace;
            textBox4.Text = Properties.Settings.Default.strTitle;
        }
        public  byte[] GetStringToBytes(string value)
        {
            SoapHexBinary shb = SoapHexBinary.Parse(value);
            return shb.Value;
        }
        public  string GetBytesToString(byte[] value)
        {
            SoapHexBinary shb = new SoapHexBinary(value);
            return shb.ToString();
        }
        private  void StringBuilderReplace(StringBuilder data, Dictionary<string,string> values)
        {
            foreach (string k in values.Keys)
            {
              var x =  data.Replace(k, values[k]);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.strPath = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.strHex = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] strSplit1 = textBox2.Text.Replace(" ", "").Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] strSplit2 = textBox3.Text.Replace(" ", "").Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var values = new Dictionary<string, string>();
            for(int i = 0; i < strSplit1.Length; i++)
            {
                values.Add(strSplit1[i], strSplit2[i]);
            }
            if (Patcher(textBox1.Text, values))
            {
                MessageBox.Show("Successful");
            }
            else
            {
                MessageBox.Show("Failed patch");
            }
        }
        private bool Patcher(string pathfile,Dictionary<string,string> values)
        {
            try
            {
                byte[] arr = File.ReadAllBytes(pathfile);
                StringBuilder sb = new StringBuilder(GetBytesToString(arr));  
                StringBuilderReplace(sb, values);
                arr = GetStringToBytes(sb.ToString());
                using (var fs = new FileStream(pathfile, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(arr, 0, arr.Length);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }         
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.strReplace = textBox3.Text;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!File.Exists(textBox1.Text + ".bak"))
            {
                File.Copy(textBox1.Text, textBox1.Text + ".bak");
                MessageBox.Show("Create backup success");
            }
            else
            {
                MessageBox.Show("Created");
            }
        
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            this.Text = "Patcher 2017 - " + textBox4.Text;
            Properties.Settings.Default.strTitle = textBox4.Text;
            Properties.Settings.Default.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\DownloadManager");
            ////storing the values  
            //key.SetValue("FName", "IDM VIP");
            //key.SetValue("LName", "Admin");
            //key.SetValue("Email", "your@email.com");
            //key.SetValue("Serial", "9QNBL-L2641-Y7WVE-QEN3I");
            //key.Close();
        }
    }
}
