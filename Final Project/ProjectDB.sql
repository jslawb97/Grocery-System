/* Check if database already exists and delete it if it does exist*/
IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'finalproject') 
BEGIN
	DROP DATABASE finalproject
	print '' print '*** dropping database finalproject'
END
GO

print '' print '*** creating database finalproject'
GO
CREATE DATABASE finalproject
GO

print '' print '*** using database finalproject'
GO
USE [finalproject]
GO

print '' print '*** Creating Employee Table'
GO
/* ****** Table [dbo].[Employee] ****** */
CREATE TABLE [dbo].[Employee](
	[EmployeeID]		[nvarchar](20)				NOT NULL,
	[PhoneNumber]		[nvarchar](15)				NOT NULL,
	[FirstName]			[nvarchar](50)				NOT NULL,
	[LastName]			[nvarchar](100)				NOT NULL,
	[Email]				[nvarchar](100)				NOT NULL,
	[PasswordHash]		[nvarchar](100)				NOT NULL DEFAULT '9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e',
	[Active]			[bit]						NOT NULL DEFAULT 1,
	CONSTRAINT [pk_EmployeeID] PRIMARY KEY([EmployeeID] ASC),
	CONSTRAINT [ak_EmployeeEmail] UNIQUE ([Email] ASC)
)
GO

print '' print '*** Creating Index for Employee.Email'
GO
CREATE NONCLUSTERED INDEX [ix_Employee_Email] ON [dbo].[Employee]([Email]);
GO

print '' print '*** Inserting Employee Records'
GO
INSERT INTO [dbo].[Employee]
		([EmployeeID], [FirstName], [LastName], [PhoneNumber], [Email])
	VALUES
		('Employee001', 'Jacob', 'Slaubaugh', '3191111111', 'jacobslaubaugh@jwsfoods.com'),
		('Employee002', 'Jeff', 'Marner', '3192222222', 'jeffmarner@jwsfoods.com'),
		('Employee003', 'Mike', 'Schnoebelen', '3193333333', 'mikeschnoebelen@jwsfoods.com'),
		('Employee004', 'Carson', 'Christiansen', '3194444444', 'carsonchristiansen@jwsfoods.com'),
		('Employee005', 'Curt', 'Wyse', '3195555555', 'curtwyse@jwsfoods.com'),
		('Employee006', 'Jonathan', 'Wyse', '3196666666', 'jonathanwyse@jwsfoods.com'),
		('Employee007', 'Wanda', 'Ropp', '3197777777', 'wandaropp@jwsfoods.com')
GO

print '' print '*** Creating Title Table'
GO
/* ****** Table [dbo].[Title] ****** */
CREATE TABLE [dbo].[Title](
	[TitleID]			[nvarchar](30)			NOT NULL,
	[Description]	[nvarchar](1000)		NOT NULL,
	CONSTRAINT [pk_TitleID] PRIMARY KEY([TitleID] ASC)
)
GO

print '' print '*** Inserting Title Records'
GO
INSERT INTO [dbo].[Title]
		([TitleID], [Description])
	VALUES
		('Manager', 'Does office and book work, also oversees all employees'),
		('Supervisor', 'Gives out jobs and takes care of customer service'),
		('Carryout', 'Carries groceries out for customers'),
		('Checkout', 'Scans items and receives customer payment'),
		('Trainer', 'Trains new employees')
GO

print '' print '*** Creating EmployeeTitle Table'
GO
/* ****** Table [dbo].[EmployeeTitle] ****** */
CREATE TABLE [dbo].[EmployeeTitle] (
	[EmployeeID]	[nvarchar](20)		NOT NULL,
	[TitleID]		[nvarchar](30)		NOT NULL,
	[Active]		bit					NOT NULL DEFAULT 1
	CONSTRAINT [pk_EmployeeIDRoleID] PRIMARY KEY([EmployeeID] ASC, [TitleID] ASC)
)
GO

print '' print '*** Inserting EmployeeTitle Records'
GO
INSERT INTO [dbo].[EmployeeTitle]
		([EmployeeID], [TitleID])
	VALUES
		('Employee001', 'Supervisor'),
		('Employee002', 'Carryout'),
		('Employee003', 'Supervisor'),
		('Employee004', 'Checkout'),
		('Employee005', 'Manager'),
		('Employee006', 'Manager'),
		('Employee007', 'Checkout')
