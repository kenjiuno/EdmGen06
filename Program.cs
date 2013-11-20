using EdmGen06.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

#if ENTITIES6
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.EntityClient;
#else
using System.Data.Common;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
#endif

namespace EdmGen06 {
    class Program {
        static void Main(string[] args) {
            if (args.Length >= 5 && args[0] == "/ModelGen") {
                new Program().ModelGen(
                    args[1],
                    args[2],
                    args[3],
                    args[4],
                    (args.Length > 5) ? new Version(args[5]) : new Version("3.0")
                    );
                return;
            }
            Console.Error.WriteLine(APP);
            Console.Error.WriteLine(" /ModelGen <connectionString> <providerName> <modelName> <targetSchema> <ver>");
            Environment.ExitCode = 1;
        }

        /// XName namespace: provider manifest ssdl
        String pSSDL;
        /// XName namespace: provider manifest csdl
        String pCSDL;
        /// XName namespace: provider manifest msl
        String pMSL;

        /// XName namespace: ModelGen edmx 
        String xEDMX;
        /// XName namespace: ModelGen ssdl 
        String xSSDL;
        /// XName namespace: ModelGen csdl 
        String xCSDL;
        /// XName namespace: ModelGen msl
        String xMSL;
        /// XName namespace: EntityStoreSchemaGenerator
        String xSTORE = "{" + "http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator" + "}";
        /// XName namespace: annotation
        String xAnno = "{" + "http://schemas.microsoft.com/ado/2009/02/edm/annotation" + "}";

        public class NS {
            public static string EDMXv1 { get { return "http://schemas.microsoft.com/ado/2007/06/edmx"; } }
            public static string EDMXv2 { get { return "http://schemas.microsoft.com/ado/2008/10/edmx"; } }
            public static string EDMXv3 { get { return "http://schemas.microsoft.com/ado/2009/11/edmx"; } }

            public static string SSDLv1 { get { return "http://schemas.microsoft.com/ado/2006/04/edm/ssdl"; } }
            public static string SSDLv2 { get { return "http://schemas.microsoft.com/ado/2009/02/edm/ssdl"; } }
            public static string SSDLv3 { get { return "http://schemas.microsoft.com/ado/2009/11/edm/ssdl"; } }

            public static string MSLv1 { get { return "urn:schemas-microsoft-com:windows:storage:mapping:CS"; } }
            public static string MSLv2 { get { return "http://schemas.microsoft.com/ado/2008/09/mapping/cs"; } }
            public static string MSLv3 { get { return "http://schemas.microsoft.com/ado/2009/11/mapping/cs"; } }

            public static string CSDLv1 { get { return "http://schemas.microsoft.com/ado/2006/04/edm"; } }
            public static string CSDLv2 { get { return "http://schemas.microsoft.com/ado/2008/09/edm"; } }
            public static string CSDLv3 { get { return "http://schemas.microsoft.com/ado/2009/11/edm"; } }
        }

        public Program() {
            trace.Listeners.Add(new ConsoleTraceListener(false));
        }

#if ENTITIES6
        static String APP { get { return "EdmGen06"; } }
#else
        static String APP { get { return "EdmGen04"; } }
#endif

        TraceSource trace = new TraceSource(APP, SourceLevels.All);

