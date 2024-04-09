using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace PhamDuongAn_0005
{
    public partial class SanPham : Form
    {
        String chuoiKN;
        SqlConnection conn ;
        String maSP = "1";
        public SanPham()
        {
            InitializeComponent();
        }

        private void SanPham_Load(object sender, EventArgs e)
        {
            chuoiKN = ConfigurationManager.ConnectionStrings["cnstr"].ConnectionString;
            conn = new SqlConnection(chuoiKN);
            gVSanPham.DataSource = LayDSSP();
            cbLoaiSP.DataSource = LayDSLSP();
            cbLoaiSP.DisplayMember = "CategoryName";
            cbLoaiSP.ValueMember = "CategoryID";

            cbNCC.DataSource = LayDSNCC();
            cbNCC.DisplayMember = "CompanyName";
            cbNCC.ValueMember = "SupplierID";
        }
          
        private DataTable LayDSSP()
        {
            SqlDataAdapter da;
            String query = "select * from Products order by ProductID Desc";
            da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        private DataTable LayDSLSP()
        {
            SqlDataAdapter da;
            String query = "select CategoryID, CategoryName from Categories";
            da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        private DataTable LayDSNCC()
        {
            SqlDataAdapter da;
            String query = "select SupplierID, CompanyName from Suppliers";
            da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = String.Format("insert into Products(ProductName,SupplierID,CategoryID,UnitPrice,UnitsInstock)" +
                       "values(N'{0}',{1},{2},{3},{4})", 
                       txtTenSP.Text, int.Parse(cbNCC.SelectedValue.ToString()), int.Parse(cbLoaiSP.SelectedValue.ToString()), Convert.ToDecimal(txtDonGia.Text), int.Parse(txtSoLuong.Text));
                
                SqlCommand cmd = new SqlCommand(query,conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                gVSanPham.DataSource = null;
                gVSanPham.DataSource = LayDSSP();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex2)
            {
                MessageBox.Show(ex2.Message);

            }
            finally
            {
                conn.Close();
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                string query = String.Format("update Products " +
                    "Set ProductName=N'{0}', CategoryID={1}, UnitPrice={2}, UnitsInStock={3}, SupplierID={4} " +
                    "where ProductID = {5}",
                    txtTenSP.Text, int.Parse(cbLoaiSP.SelectedValue.ToString()),
                    Convert.ToDecimal(txtDonGia.Text), int.Parse(txtSoLuong.Text), int.Parse(cbNCC.SelectedValue.ToString()), maSP);
                MessageBox.Show(query);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                gVSanPham.DataSource = null;
                gVSanPham.DataSource = LayDSSP();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void gVSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            maSP = gVSanPham.Rows[e.RowIndex].Cells["ProductID"].Value.ToString();
            txtTenSP.Text = gVSanPham.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
            txtSoLuong.Text = gVSanPham.Rows[e.RowIndex].Cells["UnitsInstock"].Value.ToString();
            txtDonGia.Text = gVSanPham.Rows[e.RowIndex].Cells["UnitPrice"].Value.ToString();
            cbLoaiSP.SelectedValue = gVSanPham.Rows[e.RowIndex].Cells["CategoryID"].Value.ToString();
            cbNCC.SelectedValue = gVSanPham.Rows[e.RowIndex].Cells["SupplierID"].Value.ToString();
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                
                string query = String.Format("DELETE FROM Products WHERE ProductID = {0}", maSP);
                MessageBox.Show(query);

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                gVSanPham.DataSource = null;
                gVSanPham.DataSource = LayDSSP();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }
            finally
            {
                conn.Close();
            }
        }


    }
}
