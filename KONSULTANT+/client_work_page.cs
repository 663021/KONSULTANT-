using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace KONSULTANT_
{
    public partial class client_work_page : Form
    {
        OleDbConnection SqlConnection;

        public string for_path = Application.ExecutablePath.Remove(Application.ExecutablePath.LastIndexOf("i") - 13);

        public string connectString;

        public client_work_page()
        {
            InitializeComponent();
        }

        private async void client_work_page_Load(object sender, EventArgs e)
        {
            connectString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + for_path + "KONSULTANT+.mdb";

            pictureBox1.Parent = label1;

            label2.Parent = label1;
            label3.Parent = label1;

            linkLabel1.Parent = label1;
            linkLabel3.Parent = label1;

            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            OleDbCommand command = new OleDbCommand("SELECT * FROM [Чат]", SqlConnection);

            sqlReader = command.ExecuteReader();

            while (sqlReader.Read())
            {
                int for_id = 0;

                while (true)
                {
                    if (for_id == Convert.ToInt32(sqlReader["Код"]))
                    {
                        if (this.Text == Convert.ToString(sqlReader["Пользователь"]))
                        {
                            textBox1.Text += "Вы" + ": " + Convert.ToString(sqlReader["Текст"]) + "\r\n" + "\r\n";
                        }
                        else
                        {
                            textBox1.Text += Convert.ToString(sqlReader["Пользователь"]) + " (" + Convert.ToString(sqlReader["Роль"]) + "): " + Convert.ToString(sqlReader["Текст"]) + "\r\n" + "\r\n";
                        }

                        break;
                    }

                    for_id++;
                }
            }

            textBox1.SelectionStart = textBox1.TextLength;
            textBox1.ScrollToCaret();

            sqlReader.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            login_screen open_form = new login_screen();
            open_form.Show();
            this.Hide();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            if (textBox3.Text != "")
            {
                if (textBox4.Text != "")
                {
                    if (textBox5.Text != "")
                    {
                        if (textBox6.Text != "")
                        {
                            if (textBox7.Text != "")
                            {
                                if (textBox8.Text != "")
                                {
                                    OleDbCommand command = new OleDbCommand("INSERT INTO [Клиенты] (ФИО, [E-mail], Дата, Телефон, Тема, Текст, Статус)VALUES(@fio, @mail, @data, @num, @theme, @text, @stat)", SqlConnection);
                                    command.Parameters.AddWithValue("fio", textBox3.Text);
                                    command.Parameters.AddWithValue("mail", textBox4.Text);
                                    command.Parameters.AddWithValue("data", textBox6.Text);
                                    command.Parameters.AddWithValue("num", textBox7.Text);
                                    command.Parameters.AddWithValue("theme", textBox5.Text);
                                    command.Parameters.AddWithValue("text", textBox8.Text);
                                    command.Parameters.AddWithValue("stat", "Консультант");
                                    await command.ExecuteNonQueryAsync();

                                    MessageBox.Show("Вопрос клиента был успешно добавлен!");

                                    client_work_page a = new client_work_page();
                                    a.Text = this.Text;
                                    a.Show();
                                    this.Hide();

                                    return;
                                }
                            }
                        }
                    }
                }
            }

            MessageBox.Show("Все поля должны бать заполены");
        }
    }
}
