using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpProject
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection();
        public Form1()
        {
            InitializeComponent();
            label1.Text = "현재시각 : " + DateTime.Now.ToString();
            button1.Click += delegate
            {               
                listBox1.Items.Add(DateTime.Now.ToString() + " / 데이터 조회" );
            };
            button2.Click += delegate
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " / 필터링 선택");
            };
            button3.Click += delegate
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " / 데이터 전체 조회");
            };

            button6.Click += delegate
            {              
                textBox2.Text = string.Empty;
                dateTimePicker1.Enabled = false;
            };

            button7.Click += delegate
            {
                textBox1.Text = string.Empty;
                dateTimePicker1.Enabled = true;
            };
            panel1.Enabled = false;
        }

        private void ConnectDB() 
        {
            conn.ConnectionString = string.Format("Data Source = ({0});" + "Initial Catalog = {1};" + "Integrated Security = {2};" + "Timeout = 3;", "local", "project", "SSPI");
            conn = new SqlConnection(conn.ConnectionString);
            conn.Open();
        }

        private void dbSelect()
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM sensor";

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "sensor");

                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "sensor";
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        private void dbInsert()
        {
            ConnectDB();

            try
            {
                string sqlcommand = "insert into sensor ([Index],Lot,Time,Temp,date,pH,[Current],Voltage) values (@parameter1,@parameter2,@parameter3,@parameter4,@parameter5,@parameter6,@parameter7,@parameter8)";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                int i = 0;
                for(i= 0;i<dataGridView1.RowCount;i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@parameter1", int.Parse(this.dataGridView1[0,i].Value.ToString())); // index
                    cmd.Parameters.AddWithValue("@parameter2", int.Parse(this.dataGridView1[1, i].Value.ToString())); // lot
                    cmd.Parameters.AddWithValue("@parameter3", this.dataGridView1[2,i].Value.ToString()); // time
                    cmd.Parameters.AddWithValue("@parameter4", this.dataGridView1[4,i].Value.ToString()); // Temp
                    cmd.Parameters.AddWithValue("@parameter5", dateTimePicker1.Text.ToString());          // Date
                    cmd.Parameters.AddWithValue("@parameter6", this.dataGridView1[3, i].Value.ToString()); // pH
                    cmd.Parameters.AddWithValue("@parameter7", this.dataGridView1[5, i].Value.ToString()); // current
                    cmd.Parameters.AddWithValue("@parameter8", this.dataGridView1[6,i].Value.ToString()); // voltage
                    cmd.CommandText = sqlcommand;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Data> datalist = new List<Data>();
            DataManager dm = new DataManager();
            DateTime dt = dateTimePicker1.Value;
            bool isokay = false;
            string dir = "";
            if(textBox1.Text.Trim() == "")
            {
                int[] ymd = new int[3];
                ymd[0] = dt.Year;
                ymd[1] = dt.Month;
                ymd[2] = dt.Day;
                dir = dm.getFileNameByFolder(ymd, textBox2.Text.ToString(), out isokay);
            }
            else if(textBox2.Text.Trim() == "")
            {
                dir = dm.getFileName(textBox1.Text, out isokay);
            }                  
            string howMuch =  numericUpDown1.Value.ToString();
            string fromWhere = numericUpDown2.Value.ToString();

            if(isokay)
            {
                datalist = dm.getData(dir).GetRange(int.Parse(fromWhere), int.Parse(howMuch)).ToList();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = datalist;
            }
            else
            {
                MessageBox.Show("Failed to find the requested data!");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "현재시각 : " + DateTime.Now.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 filter = new Form2();
            filter.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<Data> datalist = new List<Data>();
            DataManager dm = new DataManager();
            DateTime dt = dateTimePicker1.Value;
            bool isokay = false;
            string dir = "";
            if (textBox1.Text.Trim() == "")
            {
                int[] ymd = new int[3];
                ymd[0] = dt.Year;
                ymd[1] = dt.Month;
                ymd[2] = dt.Day;
                dir = dm.getFileNameByFolder(ymd, textBox2.Text.ToString(), out isokay);
            }
            else if (textBox2.Text.Trim() == "")
            {
                dir = dm.getFileName(textBox1.Text, out isokay);
            }
            if (isokay)
            {
                datalist = dm.getData(dir).ToList();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = datalist;
            }
            else
            {
                MessageBox.Show("Failed to find the requested data!");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            dbSelect();
            listBox1.Items.Add("데이터베이스 조회");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dbInsert();
            listBox1.Items.Add("데이터베이스에 저장");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ShowFileDailog(out string a, out string b);
            listBox1.Items.Add("파일 선택");
        }

        public string ShowFileDailog(out string fileName, out string fileFullName)
        {
            fileName = "";
            fileFullName = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "파일 선택하기";
            //ofd.FileName = "kemp";
            ofd.Filter = "csv 파일 (*.csv) | *.csv; | 모든 파일(*.*) | *.* ";
            DialogResult dr = ofd.ShowDialog();

            if(dr == DialogResult.OK)
            {
                fileName = ofd.SafeFileName;
                fileFullName = ofd.FileName;
                listBox1.Items.Add(fileName);
                listBox1.Items.Add(fileFullName);
                textBox1.Text = fileFullName;
                return fileFullName;
            }
            else
            {
                return "";
            }
        }

        public string ShowFolderDialog(out string folderFullName)
        {
            folderFullName = string.Empty;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if(dr == DialogResult.OK)
            {
                folderFullName = fbd.SelectedPath;
                listBox1.Items.Add(folderFullName);
                textBox2.Text = folderFullName;
                return folderFullName;
            }
            else
            {
                return "";
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            ShowFolderDialog(out string folderFullName);
            listBox1.Items.Add("폴더 선택");
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "" || textBox2.Text.Trim() != "")
            {
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim() != "" || textBox2.Text.Trim() != "")
            {
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
            }
        }
    }
}
