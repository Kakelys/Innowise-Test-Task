USE Fridge;


-- Group (1)
	-- (1) 
	-- ������� ������� ��������� �� ��������������, ������ ������� ���������� �� A

	SELECT * 
	FROM Fridge f
	WHERE f.Name LIKE('A%');
		
	-- (2)  
	-- ������� ������� �������������,
	-- � ������� ���� �������� � ����������, ������� ��� ���������� �� ���������

	SELECT f.Id, f.Model, f.Name, f.OwnerName 
	FROM Fridge f
	JOIN FridgeProduct fp 
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	WHERE fp.Quantity < p.DefaultQuantity;

	-- (3)
	-- � ����� ���� ��������� ����������� � ����������
	-- ������������ (����� ���� ��������� ������ �����)

	SELECT TOP(1) fm.Year
	FROM Fridge f
	JOIN FridgeModel fm
		ON f.Model = fm.Id
	JOIN FridgeProduct fp
		ON fp.Fridge = f.Id
	GROUP BY fm.Year, fp.Quantity
	ORDER BY SUM(fp.Quantity) DESC

	-- (4)
	-- ������� ��� �������� � ��� ��������� �� ������������, �
	-- ������� ������ ����� ������������ ���������. ������
	-- ������������, �� ����������

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
	-- ������� ��� �������� ��� ������������ � id 2

	SELECT p.Name 
	FROM Fridge f
	JOIN FridgeProduct fp
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	WHERE f.Id = 2;

	-- (2)
	-- ������� ��� �������� ��� ���� �������������

	SELECT f.Id, f.Name as [Fridge Name], p.Name 
	FROM Fridge f
	JOIN FridgeProduct fp
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	
	-- (3)
	--  ������� ������ ������������� � ����� ���� ��������� ��� ������� �� ���
	
	SELECT f.Id, f.Name, Count(fp.Quantity) [Products qty]
	FROM Fridge f
	JOIN FridgeProduct fp
		ON f.Id = fp.Fridge
	GROUP BY f.Id, f.Name

	-- (4)
	-- ������� ��� ������������, �������� � ��� ������,
	-- � ����� ���-�� ��������� ��� ������� �� ���
	
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
	-- ������� ������ �������������, ��� ���������� ��������, 
	-- ���������� ������� ������, ��� ���-�� �� ���������

	SELECT f.Id, f.Name, f.OwnerName 
	FROM Fridge f
	JOIN FridgeProduct fp 
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	WHERE fp.Quantity > p.DefaultQuantity;

	-- (6)
	-- ������� ������ ������������� � ��� ������� ������������
	-- ���-�� ������������ ���������, ���������� ������� ������,
	-- ��� ���-�� �� ���������

	SELECT f.Id, f.Name, f.OwnerName, COUNT(fp.Product) AS [Product count]
	FROM Fridge f
	JOIN FridgeProduct fp 
		ON f.Id = fp.Fridge
	JOIN Product p
		ON fp.Product = p.Id
	GROUP BY f.Id, f.Name, f.OwnerName, fp.Quantity, p.DefaultQuantity
	HAVING fp.Quantity > p.DefaultQuantity;
	

