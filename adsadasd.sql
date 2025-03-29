select * from Admin;
UPDATE Admin 
SET UserAdmin = 'nguyenquocanh', PassAdmin = CONVERT(VARCHAR(32), HASHBYTES('MD5', 'anhanh123'), 2)
WHERE UserAdmin = 'admin';
select * from LienHe

delete from LienHe where malienhe = 1;

select * from Laptop;
select * from DonHang;
TRUNCATE table DonHang;
TRUNCATE table ChiTietDonHang;

delete from DonHang where madon = 3;
delete from DonHang where madon = 1;