USE Fridge;


-- Group (1)
	-- (1) 
	-- Сделать выборку продуктов по ххолодильникам, модель которых начинается на A

	SELECT * 
	FROM Fridge f
	WHERE f.Name LIKE('A%');
		
	-- (2)  
	-- Сделать выборку холодильников,
	-- в которых есть продукты в количестве, меньшем чем количество по умолчанию

	SELECT f.Id, f.Model, f.Name, f.OwnerName 
	FROM Fridge f
	JOIN FridgeProduct fp 
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	WHERE fp.Quantity < p.DefaultQuantity;

	-- (3)
	-- В каком году выпустили холодильник с наибольшей
	-- вместимостью (сумма всех продуктов больше всего)

	SELECT TOP(1) fm.Year
	FROM Fridge f
	JOIN FridgeModel fm
		ON f.Model = fm.Id
	JOIN FridgeProduct fp
		ON fp.Fridge = f.Id
	GROUP BY fm.Year, fp.Quantity
	ORDER BY SUM(fp.Quantity) DESC

	-- (4)
	-- Выбрать все продукты и имя владельца из холодильника, в
	-- котором больше всего наименований продуктов. Именно
	-- наименований, не количества

	SELECT f.OwnerName, p.Name
	FROM Fridge f
	JOIN FridgeProduct fp
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	WHERE f.Id = (
				SELECT TOP(1) f.Id as Id
				FROM Fridge f
				JOIN FridgeProduct fp 
					ON f.Id = fp.Fridge
				JOIN Product p
					ON fp.Product = p.Id
				GROUP BY f.Id
				ORDER BY Count(fp.Product) DESC);


-- Group (2)
	
	-- (1)
	-- Вывести все продукты для холодильника с id 2

	SELECT p.Name 
	FROM Fridge f
	JOIN FridgeProduct fp
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	WHERE f.Id = 2;

	-- (2)
	-- Вывести все продукты для всех холодильников

	SELECT f.Id, f.Name as [Fridge Name], p.Name 
	FROM Fridge f
	JOIN FridgeProduct fp
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	
	-- (3)
	--  вывести список холодильников и сумму всех продуктов для каждого из них
	
	SELECT f.Id, f.Name, Count(fp.Quantity) [Products qty]
	FROM Fridge f
	JOIN FridgeProduct fp
		ON f.Id = fp.Fridge
	GROUP BY f.Id, f.Name

	-- (4)
	-- вывести имя холодильника, название и год модели,
	-- а также кол-во продуктов для каждого из них
	
	SELECT 
		f.Id, f.Name as [Fridge name],
		fm.Name [Model name], fm.Year,
		Count(fp.Quantity) [Products qty]
	FROM Fridge f
	JOIN FridgeProduct fp
		ON f.Id = fp.Fridge
	JOIN FridgeModel fm
		ON fm.Id = f.Model
	GROUP BY f.Id, f.Name, fm.Name, fm.Year

	-- (5)
	-- вывести список холодильников, где содержатся продукты, 
	-- количество которых больше, чем кол-во по умолчанию

	SELECT f.Id, f.Name, f.OwnerName 
	FROM Fridge f
	JOIN FridgeProduct fp 
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	WHERE fp.Quantity > p.DefaultQuantity;

	-- (6)
	-- вывести список холодильников и для каждого холодильника
	-- кол-во наименований продуктов, количество которых больше,
	-- чем кол-во по умолчанию

	SELECT f.Id, f.Name, f.OwnerName, COUNT(fp.Product) AS [Product count]
	FROM Fridge f
	JOIN FridgeProduct fp 
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	GROUP BY f.Id, f.Name, f.OwnerName, fp.Quantity, p.DefaultQuantity
	HAVING fp.Quantity > p.DefaultQuantity;
	

