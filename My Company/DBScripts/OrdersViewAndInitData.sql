create or alter view OrdersToComplete
as select o.Id, o.OrderDate
from Orders o 
where Paid=1 and o.Status < 3
and not exists (select 1 
				from ProductOrders po join
				Products p on po.ProductId = p.Id
				join Orders on po.OrderId = o.Id
				where p.MagazineCount < po.Count)
and not exists (select 1
				from Picking p
				where p.OrderId = o.Id);


insert into Orders (UserId,OrderDate,Comment,StatusId,Paid,Id) 
values ((select top(1) Id from AspNetUsers),getdate(),null,2,1,NEWID ());

insert into ProductOrders values ((select top(1) Id from Products),3,(select top(1) Id from Orders order by OrderDate DESC));
