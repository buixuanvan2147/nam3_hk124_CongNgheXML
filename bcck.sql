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
