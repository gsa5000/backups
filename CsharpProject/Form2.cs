using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpProject
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            label1.Text = "검색 항목";  
            label2.Text = "값 최소 범위";  
            label3.Text = "값 최대 범위";  
            label4.Text = "시작 인덱스";  
            label5.Text = "인덱스 범위";

            string[] combolist = new string[]{"Lot","Time","pH","Current","Voltage"};
            comboBox1.Items.AddRange(combolist);
            comboBox1.SelectedIndex = 0;
            button1.Click += delegate
            {
                check(out bool ret);
                if(ret)
                {
                    Application.OpenForms["Form2"].Close();
                }
                else
                {
                    MessageBox.Show("잘못된 양식입니다!");
                }
            };

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboBox1
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void check(out bool ret)
        {
            ret = true;
            bool[] checks = new bool[4];
            checks[0] = Char.IsNumber(textBox1.Text.Trim(), 0) && textBox1.Text != "" && textBox1.Text != null;
            checks[1] = Char.IsNumber(textBox2.Text.Trim(), 0) && textBox2.Text != "" && textBox2.Text != null;
            checks[2] = int.Parse(textBox2.Text.Trim()) >= int.Parse(textBox1.Text.Trim());
            checks[3] = Char.IsNumber(comboBox1.SelectedIndex.ToString(), 0) && comboBox1.Text != "";           
            if (checks.Contains(false))
            {
                ret = false;
            }
        }


    }
}