        void ModelGen(String connectionString, String providerName, String modelName, String targetSchema, Version yver) {
            String baseDir = Environment.CurrentDirectory;

            String fpssdl3 = Path.Combine(baseDir, modelName + ".ssdl");
            String fpcsdl3 = Path.Combine(baseDir, modelName + ".csdl");
            String fpmsl3 = Path.Combine(baseDir, modelName + ".msl");
            String fpedmx3 = Path.Combine(baseDir, modelName + ".edmx");

            if (false) { }
            else if (yver == new Version(1, 0)) { xEDMX = "{" + NS.EDMXv1 + "}"; xSSDL = "{" + NS.SSDLv1 + "}"; xCSDL = "{" + NS.CSDLv1 + "}"; xMSL = "{" + NS.MSLv1 + "}"; trace.TraceEvent(TraceEventType.Information, 101, "ModelGen v1"); }
            else if (yver == new Version(3, 0)) { xEDMX = "{" + NS.EDMXv3 + "}"; xSSDL = "{" + NS.SSDLv3 + "}"; xCSDL = "{" + NS.CSDLv3 + "}"; xMSL = "{" + NS.MSLv3 + "}"; trace.TraceEvent(TraceEventType.Information, 101, "ModelGen v3"); }
            else throw new NotSupportedException(String.Format("Version '{0}', from 1.0 3.0 ", yver));

            trace.TraceEvent(TraceEventType.Information, 101, "Getting System.Data.Common.DbProviderFactory from '{0}'", providerName);
            var fac = System.Data.Common.DbProviderFactories.GetFactory(providerName);
            if (fac == null) throw new ApplicationException();
            trace.TraceEvent(TraceEventType.Information, 101, fac.GetType().AssemblyQualifiedName);
            trace.TraceEvent(TraceEventType.Information, 101, "Ok");


            using (var db = fac.CreateConnection()) {
                trace.TraceEvent(TraceEventType.Information, 101, "Connecting");
                db.ConnectionString = connectionString;
                db.Open();
                trace.TraceEvent(TraceEventType.Information, 101, "Connected");

                trace.TraceEvent(TraceEventType.Information, 101, "Getting System.Data.Entity.Core.Common.DbProviderServices from '{0}'", providerName);
                var providerServices = ((IServiceProvider)fac).GetService(typeof(DbProviderServices)) as DbProviderServices;
                if (providerServices == null) providerServices = DbProviderServices.GetProviderServices(db);
                trace.TraceEvent(TraceEventType.Information, 101, providerServices.GetType().AssemblyQualifiedName);
                trace.TraceEvent(TraceEventType.Information, 101, "Ok");

                trace.TraceEvent(TraceEventType.Information, 101, "Get ProviderManifestToken");
                var providerManifestToken = providerServices.GetProviderManifestToken(db);
                trace.TraceEvent(TraceEventType.Information, 101, "Get ProviderManifest");
                var providerManifest = providerServices.GetProviderManifest(providerManifestToken) as DbProviderManifest;

                trace.TraceEvent(TraceEventType.Information, 101, "Get StoreSchemaDefinition");
                var storeSchemaDefinition = providerManifest.GetInformation("StoreSchemaDefinition") as XmlReader;
                trace.TraceEvent(TraceEventType.Information, 101, "Get StoreSchemaMapping");
                var storeSchemaMapping = providerManifest.GetInformation("StoreSchemaMapping") as XmlReader;

                trace.TraceEvent(TraceEventType.Information, 101, "Write temporary ProviderManifest ssdl");
                XDocument tSsdl;
                String fpssdl = Path.Combine(baseDir, "__" + providerName + ".ssdl");
                String fpssdl2 = Path.Combine(baseDir, "__" + providerName + ".ssdl.xml");
                {
                    tSsdl = XDocument.Load(storeSchemaDefinition);
                    tSsdl.Save(fpssdl);
                    tSsdl.Save(fpssdl2);
                }

                trace.TraceEvent(TraceEventType.Information, 101, "Write temporary ProviderManifest msl");
                XDocument tMsl;
                String fpmsl = Path.Combine(baseDir, "__" + providerName + ".msl");
                String fpmsl2 = Path.Combine(baseDir, "__" + providerName + ".msl.xml");
                {
                    tMsl = XDocument.Load(storeSchemaMapping);
                    tMsl.Save(fpmsl);
                    tMsl.Save(fpmsl2);
                }

                trace.TraceEvent(TraceEventType.Information, 101, "Checking ProviderManifest version.");
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
                    trace.TraceEvent(TraceEventType.Information, 101, "ProviderManifest v1");
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
                    trace.TraceEvent(TraceEventType.Information, 101, "ProviderManifest v3");
                }
                else {
                    trace.TraceEvent(TraceEventType.Error, 101, "ProviderManifest version unknown");
                    throw new ApplicationException("ProviderManifest version unknown");
                }

                trace.TraceEvent(TraceEventType.Information, 101, "Write temporary ProviderManifest csdl");
                String fpcsdl = Path.Combine(baseDir, "__" + providerName + ".csdl");
                XDocument tCsdl;
                {
                    tCsdl = XDocument.Load(xrCsdl);
                    tCsdl.Save(fpcsdl);
                }

                nut.providerManifest = providerManifest;
                nut.modelName = modelName;
                nut.targetSchema = targetSchema;

                var entityConnectionString = ""
                    + "Provider=" + providerName + ";"
                    + "Provider Connection String=\"" + db.ConnectionString + "\";"
                    + "Metadata=" + fpcsdl + "|" + fpssdl + "|" + fpmsl + ";"
                    ;

                XElement mssdl, mcsdl, mmsl;
                XDocument mEdmx = new XDocument(); // XDocument: ModelGen edmx
                mEdmx.Add(new XElement(xEDMX + "Edmx",
                    new XAttribute("Version", yver.ToString(2)),
                    new XElement(xEDMX + "Runtime",
                        mssdl = new XElement(xEDMX + "StorageModels"),
                        mcsdl = new XElement(xEDMX + "ConceptualModels"),
                        mmsl = new XElement(xEDMX + "Mappings")
                        )
                    )
                );

                trace.TraceEvent(TraceEventType.Information, 101, "Getting SchemaInformation");
                using (var Context = new SchemaInformation(entityConnectionString)) {
                    trace.TraceEvent(TraceEventType.Information, 101, "Ok");

                    String ssdlNs = nut.SsdlNs();
                    XElement ssdlSchema = new XElement(xSSDL + "Schema"
                        , new XAttribute("Namespace", ssdlNs)
                        , new XAttribute("Alias", "Self")
                        , new XAttribute("Provider", providerName)
                        , new XAttribute("ProviderManifestToken", providerManifestToken)
                        );
                    mssdl.Add(ssdlSchema);

                    XElement ssdlEntityContainer = new XElement(xSSDL + "EntityContainer"
                        , new XAttribute("Name", nut.SsdlContainer())
                        );
                    ssdlSchema.Add(ssdlEntityContainer);

                    String csdlNs = nut.CsdlNs();
                    XElement csdlSchema = new XElement(xCSDL + "Schema"
                        , new XAttribute("Namespace", csdlNs)
                        , new XAttribute("Alias", "Self")
                        );
                    mcsdl.Add(csdlSchema);
                    XElement csdlEntityContainer = new XElement(xCSDL + "EntityContainer"
                        , new XAttribute("Name", nut.CsdlContainer())
                        );
                    csdlSchema.Add(csdlEntityContainer);

                    XElement mslMapping = new XElement(xMSL + "Mapping"
                        , new XAttribute("Space", "C-S")
                        );
                    mmsl.Add(mslMapping);
                    XElement mslEntityContainerMapping = new XElement(xMSL + "EntityContainerMapping"
                        , new XAttribute("StorageEntityContainer", nut.SsdlContainer())
                        , new XAttribute("CdmEntityContainer", nut.CsdlContainer())
                        );
                    mslMapping.Add(mslEntityContainerMapping);

                    var vecTableOrView = Context.Tables.Cast<TableOrView>().Union(Context.Views);
                    foreach (var dbt in vecTableOrView) {
                        trace.TraceEvent(TraceEventType.Information, 101, "{2}: {0}.{1}", dbt.SchemaName, dbt.Name, (dbt is Table) ? "Table" : "View");

                        if (dbt.SchemaName != targetSchema) continue;

                        XElement ssdlEntitySet = new XElement(xSSDL + "EntitySet"
                            , new XAttribute("Name", nut.SsdlEntitySet(dbt))
                            , new XAttribute("EntityType", nut.SsdlEntityTypeRef(dbt))
                            , new XAttribute(xSTORE + "Type", "Tables")
                            , new XAttribute("Schema", dbt.SchemaName)
                            );
                        ssdlEntityContainer.Add(ssdlEntitySet);

                        XElement ssdlEntityType = new XElement(xSSDL + "EntityType"
                            , new XAttribute("Name", nut.SsdlEntityType(dbt))
                            );
                        ssdlSchema.Add(ssdlEntityType);

                        XElement csdlEntitySet = new XElement(xCSDL + "EntitySet"
                            , new XAttribute("Name", nut.CsdlEntitySet(dbt))
                            , new XAttribute("EntityType", nut.CsdlEntityTypeRef(dbt))
                            );
                        csdlEntityContainer.Add(csdlEntitySet);

                        XElement csdlEntityType = new XElement(xCSDL + "EntityType"
                            , new XAttribute("Name", nut.CsdlEntityType(dbt))
                            );
                        csdlSchema.Add(csdlEntityType);

                        XElement mslEntitySetMapping = new XElement(xMSL + "EntitySetMapping"
                            , new XAttribute("Name", nut.CsdlEntitySet(dbt))
                            );
                        mslEntityContainerMapping.Add(mslEntitySetMapping);

                        XElement mslEntityTypeMapping = new XElement(xMSL + "EntityTypeMapping"
                            , new XAttribute("TypeName", nut.CsdlEntityTypeRef(dbt))
                            );
                        mslEntitySetMapping.Add(mslEntityTypeMapping);

                        XElement mslMappingFragment = new XElement(xMSL + "MappingFragment"
                            , new XAttribute("StoreEntitySet", nut.SsdlEntitySet(dbt))
                            );
                        mslEntityTypeMapping.Add(mslMappingFragment);

                        XElement ssdlKey = null;
                        XElement csdlKey = null;
                        bool hasKey = false; // http://social.msdn.microsoft.com/Forums/en-US/94c227d3-3764-45b2-8c6b-e45b6cc8e169/keyless-object-workaround
                        bool hasId = dbt.Columns.Any(p => p.IsIdentity || p.Constraints.OfType<PrimaryKeyConstraint>().Any());
                        foreach (var dbc in dbt.Columns) {
                            trace.TraceEvent(TraceEventType.Information, 101, " TableColumn: {0}", dbc.Name);

                            bool isId = dbc.IsIdentity || dbc.Constraints.OfType<PrimaryKeyConstraint>().Any() || (hasId ? false : !dbc.IsNullable);

                            String ssdlName;
                            XElement ssdlProperty = new XElement(xSSDL + "Property"
                                , new XAttribute("Name", ssdlName = nut.SsdlProp(dbc))
                                , new XAttribute("Type", nut.SsdlPropType(dbc))
                                );
                            ssdlEntityType.Add(ssdlProperty);

                            bool deleteMe = nut.CsdlPropType(dbc) == null;

                            String csdlName;
                            XElement csdlProperty = new XElement(xCSDL + "Property"
                                , new XAttribute("Name", csdlName = nut.CsdlProp(dbc))
                                , new XAttribute("Type", nut.CsdlPropType(dbc) ?? "?")
                                );
                            csdlEntityType.Add(csdlProperty);

                            XElement mslScalarProperty = new XElement(xMSL + "ScalarProperty"
                                , new XAttribute("Name", csdlName)
                                , new XAttribute("ColumnName", ssdlName)
                                );
                            mslMappingFragment.Add(mslScalarProperty);

                            if (!dbc.IsNullable) {
                                ssdlProperty.SetAttributeValue("Nullable", "false");
                                csdlProperty.SetAttributeValue("Nullable", "false");
                            }
                            if (dbc.ColumnType.MaxLength.HasValue) {
                                ssdlProperty.SetAttributeValue("MaxLength", dbc.ColumnType.MaxLength.Value + "");
                                csdlProperty.SetAttributeValue("MaxLength", dbc.ColumnType.MaxLength.Value + "");
                            }
                            if (isId) {
                                hasKey = true;
                                if (ssdlKey == null) {
                                    ssdlKey = new XElement(xSSDL + "Key");
                                    ssdlEntityType.AddFirst(ssdlKey);
                                }
                                ssdlKey.Add(new XElement(xSSDL + "PropertyRef"
                                    , new XAttribute("Name", ssdlName)
                                    ));
                            }
                            if (isId) {
                                if (csdlKey == null) {
                                    csdlKey = new XElement(xCSDL + "Key");
                                    csdlEntityType.AddFirst(csdlKey);
                                }
                                csdlKey.Add(new XElement(xCSDL + "PropertyRef"
                                    , new XAttribute("Name", csdlName)
                                    ));
                                csdlProperty.SetAttributeValue(xAnno + "StoreGeneratedPattern", "Identity");
                            }

                            if (deleteMe) {
                                ssdlProperty.AddAfterSelf(new XComment(String.Format("Property {0} removed. Unknown type.", ssdlProperty.Attribute("Name"))));
                                csdlProperty.AddAfterSelf(new XComment(String.Format("Property {0} removed. Unknown type.", csdlProperty.Attribute("Name"))));
                                mslScalarProperty.AddAfterSelf(new XComment(String.Format("ScalarProperty {0} removed. Unknown type.", mslScalarProperty.Attribute("Name"))));

                                ssdlProperty.Remove();
                                csdlProperty.Remove();
                                mslScalarProperty.Remove();
                            }
                        }
                        if (!hasKey) {
                            ssdlEntitySet.AddAfterSelf(new XComment(String.Format("EntitySet {0} removed. No identical keys found.", ssdlEntitySet.Attribute("Name"))));
                            csdlEntitySet.AddAfterSelf(new XComment(String.Format("EntitySet {0} removed. No identical keys found.", csdlEntitySet.Attribute("Name"))));
                            mslEntitySetMapping.AddAfterSelf(new XComment(String.Format("EntitySetMapping {0} removed. No identical keys found.", mslEntityTypeMapping.Attribute("Name"))));

                            ssdlEntityType.AddAfterSelf(new XComment(String.Format("EntityType {0} removed. No identical keys found.", ssdlEntityType.Attribute("Name"))));
                            csdlEntityType.AddAfterSelf(new XComment(String.Format("EntityType {0} removed. No identical keys found.", csdlEntityType.Attribute("Name"))));
                            mslEntityTypeMapping.AddAfterSelf(new XComment(String.Format("EntityTypeMapping {0} removed. No identical keys found.", mslEntityTypeMapping.Attribute("Name"))));

                            ssdlEntitySet.Remove();
                            csdlEntitySet.Remove();
                            mslEntitySetMapping.Remove();

                            ssdlEntityType.Remove();
                            csdlEntityType.Remove();
                            mslEntityTypeMapping.Remove();
                        }
                    }

                    foreach (var dbco in Context.TableConstraints.OfType<ForeignKeyConstraint>()) {
                        trace.TraceEvent(TraceEventType.Information, 101, "Constraint: {0}", dbco.Name);

                        if (dbco.Parent.SchemaName != targetSchema) continue;

                        // ssdl
                        var ssdlAssociationSet = new XElement(xSSDL + "AssociationSet"
                            , new XAttribute("Name", nut.SsdlAssociationSet(dbco))
                            , new XAttribute("Association", nut.SsdlAssociationRef(dbco))
                            );
                        ssdlEntityContainer.Add(ssdlAssociationSet);

                        var ssdlAssociation = new XElement(xSSDL + "Association"
                            , new XAttribute("Name", nut.SsdlAssociation(dbco))
                            );
                        ssdlSchema.Add(ssdlAssociation);

                        var ssdlReferentialConstraint = new XElement(xSSDL + "ReferentialConstraint"
                            );

                        // csdl
                        var csdlAssociationSet = new XElement(xCSDL + "AssociationSet"
                            , new XAttribute("Name", nut.CsdlAssociationSet(dbco))
                            , new XAttribute("Association", nut.CsdlAssociationRef(dbco))
                            );
                        csdlEntityContainer.Add(csdlAssociationSet);

                        var csdlAssociation = new XElement(xCSDL + "Association"
                            , new XAttribute("Name", nut.CsdlAssociation(dbco))
                            );
                        csdlSchema.Add(csdlAssociation);

                        var csdlReferentialConstraint = new XElement(xCSDL + "ReferentialConstraint"
                            );

                        foreach (var dbfk in dbco.ForeignKeys) {
                            Addfkc(csdlSchema, dbfk, dbfk.ToColumn, dbfk.FromColumn, false, dbfk.FromColumn.IsNullable ? "0..1" : "1"
                                , ssdlAssociationSet, ssdlAssociation, ssdlReferentialConstraint, csdlAssociationSet, csdlAssociation, csdlReferentialConstraint);
                            Addfkc(csdlSchema, dbfk, dbfk.FromColumn, dbfk.ToColumn, true, "*"
                                , ssdlAssociationSet, ssdlAssociation, ssdlReferentialConstraint, csdlAssociationSet, csdlAssociation, csdlReferentialConstraint);
                        }

                        ssdlAssociation.Add(ssdlReferentialConstraint);
                        csdlAssociation.Add(csdlReferentialConstraint);
                    }

                    if (xSSDL != NS.SSDLv1)
                        foreach (var dbr in Context.Functions.Cast<Routine>().Union(Context.Procedures)) {
                            trace.TraceEvent(TraceEventType.Information, 101, "{2}: {0}.{1}", dbr.SchemaName, dbr.Name, (dbr is Function) ? "Function" : "Procedure");

                            if (dbr.SchemaName != targetSchema) continue;

                            // http://msdn.microsoft.com/ja-jp/library/bb738614(v=vs.90).aspx
                            var ssdlFunction = new XElement(xSSDL + "Function"
                                , new XAttribute("Name", dbr.Name)
                                , new XAttribute("StoreFunctionName", nut.SsdlFunction(dbr))
                                , new XAttribute("IsComposable", "false")
                                , new XAttribute("ParameterTypeSemantics", "AllowImplicitConversion")
                                , new XAttribute("Schema", nut.SsdlFunctionSchema(dbr))
                                );

                            // http://msdn.microsoft.com/ja-jp/library/vstudio/cc716710.aspx
                            var csdlFunctionImport = new XElement(xCSDL + "FunctionImport"
                                , new XAttribute("Name", nut.CsdlFunction(dbr))
                                );
                            csdlEntityContainer.Add(csdlFunctionImport);

                            // http://msdn.microsoft.com/ja-jp/library/vstudio/cc716759.aspx
                            var mslFunctionImportMapping = new XElement(xMSL + "FunctionImportMapping"
                                , new XAttribute("FunctionImportName", nut.CsdlFunction(dbr))
                                , new XAttribute("FunctionName", nut.SsdlFunctionRef(dbr))
                                );
                            mslEntityContainerMapping.Add(mslFunctionImportMapping);

                            var dbf = dbr as Function;
                            if (dbf != null) {
                                if (dbf.IsBuiltIn.HasValue) ssdlFunction.SetAttributeValue("BuiltIn", dbf.IsBuiltIn.Value ? "true" : "false");
                                if (dbf.IsNiladic.HasValue) ssdlFunction.SetAttributeValue("NiladicFunction", dbf.IsNiladic.Value ? "true" : "false");
                            }
                            var dbsf = dbr as ScalarFunction;
                            if (dbsf != null) {
                                if (dbsf.IsAggregate.HasValue) ssdlFunction.SetAttributeValue("Aggregate", dbsf.IsAggregate.Value ? "true" : "false");
                                //if (dbsf.ReturnType != null) ssdlFunction.SetAttributeValue("ReturnType", nut.SsdlPropType(dbsf.ReturnType));
                                if (dbsf.ReturnType != null) csdlFunctionImport.SetAttributeValue("ReturnType", nut.CsdlPropCollType(dbsf.ReturnType));
                            }
                            ssdlSchema.Add(ssdlFunction);

                            foreach (var dbfp in dbr.Parameters) {
                                trace.TraceEvent(TraceEventType.Information, 101, " Parameter: {0}", dbfp.Name);

                                // ssdl
                                var ssdlParameter = new XElement(xSSDL + "Parameter"
                                    , new XAttribute("Name", nut.SsdlParameterName(dbfp))
                                    , new XAttribute("Mode", nut.SsdlParameterMode(dbfp))
                                    , new XAttribute("Type", nut.SsdlPropType(dbfp.ParameterType))
                                    );
                                ssdlFunction.Add(ssdlParameter);

                                // csdl
                                var csdlParameter = new XElement(xCSDL + "Parameter"
                                    , new XAttribute("Name", nut.CsdlParameterName(dbfp))
                                    , new XAttribute("Type", nut.CsdlPropType(dbfp.ParameterType))
                                    );
                                csdlFunctionImport.Add(csdlParameter);

                                // msl

                            }
                        }

                    csdlSchema.Save(fpcsdl3);
                    mslMapping.Save(fpmsl3);
                    ssdlSchema.Save(fpssdl3);
                }

                mEdmx.Save(fpedmx3);
            }
        }

