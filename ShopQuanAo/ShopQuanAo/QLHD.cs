using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;
using System.IO;


namespace ShopQuanAo
{
    public partial class QLHD : Form
    {
        public QLHD()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void QLHD_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
            // Đường dẫn tới file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\CTHoaDon.xml";

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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy dữ liệu từ dòng đã chọn
                var row = dataGridView1.SelectedRows[0];
                txtMahd.Text = row.Cells["MaSP"].Value.ToString();
                txtMasp.Text = row.Cells["TenSP"].Value.ToString();
                txtslmua.Text = row.Cells["Gia"].Value.ToString();
            }
        }


        private void UpdateSqlFromXml()
        {
            // Đường dẫn đến file XML CTHoaDon
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\CTHoaDon.xml";
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                if (System.IO.File.Exists(xmlFilePath))
                {
                    XDocument doc = XDocument.Load(xmlFilePath);

                    foreach (XElement cthdElement in doc.Descendants("CTHoaDon"))
                    {
                        string maHD = cthdElement.Element("MaHD")?.Value;
                        string maSP = cthdElement.Element("MaSP")?.Value;
                        string slMua = cthdElement.Element("SLMua")?.Value;
                        string thanhTien = cthdElement.Element("ThanhTien")?.Value;

                        if (!string.IsNullOrEmpty(maHD) && !string.IsNullOrEmpty(maSP) &&
                            !string.IsNullOrEmpty(slMua) && !string.IsNullOrEmpty(thanhTien))
                        {
                            if (int.TryParse(slMua, out int slMuaInt) && decimal.TryParse(thanhTien, out decimal thanhTienDec))
                            {
                                string sqlQuery = @"
                            IF EXISTS (SELECT 1 FROM CTHoaDon WHERE MaHD = @MaHD AND MaSP = @MaSP)
                            UPDATE CTHoaDon SET SLMua = @SLMua, ThanhTien = @ThanhTien
                            WHERE MaHD = @MaHD AND MaSP = @MaSP
                            ELSE
                            INSERT INTO CTHoaDon (MaHD, MaSP, SLMua, ThanhTien)
                            VALUES (@MaHD, @MaSP, @SLMua, @ThanhTien)";

                                using (SqlConnection conn = new SqlConnection(connectionString))
                                {
                                    conn.Open();

                                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                                    {
                                        cmd.Parameters.AddWithValue("@MaHD", maHD);
                                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                                        cmd.Parameters.AddWithValue("@SLMua", slMuaInt);
                                        cmd.Parameters.AddWithValue("@ThanhTien", thanhTienDec);

                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Dữ liệu không hợp lệ cho CTHoaDon với mã hóa đơn {maHD} và mã sản phẩm {maSP}. Số lượng hoặc thành tiền không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            DangNhap dn = new DangNhap();
            dn.Show();
        }

        private void btnQLTK_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLTK
            QLTK TKForm = new QLTK();

            // Hiển thị Form QLTK
            TKForm.Show();
        }

        private void btnQLHD_Click(object sender, EventArgs e)
        {

        }

        private void btnQLSP_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLSP
            QLSP qlspForm = new QLSP();

            // Hiển thị Form QLSP
            qlspForm.Show();
        }

        private void btnQLNV_Click(object sender, EventArgs e)
        {
            this.Close();
            // Tạo instance của Form QLSP
            QLNV qlnvForm = new QLNV();

            // Hiển thị Form QLSP
            qlnvForm.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void QLHD_Load_1(object sender, EventArgs e)
        {

        }
        private decimal CalculateThanhTien(string maSP, int slMua)
        {
            decimal giaSP = 0;

            // Truy vấn giá sản phẩm từ cơ sở dữ liệu
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";
            string query = "SELECT Gia FROM SanPham WHERE MaSP = @MaSP"; // Sửa lại tên cột Gia

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSP", maSP);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        decimal.TryParse(result.ToString(), out giaSP);
                    }
                }
            }

            // Nếu không tìm thấy giá, trả về 0
            return giaSP * slMua;
        }

        private void btnThemHD_Click(object sender, EventArgs e)
        {
            string maHD = txtMahd.Text.Trim();
            string maSP = txtMasp.Text.Trim();
            string slMua = txtslmua.Text.Trim();
            string maNV = comboBox1.SelectedValue?.ToString();

            if (string.IsNullOrEmpty(maHD) || string.IsNullOrEmpty(maSP) || string.IsNullOrEmpty(slMua) || string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các trường!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(slMua, out int slMuaInt) || slMuaInt <= 0)
            {
                MessageBox.Show("Số lượng mua phải là số nguyên dương!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra nếu cả MaHD và MaSP đã tồn tại trong bảng CTHoaDon
                    string checkBothQuery = "SELECT COUNT(*) FROM CTHoaDon WHERE MaHD = @MaHD AND MaSP = @MaSP";
                    using (SqlCommand cmdCheckBoth = new SqlCommand(checkBothQuery, conn))
                    {
                        cmdCheckBoth.Parameters.AddWithValue("@MaHD", maHD);
                        cmdCheckBoth.Parameters.AddWithValue("@MaSP", maSP);

                        int countBoth = (int)cmdCheckBoth.ExecuteScalar();
                        if (countBoth > 0)
                        {
                            MessageBox.Show("Cặp mã hóa đơn và mã sản phẩm này đã tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Thêm dữ liệu vào bảng Hóa Đơn (Cập nhật MaHD, MaNV, Ngày lập)
                    string insertHoaDonQuery = "INSERT INTO HoaDon (MaHD, MaNV, NgayLap) VALUES (@MaHD, @MaNV, @NgayLap)";
                    using (SqlCommand cmdInsertHoaDon = new SqlCommand(insertHoaDonQuery, conn))
                    {
                        cmdInsertHoaDon.Parameters.AddWithValue("@MaHD", maHD);
                        cmdInsertHoaDon.Parameters.AddWithValue("@MaNV", maNV);
                        cmdInsertHoaDon.Parameters.AddWithValue("@NgayLap", DateTime.Now); // Ngày lập là ngày hiện tại
                        cmdInsertHoaDon.ExecuteNonQuery();
                    }

                    // Thêm dữ liệu vào bảng CTHoaDon (chi tiết hóa đơn)
                    string insertCTHoaDonQuery = "INSERT INTO CTHoaDon (MaHD, MaSP, SLMua, ThanhTien) VALUES (@MaHD, @MaSP, @SLMua, @ThanhTien)";
                    using (SqlCommand cmdInsertCTHoaDon = new SqlCommand(insertCTHoaDonQuery, conn))
                    {
                        cmdInsertCTHoaDon.Parameters.AddWithValue("@MaHD", maHD);
                        cmdInsertCTHoaDon.Parameters.AddWithValue("@MaSP", maSP);
                        cmdInsertCTHoaDon.Parameters.AddWithValue("@SLMua", slMuaInt);
                        cmdInsertCTHoaDon.Parameters.AddWithValue("@ThanhTien", CalculateThanhTien(maSP, slMuaInt)); // Hàm tính thành tiền
                        cmdInsertCTHoaDon.ExecuteNonQuery();
                    }

                    // Lưu vào file HoaDon.xml
                    SaveCTHoaDonToXml(maHD, maSP, slMuaInt, CalculateThanhTien(maSP, slMuaInt));

                    MessageBox.Show("Dữ liệu đã được thêm vào cơ sở dữ liệu và HoaDon.xml thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SaveHoaDonToXml(maHD, maNV, DateTime.Now);

                    // Thông báo thành công
                    MessageBox.Show("Dữ liệu đã được thêm vào cơ sở dữ liệu và HoaDon.xml thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Load lại dữ liệu hóa đơn từ bảng Hóa Đơn
                    LoadHoaDon();  // Gọi lại phương thức để load lại dữ liệu từ bảng HoaDon
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu vào cơ sở dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            // Lấy mã sản phẩm cần xóa từ textbox
            string maHD = txtMahd.Text.Trim();
            string maSP = txtMasp.Text.Trim();  // Thêm trường mã sản phẩm (nếu cần)

            // Đường dẫn đến file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\CTHoaDon.xml";

            // Kết nối với cơ sở dữ liệu SQL Server
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                // Kiểm tra nếu file XML đã tồn tại
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML hiện tại
                    XDocument doc = XDocument.Load(xmlFilePath);

                    // Tìm phần tử CTHoaDon với MaHD và MaSP tương ứng để xóa
                    XElement cthoadonToDelete = doc.Descendants("CTHoaDon")
                                                   .FirstOrDefault(x => x.Element("MaHD")?.Value == maHD && x.Element("MaSP")?.Value == maSP);

                    if (cthoadonToDelete != null)
                    {
                        // Xóa phần tử CTHoaDon khỏi XML
                        cthoadonToDelete.Remove();

                        // Lưu lại file XML sau khi xóa
                        doc.Save(xmlFilePath);

                        // Cập nhật SQL Server - xóa dữ liệu khỏi bảng CTHoaDon
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string deleteQuery = "DELETE FROM CTHoaDon WHERE MaHD = @MaHD AND MaSP = @MaSP";

                            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                            {
                                command.Parameters.AddWithValue("@MaHD", maHD);
                                command.Parameters.AddWithValue("@MaSP", maSP);
                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Xóa hóa đơn thành công từ file XML và cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        QLHD_Load(sender, e);  // Cập nhật lại danh sách hóa đơn trên giao diện
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn với mã " + maHD + " và mã sản phẩm " + maSP, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnSuaHD_Click(object sender, EventArgs e)
        {
            string maHD = txtMahd.Text.Trim(); // Lấy mã hóa đơn từ textbox
            string maSP = txtMasp.Text.Trim(); // Lấy mã sản phẩm từ textbox
            string slMua = txtslmua.Text.Trim(); // Lấy số lượng mua từ textbox

            if (string.IsNullOrEmpty(maHD) || string.IsNullOrEmpty(maSP) || string.IsNullOrEmpty(slMua))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các trường để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Nếu có trường trống, dừng lại
            }

            // Kiểm tra số lượng mua (phải là số nguyên và lớn hơn 0)
            if (!int.TryParse(slMua, out int slMuaInt) || slMuaInt <= 0)
            {
                MessageBox.Show("Số lượng mua phải là số nguyên dương!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Đường dẫn đến file XML
            string xmlFilePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\CTHoaDon.xml";

            try
            {
                // Kiểm tra nếu file XML đã tồn tại
                if (System.IO.File.Exists(xmlFilePath))
                {
                    // Load file XML
                    XDocument doc = XDocument.Load(xmlFilePath);

                    // Tìm phần tử hóa đơn và sản phẩm tương ứng trong file XML
                    XElement itemToEdit = doc.Descendants("CTHoaDon")
                                            .FirstOrDefault(x => x.Element("MaHD")?.Value == maHD && x.Element("MaSP")?.Value == maSP);

                    if (itemToEdit != null)
                    {
                        // Cập nhật số lượng mua mới
                        itemToEdit.Element("SLMua")?.SetValue(slMuaInt);

                        // Tính lại thành tiền nếu cần
                        itemToEdit.Element("ThanhTien")?.SetValue(CalculateThanhTien(maSP, slMuaInt));

                        // Lưu lại file XML sau khi sửa
                        doc.Save(xmlFilePath);
                        MessageBox.Show("Hóa đơn đã được sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Cập nhật lại UI hoặc gọi hàm load dữ liệu nếu cần
                        QLHD_Load(sender, e);  // Ví dụ gọi lại hàm load dữ liệu hóa đơn

                        // Cập nhật cơ sở dữ liệu SQL Server từ XML
                        UpdateSqlFromXml();  // Giả sử bạn đã có hàm này để cập nhật SQL từ XML
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn hoặc sản phẩm này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("File XML không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa hóa đơn: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void txtslmua_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                // Câu lệnh SQL để lấy dữ liệu từ bảng CTHoaDon (hoặc bảng bạn cần load)
                string sqlQuery = "SELECT MaHD, MaSP, SLMua, ThanhTien FROM CTHoaDon";

                // Tạo kết nối đến cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Tạo câu lệnh SQL
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        // Tạo DataTable để lưu trữ dữ liệu
                        DataTable dt = new DataTable();

                        // Sử dụng SqlDataAdapter để điền dữ liệu vào DataTable
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);  // Điền dữ liệu vào DataTable
                        }

                        // Gán DataTable vào DataGridView để hiển thị
                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Searchbtn_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            // Lấy nội dung trong textBoxSearch và loại bỏ khoảng trắng thừa
            string searchQuery = textBoxSearch.Text.Trim(); // Giữ nguyên nội dung tìm kiếm

            // Kiểm tra nếu người dùng đã nhập từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchQuery))
            {
                try
                {
                    // Câu lệnh SQL chỉ truy vấn bảng CTHoaDon và tìm kiếm theo MaHD hoặc MaSP
                    string sqlQuery = "SELECT c.MaHD, c.MaSP, c.SLMua, c.ThanhTien " +
                                      "FROM CTHoaDon c " +
                                      "WHERE c.MaHD = @searchQuery OR c.MaSP = @searchQuery";

                    // Tạo kết nối tới cơ sở dữ liệu SQL Server
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        // Mở kết nối
                        conn.Open();

                        // Tạo câu lệnh SQL với tham số
                        using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                        {
                            // Thêm tham số tìm kiếm
                            cmd.Parameters.AddWithValue("@searchQuery", searchQuery);

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
                                MessageBox.Show("Không tìm thấy hóa đơn hoặc sản phẩm với từ khóa: " + searchQuery, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Vui lòng nhập mã hóa đơn hoặc mã sản phẩm để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxSearch.Clear();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void LoadNhanVien()
        {
            string connectionString = "Data Source=.;Initial Catalog=ShopThoiTrang1;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MaNV, HoTen FROM NhanVien";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Gán dữ liệu vào ComboBox1
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "HoTen"; // Hiển thị tên nhân viên
                comboBox1.ValueMember = "MaNV";   // Lấy mã nhân viên
            }
        }
        private void LoadHoaDon()
        {
            string connectionString = @"Data Source=TranDinhViet;Initial Catalog=ShopThoiTrang1;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaHD, MaNV, NgayLap FROM HoaDon"; // Truy vấn từ bảng HoaDon
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    //// Gán dữ liệu vào DataGridView (giả sử bạn có DataGridView tên dgvHoaDon)
                    //dgvHoaDon.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu hóa đơn: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveHoaDonToXml(string maHD, string maNV, DateTime ngayLap)
        {
            string filePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\HoaDon.xml"; // Đường dẫn file HoaDon.xml

            // Load file XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Tạo phần tử mới cho hóa đơn
            XmlElement hoaDonElement = xmlDoc.CreateElement("HoaDon");

            XmlElement maHDElement = xmlDoc.CreateElement("MaHD");
            maHDElement.InnerText = maHD;
            hoaDonElement.AppendChild(maHDElement);

            XmlElement maNVElement = xmlDoc.CreateElement("MaNV");
            maNVElement.InnerText = maNV;
            hoaDonElement.AppendChild(maNVElement);

            XmlElement ngayLapElement = xmlDoc.CreateElement("NgayLap");
            ngayLapElement.InnerText = ngayLap.ToString("yyyy-MM-dd HH:mm:ss");
            hoaDonElement.AppendChild(ngayLapElement);

            // Thêm phần tử mới vào trong root
            xmlDoc.DocumentElement.AppendChild(hoaDonElement);

            // Lưu lại file XML
            xmlDoc.Save(filePath);
        }
        private void SaveCTHoaDonToXml(string maHD, string maSP, int slMua, decimal thanhTien)
        {
            string filePath = @"C:\Users\ahiha\source\repos\ShopQuanAo\ShopQuanAo\ShopQuanAo\bin\Debug\Data\CTHoaDon.xml"; // Đường dẫn file CTHoaDons.xml

            // Nếu file CTHoaDons.xml không tồn tại, tạo mới

            // Load file XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Tạo phần tử mới cho chi tiết hóa đơn
            XmlElement chiTietElement = xmlDoc.CreateElement("CTHoaDon");

            XmlElement maHDElement = xmlDoc.CreateElement("MaHD");
            maHDElement.InnerText = maHD;
            chiTietElement.AppendChild(maHDElement);

            XmlElement maSPEl = xmlDoc.CreateElement("MaSP");
            maSPEl.InnerText = maSP;
            chiTietElement.AppendChild(maSPEl);

            XmlElement slMuaElement = xmlDoc.CreateElement("SLMua");
            slMuaElement.InnerText = slMua.ToString();
            chiTietElement.AppendChild(slMuaElement);

            XmlElement thanhTienElement = xmlDoc.CreateElement("ThanhTien");
            thanhTienElement.InnerText = thanhTien.ToString("F2");
            chiTietElement.AppendChild(thanhTienElement);

            // Thêm phần tử mới vào trong root
            xmlDoc.DocumentElement.AppendChild(chiTietElement);

            // Lưu lại file XML
            xmlDoc.Save(filePath);
        }

        private void xuatHoaDon_Click(object sender, EventArgs e)
        {
            string filePath = "C:\\Users\\ahiha\\source\\repos\\ShopQuanAo\\ShopQuanAo\\ShopQuanAo\\bin\\Debug\\Data\\CTHoaDon.xml"; // Đường dẫn đến file XML

            if (File.Exists(filePath))
            {
                // Đọc nội dung XML
                string xmlContent = File.ReadAllText(filePath);

                // Chuyển đổi XML thành HTML với kiểu dáng bảng
                string htmlContent = ConvertXmlToHtml(xmlContent);

                // Lưu nội dung HTML vào file
                string htmlFilePath = Path.Combine(Application.StartupPath, "HoaDon.html");
                File.WriteAllText(htmlFilePath, htmlContent);

                // Mở file HTML trong trình duyệt mặc định
                System.Diagnostics.Process.Start(htmlFilePath);
            }
            else
            {
                MessageBox.Show("Không tìm thấy file XML!");
            }
        }

        private string ConvertXmlToHtml(string xmlContent)
        {
            // Tạo HTML cơ bản với CSS
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html><head><title>Hóa Đơn</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; background-color: #f4f4f9; }");
            html.AppendLine("h1 { text-align: center; color: #333; }");
            html.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
            html.AppendLine("th, td { padding: 8px; text-align: left; border: 1px solid #ddd; }");
            html.AppendLine("th { background-color: #4CAF50; color: white; }");
            html.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
            html.AppendLine("tr:hover { background-color: #ddd; }");
            html.AppendLine("</style>");
            html.AppendLine("</head><body>");
            html.AppendLine("<h1>Danh Sách Hóa Đơn</h1>");
            html.AppendLine("<table><tr><th>Mã Hóa Đơn</th><th>Mã Sản Phẩm</th><th>Số Lượng</th><th>Thành Tiền</th></tr>");

            // Dùng XmlDocument để phân tích cú pháp XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            // Duyệt qua các phần tử <CTHoaDon> trong XML
            XmlNodeList nodes = xmlDoc.SelectNodes("//CTHoaDon");
            foreach (XmlNode node in nodes)
            {
                string maHD = node["MaHD"]?.InnerText ?? "";
                string maSP = node["MaSP"]?.InnerText ?? "";
                string slMua = node["SLMua"]?.InnerText ?? "";
                string thanhTien = node["ThanhTien"]?.InnerText ?? "";

                // Thêm dòng dữ liệu vào bảng HTML
                html.AppendLine($"<tr><td>{maHD}</td><td>{maSP}</td><td>{slMua}</td><td>{thanhTien}</td></tr>");
            }

            html.AppendLine("</table></body></html>");
            return html.ToString();
        }
    }
}
     
