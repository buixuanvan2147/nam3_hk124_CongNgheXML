using System;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

namespace ShopQuanAo
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Lấy giá trị từ các textbox
            string tenDN = txttaikhoan.Text;
            string matKhau = txtMatKhau.Text;

            // Đường dẫn đến file XML chứa thông tin tài khoản
            string xmlFilePath = @"D:\nam3-HK124\Cong-nghe-XML-124CNX01\BaiTapLon\ShopQuanAo\ShopQuanAo\bin\Debug\Data\TaiKhoan.xml";

            try
            {
                // Load file XML
                XDocument doc = XDocument.Load(xmlFilePath);

                // Tìm kiếm tài khoản khớp tên đăng nhập và mật khẩu trong XML
                var user = doc.Descendants("TaiKhoan")
                      .FirstOrDefault(u =>
                          u.Element("TenDangNhap") != null && u.Element("MatKhau") != null &&
                          u.Element("TenDangNhap").Value == tenDN &&
                          u.Element("MatKhau").Value == matKhau);


                // Kiểm tra nếu tìm thấy thông tin đăng nhập
                if (user != null)
                {
                    // Kiểm tra quyền của tài khoản
                    string quyen = user.Element("Quyen")?.Value;

                    // Nếu không phải Admin
                    if (quyen != "Admin")
                    {
                        MessageBox.Show("Bạn chỉ có thể đăng nhập với tài khoản có quyền Admin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thành công với quyền Admin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tạo và hiển thị Form Main
                        Main main = new Main();
                        main.Show();

                        // Ẩn Form hiện tại
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file XML: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
