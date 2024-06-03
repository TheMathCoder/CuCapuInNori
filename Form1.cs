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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace CuCapuInNori
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();
        SqlConnection con = new SqlConnection(Date.con);
        SqlCommand cmd;
        int click = 0;
        int x1=0, x2=0, y1=0, y2=0;
        public Form1()
        {
            InitializeComponent();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd = new SqlCommand("truncate table useri", con);
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            catch (Exception ee)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                Date.Error(ee);
            }
            tabControl1.Appearance = TabAppearance.FlatButtons; tabControl1.ItemSize = new Size(0, 1); tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.SelectedIndex = 0;
        }

/*
        public class ApiResponse
        {
            public List<FlightDestination> Data { get; set; }
        }
*/
        private void Form1_Load(object sender, EventArgs e)
        {

            pictureBox1.Image = Image.FromFile(Date.folder + "icon.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Logare().ShowDialog();
            Show();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.ForeColor = Color.FromArgb(141, 236, 180);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.ForeColor = Color.FromArgb(65, 176, 110);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Logare().ShowDialog();
            Show();
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            x1 = e.X;
            y1 = e.Y;
            click = 1;

        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            click = 0;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (click == 1)
            {
                this.Location = new Point(this.Location.X - (x1 - e.X), this.Location.Y - (y1 - e.Y));
            }
        }


        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            x1 = e.X;
            y1 = e.Y;
            click = 1;
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            click = 0;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (click == 1)
            {
                this.Location = new Point(this.Location.X - (x1 - e.X), this.Location.Y - (y1 - e.Y));
            }

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
        
        private async void button6_Click_1Async(object sender, EventArgs e)
        {/*
            try
            {
                string url = "https://test.api.amadeus.com/v1/shopping/flight-destinations?origin=PAR&maxPrice=200";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "TerkqTNFoJ5NO0Nqzs2fHoeDCkDb");

                var response = await client.GetStringAsync(url);
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);

                foreach (var flight in apiResponse.Data)
                {
                    Console.WriteLine($"Type: {flight.Type}, Origin: {flight.Origin}, Destination: {flight.Destination}");
                    Console.WriteLine($"Departure Date: {flight.DepartureDate}, Return Date: {flight.ReturnDate}");
                    Console.WriteLine($"Price: {flight.Price.Total}");
                    Console.WriteLine();
                }
            }
            catch(Exception ee)
            {
                Date.Error(ee);
            }*/
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button7_ClickAsync(object sender, EventArgs e)
        {/*
            try
            {
                string url = "https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode=" + textBox1.Text.ToString().ToUpper() + "&destinationLocationCode=" + textBox2.Text.ToString().ToUpper() + "&departureDate=2024-11-01&adults=1&max=2";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "TerkqTNFoJ5NO0Nqzs2fHoeDCkDb");

                var response = await client.GetStringAsync(url);
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                int i = 1;
                foreach (var flight in apiResponse.Data)
                {
                    dataGridView1.Rows.Add(i++,flight.Price);
                }
            }
            catch (Exception ee)
            {
                Date.Error(ee);
            }*/
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }



    public static class Date
    {
        public static string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|cucapuinnori.mdf;Integrated Security=True;Connect Timeout=30";
        public static string folder = Application.StartupPath + "\\resurse\\";


        public static void Error(Exception ee)
        {
            MessageBox.Show(ee.Message.ToString(), "Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Error(Object ee)
        {
            MessageBox.Show(ee.ToString(), "Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