        private void Addfkc(XElement csdlSchema, ForeignKey dbfk, Column dbfkc, Column dbfkc2, bool isMulti, String multiplicity
            , XElement ssdlAssociationSet, XElement ssdlAssociation, XElement ssdlReferentialConstraint
            , XElement csdlAssociationSet, XElement csdlAssociation, XElement csdlReferentialConstraint
            ) {

            // ssdl
            var ssdlasEnd = new XElement(xSSDL + "End"
                , new XAttribute("Role", nut.SsdlEntitySet(dbfkc.Parent))
                , new XAttribute("EntitySet", nut.SsdlEntitySet(dbfkc.Parent))
                );
            ssdlAssociationSet.Add(ssdlasEnd);

            var ssdlaEnd = new XElement(xSSDL + "End"
                , new XAttribute("Role", nut.SsdlEntitySet(dbfkc.Parent))
                , new XAttribute("Type", nut.SsdlEntityTypeRef(dbfkc.Parent))
                , new XAttribute("Multiplicity", multiplicity)
                );
            ssdlAssociation.Add(ssdlaEnd);

            var ssdlarc = new XElement(xSSDL + (isMulti ? "Dependent" : "Principal")
                , new XAttribute("Role", nut.SsdlEntitySet(dbfkc.Parent))
                );
            ssdlReferentialConstraint.Add(ssdlarc);

            var ssdlPropertyRef = new XElement(xSSDL + "PropertyRef"
                , new XAttribute("Name", nut.SsdlProp(dbfkc))
                );
            ssdlarc.Add(ssdlPropertyRef);

            // csdl
            var csdlasEnd = new XElement(xCSDL + "End"
                , new XAttribute("Role", nut.CsdlEntitySet(dbfkc.Parent))
                , new XAttribute("EntitySet", nut.CsdlEntitySet(dbfkc.Parent))
                );
            csdlAssociationSet.Add(csdlasEnd);

            var csdlaEnd = new XElement(xCSDL + "End"
                , new XAttribute("Role", nut.CsdlEntitySet(dbfkc.Parent))
                , new XAttribute("Type", nut.CsdlEntityTypeRef(dbfkc.Parent))
                , new XAttribute("Multiplicity", multiplicity)
                );
            csdlAssociation.Add(csdlaEnd);

            var csdlarc = new XElement(xCSDL + (isMulti ? "Dependent" : "Principal")
                , new XAttribute("Role", nut.CsdlEntitySet(dbfkc.Parent))
                );
            csdlReferentialConstraint.Add(csdlarc);

            var csdlPropertyRef = new XElement(xCSDL + "PropertyRef"
                , new XAttribute("Name", nut.CsdlProp(dbfkc))
                );
            csdlarc.Add(csdlPropertyRef);

            var csdlNavigationProperty = new XElement(xCSDL + "NavigationProperty"
                , new XAttribute("Name", nut.CsdlEntityType(dbfkc.Parent))
                , new XAttribute("Relationship", nut.CsdlAssociationRef(dbfk.Constraint))
                , new XAttribute("FromRole", nut.CsdlEntityType(dbfkc2.Parent))
                , new XAttribute("ToRole", nut.CsdlEntityType(dbfkc.Parent))
                );
            var csdlEntityType = csdlSchema.Elements(xCSDL + "EntityType")
                .Where(p => p.Attribute("Name").Value == nut.CsdlEntityType(dbfkc2.Parent))
                .FirstOrDefault();
            if (csdlEntityType != null) {
                csdlEntityType.Add(csdlNavigationProperty);
            }
        }

