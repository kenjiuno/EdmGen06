EdmGen06
========

My alternative tool to EdmGen/EdmGen2

Usage
-----

Here is content from Usage.txt

```
EdmGen06
 /ModelGen   <connectionString> <providerName> <modelName> <targetSchema> <ver>
 /EFModelGen <connectionString> <providerName> <DbProviderServices> <modelName> <targetSchema> <ver>
 /DataSet <connectionString> <providerName> <modelName> <targetSchema>
 /DataSet.cs <DataSet1.xsd> <DataSet1.cs>

rem [SqlServer Example]

EdmGen06 ^
  /ModelGen ^
  "Data Source=xxxxx;Initial Catalog=xxxxx;Persist Security Info=True;User ID=xxxxx;Password=xxxxx;MultipleActiveResultSets=true;" ^
  "System.Data.SqlClient" ^
  "kodb" ^
  "dbo" ^
  "3.0"

rem [Npgsql Example]

EdmGen06 ^
  /ModelGen ^
  "Port=5432;Encoding=UTF-8;Server=xxxxx;Database=xxxxx;UserId=xxxxx;Password=xxxxx;Preload Reader=true;" ^
  "Npgsql" ^
  "DBName" ^
  "public" ^
  "3.0"

rem [Npgsql EF6 Example]

EdmGen06 ^
  /EFModelGen ^
  "Port=5432;Encoding=UTF-8;Server=xxxxx;Database=xxxxx;UserId=xxxxx;Password=xxxxx;Preload Reader=true;" ^
  "Npgsql" ^
  "Npgsql.NpgsqlServices, Npgsql.EntityFramework" ^
  "DBName" ^
  "public" ^
  "3.0"
```
