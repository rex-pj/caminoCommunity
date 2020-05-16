-- Replace all MyDBs with the name of the DB you want to change its nameUSE [MyDB];
 -- Changing Physical names and paths
 -- Replace all NewMyDB with the new name you want to set for the DB
 -- Replace 'C:\...\NewMyDB.mdf' with full path of new DB file to be used 
ALTER DATABASE Coco_IdentityDb MODIFY FILE (NAME = 'Coco_IdentityDb ', FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Coco_IdentityDb.mdf');
 -- Replace 'C:\...\NewMyDB_log.ldf' with full path of new DB log file to be used 
ALTER DATABASE Coco_IdentityDb MODIFY FILE (NAME = 'Coco_IdentityDb_log', FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Coco_IdentityDb_log.ldf');
 -- Changing logical names
ALTER DATABASE Coco_IdentityDb MODIFY FILE (NAME = Coco_IdentityDb, NEWNAME = Coco_IdentityDb1);
ALTER DATABASE Coco_IdentityDb MODIFY FILE (NAME = Coco_IdentityDb_log, NEWNAME = Coco_IdentityDb1_log);