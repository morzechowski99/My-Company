--Program powsta³ na Wydziale Informatyki Politechniki Bia³ostockiej

create or alter view OrdersToComplete
as select o.Id, o.OrderDate
from Orders o 
where Paid=1 and o.Status < 2
and not exists (select 1 
				from ProductOrders po join
				Products p on po.ProductId = p.Id
				join Orders on po.OrderId = o.Id
				where p.StockQuantity < po.Count)
and not exists (select 1
				from Picking p
				where p.OrderId = o.Id);


declare @id int
insert into Addresses (FirstName,LastName,Street,ZipCode,City,PhoneNumber) values('Jan','Kowalski','Wiejska 10/2', '15-150','Bia³ystok','+48 3243543')
set @id = SCOPE_IDENTITY()

insert into Orders (UserId,OrderDate,Comment,Status,Paid,Id,AddressId) 
values ((select top(1) Id from AspNetUsers),getdate(),null,2,1,NEWID (),@id);

insert into ProductOrders values ((select top(1) Id from Products),3,(select top(1) Id from Orders order by OrderDate DESC));
