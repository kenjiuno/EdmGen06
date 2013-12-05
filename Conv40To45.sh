#!/bin/bash
	
cat MView40.cs | awk '
	{gsub("System.Data.EntityClient","System.Data.Entity.Core.EntityClient")}
	{gsub("System.Data.Objects","System.Data.Entity.Core.Objects")}
	{gsub("System.Data.Metadata.Edm","System.Data.Entity.Core.Metadata.Edm")}
	{print}' > MView45.cs
