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
    public partial class CreareCont : Form
    {
        SqlConnection con = new SqlConnection(Date.con);
        SqlCommand cmd;
        bool valid = true;
        public CreareCont()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void CreareCont_Load(object sender, EventArgs e)
        {

        }

        public void verificaMail(string mail)
        {
            try
            {
                valid = true;
                MailAddress m = new MailAddress(mail);
            }
            catch(Exception ee)
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

                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "")
                    throw new Exception("Toate campurile trebuie completate.");
                verificaMail(textBox3.ToString());
                int cate = 0;
                cmd = new SqlCommand("select count(*) from useri where email=@email", con);
                cmd.Parameters.AddWithValue("@email", textBox3.Text.ToString());
                cate = (int)cmd.ExecuteScalar();
                if (cate != 0)
                    throw new Exception("Mailul este deja inregistrat in baza de date.");
                else
                {
                    cmd = new SqlCommand("select count(*) from useri where nrtel=@nrtel", con);
                    cmd.Parameters.AddWithValue("@nrtel", textBox6.Text.ToString());
                    cate = (int)cmd.ExecuteScalar();
                    if (cate != 0)
                        throw new Exception("Numarul de telefon este deja inregistrat in baza de date.");
                    else
                    {
                        if (textBox5.Text != textBox4.Text)
                            throw new Exception("Parolele nu coincid!");
                        else
                        {
                            if (textBox5.Text.ToString().Length < 8)
                                throw new Exception("Parola trebuie sa aiba minim 8 caractere");
                            else
                            {
                                string parola = GetHashString(textBox5.Text.ToString());
                                cmd = new SqlCommand("insert into useri(email,parola,nrtel,adresa,nume,prenume)values(@em,@par,@nr,@adr,@num,@pre)", con);
                                cmd.Parameters.AddWithValue("@em", textBox3.Text.ToString());
                                cmd.Parameters.AddWithValue("@par", parola);
                                cmd.Parameters.AddWithValue("@nr", textBox6.Text.ToString());
                                cmd.Parameters.AddWithValue("@num", textBox1.Text.ToString());
                                cmd.Parameters.AddWithValue("@pre", textBox2.Text.ToString());
                                cmd.Parameters.AddWithValue("@adr", textBox7.Text.ToString());
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                }
            }
            catch (Exception ee)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                Date.Error(ee);
            }
        }
    }




}
