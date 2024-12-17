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
        public partial class QLNV : Form
        {
            public QLNV()
            {
                InitializeComponent();
            }

            private void btnQLHD_Click(object sender, EventArgs e)
            {
            this.Close();
            // Tạo instance của Form QLSP
            QLHD qlhdForm = new QLHD();

            // Hiển thị Form QLSP
            qlhdForm.Show();
        }

            private void QLNV_Load(object sender, EventArgs e)
            {
                // Đường dẫn tới file XML
                string xmlFilePath = @"D:\nam3-HK124\Cong-nghe-XML-124CNX01\BaiTapLon\ShopQuanAo\ShopQuanAo\bin\Debug\Data\NhanVien.xml";

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



            private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                // Kiểm tra nếu click vào một ô hợp lệ (không phải header)
                if (e.RowIndex >= 0)
                {
                    try
                    {
                        // Lấy dữ liệu từ hàng được click
                        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];                   

                        // Gán dữ liệu vào các TextBox nếu cột tồn tại và không null
                        if (dataGridView1.Columns.Contains("MaNV") && dataGridView1.Columns.Contains("HoTen"))
                        {
                            txtMaNv.Text = row.Cells["MaNV"].Value?.ToString() ?? "";
                            txtTenNv.Text = row.Cells["HoTen"].Value?.ToString() ?? "";
                            txtDC.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
                            txtSDTNV.Text = row.Cells["SDTNV"].Value?.ToString() ?? "";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lấy dữ liệu từ dòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }




        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các TextBox
            string maNV = txtMaNv.Text.Trim();
            string hoTen = txtTenNv.Text.Trim();
            string diaChi = txtDC.Text.Trim();
            string sdtNV = txtSDTNV.Text.Trim();

            // Kiểm tra dữ liệu nhập vào không để trống
            if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(diaChi) || string.IsNullOrEmpty(sdtNV))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu có thông tin bị thiếu
            }

            // Kiểm tra số điện thoại hợp lệ
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdtNV, @"^\d+$"))
            {
                MessageBox.Show("Số điện thoại chỉ được chứa các chữ số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu số điện thoại không hợp lệ
            }

            // Đường dẫn file XML
            string xmlFilePath = @"D:\nam3-HK124\Cong-nghe-XML-124CNX01\BaiTapLon\ShopQuanAo\ShopQuanAo\bin\Debug\Data\NhanVien.xml";

            try
            {
                // Kiểm tra nếu file XML đã tồn tại
                XDocument doc;
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML hiện tại
                    doc = XDocument.Load(xmlFilePath);

                    // Kiểm tra mã nhân viên đã tồn tại
                    XElement existingEmployee = doc.Descendants("NhanVien")
                                                   .FirstOrDefault(x => x.Element("MaNV")?.Value == maNV);

                    if (existingEmployee != null)
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại! Vui lòng nhập mã khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Ngừng thêm dữ liệu
                    }
                }
                else
                {
                    // Tạo mới file XML nếu chưa tồn tại
                    doc = new XDocument(new XElement("NhanViens"));
                }

                // Thêm dữ liệu mới vào file XML
                XElement nhanViensElement = doc.Element("NhanViens");
                if (nhanViensElement != null)
                {
                    XElement newEmployee = new XElement("NhanVien",
                        new XElement("MaNV", maNV),
                        new XElement("HoTen", hoTen),
                        new XElement("DiaChi", diaChi),
                        new XElement("SDTNV", sdtNV)
                    );

                    nhanViensElement.Add(newEmployee);
                }

                // Lưu file XML
                doc.Save(xmlFilePath);
                MessageBox.Show("Thêm dữ liệu vào file XML thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Tải lại dữ liệu và cập nhật SQL Server
                QLNV_Load(sender, e);
                UpdateSqlFromXml();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu vào file XML hoặc cập nhật SQL Server: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtMaNv_TextChanged(object sender, EventArgs e)
            {

            }

            private void txtTenNv_TextChanged(object sender, EventArgs e)
            {

            }

            private void txtDC_TextChanged(object sender, EventArgs e)
            {

            }

            private void txtSDTNV_TextChanged(object sender, EventArgs e)
            {

            }

            private void btnSuaNv_Click(object sender, EventArgs e)
            {
                // Lấy thông tin từ các textbox
                string maNV = txtMaNv.Text;
                string hoTen = txtTenNv.Text;
                string diaChi = txtDC.Text;
                string sdtNV = txtSDTNV.Text;

                // Đường dẫn đến file XML
                string xmlFilePath = @"D:\nam3-HK124\Cong-nghe-XML-124CNX01\BaiTapLon\ShopQuanAo\ShopQuanAo\bin\Debug\Data\NhanVien.xml";

                try
                {
                    // Kiểm tra nếu file XML đã tồn tại
                    if (System.IO.File.Exists(xmlFilePath))
                    {
                        // Load file XML hiện tại
                        XDocument doc = XDocument.Load(xmlFilePath);

                        // Tìm phần tử NhanVien với MaNV tương ứng để sửa
                        XElement nhanVienToEdit = doc.Descendants("NhanVien")
                                                      .FirstOrDefault(x => x.Element("MaNV")?.Value == maNV);

                        if (nhanVienToEdit != null)
                        {
                            // Sửa dữ liệu của NhanVien
                            nhanVienToEdit.SetElementValue("HoTen", hoTen);
                            nhanVienToEdit.SetElementValue("DiaChi", diaChi);
                            nhanVienToEdit.SetElementValue("SDTNV", sdtNV);

                            // Lưu file XML sau khi sửa
                            doc.Save(xmlFilePath);
                            MessageBox.Show("Sửa dữ liệu trong file XML thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            QLNV_Load(sender, e);
                            // Gọi hàm UpdateSqlFromXml để cập nhật SQL Server từ file XML
                            UpdateSqlFromXml();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên với mã " + maNV, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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


        private void UpdateSqlFromXml()
        {
            string xmlFilePath = @"D:\nam3-HK124\Cong-nghe-XML-124CNX01\BaiTapLon\ShopQuanAo\ShopQuanAo\bin\Debug\Data\NhanVien.xml";
            string connectionString = @"Data Source=LAPTOP-V3R09SE1;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load dữ liệu từ file XML
                    XDocument doc = XDocument.Load(xmlFilePath);
                    XElement nhanViensElement = doc.Element("NhanViens");

                    if (nhanViensElement != null)
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            foreach (XElement employee in nhanViensElement.Elements("NhanVien"))
                            {
                                string maNV = employee.Element("MaNV")?.Value;
                                string hoTen = employee.Element("HoTen")?.Value;
                                string diaChi = employee.Element("DiaChi")?.Value;
                                string sdtNV = employee.Element("SDTNV")?.Value;

                                // Câu lệnh SQL để thêm hoặc cập nhật dữ liệu
                                string query = "IF EXISTS (SELECT 1 FROM NhanVien WHERE MaNV = @MaNV) " +
                                               "UPDATE NhanVien SET HoTen = @HoTen, DiaChi = @DiaChi, SDTNV = @SDTNV " +
                                               "WHERE MaNV = @MaNV " +
                                               "ELSE " +
                                               "INSERT INTO NhanVien (MaNV, HoTen, DiaChi, SDTNV) " +
                                               "VALUES (@MaNV, @HoTen, @DiaChi, @SDTNV)";

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@MaNV", maNV);
                                    command.Parameters.AddWithValue("@HoTen", hoTen);
                                    command.Parameters.AddWithValue("@DiaChi", diaChi);
                                    command.Parameters.AddWithValue("@SDTNV", sdtNV);

                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        MessageBox.Show("Cập nhật SQL Server từ file XML thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("File XML không chứa dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("File XML không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật SQL Server từ file XML: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btnXoaNv_Click(object sender, EventArgs e)
            {
                // Lấy mã nhân viên cần xóa từ textbox
                string maNV = txtMaNv.Text;

                // Đường dẫn đến file XML
                string xmlFilePath = @"D:\nam3-HK124\Cong-nghe-XML-124CNX01\BaiTapLon\ShopQuanAo\ShopQuanAo\bin\Debug\Data\NhanVien.xml";

                string connectionString = @"Data Source=LAPTOP-V3R09SE1;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

                try
                {
                    // Kiểm tra nếu file XML đã tồn tại
                    if (System.IO.File.Exists(xmlFilePath))
                    {
                        // Load file XML hiện tại
                        XDocument doc = XDocument.Load(xmlFilePath);

                        // Tìm phần tử NhanVien với MaNV tương ứng để xóa
                        XElement nhanVienToDelete = doc.Descendants("NhanVien")
                                                       .FirstOrDefault(x => x.Element("MaNV")?.Value == maNV);

                        if (nhanVienToDelete != null)
                        {
                            // Xóa phần tử NhanVien khỏi XML
                            nhanVienToDelete.Remove();

                            // Lưu file XML sau khi xóa
                            doc.Save(xmlFilePath);

                            // Cập nhật SQL Server - xóa nhân viên khỏi cơ sở dữ liệu
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                string deleteQuery = "DELETE FROM NhanVien WHERE MaNV = @MaNV";

                                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@MaNV", maNV);
                                    command.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("Xóa dữ liệu trong file XML và cơ sở dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            QLNV_Load(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên với mã " + maNV, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                string xmlFilePath = @"D:\nam3-HK124\Cong-nghe-XML-124CNX01\BaiTapLon\ShopQuanAo\ShopQuanAo\bin\Debug\Data\NhanVien.xml";

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

        private void btnQLSP_Click(object sender, EventArgs e)
            {
                this.Close();
                // Tạo instance của Form QLSP
                QLSP qlspForm = new QLSP();

                // Hiển thị Form QLSP
                qlspForm.Show();
            }

        private void btnQLTK_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLTK
            QLTK TKForm = new QLTK();

            // Hiển thị Form QLTK
            TKForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxSearch.Clear();
        }

        private void Searchbtn_Click(object sender, EventArgs e)
        {
            // Đường dẫn kết nối đến cơ sở dữ liệu SQL Server
            string connectionString = @"Data Source=LAPTOP-V3R09SE1;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            // Lấy nội dung trong textBoxSearch và loại bỏ khoảng trắng thừa
            string searchQuery = textBoxSearch.Text.Trim().ToLower(); // Chuyển sang chữ thường để tìm kiếm không phân biệt hoa-thường

            // Kiểm tra nếu người dùng đã nhập từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchQuery))
            {
                try
                {
                    // Câu lệnh SQL để tìm kiếm nhân viên có tên chứa từ khóa tìm kiếm
                    string sqlQuery = "SELECT MaNV, HoTen, DiaChi, SDTNV FROM NhanVien " +
                                      "WHERE LOWER(HoTen) LIKE @searchQuery COLLATE SQL_Latin1_General_CP1_CI_AS"; // Tìm kiếm không phân biệt hoa-thường

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
                                MessageBox.Show("Không tìm thấy nhân viên nào với từ khóa: " + searchQuery, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Vui lòng nhập tên nhân viên để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnQLNV_Click(object sender, EventArgs e)
        {

        }
    }
}
