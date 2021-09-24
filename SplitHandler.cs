using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public class SplitHandler
    {
        private txtSplit form;
        private Regex regex;
        private string orginFilePath; //with file name
        private string outputFilePath; //without file name
        private Encoding encoding;

        public SplitHandler(txtSplit form, string regex, string orginFilePath, string outputFilePath, string encoding)
        {
            this.form = form;
            this.regex = new Regex(regex);
            this.orginFilePath = orginFilePath;
            this.outputFilePath = outputFilePath;
            this.encoding = Encoding.GetEncoding(encoding);
        }

        public int Split()
        {
            StreamReader file;
            string line;
            int totalLineNum;
            string text = "";
            int counter = 0;
            int lineCounter = 0;
            try
            {
                file = new StreamReader(orginFilePath, encoding);
                totalLineNum = File.ReadLines(orginFilePath).Count();
                if (!Directory.Exists(outputFilePath)) Directory.CreateDirectory(outputFilePath);
                while ((line = file.ReadLine()) != null)
                {
                    lineCounter++;
                    form.BeginInvoke(new Action(() => form.progressBar1.Value = lineCounter * 100 / totalLineNum));
                    if (regex.IsMatch(line))
                    {
                        Console.WriteLine(line);
                        if (counter == 0)
                        {
                            text = $"{text}\n{line.Trim()}";
                            counter++;
                            continue;
                        }
                        File.WriteAllText(Path.Combine(outputFilePath, $"{counter}.txt"), text);
                        text = "";
                        counter++;
                    }
                    text = $"{text}\n{line.Trim()}";
                }
                File.WriteAllText(Path.Combine(outputFilePath, $"{counter}.txt"), text);
                file.Close();
                form.BeginInvoke(new Action(() => form.progressBar1.Value = 100));
            }
            catch (Exception)
            {
                return -1;
            }
            return counter;
        }
    }
}
