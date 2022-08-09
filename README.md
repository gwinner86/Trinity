# Trinity test
Is a worker service that access API's to retrieves the floors and the sensors per floor and store the result in mySql dataabase.

## Requirements

- **.NET CORE**: Version .NET 5
- **XAMPP**: V3.2.4
  **MySQL**: 10.4.17-MariaDB

## Installation

After cloning or adding remote origin url:

- Run `dotnet restore` to restore dpendent dotnet packages.
- After restoring packages, run `dotnet ef` to confirm that dotnet entity core framework is installed.
- Run `dotnet ef add-migration` to create a local database in your mysql instance.
- Run `dotnet ef update-database` to update your local database.
- Run `dotnet run` to execute the console worker app.
