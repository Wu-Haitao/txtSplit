using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class txtSplit : Form
    {
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

            button3.Enabled = false;
            button2.Enabled = false;
            button1.Enabled = false;
            progressBar1.Value = 0;

            Task.Factory.StartNew(() =>
             {
                 SplitHandler splitHandler = new SplitHandler(this,
                     textBox1.Text,
                     label1.Text,
                     Path.Combine(Path.GetDirectoryName(label1.Text), "out"),
                     button3.Text);
                 int counter = splitHandler.Split();
                 BeginInvoke(new Action(() =>
                 {
                     if (counter != -1)
                     {
                         label1.Text = $"切割完毕 - 共生成{counter}个文件";
                     }
                     else
                     {
                         MessageBox.Show($"出错了！");
                         label1.Text = "切割失败";
                     }
                     button2.Enabled = true;
                     button3.Enabled = true;
                 }));
             });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文件(*.txt)|*.txt";
            openFileDialog.Title = "请选择文件";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = true;
                label1.Text = openFileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = (button3.Text == "UTF-8") ? "GB2312" : "UTF-8";
        }
    }
}