GO

print '' print '*** Creating Department Table'
GO
/* ****** Table [dbo].[Department] ****** */
CREATE TABLE [dbo].[Department](
	[DepartmentID]		[int] IDENTITY (100000,1)	NOT NULL,
	[Name]				[nvarchar](30)				NOT NULL,
	[Description]		[nvarchar](1000)			NOT NULL,
	[Active]			[bit]						NOT NULL DEFAULT 1,
	CONSTRAINT [pk_DepartmentID] PRIMARY KEY([DepartmentID] ASC)
)
GO

print '' print '*** Inserting Department Records'
GO
INSERT INTO [dbo].[Department]
		([Name], [Description])
	VALUES
		('Meat', 'Takes care of fresh and frozen meat'),
		('Produce', 'Takes care of fruits and vegetables'),
		('Dairy', 'Takes care of any dairy products and other refriderated items'),
		('Frozen', 'Takes care of any frozen products excluding frozen meat'),
		('Dry Grocery', 'Takes care of dry goods that dont need to be refriderated'),
		('HBC', 'Takes care of soap, paper and medicine')
GO

print '' print '*** Creating EmployeeDepartment Table'
GO
/* ****** Table [dbo].[EmployeeDepartment] ****** */
CREATE TABLE [dbo].[EmployeeDepartment](
	[EmployeeID]		[nvarchar](30)				NOT NULL,
	[DepartmentID]		[nvarchar](30)		NOT NULL,
	CONSTRAINT [pk_EmployeeIDDepartmentID] PRIMARY KEY([EmployeeID] ASC, [DepartmentID] ASC)
)
GO

print '' print '*** Inserting EmployeeDepartment Records'
GO
INSERT INTO [dbo].[EmployeeDepartment]
		([EmployeeID], [DepartmentID])
	VALUES
		('Employee001', 'Meat'),
		('Employee002', 'Produce'),
		('Employee003', 'Dairy'),
		('Employee005', 'Dry Grocery'),
		('Employee006', 'Frozen'),
		('Employee007', 'HBC')
GO

print '' print '*** Creating Customer Table'
GO
/* ****** Table [dbo].[Customer] ****** */
CREATE TABLE [dbo].[Customer](
	[CustomerID]		[int] IDENTITY (100000,1)	NOT NULL,
	[PhoneNumber]		[nvarchar](15)				NOT NULL,
	[FirstName]			[nvarchar](50)				NOT NULL,
	[LastName]			[nvarchar](100)				NOT NULL,
	[Email]				[nvarchar](100)				NOT NULL,
	[PasswordHash]		[nvarchar](100)				NOT NULL DEFAULT '9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e',
	[Active]			[bit]						NOT NULL DEFAULT 1,
	CONSTRAINT [pk_CustomerID] PRIMARY KEY([CustomerID] ASC),
	CONSTRAINT [ak_CustomerEmail] UNIQUE ([Email] ASC)
)
GO

print '' print '*** Inserting Customer Records'
GO
INSERT INTO [dbo].[Customer]
		([FirstName], [LastName], [PhoneNumber], [Email])
	VALUES
		('Irene', 'Lastname', '3196561111', 'irene@gmail.com'),
		('Ila', 'Lastname', '3196562222', 'ila@gmail.com'),
		('John', 'Lastname', '3196563333', 'john@gmail.com'),
		('Josh', 'Lastname', '3196564444', 'josh@gmail.com'),
		('Selina', 'Lastname', '3196565555', 'selina@gmail.com'),
		('Marshall', 'Lastname', '3196566666', 'marshall@gmail.com'),
		('Eric', 'Lastname', '3196567777', 'eric@gmail.com')
GO

print '' print '*** Creating CustomerOrder Table'
GO
/* ****** Table [dbo].[CustomerOrder] ****** */
CREATE TABLE [dbo].[CustomerOrder](
	[OrderID]			[int] IDENTITY (100000,1)	NOT NULL,
	[CustomerID]		[int]						NOT NULL,
	[DepartmentID]		[int]						NOT NULL,	
	[Description]		[varchar](1000)				NOT NULL,
	[PickupDate]		[varchar](50)				NOT NULL,
	[Active]			bit							NOT NULL DEFAULT 1,
	CONSTRAINT [pk_OrderID] PRIMARY KEY([OrderID] ASC)
)
GO

