Drop View vwFoodItemCat
go
create View vwFoodItemCats as
select i.FoodId Id , i.Name ,i.Description, c.Name Category from FoodItems i , Categories c where i.FoodCategoryId = c.Id