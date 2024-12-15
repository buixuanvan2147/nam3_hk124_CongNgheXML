using System;
using System.Windows.Forms;

namespace ShopQuanAo
{
    internal class Program
    {
        // Phương thức Main, nơi ứng dụng bắt đầu
        [STAThread]  // Thêm thuộc tính này để đảm bảo ứng dụng chạy đúng với Windows Forms
        static void Main()
        {
            // Cấu hình ứng dụng và chạy form đăng nhập
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DangNhap());  // Khởi động form DangNhap
        }
    }
}
