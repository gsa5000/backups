using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CsharpProject
{
    public partial class Form4 : Form
    {
        private string currentTime;
        private DataReader reader;
        public Main m1;

        public Form4(Main form)
        {
            InitializeComponent();
           reader= new DataReader();
            m1 = form;
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            List<string> csvFilePaths = GetCSVFilePaths();

            // 콤보박스에 날짜 리스트 할당
            comboBox1.DataSource = csvFilePaths;
            comboBox1.DisplayMember = "FileName";
        }
    

        private void back_Click(object sender, EventArgs e)
        {          
            /*Form2 form2= new Form2();
                 this.Hide(); 
            form2.ShowDialog();*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(panel1.BackColor);

            // 현재 시간을 그래픽으로 표시
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd  hh:mm:ss");
            g.DrawString(currentTime, Font, Brushes.Black, 10, 10);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFileName = comboBox1.SelectedItem.ToString();

            // 선택한 파일 이름을 날짜로 변환
            DateTime selectedDate = DateTime.ParseExact(selectedFileName, "yyyy.MM.dd", null);

            // 선택한 날짜에 해당하는 CSV 파일 경로 가져오기
            string filePath = GetCSVFilePath(selectedDate);

            // 선택한 날짜의 CSV 파일 데이터 가져오기
            List<Data> dataList = reader.readDataFile(filePath);

            // CSV 파일의 모든 시간 데이터 가져오기
            List<string> timeList = dataList.Select(data => SplitTime(data.Time)).SelectMany(times => times).Distinct().ToList();

            // 그리드뷰에 데이터 바인딩
            dataGridView1.DataSource = dataList;

            // 콤보박스2에 시간 데이터 바인딩
            comboBox2.DataSource = timeList;
        }

        

        private List<string> GetCSVFilePaths()
        {
            DateTime startDate = new DateTime(2021, 9, 6);
            DateTime endDate = new DateTime(2021, 10, 27);

            List<string> fileNames = new List<string>();

            DateTime currentDate = startDate;
            while (currentDate <= endDate)
            {
                string fileName = currentDate.ToString("yyyy.MM.dd"); // 파일 이름으로 사용할 날짜 포맷
                string filePath = GetCSVFilePath(currentDate);

                if (File.Exists(filePath))
                {
                    fileNames.Add(fileName);
                }
                // 다음 날짜로 이동 (한 날짜씩 증가)
                currentDate = currentDate.AddDays(1);
            }
            return fileNames;
        }

        private string GetCSVFilePath(DateTime date)
        {
/*            string folderPath ="C:\\Users\\KB\\Desktop\\c#\\project\\project\\bin\\Debug";
            string fileName = $"kemp-abh-sensor-{date:yyyy.MM.dd}.csv";
            string filePath = Path.Combine(folderPath, fileName);*/           
            return m1.FileDir;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private List<string> SplitTime(string time)
        {
            string[] timeParts = time.Split(' '); // 시간과 분을 분리
            string[] hourMinute = timeParts[1].Split(':'); // 시간과 분을 분리

            string hour = hourMinute[0]; // 시간
            string minute = hourMinute[1]; // 분

            // 초 제거
            if (minute.Contains('.'))
                minute = minute.Substring(0, minute.IndexOf('.'));

            string formattedTime = $"오후 {hour}:{minute}"; // 오후와 시간 분을 하나로 합침

            return new List<string> { formattedTime };
        }
    }
}
