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
    public partial class login_screen : Form
    {
        OleDbConnection SqlConnection;

        public login_screen()
        {
            InitializeComponent();
        }

        public string for_path = Application.ExecutablePath.Remove(Application.ExecutablePath.LastIndexOf("i") - 13);

        public string connectString;

        private void Form1_Load(object sender, EventArgs e)
        {
            label6.Font = new Font("Bebas Neue", 18);

            label3.ForeColor = Color.FromArgb(78, 46, 144);

            label8.ForeColor = Color.FromArgb(255, 88, 0);
            label9.BackColor = Color.FromArgb(255, 88, 0);

            pictureBox1.Parent = label1;

            label2.Parent = label1;
            label3.Parent = label1;

            linkLabel1.Parent = label1;
            linkLabel3.Parent = label1;

            pictureBox2.ImageLocation = for_path +@"KONSULTANT+\Content\people.png";
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            timer1.Start();

            connectString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + for_path + "KONSULTANT+.mdb";
        }

        public int for_timer = 2;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (for_timer == 1)
            {
                label8.Location = new System.Drawing.Point(100,228);
                label8.Text = "Данное приложение создано для упрощения ведения записей о работе с клиентами, которым необходима юридическая поддержка";
                pictureBox2.ImageLocation = for_path + @"KONSULTANT+\Content\people.png";

                label9.BackColor = Color.FromArgb(255,88,0);
                label10.BackColor = Color.FromArgb(0, 0, 64);
                label11.BackColor = Color.FromArgb(0, 0, 64);

                for_timer++;
            }else if(for_timer == 2)
            {
                label8.Location = new System.Drawing.Point(100, 240);
                label8.Text = "Консультант плюс, компания занимающаяся юридической помощью и консультациями для самых разных жизненных случаев";
                pictureBox2.ImageLocation = for_path + @"KONSULTANT+\Content\bild.png";

                label9.BackColor = Color.FromArgb(0, 0, 64);
                label10.BackColor = Color.FromArgb(255, 88, 0);
                label11.BackColor = Color.FromArgb(0, 0, 64);

                for_timer++;
            }else if(for_timer == 3)
            {
                label8.Location = new System.Drawing.Point(98,260);
                label8.Text = "Это приложение было написано как дипломная работа Гусельникова Игоря";
                pictureBox2.ImageLocation = for_path + @"KONSULTANT+\Content\diplom.png";

                label9.BackColor = Color.FromArgb(0, 0, 64);
                label10.BackColor = Color.FromArgb(0, 0, 64);
                label11.BackColor = Color.FromArgb(255, 88, 0);

                for_timer -= 2;
            }

            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (textBox2.Text != "")
                {
                    SqlConnection = new OleDbConnection(connectString);
                    await SqlConnection.OpenAsync();
                    OleDbDataReader sqlReader = null;

                    OleDbCommand command = new OleDbCommand("SELECT * FROM [Пользователи]", SqlConnection);

                    try
                    {
                        sqlReader = command.ExecuteReader();

                        while (sqlReader.Read())
                        {
                            if((textBox1.Text == Convert.ToString(sqlReader["Логин"])) && (textBox2.Text == Convert.ToString(sqlReader["Пароль"]))){
                                if (Convert.ToString(sqlReader["Роль"]) == "Консультант")
                                {
                                    common_cons a = new common_cons();
                                    a.Show();
                                    a.Text = Convert.ToString(sqlReader["Логин"]);
                                    this.Hide();
                                }

                                if (Convert.ToString(sqlReader["Роль"]) == "Главный Консультант")
                                {
                                    main_cons a = new main_cons();
                                    a.Show();
                                    a.Text = Convert.ToString(sqlReader["Логин"]);
                                    this.Hide();
                                }

                                if (Convert.ToString(sqlReader["Роль"]) == "Администратор")
                                {
                                    admin_panel a = new admin_panel();
                                    a.Show();
                                    a.Text = Convert.ToString(sqlReader["Логин"]);
                                    this.Hide();
                                }

                                if (Convert.ToString(sqlReader["Роль"]) == "Работа с клиентом")
                                {
                                    client_work_page a = new client_work_page();
                                    a.Show();
                                    a.Text = Convert.ToString(sqlReader["Логин"]);
                                    this.Hide();
                                }
                            }

                            label12.Visible = true;
                            textBox1.BackColor = Color.Peru;
                            textBox2.BackColor = Color.Peru;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }else
                {
                    textBox2.BackColor = Color.Peru;
                }
            }else
            {
                if (textBox2.Text == "")
                {
                    textBox1.BackColor = Color.Peru;
                    textBox2.BackColor = Color.Peru;
                }else
                {
                    textBox1.BackColor = Color.Peru;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label12.Visible = false;

            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label12.Visible = false;

            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
        }
    }
}