print '' print '*** Inserting CustomerOrder Records'
GO
INSERT INTO [dbo].[CustomerOrder]
		([CustomerID], [DepartmentID], [Description], [PickupDate])
	VALUES
		(100000, 100000, '30 lbs of thin cut arm steaks seasoned to perfection with steak seasoning', '12-24-2017'),
		(100000, 100002, '10 Gallons of the worlds best organic milk', '12-24-2017'),
		(100000, 100004, '4 Cases of Hunts Pasta Sauce', '12-24-2017'),
		(100000, 100000, '35lbs of Ribeye, ready to go straight to the grill', '12-24-2017'),
		(100000, 100004, '12 Cases of the most colorful sherbert ice cream', '12-24-2017')
GO
		
print '' print '*** Creating Products Table'
GO
/* ****** Table [dbo].[Items] ****** */
CREATE TABLE [dbo].[Products](
	[UPC]			[int] IDENTITY (1116100000,1)	NOT NULL,
	[DepartmentID]	[int]							NOT NULL,
	[Name]			[nvarchar](100)					NOT NULL,
	[Manufacturer]	[nvarchar](100)					NOT NULL,
	[OnHand]		[int]							NOT NULL,
	[Cost]			[decimal](4,2)					NOT NULL DEFAULT 0.00,
	[Active]		bit								NOT NULL DEFAULT 1,
	CONSTRAINT [pk_UPC] PRIMARY KEY([UPC] ASC)
)
GO

