using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class txtSplit : Form
    {
        private string newFilePath;
        private StreamReader file;
        private Regex regex;
        private int fileLength;
        public txtSplit()
        {
            InitializeComponent();
        }

        private void txtSplit_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请添加正则表达式");
                return;
            }
            button2.Enabled = false;
            button1.Enabled = false;
            progressBar1.Value = 0;
            label1.Text = "切割中";
            regex = new Regex(textBox1.Text);
            string line;
            string text = "";
            string newFileName;
            int counter = 0;
            int lineCounter = 0;
            try
            {
                if (!Directory.Exists(newFilePath)) Directory.CreateDirectory(newFilePath);
                while ((line = file.ReadLine()) != null)
                {
                    lineCounter++;
                    progressBar1.Value = lineCounter * 100 / fileLength;
                    if (regex.IsMatch(line))
                    {
                        Console.WriteLine(line);
                        if (counter == 0)
                        {
                            text = $"{text}\n{line.Trim()}";
                            counter++;
                            continue;
                        }
                        newFileName = Path.Combine(newFilePath, $"{counter}.txt");
                        File.WriteAllText(newFileName, text);
                        text = "";
                        counter++;
                    }
                    text = $"{text}\n{line.Trim()}";
                }
                newFileName = Path.Combine(newFilePath, $"{counter}.txt");
                File.WriteAllText(newFileName, text);
                file.Close();
                label1.Text = $"切割完毕 - 共生成{counter}个文件";
                progressBar1.Value = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"出错了！{ex.Message}");
                label1.Text = "切割失败";
            }
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文件(*.txt)|*.txt";
            openFileDialog.Title = "请选择文件";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file = new StreamReader(openFileDialog.OpenFile());
                button1.Enabled = true;
                newFilePath = Path.Combine(Path.GetDirectoryName(openFileDialog.FileName), "out");
                label1.Text = openFileDialog.FileName;
                fileLength = File.ReadLines(openFileDialog.FileName).Count();
            }
        }
    }
}
