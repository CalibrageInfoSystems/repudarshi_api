INSERT INTO dbo.Role( Code ,NAME ,[Desc] ,ParentRoleId ,IsActive ,CreatedByUserId ,CreatedDate ,UpdatedByUserId ,UpdatedDate)
            VALUES  ( 'SA',	'Super Admin','Super Admin',NULL,1,NULL,GETDATE(), NULL, GETDATE())
INSERT INTO dbo.Role( Code ,NAME ,[Desc] ,ParentRoleId ,IsActive ,CreatedByUserId ,CreatedDate ,UpdatedByUserId ,UpdatedDate)
            VALUES  ('AM','Admin','Admin',	1,	1,1,GETDATE(), 1, GETDATE())
INSERT INTO dbo.Role( Code ,NAME ,[Desc] ,ParentRoleId ,IsActive ,CreatedByUserId ,CreatedDate ,UpdatedByUserId ,UpdatedDate)
            VALUES  ('DB',	'Delivery Boy',	'Delivery Boy',	2,	1,1,GETDATE(), 1, GETDATE())
INSERT INTO dbo.Role( Code ,NAME ,[Desc] ,ParentRoleId ,IsActive ,CreatedByUserId ,CreatedDate ,UpdatedByUserId ,UpdatedDate)
            VALUES  ('CUST','Customer',	'Customer',	NULL,	1,1,GETDATE(), 1, GETDATE())

        