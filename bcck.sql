CREATE DATABASE ShopThoiTrang1;
GO

USE ShopThoiTrang1;
GO

-- Bảng NhanVien
CREATE TABLE NhanVien (
    MaNV NCHAR(10) PRIMARY KEY,
    HoTen NVARCHAR(100),
    DiaChi NVARCHAR(200),
    SDTNV NVARCHAR(15)
);
GO

-- Bảng SanPham
CREATE TABLE SanPham (
    MaSP NCHAR(10) PRIMARY KEY,
    TenSP NVARCHAR(100),
    Loai NVARCHAR(50),
    XuatXu NVARCHAR(50),
    SoLuong INT,
    Gia DECIMAL(18, 0)
);
GO

-- Bảng HoaDon
CREATE TABLE HoaDon (
    MaHD NCHAR(10) PRIMARY KEY,
    MaNV NCHAR(10),
    NgayLap DATE,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- Bảng CTHoaDon
CREATE TABLE CTHoaDon (
    MaHD NCHAR(10),
    MaSP NCHAR(10),
    SLMua INT,
    ThanhTien DECIMAL(10, 2),
    PRIMARY KEY (MaHD, MaSP),
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- Bảng TaiKhoan
CREATE TABLE TaiKhoan (
    TaiKhoan NVARCHAR(50) PRIMARY KEY,
    MatKhau NVARCHAR(50),
    Quyen NVARCHAR(20)
);
GO

select * from HoaDon
-- Insert dữ liệu vào bảng HoaDon
	INSERT INTO HoaDon (MaHD, MaNV, NgayLap) VALUES 
	(N'HD001', N'NV002', '2024-12-15 19:14:23'),
	(N'HD002', N'NV002', '2024-12-15 19:12:57'),
	(N'HD003', N'NV001', '2024-12-15 19:26:11'),
	(N'HD004', N'NV001', '2024-12-15 21:23:47'),
	(N'HD005', N'NV001', '2024-12-15 21:57:44');
	GO

select * from SanPham

select * from nhanvien

select * from CTHoaDon
-- Chèn dữ liệu vào bảng CTHoaDon theo thứ tự MaHD
	INSERT INTO CTHoaDon (MaHD, MaSP, SLMua, ThanhTien) VALUES 
	(N'HD001', N'SP002', 2, 700000.00),
	(N'HD001', N'SP003', 2, 1600000.00),
	(N'HD002', N'SP002', 2, 700000.00),
	(N'HD003', N'SP001', 1, 210000.00);
	GO