        Nameut nut = new Nameut();

        class Nameut {
            public DbProviderManifest providerManifest { get; set; }
            public String modelName { get; set; }
            public String targetSchema { get; set; }

            public String SsdlNs() { return String.Format("{0}", targetSchema); }
            public String SsdlContainer() { return String.Format("{0}StoreContainer", modelName); }
            public String SsdlEntitySet(TableOrView dbt) { return dbt.Name; }
            public String SsdlEntityType(TableOrView dbt) { return String.Format("{0}", dbt.Name); }
            public String SsdlEntityTypeRef(TableOrView dbt) { return String.Format("{0}.{1}", SsdlNs(), SsdlEntityType(dbt)); }

            public String SsdlProp(Column dbc) { return dbc.Name; }

            public String SsdlPropType(Column dbc) { return SsdlPropType(dbc.ColumnType); }
            public String SsdlPropType(TypeSpecification ts) { return ts.TypeName; }

            public String CsdlNs() { return String.Format("{0}", modelName); }

            public String CsdlContainer() { return String.Format("{0}Entities", modelName); }
            public String CsdlEntitySet(TableOrView dbt) { return TSimpleIdentifier(dbt.Name); }
            public String CsdlEntityType(TableOrView dbt) { return TSimpleIdentifier(String.Format("{0}", dbt.Name)); }
            public String CsdlEntityTypeRef(TableOrView dbt) { return String.Format("{0}.{1}", CsdlNs(), CsdlEntityType(dbt)); }

