using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopQuanAo
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnQLNV_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLNV
            QLNV qlnvForm = new QLNV();

            // Hiển thị Form QLNV
            qlnvForm.Show();
        }

        private void btnQLSP_Click(object sender, EventArgs e)
        {
            this.Hide();
            // Tạo instance của Form QLNV
            QLSP qlnvForm = new QLSP();

            // Hiển thị Form QLNV
            qlnvForm.Show();
        }

        private void btnQLTK_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLTK
            QLTK TKForm = new QLTK();

            // Hiển thị Form QLTK
            TKForm.Show();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            DangNhap dn = new DangNhap();
            dn.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void btnQLHD_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLSP
            QLHD qlhdForm = new QLHD();

            // Hiển thị Form QLSP
            qlhdForm.Show();
        }
    }
}