print '' print '*** Inserting Product Records'
GO
INSERT INTO [dbo].[Products]
		([Name], [DepartmentID], [Manufacturer], [OnHand], [Cost])
	VALUES
		('Nyquil', 100005, 'Procter & Gamble', 6, 9.99),
		('Advil', 100005, 'Procter & Gamble', 10, 11.99),
		('Dayquil', 100005, 'Procter & Gamble', 6, 13.99),
		('Bandaids', 100005, 'Procter & Gamble', 12, 12.99),
		('Emergen-C', 100005, 'Procter & Gamble', 24, 9.99),
		('Nicotine Gum', 100005, 'Procter & Gamble', 24, 5.99),
		('Vicks VapoRub', 100005, 'Procter & Gamble', 24, 6.99),
		('Halls', 100005, 'Procter & Gamble', 24, 4.99),
		('Robitussin', 100005, 'Procter & Gamble', 24, 8.99),
		('Mucus Relief', 100005, 'Procter & Gamble', 24, 11.99),
		('Coricidin', 100005, 'Procter & Gamble', 24, 12.99),
		('Theraflu', 100005, 'Procter & Gamble', 24, 14.99),
		('Tylenol', 100005, 'Procter & Gamble', 8, 11.99),
		('Alka-Seltzer Plus', 100005, 'Procter & Gamble', 4, 11.99),
		('Ricola', 100005, 'Procter & Gamble', 24, 5.99),
		('Chicken Noodle Soup', 100004, 'Cambells', 24, 1.99),
		('Cream Of Mushroom Soup', 100004, 'Cambells', 24, 1.99),
		('Cream of Chicken Soup', 100004, 'Cambells', 24, 1.99),
		('All Purpose Flour', 100004, 'Best Choice', 24, 2.99),
		('Sugar', 100004, 'C&H', 24, 4.99),
		('Mac & Cheese', 100004, 'Kraft', 24, 0.99),
		('Ketchup', 100004, 'Heinz', 24, 1.99),
		('Ranch', 100004, 'Hidden Valley', 24, 2.99),
		('Dill Pickles', 100004, 'Vlastic', 24, 3.99),
		('Cream Of Celery Soup', 100004, 'Cambells', 24, 1.99),
		('Lucky Charms', 100004, 'General Mills', 24, 5.99),
		('Frosted Flakes', 100004, 'Kelloggs', 24, 5.99),
		('Honey Nut Cherios', 100004, 'General Mills', 24, 5.99),
		('Cherry Poptarts', 100004, 'General Mills', 24, 2.99),
		('Oatmeal', 100004, 'Quaker', 24, 2.99),
		('Canned Whole Tomatos', 100004, 'Best Choice', 24, 2.99),
		('Canned Corn', 100004, 'Best Choice', 24, 0.99),
		('Dark Italian Coffee', 100004, 'Best Choice', 24, 11.99),
		('Cranberry Juice', 100004, 'Best Choice', 24, 3.99),
		('Apple Juice', 100004, 'Best Best Choice', 24, 3.99),
		('Sweet Baby Pickles', 100004, 'Best Choice', 24, 4.99),
		('BBQ Sauce', 100004, 'Best Choice', 24, 3.99),
		('Frozen Corn', 100003, 'Best Choice', 24, 2.99),
		('Fast Food French Fries', 100003, 'Ore Ida', 24, 4.99),
		('Frozen Cherrys', 100003, 'Best Choice', 24, 8.99),
		('Cool Whip', 100003, 'Craft', 24, 1.99),
		('Jacks Cheese Pizza', 100003, 'Jacks', 24, 2.99),
		('Chocolate Ice Cream Pail', 100003, 'Blue Bunny', 24, 6.99),
		('Frozen Brocoli', 100003, 'Best Choice', 24, 2.99),
		('Banquet Chicken Nugget Meal', 100003, 'Banquet', 24, 0.99),
		('Shredded Cheddar Cheese', 100002, 'Kraft', 24, 2.99),
		('Fat Free Milk', 100002, 'A&E', 24, 3.99),
		('1% Milk', 100002, 'A&E', 24, 3.99),
		('2% Milk', 100002, 'A&E', 24, 3.99),
		('Whole Milk', 100002, 'A&E', 24, 3.99),
		('Swiss Cheese Block', 100002, 'Kraft', 24, 3.99),
		('Salted Butter', 100002, 'Best Choice', 24, 6.99),
		('Vinalla Yogurt', 100002, 'A&E', 24, 5.99),
		('Dozen Eggs', 100002, 'Best Choice', 24, 1.99),
		('Lettuce Head', 100001, 'Dole', 24, 0.99),
		('Tomatoes', 100001, 'Mexico', 24, 0.99),
		('Bananas', 100001, 'Mexico', 24, 0.99),
		('Apples', 100001, 'Mexico', 24, 0.99),
		('Pears', 100001, 'Mexico', 24, 0.99),
		('Lemons', 100001, 'Mexico', 24, 0.99),
		('Carrots', 100001, 'Mexico', 24, 0.99),
		('Garlic', 100001, 'Mexico', 24, 0.99),
		('Ginger', 100001, 'Mexico', 24, 0.99),
		('Corndogs', 100000, 'Bar-S', 24, 4.99),
		('Beef & Bean Burritos', 100000, 'Cambells', 24, 0.99),
		('Cheese & Bean Burritos', 100000, 'Cambells', 24, 0.99),
		('Top Sirloin Steaks', 100000, 'Star Ranch', 24, 7.99),
		('T-Bone Steaks', 100000, 'Star Ranch', 24, 9.99),
		('Porterhouse Steaks', 100000, 'Star Ranch', 24, 10.99),
		('Ribeye Steaks', 100000, 'Star Ranch', 24, 13.99),
		('Boneless Pork Sirloin Chops', 100000, 'Hormel', 24, 2.99),
		('Pork Sausage', 100000, 'Hormel', 24, 2.99),
		('Lean Ground Pork', 100000, 'Hormel', 24, 1.99),
		('Bun-Length Hot Dos', 100000, 'Cambells', 24, 1.99),
		('Regular Hot Dogs', 100000, 'Oscar Mayer', 24, 1.99),
		('Deli Shaved Turkey Breast', 100000, 'Cambells', 24, 3.99),
		('Lunchables', 100000, 'Oscar Mayer', 24, 3.99),
		('Beer Brats', 100000, 'Oscar Mayer', 24, 4.99),
		('Liverwurst', 100000, 'Hormel', 24, 6.99),
		('Jimmy Deans Hot Sausage', 100000, 'Mr Dean', 24, 2.99),
		('Jumbo Wieners', 100000, 'Bar-S', 24, 2.99),
		('Bacon', 100000, 'Hormel', 24, 8.99),
		('Think Sliced Bacon', 100000, 'Farmland', 24, 8.99),
		('Ham Cubes', 100000, 'Farmland', 24, 6.99),
		('Frozen Chicken Breast', 100000, 'Best Choice', 24, 12.99),
		('Frozen Chicken Tenders', 100000, 'Best Choice', 24, 12.99)
GO

