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
using Xceed.Words.NET;

namespace KONSULTANT_
{
    using Word = Microsoft.Office.Interop.Word;
    public partial class history_page : Form
    {
        OleDbConnection SqlConnection;

        public history_page()
        {
            InitializeComponent();
        }

        public string for_path = System.Windows.Forms.Application.ExecutablePath.Remove(System.Windows.Forms.Application.ExecutablePath.LastIndexOf("i") - 13);

        public string connectString;

        private async void history_page_Load(object sender, EventArgs e)
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

            OleDbCommand command1 = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            OleDbDataReader sqlReader1 = null;

            sqlReader1 = command1.ExecuteReader();

            while (await sqlReader1.ReadAsync())
            {
                if (Convert.ToString(sqlReader1["E-mail"]) == Convert.ToString(this.Tag))
                {
                    if (Convert.ToString(sqlReader1["Статус"]) == "Завершен")
                    {
                        comboBox5.Items.Add(Convert.ToString(sqlReader1["Тема"]));
                    }
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
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

        private async void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection = new OleDbConnection(connectString);
            await SqlConnection.OpenAsync();
            OleDbDataReader sqlReader = null;

            OleDbCommand command = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            sqlReader = command.ExecuteReader();

            while (await sqlReader.ReadAsync())
            {
                if (comboBox5.Text == Convert.ToString(sqlReader["Тема"]))
                {
                    textBox10.Text = Convert.ToString(sqlReader["Дата"]);
                    textBox7.Text = Convert.ToString(sqlReader["Телефон"]);
                    textBox11.Text = Convert.ToString(sqlReader["E-mail"]);
                    textBox12.Text = Convert.ToString(sqlReader["ФИО"]);
                    textBox8.Text = Convert.ToString(sqlReader["Текст"]);
                    textBox4.Text = Convert.ToString(sqlReader["Ответ"]);
                }
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

                history_page a = new history_page();
                a.Text = this.Text;
                a.Tag = this.Tag;
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

        private async void Button3_Click(object sender, EventArgs e)
        {
            OleDbCommand command1 = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

            OleDbDataReader sqlReader1 = null;

            sqlReader1 = command1.ExecuteReader();

            while (await sqlReader1.ReadAsync())
            {
                if (Convert.ToString(sqlReader1["E-mail"]) == Convert.ToString(this.Tag))
                {
                    if (Convert.ToString(sqlReader1["Статус"]) == "Завершен")
                    {
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                            return;

                        string filename = saveFileDialog1.FileName;
                        string pathDocument = filename + ".docx";
                        DocX document = DocX.Create(pathDocument);

                        document.InsertParagraph("История клиента " + Convert.ToString(sqlReader1["ФИО"])).
                                 FontSize(24).
                                 Bold().
                                 Alignment = Alignment.center;

                        Paragraph paragraph = document.InsertParagraph();
                        paragraph.Alignment = Alignment.right;

                        SqlConnection = new OleDbConnection(connectString);
                        await SqlConnection.OpenAsync();
                        OleDbDataReader sqlReader = null;

                        OleDbCommand command = new OleDbCommand("SELECT * FROM [Клиенты]", SqlConnection);

                        sqlReader = command.ExecuteReader();

                        int for_for = 0;

                        try
                        {

                            while (await sqlReader.ReadAsync())
                            {
                                if (comboBox5.Items[for_for].ToString() == Convert.ToString(sqlReader["Тема"]))
                                {
                                    document.InsertParagraph("Дата: " + Convert.ToString(sqlReader["Дата"]));
                                    document.InsertParagraph("Телефон: " + Convert.ToString(sqlReader["Телефон"]));
                                    document.InsertParagraph("E-mail: " + Convert.ToString(sqlReader["E-mail"]));
                                    document.InsertParagraph("Текст: " + Convert.ToString(sqlReader["Текст"]));
                                    document.InsertParagraph("Ответ: " + Convert.ToString(sqlReader["Ответ"]));
                                    document.InsertParagraph("");
                                    for_for++;
                                }
                            }

                            document.Save();

                            return;
                        }
                        catch
                        {
                            document.Save();
                        }
                    }
                }
            }

            
        }
    }
}