            public String CsdlProp(Column dbc) { return TSimpleIdentifier(dbc.Name, "Column"); }

            public String TSimpleIdentifier(String s) { return TSimpleIdentifier(s, "Table"); }

            public String TSimpleIdentifier(String s, String defaultPrefix) {
                s = Regex.Replace(s, "[\\[\\]\\/\\-\\.\\\\：]+", "_").TrimStart('_');
                if (s.Length >= 1 && char.IsNumber(s[0])) s = defaultPrefix + s;
                return s;
            }

            public String CsdlPropType(Column dbc) {
                return CsdlPropType(dbc.ColumnType);
            }

            public String CsdlPropType(TypeSpecification ts) {
                foreach (var storeType in providerManifest.GetStoreTypes()) {
                    if (storeType.Name == ts.TypeName) {
                        if (storeType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType) {
                            return storeType.PrimitiveTypeKind.ToString();
                        }
                    }
                }
                // Unknown type
                return null;
            }

            public String SsdlAssociationSet(ForeignKeyConstraint dbco) {
                return String.Format("{0}", dbco.Name);
            }

            public String SsdlAssociationRef(ForeignKeyConstraint dbco) {
                return String.Format("{0}.{1}", SsdlNs(), SsdlAssociationSet(dbco));
            }