print '' print '*** Creating Product DailyTasks Table'
GO
/* ****** Table [dbo].[DailyTasks] ****** */
CREATE TABLE [dbo].[DailyTasks](
	[TaskID]		[int] IDENTITY (100000,1)	NOT NULL,
	CONSTRAINT [pk_TaskID] PRIMARY KEY([TaskID] ASC)
)
GO

print '' print '*** Creating Suppliers Table'
GO
/* ****** Table [dbo].[Suppliers] ****** */
CREATE TABLE [dbo].[Suppliers](
	[SuppliersID]		[int] IDENTITY (100000,1)	NOT NULL,
	CONSTRAINT [pk_SuppliersID] PRIMARY KEY([SuppliersID] ASC)
)
GO

/* ****** Foreign Key Constraints ****** */

print '' print '*** Creating EmployeeTitle EmployeeID foreign key'
GO
ALTER TABLE [dbo].[EmployeeTitle]  WITH NOCHECK 
	ADD CONSTRAINT [FK_EmployeeID] FOREIGN KEY([EmployeeID])
	REFERENCES [dbo].[Employee] ([EmployeeID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating EmployeeTitle TitleID foreign key'
GO
ALTER TABLE [dbo].[EmployeeTitle] WITH NOCHECK 
	ADD CONSTRAINT [FK_TitleID] FOREIGN KEY([TitleID])
	REFERENCES [dbo].[Title] ([TitleID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating Product DepartmentID foreign key'
GO
ALTER TABLE [dbo].[Products] WITH NOCHECK 
	ADD CONSTRAINT [FK_DepartmentID] FOREIGN KEY([DepartmentID])
	REFERENCES [dbo].[Department] ([DepartmentID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating CustomerOrder CustomerID foreign key'
GO
ALTER TABLE [dbo].[CustomerOrder] WITH NOCHECK 
	ADD CONSTRAINT [FK_CustomerID] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[Customer] ([CustomerID])
	ON UPDATE CASCADE
GO

print '' print '*** Creating CustomerOrder DepartmentID foreign key'
GO
ALTER TABLE [dbo].[CustomerOrder] WITH NOCHECK 
	ADD CONSTRAINT [FK_OrderDepartmentID] FOREIGN KEY([DepartmentID])
	REFERENCES [dbo].[Department] ([DepartmentID])
	ON UPDATE CASCADE
GO

/* ****** Stored Procedures ******* */

print '' print '*** Creating sp_update_employee_email'
GO
CREATE PROCEDURE [dbo].[sp_update_employee_email]
	(
	@EmployeeID		[nvarchar](20),
	@Email			[nvarchar](100)
	)
AS
	BEGIN
		UPDATE [dbo].[Employee]
			SET		[Email] = @Email
			WHERE	[EmployeeID] = @EmployeeID
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_authenticate_user'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user]
	(
	@Email			[nvarchar](100),
	@PasswordHash	[nvarchar](100)
	)
AS
	BEGIN
		SELECT COUNT(EmployeeID)
		FROM	[Employee]
		WHERE	[Email] = @Email
		AND		[PasswordHash] = @PasswordHash
	END
GO

print '' print '*** Creating sp_retrieve_employee_titles'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_employee_titles]
	(
	@EmployeeID		[nvarchar](20)
	)
AS
	BEGIN
		SELECT	[TitleID]
		FROM	[EmployeeTitle]
		WHERE	[EmployeeTitle].[EmployeeID] = @EmployeeID
		AND		[Active] = 1
	END
GO

print '' print '*** Creating sp_update_passwordHash'
GO
CREATE PROCEDURE [dbo].[sp_update_passwordHash]
	(
	@EmployeeID			nvarchar(20),
	@OldPasswordHash	nvarchar(100),
	@NewPasswordHash	nvarchar(100)
	)
AS
	BEGIN
		UPDATE [Employee]
			SET 	[PasswordHash] = @NewPasswordHash
			WHERE 	[EmployeeID] = @EmployeeID
			AND 	[PasswordHash] = @OldPasswordHash
		RETURN @@ROWCOUNT
	END
GO
		
print '' print '*** Creating sp_retrieve_employee_by_email'
GO
CREATE PROCEDURE [dbo].[sp_retrieve_employee_by_email]
	(
	@Email		[nvarchar](100)
	)
AS
	BEGIN
		SELECT 	[EmployeeID], [FirstName], [LastName], [PhoneNumber], [Email], [Active]
		FROM	[Employee]
		WHERE 	[Email] = @Email
	END
GO

print '' print '*** Creating sp_update_employee'
GO
CREATE PROCEDURE [dbo].[sp_update_employee]
	(
	@FirstName		[nvarchar](50),
	@LastName		[nvarchar](100),
	@PhoneNumber	[nvarchar](15),
	@Email			[nvarchar](100),
	@OldFirstName	[nvarchar](50),
	@OldLastName	[nvarchar](100),
	@OldPhoneNumber	[nvarchar](15),
	@OldEmail		[nvarchar](100),
	@EmployeeID		[nvarchar](20)
	)
AS
	BEGIN
		UPDATE [Employee]
			SET		[FirstName] = @FirstName,
					[LastName] = @LastName,
					[PhoneNumber] = @PhoneNumber,
					[Email] = @Email
			WHERE	[EmployeeID] = @EmployeeID
			AND		[FirstName] = @OldFirstName
			AND		[LastName] = @OldLastName
			AND		[PhoneNumber] = @OldPhoneNumber
			AND		[Email] = @OldEmail
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_deactivate_employee'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_employee]
	(
	@EmployeeID			[nvarchar](20)
	)
AS
	BEGIN
		UPDATE [Employee]
			SET 	[Active] = 0
			WHERE 	[EmployeeID] = @EmployeeID
		Return @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_add_employee'
GO
CREATE PROCEDURE [dbo].[sp_add_employee]
	(
	@EmployeeID			[nvarchar](20),
	@FirstName			[nvarchar](50),
	@LastName			[nvarchar](100),
	@PhoneNumber		[nvarchar](15),
	@Email				[nvarchar](100)
	)
AS
	BEGIN
		INSERT INTO [dbo].[Employee]
			([EmployeeID], [FirstName], [LastName], [PhoneNumber], [Email])
		VALUES
			(@EmployeeID, @FirstName, @LastName, @PhoneNumber, @Email)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** Creating sp_select_department_by_active'
GO
CREATE PROCEDURE [dbo].[sp_select_department_by_active]
	(
		@Active		[bit]
	)
AS
	BEGIN
		SELECT 		[DepartmentID],[Name],[Description],[Active]
		FROM 		[Department]
		WHERE 		[Active] = @Active
		ORDER BY 	[DepartmentID]
	END
GO

print '' print '*** Creating sp_select_products_by_active'
GO
CREATE PROCEDURE [dbo].[sp_select_products_by_active]
	(
		@Active		[bit]
	)
AS
	BEGIN
		SELECT 		[UPC],[DepartmentID],[Name],[Manufacturer],[OnHand],[Cost],[Active]
		FROM 		[Products]
		WHERE 		[Active] = @Active
		ORDER BY 	[UPC]
	END
GO

print '' print '*** Creating sp_select_customers_by_active'
GO
CREATE PROCEDURE [dbo].[sp_select_customers_by_active]
	(
		@Active		[bit]
	)
AS
	BEGIN
		SELECT 		[CustomerID],[PhoneNumber],[FirstName],[LastName],[Email],[Active]
		FROM 		[Customer]
		WHERE 		[Active] = @Active
		ORDER BY 	[CustomerID]
	END
GO

print '' print '*** Creating sp_select_products_by_department'
GO
CREATE PROCEDURE [dbo].[sp_select_products_by_department]
	(
		@DepartmentID	[int]
	)
AS
	BEGIN
		SELECT 		[UPC],[DepartmentID],[Name],[Manufacturer],[OnHand],[Cost],[Active]
		FROM 		[Products]
		WHERE 		[DepartmentID] = @DepartmentID
		ORDER BY 	[UPC]
	END
GO

print '' print '*** Creating sp_select_orders_by_active'
GO
CREATE PROCEDURE [dbo].[sp_select_orders_by_active]
	(
		@Active		[bit]
	)
AS
	BEGIN
		SELECT 		[OrderID],[CustomerID],[DepartmentID],[Description],[PickupDate],[Active]
		FROM 		[CustomerOrder]
		WHERE 		[Active] = @Active
		ORDER BY 	[OrderID]
	END
GO

print '' print '*** Creating sp_select_deactived_orders'
GO
CREATE PROCEDURE [dbo].[sp_select_deactived_orders]
	(
		@Active		[bit]
	)
AS
	BEGIN
		SELECT 		[OrderID],[CustomerID],[DepartmentID],[Description],[PickupDate],[Active]
		FROM 		[CustomerOrder]
		WHERE 		[Active] = @Active
		ORDER BY 	[OrderID]
	END
GO

print '' print '*** Creating sp_select_employee_by_active'
GO
CREATE PROCEDURE [dbo].[sp_select_employee_by_active]
	(
		@Active		[bit]
	)
AS
	BEGIN
		SELECT 		[EmployeeID],[PhoneNumber],[FirstName],[LastName],[Email],[Active]
		FROM 		[Employee]
		WHERE 		[Active] = @Active
		ORDER BY 	[EmployeeID]
	END
GO

print '' print '*** Creating sp_insert_product'
GO
CREATE PROCEDURE [dbo].[sp_insert_product]
	(
	@DepartmentID	[int],
	@Name			[nvarchar](100),
	@Manufacturer	[nvarchar](100),
	@OnHand			[int],
	@Cost			[decimal](4,2)
	)
AS
	BEGIN
		INSERT INTO [dbo].[Products]
			([DepartmentID], [Name], [Manufacturer], [OnHand], [Cost])
		VALUES
			(@DepartmentID, @Name, @Manufacturer, @OnHand, @Cost)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** Creating sp_select_products_by_ID'
GO
CREATE PROCEDURE [dbo].[sp_select_products_by_ID]
	(
		@UPC		[int]
	)
AS
	BEGIN
		SELECT 		[UPC],[DepartmentID],[Name],[Manufacturer],[OnHand],[Cost],[Active]
		FROM		[Products]
		WHERE		[UPC] = @UPC
	END
GO

print '' print '*** Creating sp_update_product'
GO
CREATE PROCEDURE [dbo].[sp_update_product]
	(
	@UPC					[int],
	@DepartmentID			[int],
	@Name					[nvarchar](100),
	@Manufacturer			[nvarchar](100),
	@OnHand					[int],
	@OldDepartmentID		[int],
	@OldName				[nvarchar](100),
	@OldManufacturer		[nvarchar](100),
	@OldOnHand				[int]

	)
AS
	BEGIN
		UPDATE [dbo].[Products]
			SET [DepartmentID] = @DepartmentID,
				[Name] = @Name,
				[Manufacturer] = @Manufacturer,
				[OnHand] = @OnHand
		  WHERE [UPC] = @UPC
			AND [DepartmentID] = @OldDepartmentID
			AND [Name] = @OldName
			AND [Manufacturer] = @OldManufacturer
			AND [OnHand] = @OldOnHand
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_add_product'
GO
CREATE PROCEDURE [dbo].[sp_add_product]
	(
	@DepartmentID			[int],
	@Name					[nvarchar](100),
	@Manufacturer			[nvarchar](100),
	@OnHand					[int],
	@Cost					[decimal](4,2)
	)
AS
	BEGIN
		INSERT INTO [dbo].[Products]
			([DepartmentID], [Name], [Manufacturer], [OnHand], [Cost])
		VALUES
			(@DepartmentID, @Name, @Manufacturer, @OnHand, @Cost)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** Creating sp_deactivate_product'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_product]
	(
	@UPC					int
	)
AS
	BEGIN
		UPDATE [Products]
			SET [Active] = 0
			WHERE [UPC] = @UPC
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_update_customer'
GO
CREATE PROCEDURE [dbo].[sp_update_customer]
	(
	@CustomerID					[int],
	@PhoneNumber				[nvarchar](15),
	@FirstName					[nvarchar](100),
	@LastName					[nvarchar](100),
	@Email						[nvarchar](100),
	@OldPhoneNumber				[nvarchar](15),
	@OldFirstName				[nvarchar](100),
	@OldLastName				[nvarchar](100),
	@OldEmail					[nvarchar](100)
	)
AS
	BEGIN
		UPDATE [dbo].[Customer]
			SET [PhoneNumber] = @PhoneNumber,
				[FirstName] = @FirstName,
				[LastName] = @LastName,
				[Email] = @Email
		  WHERE [CustomerID] = @CustomerID
			AND [PhoneNumber] = @OldPhoneNumber
			AND [FirstName] = @OldFirstName
			AND [LastName] = @OldLastName
			AND [Email] = @OldEmail
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_deactivate_customer'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_customer]
	(
	@CustomerID			[nvarchar](20)
	)
AS
	BEGIN
		UPDATE [Customer]
			SET 	[Active] = 0
			WHERE 	[CustomerID] = @CustomerID
		Return @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_add_customer'
GO
CREATE PROCEDURE [dbo].[sp_add_customer]
	(
	@FirstName			[nvarchar](50),
	@LastName			[nvarchar](100),
	@PhoneNumber		[nvarchar](15),
	@Email				[nvarchar](100)
	)
AS
	BEGIN
		INSERT INTO [dbo].[Customer]
			([FirstName], [LastName], [PhoneNumber], [Email])
		VALUES
			(@FirstName, @LastName, @PhoneNumber, @Email)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** Creating sp_select_customer_by_ID'
GO
CREATE PROCEDURE [dbo].[sp_select_customer_by_ID]
	(
		@CustomerID		[int]
	)
AS
	BEGIN
		SELECT 		[CustomerID],[FirstName],[LastName],[Email]
		FROM		[Customer]
		WHERE		[CustomerID] = @CustomerID
	END
GO

print '' print '*** Creating sp_update_customer_order'
GO
CREATE PROCEDURE [dbo].[sp_update_customer_order]
	(
	@OrderID					[int],
	@CustomerID					[int],
	@DepartmentID				[int],
	@Description				[varchar](1000),
	@PickupDate					[varchar](50),
	@OldCustomerID				[int],
	@OldDepartmentID			[int],
	@OldDescription				[varchar](1000),
	@OldPickupDate				[varchar](50)
	)
AS
	BEGIN
		UPDATE [dbo].[CustomerOrder]
			SET [CustomerID] = @CustomerID,
				[DepartmentID] = @DepartmentID,
				[Description] = @Description,
				[PickupDate] = @PickupDate
		  WHERE [OrderID] = @OrderID
			AND [CustomerID] = @OldCustomerID
			AND [DepartmentID] = @OldDepartmentID
			AND [Description] = @OldDescription
			AND [PickupDate] = @OldPickupDate
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_deactivate_customer_order'
GO
CREATE PROCEDURE [dbo].[sp_deactivate_customer_order]
	(
	@OrderID			[int]
	)
AS
	BEGIN
		UPDATE [CustomerOrder]
			SET 	[Active] = 0
			WHERE 	[OrderID] = @OrderID
		Return @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_delete_customer_order'
GO
CREATE PROCEDURE [dbo].[sp_delete_customer_order]
	(
	@OrderID			[int]
	)
AS
	BEGIN
		DELETE 
		FROM 	[CustomerOrder]
		WHERE 	[OrderID] = @OrderID
		Return @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_add_customer_order'
GO
CREATE PROCEDURE [dbo].[sp_add_customer_order]
	(
	@CustomerID			[int],
	@DepartmentID		[int],
	@Description		[nvarchar](1000),
	@PickupDate			[nvarchar](50)
	)
AS
	BEGIN
		INSERT INTO [dbo].[CustomerOrder]
			([CustomerID], [DepartmentID], [Description], [PickupDate])
		VALUES
			(@CustomerID, @DepartmentID, @Description, @PickupDate)
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** Creating sp_update_department'
GO
CREATE PROCEDURE [dbo].[sp_update_department]
	(
	@DepartmentID				[int],
	@Name						[nvarchar](30),
	@Description				[nvarchar](1000),
	@OldName					[nvarchar](30),
	@OldDescription				[varchar](1000)
	)
AS
	BEGIN
		UPDATE [dbo].[Department]
			SET [Name] = @Name,
				[Description] = @Description
		  WHERE [DepartmentID] = @DepartmentID
			AND [Name] = @OldName
			AND [Description] = @OldDescription
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_deactive_department'
GO
CREATE PROCEDURE [dbo].[sp_deactive_department]
	(
	@DepartmentID			[int]
	)
AS
	BEGIN
		UPDATE [Department]
			SET 	[Active] = 0
			WHERE 	[DepartmentID] = @DepartmentID
		Return @@ROWCOUNT
	END
GO

print '' print '*** Creating sp_add_customer_order'
GO
CREATE PROCEDURE [dbo].[sp_add_department]
	(
	@Name				[nvarchar](30),
	@Description		[nvarchar](1000)
	)
AS
	BEGIN
		INSERT INTO [dbo].[Department]
			([Name], [Description])
		VALUES
			(@Name, @Description)
	END
GO

