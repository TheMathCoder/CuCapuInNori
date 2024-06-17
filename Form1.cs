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
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

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
        bool loggedIn = false;
        int loggedId=0;
        int dusintors = 0;
        int zbordirect = 0;
        ApiResponse apiResponse;
        string lastdate;
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

        public class Departure
        {
            public string IataCode { get; set; }
            public string Terminal { get; set; }
            public DateTime At { get; set; }
        }

        public class Arrival
        {
            public string IataCode { get; set; }
            public string Terminal { get; set; }
            public DateTime At { get; set; }
        }

        public class Segment
        {
            public Departure Departure { get; set; }
            public Arrival Arrival { get; set; }
            public string CarrierCode { get; set; }
            public string Number { get; set; }
            public string Duration { get; set; }
        }

        public class Itinerary
        {
            public string Duration { get; set; }
            public List<Segment> Segments { get; set; }
        }

        public class FlightDestination
        {
            public string Type { get; set; }
            public List<Itinerary> Itineraries { get; set; }
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
          //      cmd.ExecuteNonQuery();
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
            tabControl1.SelectedIndex = 1;
            getToken();
            if(loggedIn==true)
            {
                button3.Visible = true;
                panel5.Visible = true;
                button2.Visible = false;
                panel9.Visible = false;
                panel7.Visible = false;
                button7.Visible = false;
            }
            else
            {
                button3.Visible = false;
                panel5.Visible = false;
                label13.Visible = false;
                button2.Visible = true;
                panel9.Visible = true;
                panel7.Visible = true;
                button7.Visible = true;
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {

            pictureBox1.Image = Image.FromFile(Date.folder + "icon.png");
            label13.Visible = false;
            panel9.Visible = false;
            panel7.Visible = false;
            button7.Visible = false;

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
            if (new Logare().ShowDialog() == DialogResult.OK)
            {
                loggedIn = true;
                button3.Visible = true;
                panel5.Visible = true;
                button2.Visible = false;
                cmd = new SqlCommand("select iduser from useri where isonline=1", con);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                loggedId = int.Parse(dt.Rows[0][0].ToString());
                cmd = new SqlCommand("select nume, prenume from useri where iduser=@id", con);
                cmd.Parameters.Add("@id", loggedId);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                label13.Text = dt.Rows[0][0].ToString() + " " + dt.Rows[0][1].ToString();
                label13.Size = new Size(40 - int.Parse(label13.Text.Length.ToString()), 30 - int.Parse(label13.Text.Length.ToString()));
                label13.Visible = true;
                button2.Visible = false;
                panel9.Visible = true;
                panel7.Visible = true;
                button7.Visible = true;

            }

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

            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            if(tabControl1.SelectedIndex == 1)
            {
                dataGridView1.Rows.Clear();
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
                for (int i = 1; i < 7; i++)
                    comboBox5.Items.Add(i);
                for (int i = 0; i < 7; i++)
                    comboBox6.Items.Add(i);
                comboBox7.Items.Add("ECONOMY");
                comboBox7.Items.Add("PREMIUM_ECONOMY");
                comboBox7.Items.Add("BUSINESS");
                comboBox7.Items.Add("FIRST");
                comboBox8.Items.Add("RON");
                comboBox8.Items.Add("EUR");
                comboBox8.Items.Add("USD");


            }
            else
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd = new SqlCommand("select * from zboruri where idUser=@id", con);
                    cmd.Parameters.AddWithValue("@id", loggedId);
                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                        dataGridView3.Rows.Add(dt.Rows[i][2], DateTime.Parse(dt.Rows[i][3].ToString()).ToString("MM/dd/yyyy"), DateTime.Parse(dt.Rows[i][3].ToString()).ToString("hh:mm"), dt.Rows[i][4].ToString(), DateTime.Parse(dt.Rows[i][5].ToString()).ToString("MM/dd/yyyy"), DateTime.Parse(dt.Rows[i][5].ToString()).ToString("hh:mm"), dt.Rows[i][6].ToString());
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

                if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null && comboBox3.SelectedItem != null && comboBox4.SelectedItem != null && comboBox5.SelectedItem != null && comboBox6.SelectedItem != null && comboBox7.SelectedItem != null && comboBox8.SelectedItem != null)
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
                        string dP = dataPlecare.Date.ToString("yyyy-MM-dd");



                        string url = "https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode="+plec[1]+"&destinationLocationCode="+sos[1]+"&departureDate="+dP+"&adults="+comboBox5.SelectedItem.ToString()+"&children="+comboBox6.SelectedIndex.ToString()+ "&max=10&travelClass=" + comboBox7.SelectedItem.ToString()+ "&currencyCode=" + comboBox8.SelectedItem.ToString();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        var response = await client.GetStringAsync(url);
                        apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);

                        foreach (var flight in apiResponse.Data)
                        {
                            int i = 0;
                            string firstdate="";
                            if (lastprice != double.Parse(flight.Price.Total))
                            {
                                foreach (var itinerary in flight.Itineraries)
                                {
                                    foreach (var segment in itinerary.Segments)
                                    {
                                        if (i == 0)
                                        {
                                            firstdate = segment.Departure.At.ToString();
                                            i++;
                                        }
                                        string departureTerminal = segment.Departure.Terminal;
                                        string arrivalTerminal = segment.Arrival.Terminal;
                                        string departureHour = segment.Departure.At.ToString();
                                        string arrivalHour = segment.Arrival.At.ToString();
                                        string flightDuration = segment.Duration;
                                        Console.WriteLine($"Type: {flight.Type}, Origin: {segment.Departure.IataCode}, Destination: {segment.Arrival.IataCode}");
                                        Console.WriteLine($"Price: {flight.Price.Total}");
                                        Console.WriteLine($"Departure Terminal: {departureTerminal}");
                                        Console.WriteLine($"Arrival Terminal: {arrivalTerminal}");
                                        Console.WriteLine($"Departure Hour: {departureHour}");
                                        Console.WriteLine($"Arrival Hour: {arrivalHour}");
                                        Console.WriteLine($"Flight Duration: {flightDuration}");

                                        lastdate = arrivalHour;


                                    }
                                }
                                i--;
                                dataGridView1.Rows.Add(comboBox1.SelectedItem.ToString(), firstdate, comboBox2.SelectedItem.ToString(), lastdate, flight.Price.Total, "Cumpara");
                            }
                            lastprice = double.Parse(flight.Price.Total);
                        }

                        foreach (var flight in apiResponse.Data)
                         {
                             if(lastprice!= double.Parse(flight.Price.Total))
                             
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

        private void tabPage1_Click_1(object sender, EventArgs e)
        {

        }

        public void sendMail(string subiect, string mesaj, string mailaddr, string nume, string prenume, string plecare, string data, string ora, string sosire, string datas,string oras, string pret)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("themathcoder04@gmail.com");
                mail.To.Add(mailaddr);
                mail.Subject = subiect;
                LinkedResource res = new LinkedResource(Date.folder + "logo.png", MediaTypeNames.Image.Jpeg);
                res.ContentId = Guid.NewGuid().ToString();
                string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>" + @"<h1>" + mesaj + @"</h1>" + @"<br />" + @"<h2> Iti multumim ca ai ales sa folosesti serviciile noatre. Datele biletului tau sunt urmatoarele: </h2> <br /> <h2>Nume:" + nume + @"</h2>" + "<br /> <h2>Prenume:" + prenume + @"</h2> "+ @"</h2>" + "<br /> <h2>Plecare:" + plecare + @"</h2>"
+ "<br /> <h2>Data plecare:" + data + @"</h2>" + "<br /> <h2>Ora:" + ora + @"</h2>" + "<br /> <h2>Sosire:" + sosire + @"</h2>" + "<br /> <h2>Data sosire:" + datas + @"</h2>" + "<br /> <h2>Ora sosire:" + oras + @"</h2>" + "<br /> <h2>Pret:" + pret + @"</h2>";
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
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 5)
                {
                    if (loggedIn == true)
                    {
                        if (MessageBox.Show("Esti sigur ca vrei sa cumperi biletul pentru suma de: " + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(), "Cumparare bilet", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes && dataGridView1.RowCount != null)
                        {
                            string plecare = comboBox1.SelectedItem.ToString();
                            string[] plec = plecare.Split('(');
                            Random r = new Random();
                            plec[1] = plec[1].Trim(')');
                            string sosire = comboBox2.SelectedItem.ToString();
                            string[] sos = sosire.Split('(');
                            sos[1] = sos[1].Trim(')');
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            DateTime dp = new DateTime();
                            DateTime ds = new DateTime();
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                         //   dataGridView3.Rows.Add(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()).ToString(), DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()).ToString("HH:mm"), dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(), DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()), DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()).ToString("HH:mm"), float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString()));
                            cmd = new SqlCommand("insert into zboruri(idUser,orasplecare,dataplecare,orassosire,datasosire,pret)values(@id,@op,@dp,@os,@ds,@pret)", con);
                            cmd.Parameters.Add("@id",loggedId);
                            cmd.Parameters.Add("@op", dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            cmd.Parameters.Add("@dp", DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()).ToString());
                            cmd.Parameters.Add("@os", dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                            cmd.Parameters.Add("@ds", DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()).ToString());
                            cmd.Parameters.Add("@pret", float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString()));
                            cmd.ExecuteNonQuery();
                            cmd = new SqlCommand("select email,nume,prenume from useri where iduser=@id", con);
                            cmd.Parameters.AddWithValue("@id", loggedId);
                            da = new SqlDataAdapter(cmd);
                            dt = new DataTable();
                            da.Fill(dt);
                            sendMail("Confirmare Achizitie Bilet", "Salut, " + dt.Rows[0][1].ToString() + " " + dt.Rows[0][2].ToString(), dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()
                                , DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()).ToString("dd/MM/yyyy"), DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()).ToString("hh:mm"), dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(), DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()).ToString("dd/MM/yyyy"), DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()).ToString("hh:mm"), dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                            int count = 0;
                            foreach (var flight in apiResponse.Data)
                            {
                                if (flight.Price.Total == dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() && count == 0)
                                {
                                    foreach (var itinerary in flight.Itineraries)
                                    {
                                        foreach (var segment in itinerary.Segments)
                                        {
                                            string departureTerminal = segment.Departure.Terminal;
                                            string arrivalTerminal = segment.Arrival.Terminal;
                                            string departureHour = segment.Departure.At.ToString("dd/MM/yyyy HH:mm");
                                            string arrivalHour = segment.Arrival.At.ToString("dd/MM/yyyy HH:mm");
                                            string flightDuration = segment.Duration;
                                            Console.WriteLine($"Type: {flight.Type}, Origin: {segment.Departure.IataCode}, Destination: {segment.Arrival.IataCode}");
                                            Console.WriteLine($"Price: {flight.Price.Total}");
                                            Console.WriteLine($"Departure Terminal: {departureTerminal}");
                                            Console.WriteLine($"Arrival Terminal: {arrivalTerminal}");
                                            Console.WriteLine($"Departure Hour: {departureHour}");
                                            Console.WriteLine($"Arrival Hour: {arrivalHour}");
                                            Console.WriteLine($"Flight Duration: {flightDuration}");
                                            if (con.State == ConnectionState.Closed)
                                                con.Open();
                                            cmd = new SqlCommand("select orasAeroport from aeroporturi where codAeroport=@cod", con);
                                            cmd.Parameters.AddWithValue("@cod", segment.Departure.IataCode);
                                            da = new SqlDataAdapter(cmd);
                                            dt = new DataTable();
                                            da.Fill(dt);
                                            DataTable dt2 = new DataTable();
                                            cmd = new SqlCommand("select orasAeroport from aeroporturi where codAeroport=@cod", con);
                                            cmd.Parameters.AddWithValue("@cod", segment.Arrival.IataCode);
                                            da = new SqlDataAdapter(cmd);
                                            da.Fill(dt2);
                                            int unique = r.Next(10000, 99999);
                                            //  dataGridView2.Rows.Add(segment.Departure.IataCode + segment.Arrival.IataCode + unique, dt.Rows[0][0].ToString(),segment.Departure.At.ToString("dd-MM-yyyy"),segment.Departure.At.ToString("hh:mm"),dt2.Rows[0][0].ToString(), segment.Arrival.At.ToString("dd-MM-yyyy"), segment.Arrival.At.ToString("hh:mm"));
                                            cmd = new SqlCommand("insert into bilete(codzbor,orasplecare,orassosire,dataplecare,datasosire,idzbor)values(@cod,@op,@os,@dp,@ds,(select count(*) from zboruri))", con);
                                            cmd.Parameters.AddWithValue("@cod", segment.Departure.IataCode + segment.Arrival.IataCode + unique);
                                            cmd.Parameters.AddWithValue("@op", dt.Rows[0][0].ToString() + " (" + segment.Departure.IataCode.ToString() + ")" );
                                            cmd.Parameters.AddWithValue("@os", dt2.Rows[0][0].ToString() + " (" + segment.Arrival.IataCode.ToString() + ")" );

                                            cmd.Parameters.AddWithValue("@ds", segment.Arrival.At);
                                            cmd.Parameters.AddWithValue("@dp",segment.Departure.At);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    count++;
                                }
                            }

                        }
                    }
                    else
                    {
                        throw new Exception("Pentru a cumpara un bilet trebuie sa va logati.");
                    }
                    }
                }
            catch (Exception ee)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                Date.Error(ee);
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            loggedId = -1;
            loggedIn = false;
            {
                panel9.Visible = false;
                panel7.Visible = false;
                button7.Visible = false;
                button3.Visible = false;
                panel5.Visible = false;
                label13.Visible = false;
                button2.Visible = true;
            }
        }

        private async void button8_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                string url = "https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode=VIE&destinationLocationCode=SYD&departureDate=2024-07-02&adults=1&max=2&returnDate=2024-07-05";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetStringAsync(url);
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);

                foreach (var flight in apiResponse.Data)
                {
                    foreach (var itinerary in flight.Itineraries)
                    {
                        foreach (var segment in itinerary.Segments)
                        {
                            string departureTerminal = segment.Departure.Terminal;
                            string arrivalTerminal = segment.Arrival.Terminal;
                            string departureHour = segment.Departure.At.ToString("dd/MM/yyyy HH:mm");
                            string arrivalHour = segment.Arrival.At.ToString("dd/MM/yyyy HH:mm");
                            string flightDuration = segment.Duration;
                            Console.WriteLine($"Type: {flight.Type}, Origin: {segment.Departure.IataCode}, Destination: {segment.Arrival.IataCode}");
                            Console.WriteLine($"Price: {flight.Price.Total}");
                            Console.WriteLine($"Departure Terminal: {departureTerminal}");
                            Console.WriteLine($"Arrival Terminal: {arrivalTerminal}");
                            Console.WriteLine($"Departure Hour: {departureHour}");
                            Console.WriteLine($"Arrival Hour: {arrivalHour}");
                            Console.WriteLine($"Flight Duration: {flightDuration}");
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Date.Error(ee);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        { 
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView3.Rows[e.RowIndex].Cells[0].Value != null)
            {
                dataGridView2.Rows.Clear();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd = new SqlCommand("select * from bilete where idzbor=(select idzbor from zboruri where orasplecare=@op and orassosire=@os and iduser=@id)", con);
                cmd.Parameters.AddWithValue("@op",dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString());
                cmd.Parameters.AddWithValue("@os", dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString());
                cmd.Parameters.AddWithValue("@id",loggedId);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                    dataGridView2.Rows.Add(dt.Rows[i][1], dt.Rows[i][2], dt.Rows[i][4], DateTime.Parse(dt.Rows[i][4].ToString()).ToString("hh:mm"), dt.Rows[i][3].ToString(), DateTime.Parse(dt.Rows[i][5].ToString()).ToString("MM/dd/yyyy"), DateTime.Parse(dt.Rows[i][5].ToString()).ToString("hh:mm"));
            }
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
