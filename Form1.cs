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
using Newtonsoft.Json.Linq;

namespace CuCapuInNori
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();
        SqlConnection con = new SqlConnection(Date.con);
        SqlCommand cmd;
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        int click = 0;
        int x1=0, x2=0, y1=0, y2=0;
        string token = "";

        public async void getToken()
        {
            string url = "https://test.api.amadeus.com/v1/security/oauth2/token";
            var formData = new Dictionary<string, string>
            {
            { "grant_type", "client_credentials" },
            { "client_id", "lRZyM7Bb4OjzNe2unkOIWRm2RqW9AfAR" },
            { "client_secret", "SkSuQ45ImcdYJI2S" }
             };
            string rasp;

            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));


                HttpContent content = new FormUrlEncodedContent(formData);


                HttpResponseMessage response = await client.PostAsync(url, content);


                response.EnsureSuccessStatusCode();


                string responseContent = await response.Content.ReadAsStringAsync();
               
                var jsonObject = JObject.Parse(responseContent);
                rasp = jsonObject["access_token"].ToString();
            }

            token = rasp;
        }


        public class Price
        {
            public string Total { get; set; }
        }

        public class FlightDestination
        {
            public string Type { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string DepartureDate { get; set; }
            public string ReturnDate { get; set; }
            public Price Price { get; set; }
        }

        public class ApiResponse
        {
            public List<FlightDestination> Data { get; set; }
        }


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
            getToken();

        }


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
            //salut
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 1)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd = new SqlCommand("select taraAeroport from aeroporturi group by taraAeroport order by 1 asc ", con);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                    comboBox3.Items.Add(dt.Rows[i][0].ToString());
                for (int i = 0; i < dt.Rows.Count; i++)
                    comboBox4.Items.Add(dt.Rows[i][0].ToString());
                if (comboBox3.SelectedItem == "")
                    comboBox1.Enabled = false;
                for (int i = 0; i < 7; i++)
                    comboBox5.Items.Add(i);
                for (int i = 0; i < 7; i++)
                    comboBox6.Items.Add(i);

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.SelectedItem = null;
            comboBox1.Text = "";
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd = new SqlCommand("select codaeroport,orasaeroport from aeroporturi where taraaeroport=@tara", con);
                cmd.Parameters.AddWithValue("@tara", comboBox3.Items[comboBox3.SelectedIndex].ToString());
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                    comboBox1.Items.Add(dt.Rows[i][1] + " (" + dt.Rows[i][0] + ")");

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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.SelectedItem = null;
            comboBox2.Text = "";
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd = new SqlCommand("select codaeroport,orasaeroport from aeroporturi where taraaeroport=@tara", con);
                cmd.Parameters.AddWithValue("@tara", comboBox4.Items[comboBox4.SelectedIndex].ToString());
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                    comboBox2.Items.Add(dt.Rows[i][1] + " (" + dt.Rows[i][0] + ")");

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            catch (Exception ee)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                Date.Error(ee);

            }
        }

        private async void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                double lastprice=999999;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                dataGridView1.Rows.Clear();

                if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null && comboBox3.SelectedItem != null && comboBox4.SelectedItem != null)
                {
                    if (comboBox1.SelectedItem.ToString() == comboBox2.SelectedItem.ToString())
                        throw new Exception("Aeroportul de plecare si de sosire nu pot sa coincida");
                    else
                    {
                        string plecare = comboBox1.SelectedItem.ToString();
                        string[] plec = plecare.Split('(');
                        plec[1] = plec[1].Trim(')');
                        MessageBox.Show(plec[1]);
                        string sosire = comboBox2.SelectedItem.ToString();
                        string[] sos = sosire.Split('(');
                        sos[1] = sos[1].Trim(')');
                        MessageBox.Show(sos[1]);

                        DateTime dataPlecare = dateTimePicker1.Value;
                        DateTime dataSosire = dateTimePicker2.Value;
                        string dP = dataPlecare.Date.ToString("yyyy-MM-dd");
                        string dS = dataSosire.Date.ToString("yyyy-MM-dd");


                        string url = "https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode="+plec[1]+"&destinationLocationCode="+sos[1]+"&departureDate="+dP+"&adults="+comboBox5.SelectedItem.ToString()+"&children="+comboBox6.SelectedIndex.ToString()+"&max=10";
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        var response = await client.GetStringAsync(url);
                        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                        
                        foreach (var flight in apiResponse.Data)
                        {
                            Console.WriteLine($"Type: {flight.Type}, Origin: {flight.Origin}, Destination: {flight.Destination}");
                            Console.WriteLine($"Departure Date: {flight.DepartureDate}, Return Date: {flight.ReturnDate}");
                            Console.WriteLine($"Price: {flight.Price.Total}");
                            Console.WriteLine();
                            if(lastprice!= double.Parse(flight.Price.Total))
                            dataGridView1.Rows.Add(comboBox1.SelectedItem.ToString(),dP, comboBox2.SelectedItem.ToString(), dS, flight.Price.Total, "Cumpara");
                            lastprice = double.Parse(flight.Price.Total);


                        }
                    }
               
                    }
                
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            catch (Exception ee)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                Date.Error(ee);

            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

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
