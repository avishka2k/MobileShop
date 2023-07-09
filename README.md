# MobileShop

This project consists of two csharp projects.
  1. ShopClient - Customer
  2. ShopAdmin - The owner of the shop

Video Link: https://drive.google.com/file/d/1TIs06sUtu9WZsFDqicrjlxr2VwKGcrSv/view?usp=sharing

## Installation instructions

- Clone and change the dir
```
git clone https://github.com/avishka2k/MobileShop
cd MobileShop
```
OR, Download the source code [here.](https://github.com/avishka2k/MobileShop/releases/tag/v1.0.0)
- Click and open ShopAdmin.slh and Shopclinet.slh in ShopAdmin and ShopClient floders
- First run ShopAdmin
- Use the following credentials when logging into ShopAdmin.
```
Username: user@example.com
Password: 123
```
- You can get the experience of the client side by running the ShopClient.

Note: The database must be configured to get the full experience.

## Database Setup
- Open SQL Server Management Studio(SSMS). [Get more about SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Right-click Databases on object explorer, click on 'import data-tier application'

![Screenshot 2023-06-17 164311](https://github.com/avishka2k/MobileShop/assets/86360412/eaed3abf-14fa-44af-8f0b-1c7357553972)

- Then click on Next,
- Browse the MobileShopDb.bacpac ([Download MobileShopDb.bacpac from here](https://github.com/avishka2k/MobileShop/releases/tag/v1.0.0))  file from your folder and click next
- Click Next until finished
- After importing the database, right-click on the server and go to properties

![Screenshot 2023-06-17 170408](https://github.com/avishka2k/MobileShop/assets/86360412/c42714e2-12e6-4e4f-b0c0-c981f50487cf)

- Copy your server name

![Screenshot 2023-06-17 170738](https://github.com/avishka2k/MobileShop/assets/86360412/3e57ffbe-36f1-4b1e-a10d-142f58af9c59)

Now you can close the SSMS

- After configuring the database, go to Visual Studio and paste the server name in the application.json file as shown in the image below. It should be done for both ShoAdmin and ShopClient.

![Screenshot 2023-06-17 171057](https://github.com/avishka2k/MobileShop/assets/86360412/5f2de0a7-2b26-4d97-9f22-eb99f4311d23)

### Now the Application Setup is done. Enjoy it:grin::relaxed:.