            public String SsdlAssociation(ForeignKeyConstraint dbco) {
                return String.Format("{0}", SsdlAssociationSet(dbco));
            }

            public String CsdlAssociationSet(ForeignKeyConstraint dbco) {
                return String.Format("{0}", dbco.Name);
            }

            public String CsdlAssociationRef(ForeignKeyConstraint dbco) {
                return String.Format("{0}.{1}", CsdlNs(), CsdlAssociationSet(dbco));
            }

            public String CsdlAssociation(ForeignKeyConstraint dbco) {
                return String.Format("{0}", CsdlAssociationSet(dbco));
            }

            public String SsdlFunction(Routine dbf) {
                return String.Format("{0}", TSimpleIdentifier(dbf.Name));
            }

            public String SsdlFunctionSchema(Routine dbf) {
                return String.Format("{0}", dbf.SchemaName);
            }

            public String CsdlFunction(Routine dbf) {
                return String.Format("{0}", dbf.Name);
            }

            public String SsdlFunctionRef(Routine dbf) {
                return String.Format("{0}.{1}", SsdlNs(), dbf.Name);
            }

            public String SsdlParameterName(Parameter dbfp) {
                return String.Format("{0}", dbfp.Name);
            }

            public String CsdlParameterName(Parameter dbfp) {
                return String.Format("{0}", dbfp.Name);
            }

            public String SsdlParameterMode(Parameter dbfp) {
                if (false) { }
                else if (dbfp.Mode == "IN") return "In";
                else if (dbfp.Mode == "OUT") return "Out";
                else if (dbfp.Mode == "INOUT") return "InOut";
                return "";
            }

            public String CsdlPropCollType(TypeSpecification ts) {
                return String.Format("Collection({0})", CsdlPropType(ts));
            }
        }
    }
}
