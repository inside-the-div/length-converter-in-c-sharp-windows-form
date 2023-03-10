using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LengthConverter
{
    public partial class LengthHistory : Form
    {
        SqlConnection DBconnection = new SqlConnection("Data Source=.; Initial Catalog = length_converter_db; TrustServerCertificate=True; Integrated Security=True ");
        public LengthHistory()
        {
            InitializeComponent();
        }

        private void LengthHistory_Load(object sender, EventArgs e)
        {
            DisplayHistory();
            DatagridviewHistory.ClearSelection();
        }
        public void DisplayHistory()
        {
            string HistoryShow = "SELECT converted_id AS ID, " +
                "converted_from AS 'From', " +
                "converted_to AS 'TO', " +
                "converted_number AS 'Input',  " +
                "result AS Result, " +
                "converted_datetime AS 'Date Time' " +
                "FROM length_convert_history";
            SqlDataAdapter SQLselectQuery = new SqlDataAdapter(HistoryShow, DBconnection);
            DataTable HistoryDataTable = new DataTable();
            SQLselectQuery.Fill(HistoryDataTable);
            DatagridviewHistory.DataSource = HistoryDataTable;
            this.DatagridviewHistory.Columns["ID"].Visible = false;
            DatagridviewHistory.AutoResizeColumns();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            LengthConverterHome lengthConverterHome = new LengthConverterHome();
            lengthConverterHome.Show();
        }

        private void dateTimePickerSearch_ValueChanged(object sender, EventArgs e)
        {
            string SearchDate = dateTimePickerSearch.Value.ToShortDateString().ToLower();
            foreach (DataGridViewRow row in DatagridviewHistory.Rows)
            {
                string GridViewDateTime = row.Cells[5].Value.ToString().ToLower();
                string[] SplitGridViewDateTime = GridViewDateTime.Split(' ');
                string GridViewDate = SplitGridViewDateTime[0].ToString().ToLower();
                if (!GridViewDate.Contains(SearchDate))
                {
                    CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[DatagridviewHistory.DataSource];
                    currencyManager1.SuspendBinding();
                    row.Visible = false;
                    currencyManager1.ResumeBinding();
                }
                else
                {
                    CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[DatagridviewHistory.DataSource];
                    currencyManager1.SuspendBinding();
                    row.Visible = true;
                    currencyManager1.ResumeBinding();
                }
            }
            DatagridviewHistory.ClearSelection();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string Ids = string.Empty;
            foreach (DataGridViewRow row in DatagridviewHistory.SelectedRows)
            {
                int id = (int)row.Cells[0].Value;
                Ids += id.ToString() + ",";
            }
            Ids = Ids.TrimEnd(',');
            if (Ids == "")
            {
                MessageBox.Show("Select a History frist.");
            }
            else
            {
                string DeleteQuerry = "DELETE FROM length_convert_history  WHERE  converted_id IN (" + Ids + ")";
                DBconnection.Open();
                SqlCommand deleteCommand = new SqlCommand(DeleteQuerry, DBconnection);
                if (deleteCommand.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Selected History deleted successfully.");
                    DatagridviewHistory.ClearSelection();
                }
                DBconnection.Close();
                DisplayHistory();
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            string DeleteQuerry = "DELETE FROM length_convert_history";
            DBconnection.Open();
            SqlCommand deleteCommand = new SqlCommand(DeleteQuerry, DBconnection);
            if (deleteCommand.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("All History deleted successfully.");
                DatagridviewHistory.ClearSelection();
            }
            DBconnection.Close();
            DisplayHistory();
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            DisplayHistory();
        }
    }
}
