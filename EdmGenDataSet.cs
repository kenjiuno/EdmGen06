#if ENTITIES6
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
#else
using System.Data.Metadata.Edm;
#endif

using EdmGen06.Properties;
using Store;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EdmGen06 {
    public class EdmGenDataSet : EdmGenBase {
        static readonly XNamespace xxs = "http://www.w3.org/2001/XMLSchema";
        static readonly XNamespace xmsdata = "urn:schemas-microsoft-com:xml-msdata";
        static readonly XNamespace xmsprop = "urn:schemas-microsoft-com:xml-msprop";

        public void DataSet(String connectionString, String providerName, String modelName, String targetSchema) {
            String baseDir = Environment.CurrentDirectory;

            Trace.TraceEvent(TraceEventType.Information, 101, "Getting System.Data.Common.DbProviderFactory from '{0}'", providerName);
            var fac = System.Data.Common.DbProviderFactories.GetFactory(providerName);
            if (fac == null) throw new ApplicationException();
            Trace.TraceEvent(TraceEventType.Information, 101, fac.GetType().AssemblyQualifiedName);
            Trace.TraceEvent(TraceEventType.Information, 101, "Ok");


            using (var db = fac.CreateConnection()) {
                Trace.TraceEvent(TraceEventType.Information, 101, "Connecting");
                db.ConnectionString = connectionString;
                db.Open();
                Trace.TraceEvent(TraceEventType.Information, 101, "Connected");

                Trace.TraceEvent(TraceEventType.Information, 101, "Getting System.Data.Entity.Core.Common.DbProviderServices from '{0}'", providerName);
                var providerServices = ((IServiceProvider)fac).GetService(typeof(DbProviderServices)) as DbProviderServices;
                if (providerServices == null) providerServices = DbProviderServices.GetProviderServices(db);
                Trace.TraceEvent(TraceEventType.Information, 101, providerServices.GetType().AssemblyQualifiedName);
                Trace.TraceEvent(TraceEventType.Information, 101, "Ok");

                Trace.TraceEvent(TraceEventType.Information, 101, "Get ProviderManifestToken");
                var providerManifestToken = providerServices.GetProviderManifestToken(db);
                Trace.TraceEvent(TraceEventType.Information, 101, "Get ProviderManifest");
                var providerManifest = providerServices.GetProviderManifest(providerManifestToken) as DbProviderManifest;

                Trace.TraceEvent(TraceEventType.Information, 101, "Get StoreSchemaDefinition");
                var storeSchemaDefinition = providerManifest.GetInformation("StoreSchemaDefinition") as XmlReader;
                Trace.TraceEvent(TraceEventType.Information, 101, "Get StoreSchemaMapping");
                var storeSchemaMapping = providerManifest.GetInformation("StoreSchemaMapping") as XmlReader;

                Trace.TraceEvent(TraceEventType.Information, 101, "Write temporary ProviderManifest ssdl");
                XDocument tSsdl;
                String fpssdl = Path.Combine(baseDir, "__" + providerName + ".ssdl");
                String fpssdl2 = Path.Combine(baseDir, "__" + providerName + ".ssdl.xml");
                {
                    tSsdl = XDocument.Load(storeSchemaDefinition);
                    tSsdl.Save(fpssdl);
                    tSsdl.Save(fpssdl2);
                }

                Trace.TraceEvent(TraceEventType.Information, 101, "Write temporary ProviderManifest msl");
                XDocument tMsl;
                String fpmsl = Path.Combine(baseDir, "__" + providerName + ".msl");
                String fpmsl2 = Path.Combine(baseDir, "__" + providerName + ".msl.xml");
                {
                    tMsl = XDocument.Load(storeSchemaMapping);
                    tMsl.Save(fpmsl);
                    tMsl.Save(fpmsl2);
                }

                Trace.TraceEvent(TraceEventType.Information, 101, "Checking ProviderManifest version.");
                XmlReader xrCsdl = null;
                if (false) { }
                else if (tSsdl.Element("{" + NS.SSDLv1 + "}" + "Schema") != null && tMsl.Element("{" + NS.MSLv1 + "}" + "Mapping") != null) {
                    pCSDL = "{" + NS.CSDLv1 + "}";
                    pMSL = "{" + NS.MSLv1 + "}";
                    pSSDL = "{" + NS.SSDLv1 + "}";
#if ENTITIES6
                    xrCsdl = DbProviderServices.GetConceptualSchemaDefinition(DbProviderManifest.ConceptualSchemaDefinition);
#else
                    xrCsdl = XmlReader.Create(new MemoryStream(Resources.ConceptualSchemaDefinition));
#endif
                    Trace.TraceEvent(TraceEventType.Information, 101, "ProviderManifest v1");
                }
                else if (tSsdl.Element("{" + NS.SSDLv3 + "}" + "Schema") != null && tMsl.Element("{" + NS.MSLv3 + "}" + "Mapping") != null) {
                    pCSDL = "{" + NS.CSDLv3 + "}";
                    pMSL = "{" + NS.MSLv3 + "}";
                    pSSDL = "{" + NS.SSDLv3 + "}";
#if ENTITIES6
                    xrCsdl = DbProviderServices.GetConceptualSchemaDefinition(DbProviderManifest.ConceptualSchemaDefinitionVersion3);
#else
                    xrCsdl = XmlReader.Create(new MemoryStream(Resources.ConceptualSchemaDefinitionVersion3));
#endif
                    Trace.TraceEvent(TraceEventType.Information, 101, "ProviderManifest v3");
                }
                else {
                    Trace.TraceEvent(TraceEventType.Error, 101, "ProviderManifest version unknown");
                    throw new ApplicationException("ProviderManifest version unknown");
                }

                Trace.TraceEvent(TraceEventType.Information, 101, "Write temporary ProviderManifest csdl");
                String fpcsdl = Path.Combine(baseDir, "__" + providerName + ".csdl");
                XDocument tCsdl;
                {
                    tCsdl = XDocument.Load(xrCsdl);
                    tCsdl.Save(fpcsdl);
                }

                var entityConnectionString = ""
                    + "Provider=" + providerName + ";"
                    + "Provider Connection String=\"" + db.ConnectionString + "\";"
                    + "Metadata=" + fpcsdl + "|" + fpssdl + "|" + fpmsl + ";"
                    ;

                Trace.TraceEvent(TraceEventType.Information, 101, "Getting SchemaInformation");
                using (var Context = new SchemaInformation(entityConnectionString)) {
                    Trace.TraceEvent(TraceEventType.Information, 101, "Ok");

                    DataTable dtMetaDataCollections = db.GetSchema("MetaDataCollections");
                    DataTable dtDataSourceInformation = db.GetSchema("DataSourceInformation");
                    //DataTable dtDataTypes = db.GetSchema("DataTypes");
                    DataTable dtReservedWords = db.GetSchema("ReservedWords");

                    XDocument xsd = new XDocument();

                    XNamespace nsMSTNS = "http://tempuri.org/DataSet1.xsd";

                    String dbn = db.Database;
                    String dataSet1 = modelName;

                    dut.providerManifest = providerManifest;
                    dut.modelName = modelName;
                    dut.providerName = providerName;
                    dut.targetSchema = targetSchema;
                    foreach (DataRow dr in dtDataSourceInformation.Rows) {
                        dut.CompositeIdentifierSeparatorPattern = (String)dr["CompositeIdentifierSeparatorPattern"];
                        dut.IdentifierPattern = (String)dr["IdentifierPattern"];
                        dut.ParameterMarkerFormat = (String)dr["ParameterMarkerFormat"];
                        dut.ParameterMarkerPattern = (String)dr["ParameterMarkerPattern"];
                        dut.ParameterNamePattern = (String)dr["ParameterNamePattern"];
                        dut.QuotedIdentifierPattern = (String)dr["QuotedIdentifierPattern"];
                        dut.StringLiteralPattern = (String)dr["StringLiteralPattern"];
                        dut.IdentifierCase = (IdentifierCase)dr["IdentifierCase"];
                        dut.ParameterNameMaxLength = (int)dr["ParameterNameMaxLength"];
                    }

                    // http://d.hatena.ne.jp/gsf_zero1/20121219/p1

                    XElement xSchema;
                    xsd.Add(xSchema = new XElement(xxs + "schema"
                        , new XAttribute(XNamespace.Xmlns + "xs", xxs)
                        , new XAttribute(XNamespace.Xmlns + "mstns", nsMSTNS)
                        , new XAttribute(XNamespace.Xmlns + "msdata", xmsdata)
                        , new XAttribute(XNamespace.Xmlns + "msprop", xmsprop)
                        , new XAttribute("id", dataSet1)
                        , new XAttribute("targetNamespace", nsMSTNS)
                        , new XAttribute("attributeFormDefault", "qualified")
                        , new XAttribute("elementFormDefault", "qualified")
                        ));

                    {
                        XElement xAnn, xTables;
                        xSchema.Add(xAnn = new XElement(xxs + "annotation"
                            , new XElement(xxs + "appinfo"
                                , new XAttribute("source", nsDS)
                                , new XElement(nsDS + "DataSource"
                                    , new XAttribute("DefaultConnectionIndex", "0")
                                    , new XAttribute("FunctionsComponentName", "QueriesTableAdapter")
                                    , new XAttribute("Modifier", "AutoLayout, AnsiClass, Class, Public")
                                    , new XAttribute("SchemaSerializationMode", "IncludeSchema")
                                    , new XElement(nsDS + "Connections"
                                        , new XElement(nsDS + "Connection"
                                            , new XAttribute("AppSettingsObjectName", "Settings")
                                            , new XAttribute("AppSettingsPropertyName", dbn + "ConnectionString")
                                            , new XAttribute("ConnectionStringObject", "")
                                            , new XAttribute("IsAppSettingsProperty", "true")
                                            , new XAttribute("Modifier", "Assembly")
                                            , new XAttribute("Name", dbn + "ConnectionString (Settings)")
                                            , new XAttribute("ParameterPrefix", "@")
                                            , new XAttribute("Provider", providerName)
                                            )
                                        )
                                    , xTables = new XElement(nsDS + "Tables"

                                        )
                                    )
                                )
                            )
                        );

                        XElement xchoice, xelt;
                        xSchema.Add(xelt = new XElement(xxs + "element"
                            , new XAttribute("name", dataSet1)
                            , new XAttribute(xmsdata + "IsDataSet", "true")
                            , new XAttribute(xmsdata + "UseCurrentLocale", "true")
                            , new XAttribute(xmsdata + "EnableTableAdapterManager", "true")
                            , new XAttribute(xmsprop + "Generator_DataSetName", dut.Generator_DataSetName(dbn))
                            , new XAttribute(xmsprop + "Generator_UserDSName", dut.Generator_UserDSName(dbn))
                            , new XElement(xxs + "complexType"
                                , xchoice = new XElement(xxs + "choice"
                                    , new XAttribute("minOccurs", "0")
                                    , new XAttribute("maxOccurs", "unbounded")
                                    )
                                )
                            )
                        );

                        foreach (var Table in Context.Tables) {
                            DbDataAdapter da = fac.CreateDataAdapter();
                            DbCommandBuilder cb = fac.CreateCommandBuilder();
                            da.SelectCommand = db.CreateCommand();
                            da.SelectCommand.CommandText = String.Format("SELECT " + String.Join(", ", Table.Columns.Select(p => dut.SColumn(p, cb)).ToArray()) + " FROM " + dut.STable(Table, cb));
                            cb.DataAdapter = da;
                            try {
                                da.DeleteCommand = cb.GetDeleteCommand();
                            }
                            catch (InvalidOperationException) { }
                            try {
                                da.InsertCommand = cb.GetInsertCommand();
                            }
                            catch (InvalidOperationException) { }
                            try {
                                da.UpdateCommand = cb.GetUpdateCommand();
                            }
                            catch (InvalidOperationException) { }

                            XElement xDbSource;
                            XElement xTableAdapter, xMappings;
                            xTables.Add(xTableAdapter = new XElement(nsDS + "TableAdapter"
                                , new XAttribute("BaseClass", "System.ComponentModel.Component")
                                , new XAttribute("DataAccessorModifier", "AutoLayout, AnsiClass, Class, Public")
                                , new XAttribute("DataAccessorName", dut.DataAccessorName(Table))
                                , new XAttribute("GeneratorDataComponentClassName", dut.GeneratorDataComponentClassName(Table))
                                , new XAttribute("Name", Table.Name)
                                , new XAttribute("UserDataComponentName", dut.UserDataComponentName(Table))
                                , new XElement(nsDS + "MainSource"
                                    , xDbSource = new XElement(nsDS + "DbSource"
                                        , new XAttribute("ConnectionRef", dbn + "ConnectionString (Settings)")
                                        , new XAttribute("DbObjectName", dut.DbObjectName(Table, cb))
                                        , new XAttribute("DbObjectType", "Table")
                                        , new XAttribute("FillMethodName", "Fill")
                                        , new XAttribute("GenerateMethods", "Both")
                                        , new XAttribute("GenerateShortCommands", "true")
                                        , new XAttribute("GeneratorGetMethodName", "GetData")
                                        , new XAttribute("GeneratorSourceName", "Fill")
                                        , new XAttribute("GetMethodModifier", "Public")
                                        , new XAttribute("GetMethodName", "GetData")
                                        , new XAttribute("QueryType", "Rowset")
                                        , new XAttribute("ScalarCallRetval", typeof(object).AssemblyQualifiedName)
                                        , new XAttribute("UseOptimisticConcurrency", "true")
                                        , new XAttribute("UserGetMethodName", "GetData")
                                        , new XAttribute("UserSourceName", "Fill")
                                        )
                                    )
                                , xMappings = new XElement(nsDS + "Mappings"
                                    )
                                , new XElement(nsDS + "Sources"
                                    )
                                )
                            );
                            if (da.DeleteCommand != null) xDbSource.Add(
                                new XElement(nsDS + "DeleteCommand", GetCommandEl(Table, da.DeleteCommand, cb, nsDS))
                                );
                            if (da.InsertCommand != null) xDbSource.Add(
                                new XElement(nsDS + "InsertCommand", GetCommandEl(Table, da.InsertCommand, cb, nsDS))
                                );
                            if (da.SelectCommand != null) xDbSource.Add(
                                new XElement(nsDS + "SelectCommand", GetCommandEl(Table, da.SelectCommand, cb, nsDS))
                                );
                            if (da.UpdateCommand != null) xDbSource.Add(
                                new XElement(nsDS + "UpdateCommand", GetCommandEl(Table, da.UpdateCommand, cb, nsDS))
                                );

                            XElement xsequence;
                            xchoice.Add(new XElement(xxs + "element"
                                , new XAttribute("name", dut.xsname(Table))
                                , new XAttribute(xmsprop + "Generator_TableClassName", dut.Generator_TableClassName(Table))
                                , new XAttribute(xmsprop + "Generator_TableVarName", dut.Generator_TableVarName(Table))
                                , new XAttribute(xmsprop + "Generator_TablePropName", dut.Generator_TablePropName(Table))
                                , new XAttribute(xmsprop + "Generator_RowDeletingName", dut.Generator_RowDeletingName(Table))
                                , new XAttribute(xmsprop + "Generator_RowChangingName", dut.Generator_RowChangingName(Table))
                                , new XAttribute(xmsprop + "Generator_RowEvHandlerName", dut.Generator_RowEvHandlerName(Table))
                                , new XAttribute(xmsprop + "Generator_RowDeletedName", dut.Generator_RowDeletedName(Table))
                                , new XAttribute(xmsprop + "Generator_UserTableName", dut.Generator_UserTableName(Table))
                                , new XAttribute(xmsprop + "Generator_RowChangedName", dut.Generator_RowChangedName(Table))
                                , new XAttribute(xmsprop + "Generator_RowEvArgName", dut.Generator_RowEvArgName(Table))
                                , new XAttribute(xmsprop + "Generator_RowClassName", dut.Generator_RowClassName(Table))
                                , new XElement(xxs + "complexType"
                                    , xsequence = new XElement(xxs + "sequence"
                                        )
                                    )
                                ));

                            foreach (var dbc in Table.Columns) {
                                xMappings.Add(new XElement(nsDS + "Mapping"
                                    , new XAttribute("SourceColumn", dut.SourceColumn(dbc))
                                    , new XAttribute("DataSetColumn", dut.DataSetColumn(dbc))
                                    ));

                                XElement xce;
                                xsequence.Add(xce = new XElement(xxs + "element"
                                    , new XAttribute("name", dut.xsname(dbc))
                                    ));
                                if (dbc.IsIdentity) {
                                    xce.SetAttributeValue(xmsdata + "ReadOnly", "true");
                                    xce.SetAttributeValue(xmsdata + "AutoIncrement", "true");
                                    xce.SetAttributeValue(xmsdata + "AutoIncrementSeed", "-1");
                                    xce.SetAttributeValue(xmsdata + "AutoIncrementStep", "-1");
                                }

                                xce.SetAttributeValue(xmsprop + "Generator_ColumnVarNameInTable", dut.Generator_ColumnVarNameInTable(dbc));
                                xce.SetAttributeValue(xmsprop + "Generator_ColumnPropNameInRow", dut.Generator_ColumnPropNameInRow(dbc));
                                xce.SetAttributeValue(xmsprop + "Generator_ColumnPropNameInTable", dut.Generator_ColumnPropNameInTable(dbc));
                                xce.SetAttributeValue(xmsprop + "Generator_UserColumnName", dut.Generator_UserColumnName(dbc));
                                xce.SetAttributeValue("type", dut.xstype(dbc));

                                if (dbc.IsNullable) {
                                    xce.SetAttributeValue("minOccurs", "0");
                                }

                                int? MaxLen = dbc.ColumnType.MaxLength;
                                if (MaxLen.HasValue) {
                                    xce.Add(new XElement(xxs + "simpleType"
                                        , new XElement(xxs + "restriction"
                                            , new XAttribute("base", dut.xstype(dbc))
                                            , new XElement(xxs + "maxLength"
                                                , new XAttribute("value", MaxLen.Value + "")
                                                )
                                            )
                                        ));
                                    xce.Attribute("type").Remove();
                                }
                            }
                        }

                        foreach (var Tcc in Context.TableConstraints.OfType<TableOrViewColumnConstraint>()) {
                            XElement xunique;
                            xelt.Add(xunique = new XElement(xxs + "unique"
                                , new XAttribute("name", dut.xsname(Tcc))
                                , new XElement(xxs + "selector"
                                    , new XAttribute("xpath", ".//mstns:" + dut.xsname(Tcc.Parent))
                                    )
                                ));
                            foreach (var tcol in Tcc.Columns) {
                                xunique.Add(new XElement(xxs + "field"
                                    , new XAttribute("xpath", "mstns:" + dut.xsname(tcol))
                                    )
                                );
                            }
                            var pk = Tcc as PrimaryKeyConstraint;
                            if (pk != null) {
                                xunique.SetAttributeValue(xmsdata + "PrimaryKey", "true");
                            }
                        }

                        XElement xrel;
                        xSchema.Add(new XElement(xxs + "annotation"
                            , xrel = new XElement(xxs + "appinfo")
                            ));
                        foreach (var Tfk in Context.TableForeignKeys) {
                            xrel.Add(new XElement(xmsdata + "Relationship"
                                , new XAttribute("name", dut.xsname(Tfk.Constraint))
                                , new XAttribute(xmsdata + "parent", dut.xsname(Tfk.ToColumn.Parent))
                                , new XAttribute(xmsdata + "child", dut.xsname(Tfk.FromColumn.Parent))
                                , new XAttribute(xmsdata + "parentkey", dut.xsname(Tfk.ToColumn))
                                , new XAttribute(xmsdata + "childkey", dut.xsname(Tfk.FromColumn))
                                , new XAttribute(xmsprop + "Generator_UserChildTable", dut.Generator_UserChildTable(Tfk.FromColumn.Parent))
                                , new XAttribute(xmsprop + "Generator_ChildPropName", dut.Generator_ChildPropName(Tfk.FromColumn))
                                , new XAttribute(xmsprop + "Generator_UserRelationName", dut.Generator_UserRelationName(Tfk))
                                , new XAttribute(xmsprop + "Generator_RelationVarName", dut.Generator_RelationVarName(Tfk))
                                , new XAttribute(xmsprop + "Generator_UserParentTable", dut.Generator_UserParentTable(Tfk.ToColumn.Parent))
                                , new XAttribute(xmsprop + "Generator_ParentPropName", dut.Generator_ParentPropName(Tfk.ToColumn))
                                ));
                        }
                    }

                    String fpxsd = Path.Combine(baseDir, modelName + ".xsd");

                    xsd.Save(fpxsd);
                }
            }
        }

        private XElement GetCommandEl(Table Table, DbCommand dbco, DbCommandBuilder cb, XNamespace nsDS) {
            XElement xParameters = new XElement(nsDS + "Parameters");

            foreach (DbParameter dbp in dbco.Parameters) {
                bool AllowDbNull = true;
                Column dbc = Table.Columns.Where(p => p.Name == dbp.SourceColumn).FirstOrDefault();
                if (dbc != null) {
                    AllowDbNull = dbc.IsNullable;
                }
                xParameters.Add(new XElement(nsDS + "Parameter"
                    , new XAttribute("AllowDbNull", AllowDbNull ? "true" : "false")
                    , new XAttribute("AutogeneratedName", "")
                    , new XAttribute("DataSourceName", "")
                    , new XAttribute("DbType", dbp.DbType + "")
                    , new XAttribute("Direction", dbp.Direction + "")
                    , new XAttribute("ParameterName", dbp.ParameterName)
                    , new XAttribute("Precision", dut.Precision(dbc))
                    , new XAttribute("ProviderType", dut.ProviderType(dbp, dbc))
                    , new XAttribute("Scale", dut.Scale(dbc))
                    , new XAttribute("Size", dut.Size(dbc))
                    , new XAttribute("SourceColumn", dbp.SourceColumn)
                    , new XAttribute("SourceColumnNullMapping", dbp.SourceColumnNullMapping ? "true" : "false")
                    , new XAttribute("SourceVersion", dbp.SourceVersion)
                    )
                );
            }

            return new XElement(nsDS + "DbCommand"
                , new XAttribute("CommandType", "Text")
                , new XAttribute("ModifiedByUser", "false")
                , new XElement(nsDS + "CommandText"
                    , new XText(dbco.CommandText)
                    )
                , xParameters
                );
        }

        DUt dut = new DUt();

        class DUt {
            public DbProviderManifest providerManifest { get; set; }

            public String providerName { get; set; }
            public String modelName { get; set; }
            public String targetSchema { get; set; }

            public String StringLiteralPattern { get; set; }
            public String QuotedIdentifierPattern { get; set; }
            public String ParameterNamePattern { get; set; }
            public int ParameterNameMaxLength { get; set; }
            public String ParameterMarkerPattern { get; set; }
            public String ParameterMarkerFormat { get; set; }
            public IdentifierCase IdentifierCase { get; set; }
            public String IdentifierPattern { get; set; }
            public String CompositeIdentifierSeparatorPattern { get; set; }

            internal string STable(Table Table, DbCommandBuilder cb) {
                if (cb.CatalogLocation != CatalogLocation.Start) throw new NotSupportedException("CatalogLocation: " + cb.CatalogLocation);
                return ""
                    + cb.QuoteIdentifier(Table.CatalogName)
                    + cb.CatalogSeparator
                    + cb.QuoteIdentifier(Table.SchemaName)
                    + cb.SchemaSeparator
                    + cb.QuoteIdentifier(Table.Name)
                    ;
            }

            public String DbObjectName(Table Table, DbCommandBuilder cb) {
                return ""
                    + cb.QuoteIdentifier(Table.CatalogName)
                    + cb.CatalogSeparator
                    + cb.QuoteIdentifier(Table.SchemaName)
                    + cb.SchemaSeparator
                    + cb.QuoteIdentifier(Table.Name)
                    ;
            }

            internal String SColumn(Column p, DbCommandBuilder cb) {
                return cb.QuoteIdentifier(p.Name);
            }

            public String Precision(Column dbc) {
                int? v = new int?();
                if (dbc != null) v = dbc.ColumnType.Precision;
                return Convert.ToString(v ?? 0);
            }

            public String Scale(Column dbc) {
                int? v = new int?();
                if (dbc != null) v = dbc.ColumnType.Scale;
                return Convert.ToString(v ?? 0);
            }

            public String Size(Column dbc) {
                int? v = new int?();
                if (dbc != null) {
                    v = dbc.ColumnType.MaxLength;
                    if (v.HasValue && v.Value == -1) v = null;
                }
                return Convert.ToString(v ?? 0);
            }

            public String ProviderType(DbParameter dbp, Column dbc) {
                if (dbc != null)
                    return dbc.ColumnType.TypeName;
                foreach (var storeType in providerManifest.GetStoreTypes()) {
                    if (storeType.ClrEquivalentType.Name == dbp.DbType + "") {
                        if (storeType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType) {
                            return storeType.Name;
                        }
                    }
                }
                return null;
            }

            private string CLRSafe(TableOrView Table) { return TSimpleIdentifier(Table.Name, ""); }
            private string CLRSafe(TableOrView Table, string prefix) { return TSimpleIdentifier(Table.Name, prefix); }
            private string CLRSafe(Column dbc) { return TSimpleIdentifier(dbc.Name, ""); }
            private string CLRSafe(Column dbc, string prefix) { return TSimpleIdentifier(dbc.Name, prefix); }
            private string CLRSafe(String s, string prefix) { return TSimpleIdentifier(s, prefix); }
            private string CLRSafe(ForeignKey Tfk) { return TSimpleIdentifier(Tfk.Constraint.Name, ""); }

            public String Generator_TableClassName(Table Table) { return String.Format("{0}DataTable", CLRSafe(Table, "_")); }
            public String Generator_TableVarName(Table Table) { return String.Format("table{0}", CLRSafe(Table)); }
            public String Generator_TablePropName(Table Table) { return String.Format("{0}", CLRSafe(Table, "_")); }
            public String Generator_RowDeletingName(Table Table) { return String.Format("{0}RowDeleting", CLRSafe(Table, "_")); }
            public String Generator_RowChangingName(Table Table) { return String.Format("{0}RowChanging", CLRSafe(Table, "_")); }
            public String Generator_RowEvHandlerName(Table Table) { return String.Format("{0}RowChangeEventHandler", CLRSafe(Table, "_")); }
            public String Generator_RowDeletedName(Table Table) { return String.Format("{0}RowDeleted", CLRSafe(Table, "_")); }
            public String Generator_UserTableName(Table Table) { return String.Format("{0}", CLRSafe(Table)); }
            public String Generator_RowChangedName(Table Table) { return String.Format("{0}RowChanged", CLRSafe(Table, "_")); }
            public String Generator_RowEvArgName(Table Table) { return String.Format("{0}RowChangeEvent", CLRSafe(Table, "_")); }
            public String Generator_RowClassName(Table Table) { return String.Format("{0}Row", CLRSafe(Table, "_")); }

            public String Generator_ColumnVarNameInTable(Column dbc) { return String.Format("column{0}", CLRSafe(dbc)); }
            public String Generator_ColumnPropNameInRow(Column dbc) { return String.Format("{0}", CLRSafe(dbc, "_")); }
            public String Generator_ColumnPropNameInTable(Column dbc) { return String.Format("Id{0}", CLRSafe(dbc)); }

            public String Generator_UserColumnName(Column dbc) { return String.Format("{0}", (dbc.Name)); } // raw

            public String Generator_DataSetName(string dbn) { return "DataSet1"; }

            public String Generator_UserDSName(string dbn) { return "DataSet1"; }

            public String xstype(Column dbc) {
                foreach (var storeType in providerManifest.GetStoreTypes()) {
                    if (storeType.Name == dbc.ColumnType.TypeName) {
                        if (storeType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType) {
                            if (storeType.ClrEquivalentType == typeof(string)) return "xs:string";
                            if (storeType.ClrEquivalentType == typeof(int)) return "xs:int";
                            if (storeType.ClrEquivalentType == typeof(long)) return "xs:long";
                            if (storeType.ClrEquivalentType == typeof(bool)) return "xs:boolean";
                            if (storeType.ClrEquivalentType == typeof(DateTime)) return "xs:dateTime";
                            if (storeType.ClrEquivalentType == typeof(double)) return "xs:double";
                            if (storeType.ClrEquivalentType == typeof(decimal)) return "xs:decimal";
                            if (storeType.ClrEquivalentType == typeof(float)) return "xs:float";
                            if (storeType.ClrEquivalentType == typeof(short)) return "xs:short";
                            if (storeType.ClrEquivalentType == typeof(byte)) return "xs:unsignedByte";
                        }
                    }
                }
                if (false) { }
                else if (dbc.ColumnType.TypeName == "time") return "xs:duration";
                else if (dbc.ColumnType.TypeName == "oid") return "xs:long";
                else if (dbc.ColumnType.TypeName == "bytea") return "xs:hexBinary";
                else return "xs:string";
            }

            public String TSimpleIdentifier(String s, String defaultPrefix) {
                s = Regex.Replace(s, "[\\[\\]\\/\\-\\.\\\\：]+", "_").TrimStart('_');
                if (s.Length >= 1 && char.IsNumber(s[0])) s = defaultPrefix + s;
                return s;
            }

            public String DataAccessorName(Table Table) { return String.Format("{0}TableAdapter", Table.Name); }

            public String GeneratorDataComponentClassName(Table Table) { return CLRSafe(DataAccessorName(Table), "_"); }

            public String UserDataComponentName(Table Table) { return DataAccessorName(Table); }

            public String xsname(Column dbc) { return XSafe(dbc.Name); }
            public String xsname(TableOrView Table) { return XSafe(Table.Name); }
            public String xsname(Store.Constraint Tc) { return XSafe(Tc.Name); }

            private string XSafe(String p) { // xs:name safe
                String s = null;
                if (!String.IsNullOrEmpty(p)) {
                    s = String.Empty;
                    for (int x = 0; x < p.Length; x++) {
                        char c = p[x];
                        if ((x == 0 && !char.IsLetter(c)) || c == '：' || (c < 256 && c != '_' && c != '#' && !char.IsLetterOrDigit(c))) {
                            s += String.Format("_x{0:X4}_", (int)c);
                        }
                        else {
                            s += c;
                        }
                    }
                }
                return s;
            }

            public String Generator_UserChildTable(TableOrView tableOrView) { return CLRSafe(tableOrView); }
            public String Generator_ChildPropName(Column column) { return String.Format("Get{0}Rows", CLRSafe(column.Parent)); }
            public String Generator_UserRelationName(ForeignKey Tfk) { return CLRSafe(Tfk); }
            public String Generator_RelationVarName(ForeignKey Tfk) { return String.Format("relation{0}", CLRSafe(Tfk)); }
            public String Generator_UserParentTable(TableOrView tableOrView) { return CLRSafe(tableOrView); }
            public String Generator_ParentPropName(Column column) { return String.Format("{0}Row", CLRSafe(column.Parent, "_")); }
            public String SourceColumn(Column dbc) { return dbc.Name; }
            public String DataSetColumn(Column dbc) { return dbc.Name; }
        }

        XNamespace nsDS = "urn:schemas-microsoft-com:xml-msdatasource";

        public void DataSet_cs(String fpxsd, String fpcs) {
            //XDocument xsd = XDocument.Load(fpxsd);

            CodeNamespace csns = new CodeNamespace("Test");

            DataSet ds = new System.Data.DataSet();
            ds.ReadXmlSchema(fpxsd);

            String targetNamespace = ds.Namespace;

            {
                String Generator_DataSetName = "" + ds.ExtendedProperties["Generator_DataSetName"];

                var dataSet1 = new CodeTypeDeclaration(Generator_DataSetName);
                csns.Types.Add(dataSet1);

                dataSet1.CustomAttributes.Add(new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(SerializableAttribute))
                    ));
                dataSet1.CustomAttributes.Add(new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(DesignerCategoryAttribute))
                    , new CodeAttributeArgument(new CodePrimitiveExpression("code"))
                    ));
                dataSet1.CustomAttributes.Add(new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(ToolboxItemAttribute))
                    , new CodeAttributeArgument(new CodePrimitiveExpression(true))
                    ));
                dataSet1.CustomAttributes.Add(new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(XmlSchemaProviderAttribute))
                    , new CodeAttributeArgument(new CodePrimitiveExpression("GetTypedDataSetSchema"))
                    ));
                dataSet1.CustomAttributes.Add(new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(XmlRootAttribute))
                    , new CodeAttributeArgument(new CodePrimitiveExpression("vs.data.DataSet"))
                    ));
                dataSet1.BaseTypes.Add(new CodeTypeReference(typeof(DataSet)));

                foreach (DataTable dt in ds.Tables) {
                    String Generator_TableClassName = "" + dt.ExtendedProperties["Generator_TableClassName"];
                    String Generator_TableVarName = "" + dt.ExtendedProperties["Generator_TableVarName"];
                    var privateTable = new CodeMemberField(Generator_TableClassName, Generator_TableVarName);

                    dataSet1.Members.Add(privateTable);
                }
                foreach (DataRelation rel in ds.Relations) {
                    String Generator_RelationVarName = "" + rel.ExtendedProperties["Generator_RelationVarName"];
                    var privateRel = new CodeMemberField(typeof(DataRelation), Generator_RelationVarName);
                    dataSet1.Members.Add(privateRel);
                }

                dataSet1.Members.Add(new CodeMemberField(typeof(SchemaSerializationMode), "_schemaSerializationMode") { InitExpression = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(SchemaSerializationMode)), "IncludeSchema") });

                // ctor()
                {
                    var ctor = new CodeConstructor();
                    ctor.Attributes = MemberAttributes.Public;
                    dataSet1.Members.Add(ctor);

                    ctor.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "BeginInit"));
                    ctor.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "InitClass"));

                    ctor.Statements.Add(new CodeVariableDeclarationStatement(typeof(CollectionChangeEventHandler), "schemaChangedHandler") {
                        InitExpression = new CodeObjectCreateExpression(typeof(CollectionChangeEventHandler), new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "SchemaChanged"))
                    });
                    ctor.Statements.Add(new CodeAttachEventStatement(new CodeEventReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Tables"), "CollectionChanged"), new CodeVariableReferenceExpression("schemaChangedHandler")));
                    ctor.Statements.Add(new CodeAttachEventStatement(new CodeEventReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Relations"), "CollectionChanged"), new CodeVariableReferenceExpression("schemaChangedHandler")));
                    ctor.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "EndInit"));
                }

                foreach (DataTable dt in ds.Tables) {
                    String Generator_TableClassName = "" + dt.ExtendedProperties["Generator_TableClassName"];
                    String Generator_TableVarName = "" + dt.ExtendedProperties["Generator_TableVarName"];
                    String Generator_TablePropName = "" + dt.ExtendedProperties["Generator_TablePropName"];

                    var publicTable = new CodeMemberProperty();
                    publicTable.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    publicTable.Type = new CodeTypeReference(Generator_TableClassName);
                    publicTable.Name = Generator_TablePropName;
                    publicTable.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), Generator_TableVarName)));

                    publicTable.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));
                    publicTable.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(BrowsableAttribute))
                        , new CodeAttributeArgument(new CodePrimitiveExpression(false))
                        ));
                    publicTable.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DesignerSerializationVisibilityAttribute))
                        , new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DesignerSerializationVisibility)), "Content"))
                        ));

                    dataSet1.Members.Add(publicTable);
                }

                // SchemaSerializationMode
                {
                    var mem = new CodeMemberProperty();
                    mem.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                    mem.Type = new CodeTypeReference(typeof(SchemaSerializationMode));
                    mem.Name = "SchemaSerializationMode";

                    mem.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_schemaSerializationMode")));

                    mem.SetStatements.Add(new CodeAssignStatement(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_schemaSerializationMode"),
                        new CodePropertySetValueReferenceExpression()
                        ));

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));
                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(BrowsableAttribute))
                        , new CodeAttributeArgument(new CodePrimitiveExpression(true))
                        ));
                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DesignerSerializationVisibilityAttribute))
                        , new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DesignerSerializationVisibility)), "Visible"))
                        ));

                    dataSet1.Members.Add(mem);
                }

                // Tables
                // Relations

                // InitializeDerivedDataSet
                {
                    var mem = new CodeMemberMethod();
                    dataSet1.Members.Add(mem);
                    mem.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                    mem.Name = "InitializeDerivedDataSet";

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    mem.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "BeginInit"));
                    mem.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "InitClass"));
                    mem.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "EndInit"));

                }

                // Clone
                {
                    var mem = new CodeMemberMethod();
                    dataSet1.Members.Add(mem);
                    mem.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                    mem.ReturnType = new CodeTypeReference(typeof(DataSet));
                    mem.Name = "Clone";

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    mem.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(dataSet1.Name), "cln") {
                        InitExpression = new CodeCastExpression(
                            new CodeTypeReference(dataSet1.Name)
                            , new CodeMethodInvokeExpression(
                                new CodeBaseReferenceExpression(), "Clone")
                            )
                    });
                    mem.Statements.Add(
                        new CodeMethodInvokeExpression(
                            new CodeVariableReferenceExpression("cln"), "InitVars")
                        );
                    mem.Statements.Add(new CodeAssignStatement(
                        new CodePropertyReferenceExpression(
                            new CodeVariableReferenceExpression("cln"), "SchemaSerializationMode")
                        , new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(), "SchemaSerializationMode")
                        ));
                    mem.Statements.Add(new CodeMethodReturnStatement(
                        new CodeVariableReferenceExpression("cln")
                        ));
                }

                // ShouldSerializeTables
                {
                    var mem = new CodeMemberMethod();
                    dataSet1.Members.Add(mem);
                    mem.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                    mem.ReturnType = new CodeTypeReference(typeof(bool));
                    mem.Name = "ShouldSerializeTables";

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    mem.Statements.Add(
                        new CodeMethodReturnStatement(new CodePrimitiveExpression(false))
                        );
                }

                // ShouldSerializeRelations
                {
                    var mem = new CodeMemberMethod();
                    dataSet1.Members.Add(mem);
                    mem.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                    mem.ReturnType = new CodeTypeReference(typeof(bool));
                    mem.Name = "ShouldSerializeRelations";

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    mem.Statements.Add(
                        new CodeMethodReturnStatement(new CodePrimitiveExpression(false))
                        );
                }

                // ReadXmlSerializable
                {
                    var mem = new CodeMemberMethod();
                    dataSet1.Members.Add(mem);
                    mem.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                    mem.Name = "ReadXmlSerializable";

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    mem.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XmlReader), "reader"));

                    CodeStatement insertAt;

                    var if1 = new CodeConditionStatement(
                        // condition
                        new CodeBinaryOperatorExpression(
                        // left
                            new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                "DetermineSchemaSerializationMode",
                                new CodeVariableReferenceExpression("reader")
                                )
                        // op
                            , CodeBinaryOperatorType.ValueEquality
                        // right
                            , new CodeFieldReferenceExpression(
                                new CodeTypeReferenceExpression(typeof(SchemaSerializationMode))
                                , "IncludeSchema"
                            )
                        )
                        // true
                        , new CodeStatement[] {
                            new CodeExpressionStatement(
                                new CodeMethodInvokeExpression(
                                    new CodeThisReferenceExpression()
                                    , "Reset"
                                    )
                                )
                            , new CodeVariableDeclarationStatement(
                                typeof(DataSet)
                                , "ds"
                                , new CodeObjectCreateExpression(
                                    typeof(DataSet)
                                    )
                                )
                            , new CodeExpressionStatement(
                                new CodeMethodInvokeExpression(
                                    new CodeVariableReferenceExpression("ds")
                                    , "ReadXml"
                                    , new CodeVariableReferenceExpression("reader")
                                    )
                                )
                            , insertAt = new CodeAssignStatement(
                                new CodePropertyReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , "DataSetName"
                                    )
                                , new CodePropertyReferenceExpression(
                                    new CodeVariableReferenceExpression("ds")
                                    , "DataSetName"
                                    )
                                )
                            , new CodeAssignStatement(
                                new CodePropertyReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , "Prefix"
                                    )
                                , new CodePropertyReferenceExpression(
                                    new CodeVariableReferenceExpression("ds")
                                    , "Prefix"
                                    )
                                )
                            , new CodeAssignStatement(
                                new CodePropertyReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , "Namespace"
                                    )
                                , new CodePropertyReferenceExpression(
                                    new CodeVariableReferenceExpression("ds")
                                    , "Namespace"
                                    )
                                )
                            , new CodeAssignStatement(
                                new CodePropertyReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , "Locale"
                                    )
                                , new CodePropertyReferenceExpression(
                                    new CodeVariableReferenceExpression("ds")
                                    , "Locale"
                                    )
                                )
                            , new CodeAssignStatement(
                                new CodePropertyReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , "CaseSensitive"
                                    )
                                , new CodePropertyReferenceExpression(
                                    new CodeVariableReferenceExpression("ds")
                                    , "CaseSensitive"
                                    )
                                )
                            , new CodeAssignStatement(
                                new CodePropertyReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , "EnforceConstraints"
                                    )
                                , new CodePropertyReferenceExpression(
                                    new CodeVariableReferenceExpression("ds")
                                    , "EnforceConstraints"
                                    )
                                )
                            , new CodeExpressionStatement(  
                                new CodeMethodInvokeExpression(
                                    new CodeThisReferenceExpression()
                                    , "Merge"
                                    , new CodeVariableReferenceExpression("ds")
                                    , new CodePrimitiveExpression(false)
                                    , new CodeFieldReferenceExpression(
                                        new CodeTypeReferenceExpression(typeof(MissingSchemaAction))
                                        , "Add"
                                        )
                                    )
                                )
                            , new CodeExpressionStatement(
                                new CodeMethodInvokeExpression(
                                    new CodeThisReferenceExpression()
                                    , "InitVars"
                                    )
                                ),
                        }
                        // false
                        , new CodeStatement[] {
                            new CodeExpressionStatement(
                                new CodeMethodInvokeExpression(
                                    new CodeThisReferenceExpression()
                                    , "ReadXml"
                                    , new CodeVariableReferenceExpression("reader")
                                    )
                                )
                            , new CodeExpressionStatement(
                                new CodeMethodInvokeExpression(
                                    new CodeThisReferenceExpression()
                                    , "InitVars"
                                    )
                                ),
                        }
                    );
                    mem.Statements.Add(if1);

                    foreach (DataTable dt in ds.Tables) {
                        String Generator_TablePropName = "" + dt.ExtendedProperties["Generator_TablePropName"];
                        String Generator_TableClassName = "" + dt.ExtendedProperties["Generator_TableClassName"];

                        if1.TrueStatements.Insert(
                            if1.TrueStatements.IndexOf(insertAt)
                            , new CodeConditionStatement(
                                new CodeBinaryOperatorExpression(
                                    new CodeIndexerExpression( // left
                                        new CodePropertyReferenceExpression(
                                            new CodeVariableReferenceExpression("ds")
                                            , "Tables"
                                                )
                                            , new CodePrimitiveExpression(Generator_TablePropName)
                                            )
                                    , CodeBinaryOperatorType.IdentityInequality // op
                                    , new CodePrimitiveExpression(null) // right
                                    )
                                , new CodeStatement[] { // true
                                    new CodeExpressionStatement(
                                        new CodeMethodInvokeExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodeBaseReferenceExpression()
                                                , "Tables"
                                                )
                                            , "Add"
                                            , new CodeObjectCreateExpression(
                                                Generator_TableClassName
                                                , new CodeIndexerExpression(
                                                    new CodePropertyReferenceExpression(
                                                        new CodeVariableReferenceExpression("ds")
                                                        , "Tables"
                                                        )
                                                    , new CodePrimitiveExpression(Generator_TablePropName)
                                                    )
                                                )
                                            )
                                        )
                                }
                                )
                            );
                    }
                }

                // GetSchemaSerializable
                {
                    var mem = new CodeMemberMethod();
                    mem.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                    mem.ReturnType = new CodeTypeReference(typeof(XmlSchema));
                    mem.Name = "GetSchemaSerializable";
                    dataSet1.Members.Add(mem);

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    mem.Statements.Add(
                        new CodeVariableDeclarationStatement(
                            typeof(MemoryStream)
                            , "stream"
                            )
                        );
                    mem.Statements.Add(
                        new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression()
                            , "WriteXmlSchema"
                            , new CodeObjectCreateExpression(
                                typeof(XmlTextWriter)
                                , new CodeVariableReferenceExpression("stream")
                                , new CodePrimitiveExpression(null)
                                )
                            )
                        );
                    mem.Statements.Add(
                        new CodeAssignStatement(
                            new CodePropertyReferenceExpression(
                                new CodeVariableReferenceExpression("stream")
                                , "Position"
                                )
                            , new CodePrimitiveExpression((int)0)
                            )
                        );
                    mem.Statements.Add(
                        new CodeMethodReturnStatement(
                            new CodeMethodInvokeExpression(
                                new CodeTypeReferenceExpression(typeof(XmlSchema))
                                , "Read"
                                , new CodeObjectCreateExpression(
                                    typeof(XmlTextReader)
                                    , new CodeVariableReferenceExpression("stream")
                                    )
                                , new CodePrimitiveExpression(null)
                                )
                            )
                        );
                }

                // InitVars
                {
                    var mem = new CodeMemberMethod();
                    mem.Attributes = MemberAttributes.FamilyAndAssembly;
                    mem.Name = "InitVars";
                    dataSet1.Members.Add(mem);

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    mem.Statements.Add(
                        new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression()
                            , "InitVars"
                            , new CodePrimitiveExpression(true)
                            )
                        );
                }

                // InitVars
                {
                    var mem = new CodeMemberMethod();
                    mem.Attributes = MemberAttributes.FamilyAndAssembly;
                    mem.Name = "InitVars";
                    dataSet1.Members.Add(mem);

                    mem.Parameters.Add(
                        new CodeParameterDeclarationExpression(
                            typeof(bool)
                            , "initTable"
                            )
                        );

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    foreach (DataTable dt in ds.Tables) {
                        String Generator_TablePropName = "" + dt.ExtendedProperties["Generator_TablePropName"];
                        String Generator_TableClassName = "" + dt.ExtendedProperties["Generator_TableClassName"];
                        String Generator_TableVarName = "" + dt.ExtendedProperties["Generator_TableVarName"];

                        mem.Statements.Add(
                            new CodeAssignStatement(
                                new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , Generator_TableVarName
                                    )
                                    , new CodeCastExpression(
                                        Generator_TableClassName
                                        , new CodeIndexerExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodeBaseReferenceExpression()
                                                , "Tables"
                                                )
                                            , new CodePrimitiveExpression(Generator_TablePropName)
                                            )
                                        )
                                    )
                            );
                        mem.Statements.Add(
                            new CodeConditionStatement(
                                new CodeBinaryOperatorExpression(
                                    new CodeBinaryOperatorExpression(// left
                                        new CodeVariableReferenceExpression("initTable")// left
                                        , CodeBinaryOperatorType.ValueEquality// op
                                        , new CodePrimitiveExpression(true)// right
                                        )
                                    , CodeBinaryOperatorType.BooleanAnd// op
                                    , new CodeBinaryOperatorExpression(// right
                                        new CodeFieldReferenceExpression(// left
                                            new CodeThisReferenceExpression()
                                            , Generator_TableVarName
                                            )
                                        , CodeBinaryOperatorType.IdentityInequality// op
                                        , new CodePrimitiveExpression(null)// right
                                        )
                                    )
                                , new CodeExpressionStatement(// true
                                    new CodeMethodInvokeExpression(
                                        new CodeFieldReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , Generator_TableVarName
                                            )
                                            , "InitVars"
                                        )
                                    )
                                )
                            );
                    }

                    foreach (DataRelation rel in ds.Relations) {
                        String Generator_RelationVarName = "" + rel.ExtendedProperties["Generator_RelationVarName"];
                        String Generator_UserRelationName = "" + rel.ExtendedProperties["Generator_UserRelationName"];

                        mem.Statements.Add(
                            new CodeAssignStatement(
                                new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , Generator_RelationVarName
                                    )
                                , new CodeIndexerExpression(
                                    new CodePropertyReferenceExpression(
                                        new CodeThisReferenceExpression()
                                        , "Relations"
                                        )
                                    , new CodePrimitiveExpression(Generator_UserRelationName)
                                    )
                                )
                            );
                    }
                }

                // InitClass
                {
                    var mem = new CodeMemberMethod();
                    mem.Attributes = MemberAttributes.Private;
                    mem.Name = "InitClass";
                    dataSet1.Members.Add(mem);

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    mem.Statements.Add(
                        new CodeAssignStatement(
                            new CodePropertyReferenceExpression(
                                new CodeThisReferenceExpression()
                                , "DataSetName"
                                )
                            , new CodePrimitiveExpression(Generator_DataSetName)
                            )
                        );
                    mem.Statements.Add(
                        new CodeAssignStatement(
                            new CodePropertyReferenceExpression(
                                new CodeThisReferenceExpression()
                                , "Prefix"
                                )
                            , new CodePrimitiveExpression("")
                            )
                        );
                    mem.Statements.Add(
                        new CodeAssignStatement(
                            new CodePropertyReferenceExpression(
                                new CodeThisReferenceExpression()
                                , "Namespace"
                                )
                            , new CodePrimitiveExpression(targetNamespace)
                            )
                        );
                    mem.Statements.Add(
                        new CodeAssignStatement(
                            new CodePropertyReferenceExpression(
                                new CodeThisReferenceExpression()
                                , "EnforceConstraints"
                                )
                            , new CodePrimitiveExpression(true)
                            )
                        );
                    mem.Statements.Add(
                        new CodeAssignStatement(
                            new CodePropertyReferenceExpression(
                                new CodeThisReferenceExpression()
                                , "SchemaSerializationMode"
                                )
                            , new CodeFieldReferenceExpression(
                                new CodeTypeReferenceExpression(typeof(SchemaSerializationMode))
                                , "IncludeSchema"
                                )
                            )
                        );

                    foreach (DataTable dt in ds.Tables) {
                        String Generator_TableVarName = "" + dt.ExtendedProperties["Generator_TableVarName"];
                        String Generator_TableClassName = "" + dt.ExtendedProperties["Generator_TableClassName"];

                        mem.Statements.Add(
                            new CodeAssignStatement(
                                new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , Generator_TableVarName
                                    )
                                , new CodeObjectCreateExpression(
                                    Generator_TableClassName
                                    )
                                )
                            );
                        mem.Statements.Add(
                            new CodeMethodInvokeExpression(
                                new CodePropertyReferenceExpression(
                                    new CodeBaseReferenceExpression()
                                    , "Tables"
                                    )
                                , "Add"
                                , new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , Generator_TableVarName
                                    )
                                )
                            );
                    }

                    foreach (DataRelation rel in ds.Relations) {
                        String Generator_RelationVarName = "" + rel.ExtendedProperties["Generator_RelationVarName"];
                        String Generator_UserRelationName = "" + rel.ExtendedProperties["Generator_UserRelationName"];

                        var parentColumns = new CodeArrayCreateExpression(
                            typeof(DataColumn)
                            );

                        var childColumns = new CodeArrayCreateExpression(
                            typeof(DataColumn)
                            );

                        for (int w = 0; w < 2; w++) {
                            foreach (DataColumn dc in ((w == 0) ? rel.ParentColumns : rel.ChildColumns)) {
                                DataTable dt = dc.Table;
                                String Generator_TableVarName = "" + dt.ExtendedProperties["Generator_TableVarName"];
                                String Generator_ColumnPropNameInTable = "" + dc.ExtendedProperties["Generator_ColumnPropNameInTable"];

                                ((w == 0) ? parentColumns : childColumns).Initializers.Add(
                                    new CodePropertyReferenceExpression(
                                        new CodeFieldReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , Generator_TableVarName
                                            )
                                        , Generator_ColumnPropNameInTable
                                        )
                                    );
                            }
                        }

                        mem.Statements.Add(
                            new CodeAssignStatement(
                                new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , Generator_RelationVarName
                                    )
                                , new CodeObjectCreateExpression(
                                    typeof(DataRelation)
                                    , new CodePrimitiveExpression(Generator_UserRelationName)
                                    , parentColumns
                                    , childColumns
                                    , new CodePrimitiveExpression(false)
                                    )
                                )
                            );

                        mem.Statements.Add(
                            new CodeExpressionStatement(
                                new CodeMethodInvokeExpression(
                                    new CodePropertyReferenceExpression(
                                        new CodeThisReferenceExpression()
                                        , "Relations"
                                        )
                                    , "Add"
                                    , new CodeFieldReferenceExpression(
                                        new CodeThisReferenceExpression()
                                        , Generator_RelationVarName
                                        )
                                    )
                                )
                            );
                    }
                }

                // ShouldSerialize???
                {
                    foreach (DataTable dt in ds.Tables) {
                        String Generator_TableVarName = "" + dt.ExtendedProperties["Generator_TableVarName"];
                        String Generator_TableClassName = "" + dt.ExtendedProperties["Generator_TableClassName"];
                        String Generator_TablePropName = "" + dt.ExtendedProperties["Generator_TablePropName"];

                        var mem = new CodeMemberMethod();
                        mem.Attributes = MemberAttributes.Private | MemberAttributes.Final;
                        mem.Name = "ShouldSerialize" + Generator_TablePropName;
                        mem.ReturnType = new CodeTypeReference(typeof(bool));
                        dataSet1.Members.Add(mem);

                        mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                            new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                            ));

                        mem.Statements.Add(
                            new CodeMethodReturnStatement(
                                new CodePrimitiveExpression(false)
                                )
                            );
                    }
                }

                // SchemaChanged
                {
                    var mem = new CodeMemberMethod();
                    mem.Attributes = MemberAttributes.Private | MemberAttributes.Final;
                    mem.Name = "SchemaChanged";
                    dataSet1.Members.Add(mem);

                    mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                        new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                        ));

                    mem.Parameters.Add(
                        new CodeParameterDeclarationExpression(
                            typeof(object)
                            , "sender"
                            )
                        );
                    mem.Parameters.Add(
                        new CodeParameterDeclarationExpression(
                            typeof(CollectionChangeEventArgs)
                            , "e"
                            )
                        );

                    mem.Statements.Add(
                        new CodeConditionStatement(
                            new CodeBinaryOperatorExpression(
                                new CodePropertyReferenceExpression( // left
                                    new CodeVariableReferenceExpression("e")
                                    , "Action"
                                    )
                                , CodeBinaryOperatorType.ValueEquality // op
                                , new CodeFieldReferenceExpression( // right
                                    new CodeTypeReferenceExpression(
                                        typeof(CollectionChangeAction)
                                    )
                                    , "Remove"
                                    )
                                )
                            , new CodeExpressionStatement(
                                new CodeMethodInvokeExpression( // true
                                    new CodeThisReferenceExpression()
                                    , "InitVars"
                                    )
                                )
                            )
                        );
                }

                // GetTypedDataSetSchema

                // ???ChangeEventHandler
                {
                    foreach (DataTable dt in ds.Tables) {
                        String Generator_RowEvHandlerName = "" + dt.ExtendedProperties["Generator_RowEvHandlerName"];
                        String Generator_RowEvArgName = "" + dt.ExtendedProperties["Generator_RowEvArgName"];

                        var mem = new CodeTypeDelegate();
                        mem.Attributes = MemberAttributes.Public;
                        mem.Name = Generator_RowEvHandlerName;
                        mem.Parameters.Add(
                            new CodeParameterDeclarationExpression(
                                typeof(object)
                                , "sender"
                                )
                            );
                        mem.Parameters.Add(
                            new CodeParameterDeclarationExpression(
                                Generator_RowEvArgName
                                , "e"
                                )
                            );

                        dataSet1.Members.Add(mem);
                    }
                }

                // ???DataTable
                {
                    foreach (DataTable dt in ds.Tables) {
                        String Generator_TableClassName = "" + dt.ExtendedProperties["Generator_TableClassName"];
                        String Generator_RowClassName = "" + dt.ExtendedProperties["Generator_RowClassName"];
                        String Generator_TablePropName = "" + dt.ExtendedProperties["Generator_TablePropName"];

                        String Generator_RowEvHandlerName = "" + dt.ExtendedProperties["Generator_RowEvHandlerName"];

                        var cls = new CodeTypeDeclaration(Generator_TableClassName);
                        cls.Attributes = MemberAttributes.Public;
                        cls.IsPartial = true;
                        cls.BaseTypes.Add(
                            new CodeTypeReference(
                                typeof(System.Data.TypedTableBase<>).FullName
                                , new CodeTypeReference(Generator_RowClassName)
                                )
                            );

                        foreach (DataColumn dc in dt.Columns) {
                            String Generator_ColumnVarNameInTable = "" + dc.ExtendedProperties["Generator_ColumnVarNameInTable"];

                            cls.Members.Add(
                                new CodeMemberField(
                                    typeof(DataColumn)
                                    , Generator_ColumnVarNameInTable
                                    )
                                );
                        }

                        // ctor
                        {
                            var mem = new CodeConstructor();
                            mem.Attributes = MemberAttributes.Public | MemberAttributes.Final;

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeThisReferenceExpression()
                                        , "TableName"
                                        )
                                        , new CodePrimitiveExpression(Generator_TablePropName)
                                    )
                                );
                            mem.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodeThisReferenceExpression()
                                    , "BeginInit"
                                    )
                                );
                            mem.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodeThisReferenceExpression()
                                    , "InitClass"
                                    )
                                );
                            mem.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodeThisReferenceExpression()
                                    , "EndInit"
                                    )
                                );

                            cls.Members.Add(mem);
                        }

                        // ctor(DataTable table)
                        {
                            var mem = new CodeConstructor();
                            mem.Attributes = MemberAttributes.Family | MemberAttributes.Final;
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Parameters.Add(
                                new CodeParameterDeclarationExpression(
                                    typeof(DataTable)
                                    , "table"
                                    )
                                );

                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeThisReferenceExpression()
                                        , "TableName"
                                        )
                                    , new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("table")
                                        , "TableName"
                                        )
                                    )
                                );

                            mem.Statements.Add(
                                new CodeConditionStatement(
                                    new CodeBinaryOperatorExpression(
                                        new CodePropertyReferenceExpression(
                                            new CodeVariableReferenceExpression("table")
                                            , "CaseSensitive"
                                            )
                                        , CodeBinaryOperatorType.IdentityInequality
                                        , new CodePropertyReferenceExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodeVariableReferenceExpression("table")
                                                , "DataSet"
                                                )
                                            , "CaseSensitive"
                                            )
                                        )
                                    , new CodeAssignStatement(
                                        new CodePropertyReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , "CaseSensitive"
                                            )
                                        , new CodePropertyReferenceExpression(
                                            new CodeVariableReferenceExpression("table")
                                            , "CaseSensitive"
                                            )
                                        )
                                    )
                                );

                            mem.Statements.Add(
                                new CodeConditionStatement(
                                    new CodeBinaryOperatorExpression(
                                        new CodeMethodInvokeExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodeVariableReferenceExpression("table")
                                                , "Locale"
                                                )
                                            , "ToString"
                                            )
                                        , CodeBinaryOperatorType.IdentityInequality
                                        , new CodeMethodInvokeExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodePropertyReferenceExpression(
                                                    new CodeVariableReferenceExpression("table")
                                                    , "DataSet"
                                                    )
                                                , "Locale"
                                                )
                                            , "ToString"
                                            )
                                        )
                                    , new CodeAssignStatement(
                                        new CodePropertyReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , "Locale"
                                            )
                                        , new CodePropertyReferenceExpression(
                                            new CodeVariableReferenceExpression("table")
                                            , "Locale"
                                            )
                                        )
                                    )
                                );

                            mem.Statements.Add(
                                new CodeConditionStatement(
                                    new CodeBinaryOperatorExpression(
                                        new CodePropertyReferenceExpression(
                                            new CodeVariableReferenceExpression("table")
                                            , "Namespace"
                                            )
                                        , CodeBinaryOperatorType.IdentityInequality
                                        , new CodePropertyReferenceExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodeVariableReferenceExpression("table")
                                                , "DataSet"
                                                )
                                            , "Namespace"
                                            )
                                        )
                                    , new CodeAssignStatement(
                                        new CodePropertyReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , "Namespace"
                                            )
                                        , new CodePropertyReferenceExpression(
                                            new CodeVariableReferenceExpression("table")
                                            , "Namespace"
                                            )
                                        )
                                    )
                                );

                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeThisReferenceExpression()
                                        , "Prefix"
                                        )
                                    , new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("table")
                                        , "Prefix"
                                        )
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeThisReferenceExpression()
                                        , "MinimumCapacity"
                                        )
                                    , new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("table")
                                        , "MinimumCapacity"
                                        )
                                    )
                                );

                        }

                        // ctor(SerializationInfo info, StreamingContext context)
                        {
                            var mem = new CodeConstructor();
                            mem.Attributes = MemberAttributes.FamilyAndAssembly | MemberAttributes.Final;
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Parameters.Add(
                                new CodeParameterDeclarationExpression(
                                    typeof(SerializationInfo)
                                    , "info"
                                    )
                                );
                            mem.Parameters.Add(
                                new CodeParameterDeclarationExpression(
                                    typeof(StreamingContext)
                                    , "context"
                                    )
                                );

                            mem.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("info"));
                            mem.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("context"));

                            mem.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodeThisReferenceExpression()
                                    , "InitVars"
                                    )
                                );
                        }

                        // ???Column
                        {
                            foreach (DataColumn dc in dt.Columns) {
                                String Generator_ColumnPropNameInTable = "" + dc.ExtendedProperties["Generator_ColumnPropNameInTable"];
                                String Generator_ColumnVarNameInTable = "" + dc.ExtendedProperties["Generator_ColumnVarNameInTable"];

                                var mem = new CodeMemberProperty();
                                mem.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                                mem.Name = Generator_ColumnPropNameInTable;
                                mem.Type = new CodeTypeReference(typeof(DataColumn));
                                cls.Members.Add(mem);

                                mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                    new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                    ));

                                mem.GetStatements.Add(
                                    new CodeMethodReturnStatement(
                                        new CodeFieldReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , Generator_ColumnVarNameInTable
                                            )
                                        )
                                    );
                            }
                        }

                        // Count
                        {
                            var mem = new CodeMemberProperty();
                            mem.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                            mem.Name = "Count";
                            mem.Type = new CodeTypeReference(typeof(int));
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));
                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(BrowsableAttribute))
                                , new CodeAttributeArgument(
                                    new CodePrimitiveExpression(false)
                                    )
                                ));

                            mem.GetStatements.Add(
                                new CodeMethodReturnStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodePropertyReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , "Rows"
                                            )
                                        , "Count"
                                        )
                                    )
                                );
                        }


                        // this[]
                        {
                            var mem = new CodeMemberProperty();
                            mem.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                            mem.Name = "Item";
                            mem.Type = new CodeTypeReference(Generator_RowClassName);
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Parameters.Add(
                                new CodeParameterDeclarationExpression(
                                    typeof(int)
                                    , "index"
                                    )
                                );

                            mem.GetStatements.Add(
                                new CodeMethodReturnStatement(
                                    new CodeCastExpression(
                                        Generator_RowClassName
                                        , new CodeIndexerExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodeThisReferenceExpression()
                                                , "Rows"
                                                )
                                            , new CodeVariableReferenceExpression("index")
                                            )
                                        )
                                    )
                                );
                        }

                        // ???RowChanging
                        // ???RowChanged
                        // ???RowDeleting
                        // ???RowDeleted
                        {
                            for (int w = 0; w < 4; w++) {
                                var mem = new CodeMemberEvent();
                                cls.Members.Add(mem);
                                mem.Attributes = MemberAttributes.Public;
                                if (false) { }
                                else if (w == 0) mem.Name = "" + dt.ExtendedProperties["Generator_RowChangingName"];
                                else if (w == 1) mem.Name = "" + dt.ExtendedProperties["Generator_RowChangedName"];
                                else if (w == 2) mem.Name = "" + dt.ExtendedProperties["Generator_RowDeletingName"];
                                else if (w == 3) mem.Name = "" + dt.ExtendedProperties["Generator_RowDeletedName"];

                                mem.Type = new CodeTypeReference(Generator_RowEvHandlerName);
                            }
                        }

                        // Add???Row
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                            mem.Name = String.Format("Add{0}", Generator_RowClassName);
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Parameters.Add(
                                new CodeParameterDeclarationExpression(
                                    Generator_RowClassName
                                    , "row"
                                    )
                                );

                            mem.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodePropertyReferenceExpression(
                                        new CodeThisReferenceExpression()
                                        , "Rows"
                                        )
                                    , "Add"
                                    , new CodeVariableReferenceExpression("row")
                                    )
                                );
                        }

                        // Add???Row
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                            mem.Name = String.Format("Add{0}", Generator_RowClassName);
                            mem.ReturnType = new CodeTypeReference(Generator_RowClassName);
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Statements.Add(
                                new CodeVariableDeclarationStatement(
                                    Generator_RowClassName
                                    , "row"
                                    , new CodeCastExpression(
                                        Generator_RowClassName
                                        , new CodeMethodInvokeExpression(
                                            new CodeThisReferenceExpression()
                                            , "NewRow"
                                            )
                                        )
                                    )
                                );

                            CodeArrayCreateExpression vals;

                            mem.Statements.Add(
                                new CodeVariableDeclarationStatement(
                                    typeof(object[])
                                    , "values"
                                    , vals = new CodeArrayCreateExpression(
                                        typeof(object)
                                        )
                                    )
                                );

                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("row")
                                        , "ItemArray"
                                        )
                                    , new CodeVariableReferenceExpression("values")
                                    )
                                );

                            mem.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodePropertyReferenceExpression(
                                        new CodeThisReferenceExpression()
                                        , "Rows"
                                        )
                                    , "Add"
                                    , new CodeVariableReferenceExpression("row")
                                    )
                                );

                            mem.Statements.Add(
                                new CodeMethodReturnStatement(
                                    new CodeVariableReferenceExpression("row")
                                    )
                                );

                            foreach (DataColumn dc in dt.Columns) {
                                String Generator_ColumnPropNameInRow = "" + dc.ExtendedProperties["Generator_ColumnPropNameInRow"];
                                String Generator_ColumnVarNameInTable = "" + dc.ExtendedProperties["Generator_ColumnVarNameInTable"];
                                bool isNullable = dc.AllowDBNull;
                                bool isValueType = dc.DataType.IsValueType;
                                bool autoIncrement = dc.AutoIncrement;
                                var colTy = isValueType && isNullable
                                    ? new CodeTypeReference(typeof(Nullable<>).FullName, new CodeTypeReference(dc.DataType))
                                    : new CodeTypeReference(dc.DataType)
                                    ;

                                if (!autoIncrement) {
                                    mem.Parameters.Add(
                                        new CodeParameterDeclarationExpression(
                                            colTy
                                            , Generator_ColumnPropNameInRow
                                            )
                                        );

                                    vals.Initializers.Add(
                                        new CodeVariableReferenceExpression(Generator_ColumnPropNameInRow)
                                        );
                                }
                                else {
                                    vals.Initializers.Add(
                                        new CodePrimitiveExpression(null)
                                        );
                                }
                            }
                        }

                        // FindBy???
                        if (dt.PrimaryKey != null && dt.PrimaryKey.Length != 0) {
                            DataColumn[] pks = dt.PrimaryKey;

                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                            mem.Name = String.Format("FindBy{0}", String.Join("_", pks.Select(p => "" + p.ExtendedProperties["Generator_ColumnPropNameInRow"]).ToArray()));
                            mem.ReturnType = new CodeTypeReference(Generator_RowClassName);
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            CodeArrayCreateExpression ce = new CodeArrayCreateExpression(
                                typeof(object)
                                );

                            foreach (DataColumn dc in pks) {
                                mem.Parameters.Add(
                                    new CodeParameterDeclarationExpression(
                                        dc.DataType
                                        , "" + dc.ExtendedProperties["Generator_ColumnPropNameInRow"]
                                        )
                                    );

                                ce.Initializers.Add(
                                    new CodeVariableReferenceExpression(
                                        "" + dc.ExtendedProperties["Generator_ColumnPropNameInRow"]
                                        )
                                    );
                            }

                            mem.Statements.Add(
                                new CodeMethodReturnStatement(
                                    new CodeCastExpression(
                                        mem.ReturnType
                                        , new CodeMethodInvokeExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodeThisReferenceExpression()
                                                , "Rows"
                                                )
                                            , "Find"
                                            , ce
                                            )
                                        )
                                    )
                                );
                        }

                        // Clone
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                            mem.Name = "Clone";
                            mem.ReturnType = new CodeTypeReference(typeof(DataTable));
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Statements.Add(
                                new CodeVariableDeclarationStatement(
                                    Generator_TableClassName
                                    , "cln"
                                    , new CodeCastExpression(
                                        Generator_TableClassName
                                        , new CodeMethodInvokeExpression(
                                            new CodeBaseReferenceExpression()
                                            , "Clone"
                                            )
                                        )
                                    )
                                );
                            mem.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodeVariableReferenceExpression("cln")
                                    , "InitVars"
                                    )
                                );
                            mem.Statements.Add(
                                new CodeMethodReturnStatement(
                                    new CodeVariableReferenceExpression("cln")
                                    )
                                );
                        }

                        // CreateInstance
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                            mem.Name = "CreateInstance";
                            mem.ReturnType = new CodeTypeReference(typeof(DataTable));
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Statements.Add(
                                new CodeMethodReturnStatement(
                                    new CodeObjectCreateExpression(
                                        Generator_TableClassName
                                        )
                                    )
                                );
                        }

                        // InitVars
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.FamilyAndAssembly;
                            mem.Name = "InitVars";
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            foreach (DataColumn dc in dt.Columns) {
                                String Generator_ColumnVarNameInTable = "" + dc.ExtendedProperties["Generator_ColumnVarNameInTable"];

                                mem.Statements.Add(
                                    new CodeAssignStatement(
                                        new CodeFieldReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , Generator_ColumnVarNameInTable
                                            )
                                        , new CodeIndexerExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodeBaseReferenceExpression()
                                                , "Columns"
                                                )
                                            , new CodePrimitiveExpression(dc.ColumnName)
                                            )
                                        )
                                    );
                            }
                        }

                        // InitClass
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Private | MemberAttributes.Final;
                            mem.Name = "InitClass";
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            foreach (DataColumn dc in dt.Columns) {
                                String Generator_ColumnVarNameInTable = "" + dc.ExtendedProperties["Generator_ColumnVarNameInTable"];
                                String Generator_UserColumnName = "" + dc.ExtendedProperties["Generator_UserColumnName"];

                                mem.Statements.Add(
                                    new CodeAssignStatement(
                                        new CodeFieldReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , Generator_ColumnVarNameInTable
                                            )
                                        , new CodeObjectCreateExpression(
                                            typeof(DataColumn)
                                            , new CodePrimitiveExpression(dc.ColumnName)
                                            , new CodeTypeOfExpression(dc.DataType)
                                            , new CodePrimitiveExpression(dc.Expression)
                                            , GUt.GenVal(dc.ColumnMapping)
                                            )
                                        )
                                    );

                                foreach (Object k in dc.ExtendedProperties.Keys) {
                                    Object v = dc.ExtendedProperties[k];
                                    mem.Statements.Add(
                                        new CodeMethodInvokeExpression(
                                            new CodePropertyReferenceExpression(
                                                new CodeFieldReferenceExpression(
                                                    new CodeThisReferenceExpression()
                                                    , Generator_ColumnVarNameInTable
                                                    )
                                                , "ExtendedProperties"
                                                )
                                            , "Add"
                                            , new CodePrimitiveExpression(k)
                                            , new CodePrimitiveExpression(v)
                                            )
                                        );

                                }

                                mem.Statements.Add(
                                    new CodeMethodInvokeExpression(
                                        new CodePropertyReferenceExpression(
                                            new CodeBaseReferenceExpression()
                                            , "Columns"
                                            )
                                        , "Add"
                                        , new CodeFieldReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , Generator_ColumnVarNameInTable
                                            )
                                        )
                                    );

                            }

                            foreach (System.Data.UniqueConstraint dco in dt.Constraints) {
                                var cols = dco.Columns.Select(p => new CodeFieldReferenceExpression(
                                    new CodeThisReferenceExpression()
                                    , "" + p.ExtendedProperties["Generator_ColumnVarNameInTable"]
                                    )
                                );

                                mem.Statements.Add(
                                    new CodeMethodInvokeExpression(
                                        new CodePropertyReferenceExpression(
                                            new CodeThisReferenceExpression()
                                            , "Constraints"
                                            )
                                        , "Add"
                                        , new CodeObjectCreateExpression(
                                            typeof(System.Data.UniqueConstraint)
                                            , new CodePrimitiveExpression(dco.ConstraintName)
                                            , new CodeArrayCreateExpression(
                                                typeof(DataColumn)
                                                , cols.ToArray()
                                                )
                                            )
                                        )
                                    );
                            }

                            foreach (DataColumn dc in dt.Columns) {
                                DiffUt d = new DiffUt();
                                d.Add(dc.AutoIncrement, "AutoIncrement", false);
                                d.Add(dc.AutoIncrementSeed, "AutoIncrementSeed", 0L);
                                d.Add(dc.AutoIncrementStep, "AutoIncrementStep", 1L);
                                d.Add(dc.AllowDBNull, "AllowDBNull", true);
                                d.Add(dc.Caption, "Caption", dc.ColumnName);
                                d.Add(dc.ColumnMapping, "ColumnMapping", MappingType.Element);
                                d.Add(dc.DateTimeMode, "DateTimeMode", DataSetDateTime.UnspecifiedLocal);
                                d.Add(dc.DefaultValue, "DefaultValue", DBNull.Value);
                                d.Add(dc.Expression, "Expression", "");
                                d.Add(dc.MaxLength, "MaxLength", -1);
                                d.Add(dc.Namespace, "Namespace", dc.Namespace);
                                d.Add(dc.Prefix, "Prefix", "");
                                d.Add(dc.ReadOnly, "ReadOnly", false);

                                foreach (var kv in d.Items) {
                                    mem.Statements.Add(
                                        new CodeAssignStatement(
                                            new CodePropertyReferenceExpression(
                                                new CodeFieldReferenceExpression(
                                                    new CodeThisReferenceExpression()
                                                    , "" + dc.ExtendedProperties["Generator_ColumnVarNameInTable"]
                                                    )
                                                , kv.Key
                                                )
                                            , GUt.GenVal(kv.Value)
                                            )
                                        );
                                }
                            }
                        }

                        // New?Row
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                            mem.Name = String.Format("New{0}Row", dt.ExtendedProperties["Generator_TablePropName"]);
                            mem.ReturnType = new CodeTypeReference("" + dt.ExtendedProperties["Generator_RowClassName"]);
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Statements.Add(
                                new CodeMethodReturnStatement(
                                    new CodeCastExpression(
                                        mem.ReturnType
                                        , new CodeMethodInvokeExpression(
                                            new CodeThisReferenceExpression()
                                            , "NewRow"
                                            )
                                        )
                                    )
                                );
                        }

                        // NewRowFromBuilder
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                            mem.Name = "NewRowFromBuilder";
                            mem.ReturnType = new CodeTypeReference(typeof(DataRow));
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Parameters.Add(
                                new CodeParameterDeclarationExpression(
                                    typeof(DataRowBuilder)
                                    , "builder"
                                    )
                                );

                            mem.Statements.Add(
                                new CodeMethodReturnStatement(
                                    new CodeObjectCreateExpression(
                                        new CodeTypeReference("" + dt.ExtendedProperties["Generator_RowClassName"])
                                        , new CodeVariableReferenceExpression("builder")
                                        )
                                    )
                                );
                        }

                        // GetRowType
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                            mem.Name = "GetRowType";
                            mem.ReturnType = new CodeTypeReference(typeof(Type));
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                ));

                            mem.Statements.Add(
                                new CodeMethodReturnStatement(
                                    new CodeTypeOfExpression(
                                        new CodeTypeReference("" + dt.ExtendedProperties["Generator_RowClassName"])
                                        )
                                    )
                                );
                        }

                        // OnRowChanged
                        // OnRowChanging
                        // OnRowDeleted
                        // OnRowDeleting
                        {
                            for (int w = 0; w < 4; w++) {
                                var mem = new CodeMemberMethod();
                                mem.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                                if (false) { }
                                else if (w == 0) mem.Name = "OnRowChanged";
                                else if (w == 1) mem.Name = "OnRowChanging";
                                else if (w == 2) mem.Name = "OnRowDeleted";
                                else if (w == 3) mem.Name = "OnRowDeleting";
                                cls.Members.Add(mem);

                                mem.CustomAttributes.Add(new CodeAttributeDeclaration(
                                    new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                    ));

                                mem.Parameters.Add(
                                    new CodeParameterDeclarationExpression(
                                        typeof(DataRowChangeEventArgs)
                                        , "e"
                                        )
                                    );

                                String evName = null;
                                if (false) { }
                                else if (w == 0) evName = "" + dt.ExtendedProperties["Generator_RowChangedName"];
                                else if (w == 1) evName = "" + dt.ExtendedProperties["Generator_RowChangingName"];
                                else if (w == 2) evName = "" + dt.ExtendedProperties["Generator_RowDeletedName"];
                                else if (w == 3) evName = "" + dt.ExtendedProperties["Generator_RowDeletingName"];

                                mem.Statements.Add(
                                    new CodeMethodInvokeExpression(
                                        new CodeBaseReferenceExpression()
                                        , mem.Name
                                        , new CodeVariableReferenceExpression("e")
                                        )
                                    );

                                mem.Statements.Add(
                                    new CodeConditionStatement(
                                        new CodeBinaryOperatorExpression(
                                            new CodeEventReferenceExpression( //left
                                                new CodeThisReferenceExpression()
                                                , evName
                                                )
                                            , CodeBinaryOperatorType.IdentityInequality // op
                                            , new CodePrimitiveExpression(null) // right
                                            )
                                        , new CodeExpressionStatement( // true
                                            new CodeMethodInvokeExpression(
                                                new CodeThisReferenceExpression()
                                                , evName
                                                , new CodeThisReferenceExpression()
                                                , new CodeObjectCreateExpression(
                                                    "" + dt.ExtendedProperties["Generator_RowEvArgName"]
                                                    , new CodeCastExpression(
                                                        new CodeTypeReference("" + dt.ExtendedProperties["Generator_RowClassName"])
                                                        , new CodePropertyReferenceExpression(
                                                            new CodeVariableReferenceExpression("e")
                                                            , "Row"
                                                            )
                                                        )
                                                    , new CodePropertyReferenceExpression(
                                                        new CodeVariableReferenceExpression("e")
                                                        , "Action"
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    );
                            }
                        }

                        // GetTypedTableSchema
                        {
                            var mem = new CodeMemberMethod();
                            mem.Attributes = MemberAttributes.Family | MemberAttributes.Override;
                            mem.Name = "GetTypedTableSchema";
                            mem.ReturnType = new CodeTypeReference(typeof(XmlSchemaComplexType));
                            cls.Members.Add(mem);

                            mem.CustomAttributes.Add(
                                new CodeAttributeDeclaration(
                                    new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute))
                                    )
                                );

                            mem.Parameters.Add(
                                new CodeParameterDeclarationExpression(
                                    typeof(XmlSchemaSet)
                                    , "xs"
                                    )
                                );

                            mem.Statements.Add(
                                new CodeVariableDeclarationStatement(
                                    typeof(XmlSchemaComplexType)
                                    , "type"
                                    , new CodeObjectCreateExpression(
                                        typeof(XmlSchemaComplexType)
                                        )
                                    )
                                );
                            mem.Statements.Add(
                                new CodeVariableDeclarationStatement(
                                    typeof(XmlSchemaSequence)
                                    , "sequence"
                                    , new CodeObjectCreateExpression(
                                        typeof(XmlSchemaSequence)
                                        )
                                    )
                                );
                            mem.Statements.Add(
                                new CodeVariableDeclarationStatement(
                                    "" + ds.ExtendedProperties["Generator_DataSetName"]
                                    , "ds"
                                    , new CodeObjectCreateExpression(
                                        "" + ds.ExtendedProperties["Generator_DataSetName"]
                                        )
                                    )
                                );
                            mem.Statements.Add(
                                new CodeVariableDeclarationStatement(
                                    typeof(XmlSchemaAny)
                                    , "any1"
                                    , new CodeObjectCreateExpression(
                                        typeof(XmlSchemaAny)
                                        )
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("any1")
                                        , "Namespace"
                                        )
                                    , new CodePrimitiveExpression("http://www.w3.org/2001/XMLSchema")
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("any1")
                                        , "MinOccurs"
                                        )
                                    , new CodePrimitiveExpression(0)
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("any1")
                                        , "MaxOccurs"
                                        )
                                    , new CodeFieldReferenceExpression(
                                        new CodeTypeReferenceExpression(typeof(decimal))
                                        , "MaxValue"
                                        )
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("any1")
                                        , "ProcessContents"
                                        )
                                    , GUt.GenVal(XmlSchemaContentProcessing.Lax)
                                    )
                                );
                            mem.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("sequence")
                                        , "Items"
                                        )
                                    , "Add"
                                    , new CodeVariableReferenceExpression("any1")
                                    )
                                );

                            mem.Statements.Add(
                                new CodeVariableDeclarationStatement(
                                    typeof(XmlSchemaAny)
                                    , "any2"
                                    , new CodeObjectCreateExpression(
                                        typeof(XmlSchemaAny)
                                        )
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("any2")
                                        , "Namespace"
                                        )
                                    , new CodePrimitiveExpression("urn:schemas-microsoft-com:xml-diffgram-v1")
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("any2")
                                        , "MinOccurs"
                                        )
                                    , new CodePrimitiveExpression(1)
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("any2")
                                        , "ProcessContents"
                                        )
                                    , GUt.GenVal(XmlSchemaContentProcessing.Lax)
                                    )
                                );
                            mem.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("sequence")
                                        , "Items"
                                        )
                                    , "Add"
                                    , new CodeVariableReferenceExpression("any2")
                                    )
                                );

                            mem.Statements.Add(
                                new CodeVariableDeclarationStatement(
                                    typeof(XmlSchemaAttribute)
                                    , "attribute1"
                                    , new CodeObjectCreateExpression(
                                        typeof(XmlSchemaAttribute)
                                        )
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("attribute1")
                                        , "Name"
                                        )
                                    , new CodePrimitiveExpression("namespace")
                                    )
                                );
                            mem.Statements.Add(
                                new CodeAssignStatement(
                                    new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("attribute1")
                                        , "FixedValue"
                                        )
                                    , new CodePropertyReferenceExpression(
                                        new CodeVariableReferenceExpression("ds")
                                        , "Namespace"
                                        )
                                    )
                                );


                        }

                        dataSet1.Members.Add(cls);
                    }
                }
            }

            {
                CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
                codeCompileUnit.Namespaces.Add(csns);

                var codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
                var codeGeneratorOptions = new CodeGeneratorOptions();
                using (StreamWriter wr = new StreamWriter(fpcs, false)) {
                    codeProvider.GenerateCodeFromCompileUnit(codeCompileUnit, wr, codeGeneratorOptions);
                }
            }
        }


        class DiffUt {
            public DiffUt() {

            }

            public void Add(Object val, String name, Object defval) {
                if (val == null && defval == null) {
                    return;
                }
                else if (val == null || defval == null) {

                }
                else if (defval.Equals(val)) {
                    return;
                }

                _Items.Add(name, val);
            }

            Dictionary<string, object> _Items = new Dictionary<string, object>();

            public IDictionary<string, object> Items { get { return new Dictionary<string, object>(_Items); } }
        }

        class GUt {
            internal static void Assign1(CodeStatementCollection stmts, Type ty, Object val, String name, Object defval) {
                if (val == null && defval == null) {
                    return;
                }
                else if (val == null || defval == null) {

                }
                else if (defval.Equals(val)) {
                    return;
                }

                stmts.Add(
                    new CodeAssignStatement(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression()
                            , name
                            )
                        , GenVal(val)
                        )
                    );
            }

            internal static CodeExpression GenVal(Object val) {
                if (val is Enum) {
                    Enum e = (Enum)val;
                    return new CodeFieldReferenceExpression(
                        new CodeTypeReferenceExpression(e.GetType())
                        , e.ToString("G")
                        );
                }
                if (val is DBNull) {
                    return new CodeFieldReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(DBNull))
                        , "Value"
                        );
                }
                return new CodePrimitiveExpression(val);
            }
        }
    }
}
