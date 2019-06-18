using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.Data.OleDb;

namespace KONSULTANT_
{
    public partial class message_page : Form
    {
        OleDbConnection SqlConnection;

        public message_page()
        {
            InitializeComponent();
        }

        public string for_path = Application.ExecutablePath.Remove(Application.ExecutablePath.LastIndexOf("i") - 13);

        public string connectString;

        public string user_name;

        private async void button2_Click(object sender, EventArgs e)
        {
            if(textBox8.Text == "")
            {
                MessageBox.Show("С начала введите ответ");
                return;
            }

            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            OleDbCommand command = new OleDbCommand("SELECT * FROM [Пользователи]", SqlConnection);

            sqlReader = command.ExecuteReader();

            while (sqlReader.Read())
            {
                if (Convert.ToString(sqlReader["Логин"]) == this.Text)
                {
                    user_name = Convert.ToString(sqlReader["ФИО"]);
                }
            }

            MailAddress fromMailAddres = new MailAddress("dmitriy.vylkov1@gmail.com", "Консультант: " + user_name);
            MailAddress toAddress = new MailAddress(textBox6.Text, textBox5.Text);

            try
            {

                using (MailMessage mailMessage = new MailMessage(fromMailAddres, toAddress))
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    mailMessage.Subject = "KONSULTANT+ : " + textBox4.Text;
                    mailMessage.Body = textBox8.Text;

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromMailAddres.Address, "lbvf6939bigzbinewqaz");

                    smtpClient.Send(mailMessage);
                }

                command = new OleDbCommand("UPDATE [Клиенты] SET [Статус]=@status, [Ответ]=@otv WHERE [Код]=@pri", SqlConnection);
                command.Parameters.AddWithValue("status", "Завершен");
                command.Parameters.AddWithValue("otv", textBox8.Text);
                command.Parameters.AddWithValue("pri", Convert.ToString(this.Tag));
                await command.ExecuteNonQueryAsync();
                await command.ExecuteNonQueryAsync();

                MessageBox.Show("Ответ успешно отправлен");

                button1_Click(sender, e);
            }
            catch
            {
                MessageBox.Show("Отсутствует подключение к интернету");
            }

        }

        private async void message_page_Load(object sender, EventArgs e)
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
                            textBox2.Text += "Вы" + ": " + Convert.ToString(sqlReader["Текст"]) + "\r\n" + "\r\n";
                        }
                        else
                        {
                            textBox2.Text += Convert.ToString(sqlReader["Пользователь"]) + " (" + Convert.ToString(sqlReader["Роль"]) + "): " + Convert.ToString(sqlReader["Текст"]) + "\r\n" + "\r\n";
                        }

                        break;
                    }

                    for_id++;
                }
            }

            textBox2.SelectionStart = textBox1.TextLength;
            textBox2.ScrollToCaret();

            sqlReader.Close();

            command = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            OleDbDataReader sqlReader1 = null;

            sqlReader1 = command.ExecuteReader();

            while (await sqlReader1.ReadAsync())
            {
                if (Convert.ToString(sqlReader1["Код"]) == Convert.ToString(this.Tag))
                {
                    textBox6.Text = Convert.ToString(sqlReader1["E-mail"]);
                    textBox5.Text = Convert.ToString(sqlReader1["ФИО"]);
                    textBox4.Text = Convert.ToString(sqlReader1["Тема"]);
                    textBox1.Text = Convert.ToString(sqlReader1["Текст"]);
                }

            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            OleDbCommand command = new OleDbCommand("SELECT * FROM [Пользователи]", SqlConnection);

            sqlReader = command.ExecuteReader();

            while (sqlReader.Read())
            {
                if (Convert.ToString(sqlReader["Логин"]) == this.Text)
                {
                    if (Convert.ToString(sqlReader["Роль"]) == "Консультант")
                    {
                        common_cons a = new common_cons();
                        a.Show();
                        a.Text = this.Text;
                        this.Hide();
                    }

                    if (Convert.ToString(sqlReader["Роль"]) == "Главный Консультант")
                    {
                        main_cons a = new main_cons();
                        a.Show();
                        a.Text = this.Text;
                        this.Hide();
                    }
                }
            }
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
                OleDbCommand command = new OleDbCommand("INSERT INTO [Чат] (Пользователь, Текст, Роль)VALUES(@login, @text, @rol)", SqlConnection);
                command.Parameters.AddWithValue("login", this.Text);
                command.Parameters.AddWithValue("text", textBox3.Text);
                command.Parameters.AddWithValue("rol", "Консультант");
                await command.ExecuteNonQueryAsync();

                command = new OleDbCommand("SELECT * FROM [Чат]", SqlConnection);

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
                                textBox2.Text += "Вы" + ": " + Convert.ToString(sqlReader["Текст"]) + "\r\n" + "\r\n";
                            }
                            else
                            {
                                textBox2.Text += Convert.ToString(sqlReader["Пользователь"]) + " (" + Convert.ToString(sqlReader["Роль"]) + "): " + Convert.ToString(sqlReader["Текст"]) + "\r\n" + "\r\n";
                            }

                            break;
                        }

                        for_id++;
                    }
                }

                message_page a = new message_page();
                a.Text = this.Text;
                a.Tag = this.Tag;
                a.Show();
                this.Hide();
            }
            else
            {
                textBox3.BackColor = Color.Peru;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.BackColor = Color.White;
        }
    }
}
