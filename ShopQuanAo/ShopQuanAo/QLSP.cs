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
    public partial class QLSP : Form
    {
        public QLSP()
        {
            InitializeComponent();
        }
        private void QLSP_Load(object sender, EventArgs e)
        {
            // Đường dẫn tới file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\SanPham.xml";

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
                        // Gắn dữ liệu vào DataGridView
                        dataGridView1.DataSource = dataSet.Tables[0];                       
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
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Chỉ xử lý khi click vào một hàng hợp lệ
            {
                try
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Gán dữ liệu vào các TextBox
                    textBoxMaSP.Text = row.Cells["MaSP"].Value?.ToString() ?? "";
                    textBoxTenSP.Text = row.Cells["TenSP"].Value?.ToString() ?? "";
                    textBoxGiaSP.Text = row.Cells["Gia"].Value?.ToString() ?? "";
                    textBoxLoaiSP.Text = row.Cells["Loai"].Value?.ToString() ?? "";
                    textBoxXuatXuSP.Text = row.Cells["XuatXu"].Value?.ToString() ?? "";
                    textBoxSLSP.Text = row.Cells["SoLuong"].Value?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu từ dòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtTenNv_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void UpdateSqlFromXml()
        {
            // Đường dẫn đến file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\SanPham.xml";
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                // Kiểm tra sự tồn tại của file XML
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Tải dữ liệu từ file XML
                    XDocument doc = XDocument.Load(xmlFilePath);

                    // Lặp qua tất cả các sản phẩm trong XML và thực hiện thao tác chèn hoặc cập nhật vào SQL Server
                    foreach (XElement productElement in doc.Descendants("SanPham"))
                    {
                        // Lấy dữ liệu từ từng phần tử sản phẩm trong XML
                        string maSP = productElement.Element("MaSP")?.Value;
                        string tenSP = productElement.Element("TenSP")?.Value;
                        string giaSP = productElement.Element("Gia")?.Value;
                        string loaiSP = productElement.Element("Loai")?.Value;
                        string xuatXu = productElement.Element("XuatXu")?.Value;
                        string soLuong = productElement.Element("SoLuong")?.Value;

                        // Kiểm tra dữ liệu sản phẩm hợp lệ
                        if (!string.IsNullOrEmpty(maSP) && !string.IsNullOrEmpty(tenSP) && !string.IsNullOrEmpty(giaSP) &&
                            !string.IsNullOrEmpty(loaiSP) && !string.IsNullOrEmpty(xuatXu) && !string.IsNullOrEmpty(soLuong))
                        {
                            // Kiểm tra tính hợp lệ của giá và số lượng
                            if (decimal.TryParse(giaSP, out decimal gia) && int.TryParse(soLuong, out int soLuongInt))
                            {
                                // Câu lệnh SQL để chèn hoặc cập nhật dữ liệu vào bảng SanPham
                                string sqlQuery = "IF EXISTS (SELECT 1 FROM SanPham WHERE MaSP = @MaSP) " +
                                                  "UPDATE SanPham SET TenSP = @TenSP, Gia = @Gia, Loai = @LoaiSP, XuatXu = @XuatXu, SoLuong = @SoLuong " +
                                                  "WHERE MaSP = @MaSP " +
                                                  "ELSE " +
                                                  "INSERT INTO SanPham (MaSP, TenSP, Gia, Loai, XuatXu, SoLuong) " +
                                                  "VALUES (@MaSP, @TenSP, @Gia, @LoaiSP, @XuatXu, @SoLuong)";

                                // Tạo kết nối đến SQL Server và thực thi câu lệnh SQL
                                using (SqlConnection conn = new SqlConnection(connectionString))
                                {
                                    conn.Open();

                                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                                    {
                                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                                        cmd.Parameters.AddWithValue("@TenSP", tenSP);
                                        cmd.Parameters.AddWithValue("@Gia", gia);
                                        cmd.Parameters.AddWithValue("@LoaiSP", loaiSP);
                                        cmd.Parameters.AddWithValue("@XuatXu", xuatXu);
                                        cmd.Parameters.AddWithValue("@SoLuong", soLuongInt);

                                        // Thực thi câu lệnh SQL
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Dữ liệu không hợp lệ cho sản phẩm {maSP}. Giá hoặc số lượng không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    MessageBox.Show("Dữ liệu đã được cập nhật thành công vào cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("File XML không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu vào cơ sở dữ liệu SQL Server: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các textbox
            string maSP = textBoxMaSP.Text.Trim();
            string tenSP = textBoxTenSP.Text.Trim();
            string giaSP = textBoxGiaSP.Text.Trim();
            string loaiSP = textBoxLoaiSP.Text.Trim();
            string xuatXu = textBoxXuatXuSP.Text.Trim();
            string soLuong = textBoxSLSP.Text.Trim();

            // Kiểm tra các trường có bị bỏ trống không
            if (string.IsNullOrEmpty(maSP) || string.IsNullOrEmpty(tenSP) || string.IsNullOrEmpty(giaSP) ||
                string.IsNullOrEmpty(loaiSP) || string.IsNullOrEmpty(xuatXu) || string.IsNullOrEmpty(soLuong))
            {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả các trường!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu có trường trống
            }

            // Kiểm tra giá sản phẩm (phải là số và lớn hơn 0)
            if (!decimal.TryParse(giaSP, out decimal gia) || gia <= 0)
            {
                MessageBox.Show("Giá sản phẩm phải là số dương!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra số lượng sản phẩm (phải là số nguyên và lớn hơn hoặc bằng 0)
            if (!int.TryParse(soLuong, out int soLuongSP) || soLuongSP < 0)
            {
                MessageBox.Show("Số lượng sản phẩm phải là số nguyên và không âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Đường dẫn file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\SanPham.xml";

            try
            {
                // Kiểm tra nếu file XML đã tồn tại
                XDocument doc;
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML hiện tại
                    doc = XDocument.Load(xmlFilePath);

                    // Kiểm tra mã sản phẩm đã tồn tại
                    XElement existingProduct = doc.Descendants("SanPham")
                                                  .FirstOrDefault(x => x.Element("MaSP")?.Value == maSP);

                    if (existingProduct != null)
                    {
                        MessageBox.Show("Mã sản phẩm đã tồn tại! Vui lòng nhập mã khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Ngừng thêm dữ liệu
                    }
                }
                else
                {
                    // Tạo mới file XML nếu chưa tồn tại
                    doc = new XDocument(new XElement("SanPhams"));
                }

                // Thêm dữ liệu mới vào file XML
                XElement sanPhamsElement = doc.Element("SanPhams");
                if (sanPhamsElement != null)
                {
                    XElement newProduct = new XElement("SanPham",
                        new XElement("MaSP", maSP),
                        new XElement("TenSP", tenSP),
                        new XElement("Gia", giaSP),
                        new XElement("Loai", loaiSP),
                        new XElement("XuatXu", xuatXu),
                        new XElement("SoLuong", soLuong)
                    );

                    sanPhamsElement.Add(newProduct);
                }

                // Lưu file XML
                doc.Save(xmlFilePath);
                MessageBox.Show("Thêm dữ liệu vào file XML thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Gọi lại hàm load lại dữ liệu hoặc cập nhật UI nếu cần
                QLSP_Load(sender, e);  // Ví dụ nếu có hàm load lại dữ liệu sản phẩm

                // Gọi hàm UpdateSqlFromXml để cập nhật SQL Server từ file XML
                UpdateSqlFromXml();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu vào file XML hoặc cập nhật SQL Server: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các textbox
            string maSP = textBoxMaSP.Text.Trim();
            string tenSP = textBoxTenSP.Text.Trim();
            string giaSP = textBoxGiaSP.Text.Trim();
            string loaiSP = textBoxLoaiSP.Text.Trim();
            string xuatXu = textBoxXuatXuSP.Text.Trim();
            string soLuong = textBoxSLSP.Text.Trim();

            // Kiểm tra các trường có bị bỏ trống không
            if (string.IsNullOrEmpty(maSP) || string.IsNullOrEmpty(tenSP) || string.IsNullOrEmpty(giaSP) ||
                string.IsNullOrEmpty(loaiSP) || string.IsNullOrEmpty(xuatXu) || string.IsNullOrEmpty(soLuong))
            {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả các trường!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu có trường trống
            }

            // Đường dẫn đến file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\SanPham.xml";

            try
            {
                // Kiểm tra nếu file XML đã tồn tại
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML hiện tại
                    XDocument doc = XDocument.Load(xmlFilePath);

                    // Tìm phần tử Sản Phẩm với MaSP tương ứng để sửa
                    XElement sanPhamToEdit = doc.Descendants("SanPham")
                                                  .FirstOrDefault(x => x.Element("MaSP")?.Value == maSP);

                    if (sanPhamToEdit != null)
                    {
                        // Sửa dữ liệu của Sản Phẩm
                        sanPhamToEdit.SetElementValue("TenSP", tenSP);
                        sanPhamToEdit.SetElementValue("Gia", giaSP);
                        sanPhamToEdit.SetElementValue("Loai", loaiSP);
                        sanPhamToEdit.SetElementValue("XuatXu", xuatXu);
                        sanPhamToEdit.SetElementValue("SoLuong", soLuong);

                        // Lưu file XML sau khi sửa
                        doc.Save(xmlFilePath);
                        MessageBox.Show("Sửa dữ liệu trong file XML thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        QLSP_Load(sender, e);
                        // Gọi hàm UpdateSqlFromXml để cập nhật SQL Server từ file XML
                        UpdateSqlFromXml();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm với mã " + maSP, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            // Lấy mã sản phẩm cần xóa từ textbox
            string maSP = textBoxMaSP.Text.Trim();

            // Đường dẫn đến file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\SanPham.xml";

            // Kết nối với cơ sở dữ liệu SQL Server
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                // Kiểm tra nếu file XML đã tồn tại
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML hiện tại
                    XDocument doc = XDocument.Load(xmlFilePath);

                    // Tìm phần tử sản phẩm với MaSP tương ứng để xóa
                    XElement sanPhamToDelete = doc.Descendants("SanPham")
                                                   .FirstOrDefault(x => x.Element("MaSP")?.Value == maSP);

                    if (sanPhamToDelete != null)
                    {
                        // Xóa phần tử sản phẩm khỏi XML
                        sanPhamToDelete.Remove();

                        // Lưu lại file XML sau khi xóa
                        doc.Save(xmlFilePath);

                        // Cập nhật SQL Server - xóa sản phẩm khỏi cơ sở dữ liệu
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string deleteQuery = "DELETE FROM SanPham WHERE MaSP = @MaSP";

                            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                            {
                                command.Parameters.AddWithValue("@MaSP", maSP);
                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Xóa sản phẩm thành công từ file XML và cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        QLSP_Load(sender, e);  // Cập nhật lại danh sách sản phẩm trên giao diện
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm với mã " + maSP, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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


        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Đường dẫn tới file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\SanPham.xml";

            try
            {
                // Kiểm tra xem file XML có tồn tại không
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(xmlFilePath);

                    // Gắn dữ liệu vào DataGridView
                    if (dataSet.Tables.Count > 0)
                    {
                        dataGridView1.DataSource = dataSet.Tables[0];
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file XML: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            DangNhap dn = new DangNhap();
            dn.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {

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
                    string sqlQuery = "SELECT MaSP, TenSP, Loai, XuatXu, SoLuong, Gia FROM SanPham " +
                                      "WHERE LOWER(TenSP) LIKE @searchQuery COLLATE SQL_Latin1_General_CP1_CI_AS"; // Tìm kiếm không phân biệt hoa-thường

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
                MessageBox.Show("Vui lòng nhập tên sản phẩm để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQLSP_Click(object sender, EventArgs e)
        {

        }

        private void btnQLTK_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLTK
            QLTK TKForm = new QLTK();

            // Hiển thị Form QLTK
            TKForm.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
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
