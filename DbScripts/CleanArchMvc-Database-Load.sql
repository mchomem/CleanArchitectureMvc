use CleanArchMvc

-- Bank load
delete from Product
go

delete from Category
go

dbcc checkident('Category', RESEED, 0)
go

declare @maxRowsCategories int = 0

while (@maxRowsCategories < 20)
begin
	set @maxRowsCategories = @maxRowsCategories + 1

	insert into Category([Name]) values('Category ' + cast(@maxRowsCategories as varchar))
end
go

dbcc checkident('Product', RESEED, 0)
go

declare @maxRowsProducts int = 0

while (@maxRowsProducts < 100)
begin
	set @maxRowsProducts = @maxRowsProducts + 1
	insert into Product([Name], [Description], Price, Stock, [Image], CategoryId)
	values ('Product ' + cast(@maxRowsProducts as varchar), 'Product description ' + cast(@maxRowsProducts as varchar), round(rand() * 100, 2, 0), floor(rand() * 100), null, 1)
end

select * from Product
