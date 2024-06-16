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
using System.Net;
using System.Net.Mime;

namespace CuCapuInNori
{
    public partial class CreareCont : Form
    {
        SqlConnection con = new SqlConnection(Date.con);
        SqlCommand cmd;
        bool valid = true;
        Random r = new Random();
        int see = 0;
        int see2 = 0;
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

        public void sendMail(string subiect, string mesaj, string mailaddr,string cod)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("themathcoder04@gmail.com");
                mail.To.Add(mailaddr);
                mail.Subject = subiect;
                LinkedResource res = new LinkedResource(Date.folder + "logo.png",MediaTypeNames.Image.Jpeg);
                res.ContentId = Guid.NewGuid().ToString();
                string htmlBody =  @"<img src='cid:" + res.ContentId + @"'/>" + @"<h1>" + mesaj + @"</h1>" + @"<br />" + @"<h2> Iti multumim pentru ca ai ales aplicatia noastra. Te vei putea conecta cu contul tau dupa ce il activezi. Pentru a activa contul, logheaza-te cu datele tale si in casuta care apare, introdu codul de mai jos: </h2> <br /> <h1>" + cod + @"</h1>";
                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(res);
                mail.AlternateViews.Add(alternateView);
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("themathcoder04@gmail.com", "darw qcoe pvhg trgd");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
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
                                
                                cmd = new SqlCommand("insert into verificari (iduser,codvalidare) values((select iduser from useri where email=@email),@codvalidare)", con);
                                cmd.Parameters.AddWithValue("@email", textBox3.Text.ToString());
                                string cod = r.Next(1000, 9999).ToString();
                                cmd.Parameters.AddWithValue("@codvalidare", cod);
                                cmd.ExecuteNonQuery();
                                sendMail("Confirmare creare cont", "Salut, " + textBox1.Text.ToString() + "." + " Contul tau pe aplicatia CuCapuInNori a fost creat cu succes! ", textBox3.Text.ToString(),cod);
                                DialogResult = DialogResult.OK;
                                this.Close();
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

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (see == 0)
            {
                see++;
                textBox5.PasswordChar = '\0';
                button2.BackgroundImage = Image.FromFile(Date.folder + "hide.png");
            }
            else
            {
                see--;
                textBox5.PasswordChar = '*';
                button2.BackgroundImage = Image.FromFile(Date.folder + "see.png");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (see2 == 0)
            {
                see2++;
                textBox4.PasswordChar = '\0';
                button4.BackgroundImage = Image.FromFile(Date.folder + "hide.png");
            }
            else
            {
                see2--;
                textBox4.PasswordChar = '*';
                button4.BackgroundImage = Image.FromFile(Date.folder + "see.png");
            }
        }
    }




}
