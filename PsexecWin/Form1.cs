using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.IO;
using System.Net;

namespace PsexecWin
{
    public partial class Form1 : Form
    {
        private const string FILE_NAME = "ComboBoxUPdate.txt";
        private const string V = "\\";
        private string ipps;
        private bool keyerr = true;
        public Form1()
        {
            InitializeComponent();
            CreateFile(FILE_NAME);
            UpdatedCombobox();       
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckIp(Convert.ToString(comboBox1.Text)) == true)
            {
                if ((Testping(Convert.ToString(comboBox1.Text))) == true)
                {
                    connect(Convert.ToString(comboBox1.Text));
                }
                else MessageBox.Show(" Удаленный ip адрес не доступен!!! \n Проверьте правильность написания ip адреса \n Проверьте подключение к сети", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            else
            {
                DNS_show(Convert.ToString(comboBox1.Text));
                if (keyerr == true)
                {
                    if ((Testping(Convert.ToString(ipps))) == true)
                    {
                        if (CheckIp(Convert.ToString(ipps)) == true)
                        {
                            connect(ipps);
                        }
                        else MessageBox.Show(" ip не найден ", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else MessageBox.Show(" Удаленный ip адрес не доступен!!! \n Проверьте правильность написания ip адреса \n Проверьте подключение к сети", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else MessageBox.Show(" ip не найден ", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private bool Testping(string x)
        {
                Ping png = new Ping();
                PingReply pingReply = png.Send(x);
                if (pingReply.Status == IPStatus.Success)
                return true;
                else return false;
           
        }
        static bool CheckIp(string address)
        {
            var nums = address.Split('.');
            int useless;
            return nums.Length == 4 && nums.All(n => int.TryParse(n, out useless)) &&
                   nums.Select(int.Parse).All(n => n < 256);
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данное приложение разработано для удобства использования другого приложения 'Psexec.exe'\nЗапускает командную строку удаленного компьютера  \n \n Версия 1.1 \n 1. Исправлена ошибка, неизвестные имена \n 2. Появилась обработка нажатия клавиши “Enter” \n \n Предложение, баги и т.д. пишите. TokarevML@nornik.ru  ", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
        private void UpdatedCombobox()
        {
            using (StreamReader sr = File.OpenText(FILE_NAME))
            {
                String input;
                while ((input = sr.ReadLine()) != null)
                {
                    comboBox1.Items.Add(input);
                    comboBox1.Text = comboBox1.Items[0].ToString();
                }              
                sr.Close();
            }
           
        }
        private void SaveComboBoxText ()
        {
            if (ScanFile() == true)
            {
                using (StreamWriter sr = File.AppendText(FILE_NAME))
                {
                    sr.WriteLine(ReadComboBox());
                    sr.Close();
                }
                UpdatedCombobox();
            }

        }
        private string ReadComboBox ()
        {
            return Convert.ToString(comboBox1.Text);
        }
        private bool ScanFile ()
        {
            StreamReader sr = new StreamReader(FILE_NAME);
            while (!sr.EndOfStream)
            {
                string st = sr.ReadLine();
                if (st.StartsWith(comboBox1.Text))
                {
                    listBox1.Items.Add(Convert.ToString(st));
                    return false;
                }
            }
            sr.Close();
            return true;
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void CreateFile(string curFile)
        {
            if (!File.Exists(curFile))
            {
                FileStream st = File.Create(curFile);
                st.Close();
            }
        }
        private void DNS_show (string nameps)
        { 
            try
            {
                IPHostEntry host2 = Dns.GetHostEntry(nameps);
                foreach (IPAddress ip in host2.AddressList)
                    ipps = ip.ToString();
            }
            catch (Exception t)
            {
                keyerr = false;
                return;
            }
            keyerr = true;
        }
        private void connect(string ipcon)
        {
            string ip = Convert.ToString(V + V + ipcon);
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.WorkingDirectory = @"%CD%";
            psi.Arguments = "/k " + "psexec.exe " + ip + " cmd.exe";
            try
            {
                Process.Start(psi);
            }

            catch (Exception t)
            {
                MessageBox.Show("Ошибка " + t, "сообщение", MessageBoxButtons.OK);
            }
            SaveComboBoxText();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Пока настроек нет.\n Условия работы программы: \n 1. Нахождение файла psexec.exe в папке с программой\n 2. Иметь учетную запись администратора на удаленном ПК", "Настройки", MessageBoxButtons.OK);
        }
    }
}
