using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ShopQuanAo
{
    public partial class QLTK : Form
    {
        public QLTK()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Chỉ xử lý khi click vào một hàng hợp lệ
            {
                try
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Kiểm tra xem các cột có tồn tại trước khi gán dữ liệu
                    if (dataGridView1.Columns.Contains("TenDangNhap"))
                        textBoxTK.Text = row.Cells["TenDangNhap"].Value?.ToString() ?? "";  // Tên đăng nhập

                    if (dataGridView1.Columns.Contains("MatKhau"))
                        textBoxMK.Text = row.Cells["MatKhau"].Value?.ToString() ?? "";  // Mật khẩu

                    if (dataGridView1.Columns.Contains("Quyen"))
                        comboBox1.SelectedItem = row.Cells["Quyen"].Value?.ToString() ?? "";  // Quyền
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu từ dòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxSearch.Clear();
        }

        private void Searchbtn_Click(object sender, EventArgs e)
        {
            // Đường dẫn kết nối đến cơ sở dữ liệu SQL Server
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            // Lấy nội dung trong textBoxSearch và loại bỏ khoảng trắng thừa
            string searchQuery = textBoxSearch.Text.Trim().ToLower(); // Chuyển sang chữ thường để tìm kiếm không phân biệt hoa-thường

            // Kiểm tra nếu người dùng đã nhập từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchQuery))
            {
                try
                {
                    // Câu lệnh SQL để tìm kiếm sản phẩm có tên chứa từ khóa tìm kiếm
                    string sqlQuery = "SELECT TenDangNhap, MatKhau, Quyen FROM TaiKhoan " +
                                      "WHERE LOWER(TenDangNhap) LIKE @searchQuery COLLATE SQL_Latin1_General_CP1_CI_AS"; // Tìm kiếm không phân biệt hoa-thường

                    // Tạo kết nối tới cơ sở dữ liệu SQL Server
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        // Mở kết nối
                        conn.Open();

                        // Tạo câu lệnh SQL với tham số
                        using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                        {
                            // Thêm tham số tìm kiếm, sử dụng '%' để tìm kiếm phần tử chứa từ khóa (cả trước và sau từ khóa)
                            cmd.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");

                            // Tạo DataTable để lưu kết quả
                            DataTable dt = new DataTable();

                            // Tạo DataAdapter để điền dữ liệu vào DataTable
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);  // Điền dữ liệu vào DataTable
                            }

                            // Kiểm tra nếu không có dữ liệu trả về
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("Không tìm thấy sản phẩm nào với từ khóa: " + searchQuery, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                // Gán DataTable vào DataGridView
                                dataGridView1.DataSource = dt;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQLNV_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLNV
            QLNV qlnvForm = new QLNV();

            // Hiển thị Form QLNV
            qlnvForm.Show();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            DangNhap dn = new DangNhap();
            dn.Show();
        }

        private void btnQLSP_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLSP
            QLSP qlspForm = new QLSP();

            // Hiển thị Form QLSP
            qlspForm.Show();
        }

        private void QLTK_Load(object sender, EventArgs e)
        {
            // Đường dẫn tới file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\TaiKhoan.xml";

            try
            {
                // Kiểm tra xem file XML có tồn tại không
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(xmlFilePath);

                    // Kiểm tra xem có bảng dữ liệu không
                    if (dataSet.Tables.Count > 0)
                    {
                        // Đảm bảo bảng có các cột đúng theo cấu trúc
                        DataTable table = dataSet.Tables[0];
                        if (table.Columns.Contains("TenDangNhap") &&
                            table.Columns.Contains("MatKhau") &&
                            table.Columns.Contains("Quyen"))
                        {
                            // Gắn dữ liệu vào DataGridView
                            dataGridView1.DataSource = table;
                        }
                        else
                        {
                            MessageBox.Show("File XML không khớp với cấu trúc dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bảng dữ liệu trong file XML!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("File XML không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.IO.IOException ioEx)
            {
                MessageBox.Show("Lỗi khi đọc file XML: " + ioEx.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Cập nhật combobox Quyền
            comboBox1.Items.Clear();
            comboBox1.Items.Add("");  // Mặc định để trống
            comboBox1.Items.Add("Admin");
            comboBox1.Items.Add("User");
            comboBox1.SelectedIndex = 0;  // Mặc định chọn giá trị trống
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            QLTK_Load(sender, e);
        }

        private void btnXoaTK_Click(object sender, EventArgs e)
        {
            // Lấy tên tài khoản cần xóa từ textbox
            string taiKhoan = textBoxTK.Text.Trim();

            // Đường dẫn đến file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\TaiKhoan.xml";

            // Kết nối với cơ sở dữ liệu SQL Server
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                // Kiểm tra nếu file XML đã tồn tại
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML hiện tại
                    XDocument doc = XDocument.Load(xmlFilePath);

                    // Tìm phần tử tài khoản với TenDangNhap tương ứng để xóa
                    XElement taiKhoanToDelete = doc.Descendants("TaiKhoan")
                                                   .FirstOrDefault(x => x.Element("TenDangNhap")?.Value == taiKhoan);

                    if (taiKhoanToDelete != null)
                    {
                        // Kiểm tra quyền của tài khoản cần xóa
                        string quyen = taiKhoanToDelete.Element("Quyen")?.Value;

                        // Kiểm tra nếu tài khoản cần xóa là Admin, đảm bảo còn ít nhất 1 tài khoản Admin
                        if (quyen == "Admin")
                        {
                            // Kiểm tra số lượng tài khoản Admin còn lại trong XML
                            int adminCount = doc.Descendants("TaiKhoan")
                                                .Count(x => x.Element("Quyen")?.Value == "Admin");

                            // Nếu chỉ còn 1 tài khoản Admin, không cho phép xóa
                            if (adminCount <= 1)
                            {
                                MessageBox.Show("Không thể xóa tài khoản Admin vì hệ thống yêu cầu ít nhất 1 tài khoản Admin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        // Xóa phần tử tài khoản khỏi XML
                        taiKhoanToDelete.Remove();

                        // Lưu lại file XML sau khi xóa
                        doc.Save(xmlFilePath);

                        // Cập nhật SQL Server - xóa tài khoản khỏi cơ sở dữ liệu
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string deleteQuery = "DELETE FROM TaiKhoan WHERE TenDangNhap = @TaiKhoan";

                            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                            {
                                command.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Xóa tài khoản thành công từ file XML và cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        QLTK_Load(sender, e);  // Cập nhật lại danh sách tài khoản trên giao diện
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản với tên " + taiKhoan, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("File XML không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dữ liệu trong file XML hoặc cơ sở dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSqlFromXml()
        {
            // Đường dẫn đến file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\TaiKhoan.xml";
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                // Kiểm tra sự tồn tại của file XML
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Tải dữ liệu từ file XML
                    XDocument doc = XDocument.Load(xmlFilePath);

                    // Lặp qua tất cả các tài khoản trong XML và thực hiện thao tác chèn hoặc cập nhật vào SQL Server
                    foreach (XElement accountElement in doc.Descendants("TaiKhoan"))
                    {
                        // Lấy dữ liệu từ từng phần tử tài khoản trong XML
                        string tenDangNhap = accountElement.Element("TenDangNhap")?.Value;
                        string matKhau = accountElement.Element("MatKhau")?.Value;
                        string quyen = accountElement.Element("Quyen")?.Value;

                        // Kiểm tra dữ liệu tài khoản hợp lệ
                        if (!string.IsNullOrEmpty(tenDangNhap) && !string.IsNullOrEmpty(matKhau) && !string.IsNullOrEmpty(quyen))
                        {
                            // Câu lệnh SQL để chèn hoặc cập nhật dữ liệu vào bảng TaiKhoan
                            string sqlQuery = "IF EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap) " +
                                              "UPDATE TaiKhoan SET MatKhau = @MatKhau, Quyen = @Quyen " +
                                              "WHERE TenDangNhap = @TenDangNhap " +
                                              "ELSE " +
                                              "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Quyen) " +
                                              "VALUES (@TenDangNhap, @MatKhau, @Quyen)";

                            // Tạo kết nối đến SQL Server và thực thi câu lệnh SQL
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                conn.Open();

                                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                                {
                                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                                    cmd.Parameters.AddWithValue("@Quyen", quyen);

                                    // Thực thi câu lệnh SQL
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Dữ liệu không hợp lệ cho tài khoản {tenDangNhap}. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    MessageBox.Show("Dữ liệu tài khoản đã được cập nhật thành công vào cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("File XML không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu tài khoản vào cơ sở dữ liệu SQL Server: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnThemTK_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các textbox và combobox
            string tenDN = textBoxTK.Text.Trim();
            string matKhau = textBoxMK.Text.Trim();
            string quyen = comboBox1.Text.Trim();

            // Kiểm tra các trường có bị bỏ trống không
            if (string.IsNullOrEmpty(tenDN) || string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(quyen))
            {
                MessageBox.Show("Vui lòng bổ sung đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu có trường trống
            }

            // Đường dẫn file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\TaiKhoan.xml";

            try
            {
                // Tạo biến tài liệu XML
                XDocument doc;

                // Kiểm tra nếu file XML đã tồn tại
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML hiện tại
                    doc = XDocument.Load(xmlFilePath);

                    // Kiểm tra nếu tên đăng nhập đã tồn tại
                    XElement existingAccount = doc.Descendants("TaiKhoan")
                                                  .FirstOrDefault(x => x.Element("TenDangNhap")?.Value == tenDN);

                    if (existingAccount != null)
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại! Vui lòng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Ngừng thêm dữ liệu
                    }
                }
                else
                {
                    // Tạo mới file XML nếu chưa tồn tại
                    doc = new XDocument(new XElement("TaiKhoans"));
                }

                // Thêm tài khoản mới vào file XML
                XElement newAccount = new XElement("TaiKhoan",
                    new XElement("TenDangNhap", tenDN),
                    new XElement("MatKhau", matKhau),
                    new XElement("Quyen", quyen)
                );

                doc.Element("TaiKhoans")?.Add(newAccount);

                // Lưu file XML
                doc.Save(xmlFilePath);
                MessageBox.Show("Thêm tài khoản thành công vào file XML!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Gọi lại hàm load lại dữ liệu trên giao diện
                btnLoad_Click(sender, e);

                // Gọi hàm cập nhật SQL Server từ file XML nếu cần
                UpdateSqlFromXml();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm tài khoản vào file XML: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaTK_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các textbox
            string tenDangNhap = textBoxTK.Text.Trim();
            string matKhau = textBoxMK.Text.Trim();
            string quyen = comboBox1.Text.Trim();

            // Kiểm tra các trường có bị bỏ trống không
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(quyen))
            {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả các trường!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu có trường trống
            }

            // Đường dẫn đến file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\TaiKhoan.xml";

            try
            {
                // Kiểm tra nếu file XML đã tồn tại
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML hiện tại
                    XDocument doc = XDocument.Load(xmlFilePath);

                    // Tìm phần tử Tài Khoản với TenDangNhap tương ứng để sửa
                    XElement taiKhoanToEdit = doc.Descendants("TaiKhoan")
                                                 .FirstOrDefault(x => x.Element("TenDangNhap")?.Value == tenDangNhap);

                    if (taiKhoanToEdit != null)
                    {
                        // Sửa dữ liệu của Tài Khoản
                        taiKhoanToEdit.SetElementValue("MatKhau", matKhau);
                        taiKhoanToEdit.SetElementValue("Quyen", quyen);

                        // Lưu file XML sau khi sửa
                        doc.Save(xmlFilePath);
                        MessageBox.Show("Sửa dữ liệu trong file XML thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        QLTK_Load(sender, e);
                        // Gọi hàm UpdateSqlFromXmlForTaiKhoan để cập nhật SQL Server từ file XML
                        UpdateSqlFromXml();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản với tên đăng nhập " + tenDangNhap, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("File XML không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa dữ liệu trong file XML hoặc cập nhật SQL Server: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
