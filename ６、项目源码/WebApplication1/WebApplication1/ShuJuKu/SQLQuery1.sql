select * from YongHu where YongHuID ='10000001' and MiMa = '12345678' and BuMen = '管理员部门'
/*
insert YongHu(XingMing,MiMa,ShengRi,BuMen) values('骸骨','87654321','2006.1.1','系统部门')
*/;
select * from  GudinZiChan join CangKu on GudinZiChan.CangKuID=CangKu.CangKuID where ChuChangRiQi > '2005';
select * from  GudinZiChan join WeiXiu on GudinZiChan.GuZiID=WeiXiu.GuZiID;
select * from  GudinZiChan join Jei on GudinZiChan.GuZiID=Jei.GuZiID join Huan on GudinZiChan.GuZiID=Huan.GuZiID where ChuChangRiQi >  '2005';
select * from GudinZiChan join WeiXiu on GudinZiChan.GuZiID=WeiXiu.GuZiID where GuZiMing='电脑';
select * from  GudinZiChan join Jei on GudinZiChan.GuZiID=Jei.GuZiID join Huan on GudinZiChan.GuZiID=Huan.GuZiID
select * from CangKu where CangKuID ='{CangKuID}'