using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Security.Cryptography;

namespace CuCapuInNori
{
    public partial class Logare : Form
    {
        SqlConnection con = new SqlConnection(Date.con);
        SqlCommand cmd;
        bool valid = true;
        int see = 0;
        public Logare()
        {
            InitializeComponent();
        }

        public void verificaMail(string mail)
        {
            try
            {
                valid = true;
                MailAddress m = new MailAddress(mail);
            }
            catch (Exception ee)
            {
                Date.Error("Mailul nu este valid.");
                valid = false;
            }
        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (textBox1.Text == "" || textBox2.Text == "")
                    throw new Exception("Toate campurile trebuie completate.");
                verificaMail(textBox1.Text.ToString());
                int cate = 0;
                string pass = GetHashString(textBox2.Text.ToString());
                cmd = new SqlCommand("select count(*) from useri where email=@mail and parola=@pass", con);
                cmd.Parameters.AddWithValue("@mail", textBox1.Text.ToString());
                cmd.Parameters.AddWithValue("@pass",pass);

                cate = (int)cmd.ExecuteScalar();
                if (cate == 0)
                    throw new Exception("Email/parola gresite.");
                else
                {
                    cmd = new SqlCommand("select count(*) from useri where email=@mail and parola=@pass and isvalidated=1", con);
                    cmd.Parameters.AddWithValue("@mail", textBox1.Text.ToString());
                    cmd.Parameters.AddWithValue("@pass", pass);
                    int valid = (int)cmd.ExecuteScalar();
                    if (valid == 1)
                    {
                        cmd = new SqlCommand("update useri set isonline=1 where email=@email", con);
                        cmd.Parameters.AddWithValue("@email", textBox1.Text.ToString());
                        cmd.ExecuteNonQuery();
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        if (label4.Visible == false)
                        {
                            MessageBox.Show("Contul tau nu este activat. In momentul inregistrarii ai primit un cod de validare pe mail. Introdu-l in casuta de mai jos.");
                            textBox3.Visible = true;
                            label4.Visible = true;
                            panel6.Visible = true;
                        }
                        else if (textBox3.Text.ToString().Length < 4)
                        {
                            Date.Error("Introdu un cod valid de 4 cifre.");
                        }
                        else
                        {
                            cmd = new SqlCommand("select codvalidare from verificari where iduser = (select iduser from useri where email=@email)", con);
                            cmd.Parameters.AddWithValue("@email", textBox1.Text.ToString());
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if(dt.Rows[0][0].ToString() == textBox3.Text.ToString())
                            {
                                cmd = new SqlCommand("delete from verificari where iduser = (select iduser from useri where email=@email)",con);
                                cmd.Parameters.AddWithValue("@email", textBox1.Text.ToString());
                                cmd.ExecuteNonQuery();
                                cmd = new SqlCommand("update useri set isvalidated=1 where iduser = (select iduser from useri where email=@email)", con);
                                cmd.Parameters.AddWithValue("@email", textBox1.Text.ToString());
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Contul tau a fost validat cu succes!");
                                DialogResult = DialogResult.OK;
                                label4.Visible = false;
                                textBox3.Visible = false;
                                panel6.Visible = false;
                                this.Close();
                            }
                            else
                            {
                                Date.Error("Codul introdus este incorect.");
                            }
                        }
                    }
                }

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            catch(Exception ee)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                Date.Error(ee);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CreareCont().ShowDialog();
            Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void Logare_Load(object sender, EventArgs e)
        {
            label4.Visible = false;
            textBox3.Visible = false;
            panel6.Visible = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (see == 0)
            {
                see++;
                textBox2.PasswordChar = '\0';
                button4.BackgroundImage = Image.FromFile(Date.folder + "hide.png");
            }
            else
            {
                see--;
                textBox2.PasswordChar = '*';
                button4.BackgroundImage = Image.FromFile(Date.folder + "see.png");
            }

        }
    }
}
