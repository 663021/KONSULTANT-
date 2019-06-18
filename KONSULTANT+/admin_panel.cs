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
    public partial class admin_panel : Form
    {
        OleDbConnection SqlConnection;

        public admin_panel()
        {
            InitializeComponent();
        }

        public string for_path = Application.ExecutablePath.Remove(Application.ExecutablePath.LastIndexOf("i") - 13);

        public string connectString;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            login_screen open_form = new login_screen();
            open_form.Show();
            this.Hide();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            if (textBox4.Text != "")
            {
                if (textBox3.Text != "")
                {
                    if (comboBox1.Text != "")
                    {
                        OleDbCommand command = new OleDbCommand("INSERT INTO [Пользователи] (Логин, Пароль, ФИО, Роль)VALUES(@login, @password, @fio, @rol)", SqlConnection);
                        command.Parameters.AddWithValue("login", textBox4.Text);
                        command.Parameters.AddWithValue("password", textBox3.Text);
                        command.Parameters.AddWithValue("fio", textBox13.Text);
                        command.Parameters.AddWithValue("rol", comboBox1.Text);
                        await command.ExecuteNonQueryAsync();

                        MessageBox.Show("Пользователь был успешно добавлен!");

                        admin_panel a = new admin_panel();
                        a.Text = this.Text;
                        a.Show();
                        this.Hide();

                        return;
                    }
                    else
                    {
                        comboBox1.BackColor = Color.Peru;
                    }
                }
                else
                {
                    textBox3.BackColor = Color.Peru;
                }
            }
            else
            {
                textBox4.BackColor = Color.Peru;
            }

            MessageBox.Show("Все поля должны бать заполены");
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.BackColor = Color.White;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.BackColor = Color.White;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = Color.White;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            login_screen open_form = new login_screen();
            open_form.Show();
            this.Hide();
        }

        private async void admin_panel_Load(object sender, EventArgs e)
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

            textBox1.ScrollToCaret();

            sqlReader.Close();

            command = new OleDbCommand("SELECT * FROM [Пользователи]", SqlConnection);

            sqlReader = command.ExecuteReader();

            while (await sqlReader.ReadAsync())
            {
                comboBox3.Items.Add(Convert.ToString(sqlReader["Логин"]));
                comboBox4.Items.Add(Convert.ToString(sqlReader["Логин"]));
            }

            sqlReader.Close();

            command = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            sqlReader = command.ExecuteReader();

            while (await sqlReader.ReadAsync())
            {
                comboBox5.Items.Add(Convert.ToString(sqlReader["Телефон"]));
            }
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

                admin_panel a = new admin_panel();
                a.Text = this.Text;
                a.Show();
                this.Hide();
            }
            else
            {
                textBox2.BackColor = Color.Peru;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.BackColor = Color.White;
        }

        private async void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            OleDbCommand command = new OleDbCommand("SELECT * FROM [Пользователи]", SqlConnection);

            sqlReader = command.ExecuteReader();

            while (await sqlReader.ReadAsync())
            {
                if (comboBox3.Text == Convert.ToString(sqlReader["Логин"]))
                {
                    textBox5.Text = Convert.ToString(sqlReader["Логин"]);
                    textBox6.Text = Convert.ToString(sqlReader["Пароль"]);
                    comboBox2.Text = Convert.ToString(sqlReader["Роль"]);
                }
            }

            if(comboBox2.Text == "admin")
            {
                comboBox2.Text = "Администратор";
            }else if (comboBox2.Text == "main_cons")
            {
                comboBox2.Text = "Главный Консультант";
            }
            else if (comboBox2.Text == "common_cons")
            {
                comboBox2.Text = "Консультант";
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (textBox5.Text != "")
            {
                if (textBox6.Text != "")
                {
                    if (comboBox2.Text != "")
                    {
                        SqlConnection = new OleDbConnection(connectString);
                        await SqlConnection.OpenAsync();
                        OleDbDataReader sqlReader = null;

                        OleDbCommand command = new OleDbCommand("UPDATE [Пользователи] SET [Логин]=@login, [Пароль]=@pass, [Роль]=@rol WHERE [Логин]=@log", SqlConnection);
                        command.Parameters.AddWithValue("login", textBox5.Text);
                        command.Parameters.AddWithValue("pass", textBox6.Text);
                        command.Parameters.AddWithValue("rol", comboBox2.Text);
                        command.Parameters.AddWithValue("log", comboBox3.Text);
                        await command.ExecuteNonQueryAsync();
                        await command.ExecuteNonQueryAsync();

                        MessageBox.Show("Данные пользователя успешно изменены");

                        admin_panel open_form = new admin_panel();
                        open_form.Text = this.Text;
                        open_form.Show();
                        this.Hide();
                    }
                    else
                    {
                        comboBox2.BackColor = Color.Peru;
                    }
                }
                else
                {
                    textBox6.BackColor = Color.Peru;
                }
            }
            else
            {
                textBox5.BackColor = Color.Peru;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox5.BackColor = Color.White;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBox6.BackColor = Color.White;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.BackColor = Color.White;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            OleDbCommand command = new OleDbCommand("DELETE * FROM [Пользователи] WHERE [Логин]=@log", SqlConnection);
            command.Parameters.AddWithValue("log", comboBox4.Text);
            await command.ExecuteNonQueryAsync();

            MessageBox.Show("Пользователь успешно удален");

            admin_panel open_form = new admin_panel();
            open_form.Text = this.Text;
            open_form.Show();
            this.Hide();
        }

        private async void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            OleDbCommand command = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            sqlReader = command.ExecuteReader();

            while (await sqlReader.ReadAsync())
            {
                if (comboBox5.Text == Convert.ToString(sqlReader["Телефон"]))
                {
                    textBox10.Text = Convert.ToString(sqlReader["Дата"]);
                    textBox7.Text = Convert.ToString(sqlReader["Телефон"]);
                    textBox9.Text = Convert.ToString(sqlReader["Тема"]);
                    textBox11.Text = Convert.ToString(sqlReader["E-mail"]);
                    textBox12.Text = Convert.ToString(sqlReader["ФИО"]);
                    textBox8.Text = Convert.ToString(sqlReader["Текст"]);
                    comboBox6.Text = Convert.ToString(sqlReader["Статус"]);
                }
            }
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            if (comboBox6.Text != "")
            {
                SqlConnection = new OleDbConnection(connectString);
                await SqlConnection.OpenAsync();
                OleDbDataReader sqlReader = null;

                OleDbCommand command = new OleDbCommand("UPDATE [Клиенты] SET [Статус]=@status WHERE [Телефон]=@num", SqlConnection);
                command.Parameters.AddWithValue("status", comboBox6.Text);
                command.Parameters.AddWithValue("num", comboBox5.Text);
                await command.ExecuteNonQueryAsync();
                await command.ExecuteNonQueryAsync();

                MessageBox.Show("Статус вопроса успешно изменен");

                admin_panel open_form = new admin_panel();
                open_form.Text = this.Text;
                open_form.Show();
                this.Hide();
            }else
            {
                comboBox6.BackColor = Color.Peru;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox6.BackColor = Color.White;
        }
    }
}
