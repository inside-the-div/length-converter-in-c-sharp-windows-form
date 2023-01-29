using System.Data;
using System.Data.SqlClient;

namespace LengthConverter
{
    public partial class LengthConverterHome : Form
    {
        SqlConnection DBconnection = new SqlConnection(Properties.Settings.Default.con);
        public LengthConverterHome()
        {
            InitializeComponent();
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyUp);
        }

        private void LengthConverterHome_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            this.Hide();
            LengthHistory lengthHistory = new LengthHistory();
            lengthHistory.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.' || e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox2.Text == ".")
            {
                textBox2.Text = string.Empty;
            }
            else
            {
                double Number = Convert.ToDouble(textBox2.Text);
                double Result = LengthCconverter(Number, comboBox2.SelectedItem.ToString(), comboBox1.SelectedItem.ToString());
                textBox1.Text = Result.ToString();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == ".")
            {
                textBox2.Text = string.Empty;
            }
            else
            {
                double Number = Convert.ToDouble(textBox1.Text);
                double Result = LengthCconverter(Number, comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());
                textBox2.Text = Result.ToString();
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == ".")
            {
                textBox2.Text = string.Empty;
            }
            else
            {
                double Number = Convert.ToDouble(textBox1.Text);
                double Result = LengthCconverter(Number, comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());
                textBox2.Text = Result.ToString();
                if (e.KeyCode == Keys.Enter)
                {
                    string time = DateTime.Now.ToString();
                    string HistorySaveQuery = " INSERT INTO length_convert_history " +
                        "(converted_from, converted_to, converted_number, result, converted_datetime) VALUES " +
                        "('" + comboBox1.SelectedItem.ToString() + "', '"
                        + comboBox2.SelectedItem.ToString() + "', "
                        + Number + ", '" +
                        Result.ToString() + "','" +
                        time + "')";
                    DBconnection.Open();
                    SqlCommand command = new SqlCommand(HistorySaveQuery, DBconnection);
                    command.ExecuteNonQuery();
                    DBconnection.Close();
                }
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox2.Text == "" || textBox2.Text == ".")
            {
                textBox2.Text = string.Empty;
            }
            else
            {
                double Number = Convert.ToDouble(textBox2.Text);
                double Result = LengthCconverter(Number, comboBox2.SelectedItem.ToString(), comboBox1.SelectedItem.ToString());
                textBox1.Text = Result.ToString();
                if (e.KeyCode == Keys.Enter)
                {
                    string time = DateTime.Now.ToString();
                    string HistorySaveQuery = " INSERT INTO length_convert_history " +
                        "(converted_from, converted_to, converted_number, result, converted_datetime) VALUES " +
                        "('" + comboBox2.SelectedItem.ToString() + "', '"
                        + comboBox1.SelectedItem.ToString() + "', "
                        + Number + ", '" +
                        Result.ToString() + "','" +
                        time + "')";
                    DBconnection.Open();
                    SqlCommand command = new SqlCommand(HistorySaveQuery, DBconnection);
                    command.ExecuteNonQuery();
                    DBconnection.Close();

                }
            }
        }

        public double LengthCconverter(double value, string from_unit, string to_unit)
        {
            double result = 0;
            double conversionFactor = 0;
            switch (from_unit)
            {
                case "Millimeter":
                    conversionFactor = 1;
                    break;
                case "Centimeter":
                    conversionFactor = 10;
                    break;
                case "Decimeter":
                    conversionFactor = 100;
                    break;
                case "Meter":
                    conversionFactor = 1000;
                    break;
                case "Kilometer":
                    conversionFactor = 1000000;
                    break;
                case "Foot":
                    conversionFactor = 304.8;
                    break;
                case "Inch":
                    conversionFactor = 25.4;
                    break;
                case "Mile":
                    conversionFactor = 1609344;
                    break;
                case "Yard":
                    conversionFactor = 914.4;
                    break;
            }

            switch (to_unit)
            {
                case "Millimeter":
                    result = value * conversionFactor;
                    break;
                case "Centimeter":
                    result = value * conversionFactor / 10;
                    break;
                case "Decimeter":
                    result = value * conversionFactor / 100;
                    break;
                case "Meter":
                    result = value * conversionFactor / 1000;
                    break;
                case "Kilometer":
                    result = value * conversionFactor / 1000000;
                    break;
                case "Foot":
                    result = value * conversionFactor / 304.8;
                    break;
                case "Inch":
                    result = value * conversionFactor / 25.4;
                    break;
                case "Mile":
                    result = value * conversionFactor / 1609344;
                    break;
                case "Yard":
                    result = value * conversionFactor / 914.4;
                    break;
            }
            return result;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}