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
                    DialogResult = DialogResult.OK;
                    this.Close();
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

        }
    }
}
