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
    public partial class main_cons : Form
    {
        public main_cons()
        {
            InitializeComponent();
        }

        OleDbConnection SqlConnection;

        public string for_path = Application.ExecutablePath.Remove(Application.ExecutablePath.LastIndexOf("i") - 13);

        public string connectString;

        public int for_count = 0;

        private async void main_cons_Load(object sender, EventArgs e)
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

            button6.Enabled = false;
            button6.ForeColor = Color.Gray;

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

            OleDbCommand command1 = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            OleDbDataReader sqlReader1 = null;

            sqlReader1 = command1.ExecuteReader();

            int for_while = 1;

            while (await sqlReader1.ReadAsync())
            {
                if (Convert.ToString(sqlReader1["Код"]) == Convert.ToString(for_while))
                {
                    if (Convert.ToString(sqlReader1["Статус"]) == "Главный Консультант")
                    {
                        textBox4.Text = Convert.ToString(sqlReader1["E-mail"]);
                        textBox3.Text = Convert.ToString(sqlReader1["ФИО"]);
                        textBox6.Text = Convert.ToString(sqlReader1["Дата"]);
                        textBox7.Text = Convert.ToString(sqlReader1["Телефон"]);
                        textBox5.Text = Convert.ToString(sqlReader1["Тема"]);
                        textBox8.Text = Convert.ToString(sqlReader1["Текст"]);

                        textBox9.Text = Convert.ToString(sqlReader1["Код"]);

                        for_count = for_while;

                        break;
                    }
                }

                for_while++;
            }

            sqlReader1.Close();

            command1 = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            sqlReader1 = null;

            sqlReader1 = command1.ExecuteReader();

            while (await sqlReader1.ReadAsync())
            {
                if (Convert.ToString(sqlReader1["E-mail"]) == textBox4.Text)
                {
                    if (Convert.ToString(sqlReader1["Статус"]) == "Завершен")
                    {
                        button6.Enabled = true;
                        button6.ForeColor = Color.LimeGreen;
                    }
                }
            }

            if (textBox9.Text == "")
            {
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                textBox7.Visible = false;

                textBox8.Visible = false;

                button2.Visible = false;

                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;

                label11.Visible = true;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            login_screen open_form = new login_screen();
            open_form.Show();
            this.Hide();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            message_page a = new message_page();
            a.Text = this.Text;
            a.Tag = textBox9.Text;
            a.Show();
            this.Hide();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            if (textBox2.Text != "")
            {
                OleDbCommand command = new OleDbCommand("INSERT INTO [Чат] (Пользователь, Текст, Роль)VALUES(@login, @text, @rol)", SqlConnection);
                command.Parameters.AddWithValue("login", this.Text);
                command.Parameters.AddWithValue("text", textBox2.Text);
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

                main_cons a = new main_cons();
                a.Text = this.Text;
                a.Show();
                this.Hide();
            }
            else
            {
                textBox2.BackColor = Color.Peru;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.BackColor = Color.White;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            history_page o = new history_page();
            o.Tag = textBox4.Text;
            o.Text = this.Text;
            o.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            SqlConnection.Open();
            OleDbDataReader sqlReader = null;

            OleDbCommand command1 = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            OleDbDataReader sqlReader1 = null;

            sqlReader1 = command1.ExecuteReader();

            for_count++;

            while (sqlReader1.Read())
            {
                if (Convert.ToString(sqlReader1["Код"]) == Convert.ToString(for_count))
                {
                    if (Convert.ToString(sqlReader1["Статус"]) == "Главный Консультант")
                    {
                        textBox4.Text = Convert.ToString(sqlReader1["E-mail"]);
                        textBox3.Text = Convert.ToString(sqlReader1["ФИО"]);
                        textBox6.Text = Convert.ToString(sqlReader1["Дата"]);
                        textBox7.Text = Convert.ToString(sqlReader1["Телефон"]);
                        textBox5.Text = Convert.ToString(sqlReader1["Тема"]);
                        textBox8.Text = Convert.ToString(sqlReader1["Текст"]);

                        textBox9.Text = Convert.ToString(sqlReader1["Код"]);

                        return;
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            SqlConnection.Open();
            OleDbDataReader sqlReader = null;

            OleDbCommand command1 = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            OleDbDataReader sqlReader1 = null;

            sqlReader1 = command1.ExecuteReader();

            for_count--;

            while (sqlReader1.Read())
            {
                if (Convert.ToString(sqlReader1["Код"]) == Convert.ToString(for_count))
                {
                    if (Convert.ToString(sqlReader1["Статус"]) == "Главный Консультант")
                    {
                        textBox4.Text = Convert.ToString(sqlReader1["E-mail"]);
                        textBox3.Text = Convert.ToString(sqlReader1["ФИО"]);
                        textBox6.Text = Convert.ToString(sqlReader1["Дата"]);
                        textBox7.Text = Convert.ToString(sqlReader1["Телефон"]);
                        textBox5.Text = Convert.ToString(sqlReader1["Тема"]);
                        textBox8.Text = Convert.ToString(sqlReader1["Текст"]);

                        textBox9.Text = Convert.ToString(sqlReader1["Код"]);

                        return;
                    }
                }
            }
        }
    }
}
