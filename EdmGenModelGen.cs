﻿#if ENTITIES6
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
#else
using System.Data.Metadata.Edm;
#endif

using EdmGen06.Properties;
using Store;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace EdmGen06 {
    public class EdmGenModelGen : EdmGenBase {
        public void ModelGen(String connectionString, String providerName, String modelName, String targetSchema, Version yver) {
            ModelGen2(connectionString, providerName, null, modelName, targetSchema, yver);
        }

        public void ModelGen2(String connectionString, String providerName, String typeProviderServices, String modelName, String targetSchema, Version yver) {
            String baseDir = Environment.CurrentDirectory;

            String fpssdl3 = Path.Combine(baseDir, modelName + ".ssdl");
            String fpcsdl3 = Path.Combine(baseDir, modelName + ".csdl");
            String fpmsl3 = Path.Combine(baseDir, modelName + ".msl");
            String fpedmx3 = Path.Combine(baseDir, modelName + ".edmx");
            String fpconfig = Path.Combine(baseDir, modelName + ".App.config");

            if (false) { }
            else if (yver == new Version(1, 0)) { xEDMX = "{" + NS.EDMXv1 + "}"; xSSDL = "{" + NS.SSDLv1 + "}"; xCSDL = "{" + NS.CSDLv1 + "}"; xMSL = "{" + NS.MSLv1 + "}"; trace.TraceEvent(TraceEventType.Information, 101, "ModelGen v1"); }
            else if (yver == new Version(3, 0)) { xEDMX = "{" + NS.EDMXv3 + "}"; xSSDL = "{" + NS.SSDLv3 + "}"; xCSDL = "{" + NS.CSDLv3 + "}"; xMSL = "{" + NS.MSLv3 + "}"; trace.TraceEvent(TraceEventType.Information, 101, "ModelGen v3"); }
            else throw new NotSupportedException(String.Format("Version '{0}', from 1.0 3.0 ", yver));

            trace.TraceEvent(TraceEventType.Information, 101, "Getting {1} from '{0}'", providerName, typeof(DbProviderFactory).FullName);
            var fac = System.Data.Common.DbProviderFactories.GetFactory(providerName);
            if (fac == null) throw new ApplicationException();
            trace.TraceEvent(TraceEventType.Information, 101, fac.GetType().AssemblyQualifiedName);
            trace.TraceEvent(TraceEventType.Information, 101, fac.GetType().Assembly.CodeBase);
            trace.TraceEvent(TraceEventType.Information, 101, "Ok");

            Type tyEFv6 = null;

            using (var db = fac.CreateConnection()) {
                trace.TraceEvent(TraceEventType.Information, 101, "Connecting");
                db.ConnectionString = connectionString;
                db.Open();
                trace.TraceEvent(TraceEventType.Information, 101, "Connected");

                trace.TraceEvent(TraceEventType.Information, 101, "Getting {1} from '{0}'", providerName, typeof(DbProviderServices).FullName);
                DbProviderServices providerServices = null;
                if (providerServices == null) {
                    if (typeProviderServices != null) {
                        var ty = Type.GetType(typeProviderServices);
                        if (ty != null) providerServices = (DbProviderServices)ty.InvokeMember("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, null, null, new object[0]);
                        if (providerServices != null) trace.TraceEvent(TraceEventType.Information, 101, " from Instance property");
                    }
                }
#if !ENTITIES6
                if (providerServices == null) {
                    providerServices = ((IServiceProvider)fac).GetService(typeof(DbProviderServices)) as DbProviderServices;
                    if (providerServices != null) trace.TraceEvent(TraceEventType.Information, 101, " from IServiceProvider.GetService method");
                }
#endif
                if (providerServices == null) {
                    providerServices = DbProviderServices.GetProviderServices(db);
                    if (providerServices != null) trace.TraceEvent(TraceEventType.Information, 101, " from DbProviderServices.GetProviderServices method");
                }
                trace.TraceEvent(TraceEventType.Information, 101, providerServices.GetType().AssemblyQualifiedName);
                trace.TraceEvent(TraceEventType.Information, 101, providerServices.GetType().Assembly.CodeBase);
                trace.TraceEvent(TraceEventType.Information, 101, "Ok");

                tyEFv6 = providerServices.GetType();

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
                mEdmx.Add(
                    new XElement(xEDMX + "Edmx",
                        new XAttribute("Version", yver.ToString(2)),
                        new XElement(xEDMX + "Runtime",
                            mssdl = new XElement(xEDMX + "StorageModels"),
                            mcsdl = new XElement(xEDMX + "ConceptualModels"),
                            mmsl = new XElement(xEDMX + "Mappings")
                            ),
                        new XElement(xEDMX + "Designer",
                            new XElement(xEDMX + "Connection"
                                ),
                            new XElement(xEDMX + "Options",
                                new XElement(xEDMX + "DesignerInfoPropertySet",
                                    new XElement(xEDMX + "DesignerProperty",
                                        new XAttribute("Name", "EnablePluralization"),
                                        new XAttribute("Value", "False")
                                        ),
                                    new XElement(xEDMX + "DesignerProperty",
                                        new XAttribute("Name", "IncludeForeignKeysInModel"),
                                        new XAttribute("Value", "True")
                                        ),
                                    new XElement(xEDMX + "DesignerProperty",
                                        new XAttribute("Name", "UseLegacyProvider"),
                                        new XAttribute("Value", "True")
                                        )
                                    )
                                ),
                            new XElement(xEDMX + "Diagrams"
                                )
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

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all Tables");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.Tables.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all TableForeignKeys");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.TableForeignKeys.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all TableColumns");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.TableColumns.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all TableConstraints");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.TableConstraints.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all Views");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.Views.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all ViewForeignKeys");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.ViewForeignKeys.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all ViewColumns");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.ViewColumns.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all ViewConstraints");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.ViewConstraints.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all Functions");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.Functions.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all FunctionParameters");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.FunctionParameters.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all Procedures");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.Procedures.ToArray().Length));

                    trace.TraceEvent(TraceEventType.Information, 101, "Loading all ProcedureParameters");
                    trace.TraceEvent(TraceEventType.Verbose, 101, String.Format("{0:#,##0} items", Context.ProcedureParameters.ToArray().Length));

                    var allTableOrView = Context.Tables.Cast<TableOrView>().Union(Context.Views.Cast<TableOrView>()).ToArray();

                    nut.localTypes = allTableOrView.Select(p => p.Name).ToArray();

                    List<TableOrView> processTableOrView = allTableOrView.Where(dbt => dbt.SchemaName == targetSchema).ToList();

                    while (true) {
                        bool more = false;
                        foreach (var dbco in Context.TableConstraints.OfType<ForeignKeyConstraint>()) {
                            foreach (var dbfk in dbco.ForeignKeys) {

                                {
                                    var dbt = dbfk.FromColumn.Parent;
                                    if (!processTableOrView.Contains(dbt) && allTableOrView.Contains(dbt)) { processTableOrView.Add(dbt); more |= true; }
                                }

                                {
                                    var dbt = dbfk.ToColumn.Parent;
                                    if (!processTableOrView.Contains(dbt) && allTableOrView.Contains(dbt)) { processTableOrView.Add(dbt); more |= true; }
                                }
                            }
                        }
                        if (!more) break;
                    }

                    foreach (var dbt in processTableOrView) {
                        trace.TraceEvent(TraceEventType.Information, 101, "{2}: {0}.{1}", dbt.SchemaName, dbt.Name, (dbt is Table) ? "Table" : "View");

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

                            bool isIdGen = dbc.IsIdentity;
                            bool isId = isIdGen || dbc.Constraints.OfType<PrimaryKeyConstraint>().Any() || (hasId ? false : !dbc.IsNullable);

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
                                int maxLen = dbc.ColumnType.MaxLength.Value;
                                if (maxLen != -1) {
                                    ssdlProperty.SetAttributeValue("MaxLength", dbc.ColumnType.MaxLength.Value + "");
                                    csdlProperty.SetAttributeValue("MaxLength", dbc.ColumnType.MaxLength.Value + "");
                                }
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
                                if (isIdGen) ssdlProperty.SetAttributeValue("StoreGeneratedPattern", "Identity");
                                if (isIdGen) ssdlProperty.SetAttributeValue(xAnno + "StoreGeneratedPattern", "Identity");
                            }
                            if (isId) {
                                if (csdlKey == null) {
                                    csdlKey = new XElement(xCSDL + "Key");
                                    csdlEntityType.AddFirst(csdlKey);
                                }
                                csdlKey.Add(new XElement(xCSDL + "PropertyRef"
                                    , new XAttribute("Name", csdlName)
                                    ));
                                if (isIdGen) csdlProperty.SetAttributeValue(xAnno + "StoreGeneratedPattern", "Identity");
                            }

                            if (deleteMe) {
                                ssdlProperty.AddAfterSelf(new XComment(String.Format("Property {0} removed. Unknown type {1}.", ssdlProperty.Attribute("Name"), dbc.ColumnType.TypeName)));
                                csdlProperty.AddAfterSelf(new XComment(String.Format("Property {0} removed. Unknown type {1}.", csdlProperty.Attribute("Name"), dbc.ColumnType.TypeName)));
                                mslScalarProperty.AddAfterSelf(new XComment(String.Format("ScalarProperty {0} removed. Unknown type {1}.", mslScalarProperty.Attribute("Name"), dbc.ColumnType.TypeName)));

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
                            int pkcFrom = 0;
                            foreach (var dbpk in dbfk.FromColumn.Constraints.OfType<PrimaryKeyConstraint>()) {
                                pkcFrom = dbpk.Columns.Count;
                                break;
                            }
                            int pkcTo = 0;
                            foreach (var dbpk in dbfk.ToColumn.Constraints.OfType<PrimaryKeyConstraint>()) {
                                pkcTo = dbpk.Columns.Count;
                                break;
                            }
                            bool same = (pkcFrom == dbco.ForeignKeys.Count && dbco.ForeignKeys.Count == pkcTo);

                            Addfkc(csdlSchema, dbfk, dbfk.ToColumn, dbfk.FromColumn, false, dbfk.FromColumn.IsNullable ? "0..1" : "1"
                                , ssdlAssociationSet, ssdlAssociation, ssdlReferentialConstraint, csdlAssociationSet, csdlAssociation, csdlReferentialConstraint);
                            Addfkc(csdlSchema, dbfk, dbfk.FromColumn, dbfk.ToColumn, true, same ? "0..1" : "*"
                                , ssdlAssociationSet, ssdlAssociation, ssdlReferentialConstraint, csdlAssociationSet, csdlAssociation, csdlReferentialConstraint);
                        }

                        ssdlAssociation.Add(ssdlReferentialConstraint);
                        csdlAssociation.Add(csdlReferentialConstraint);
                    }

                    if (xSSDL != NS.SSDLv1)
                        foreach (var dbr in Context.Functions.Cast<Routine>().Union(Context.Procedures.Cast<Routine>())) {
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
                            bool deleteMe = false;
                            var dbsf = dbr as ScalarFunction;
                            if (dbsf != null) {
                                if (dbsf.IsAggregate.HasValue) ssdlFunction.SetAttributeValue("Aggregate", dbsf.IsAggregate.Value ? "true" : "false");
                                //if (dbsf.ReturnType != null) ssdlFunction.SetAttributeValue("ReturnType", nut.SsdlPropType(dbsf.ReturnType));
                                if (dbsf.ReturnType != null) {
                                    String ty = nut.CsdlPropCollType(dbsf.ReturnType);
                                    if (ty != null) {
                                        csdlFunctionImport.SetAttributeValue("ReturnType", ty);
                                    }
                                    else {
                                        deleteMe = true;
                                    }
                                }
                            }
                            ssdlSchema.Add(ssdlFunction);

                            if (deleteMe) {
                                ssdlFunction.AddAfterSelf(new XComment(String.Format("Function {0} removed. Unknown ReturnType {1}"
                                    , ssdlFunction.Attribute("Name")
                                    , (dbsf != null && dbsf.ReturnType != null) ? dbsf.ReturnType.TypeName : "")
                                    ));

                                ssdlFunction.Remove();
                                csdlFunctionImport.Remove();
                                mslFunctionImportMapping.Remove();
                                continue;
                            }

                            foreach (var dbfp in dbr.Parameters) {
                                trace.TraceEvent(TraceEventType.Information, 101, " Parameter: {0}", dbfp.Name);

                                // ssdl

                                if (nut.SsdlPropType(dbfp.ParameterType) == null) {
                                    ssdlFunction.AddAfterSelf(new XComment(String.Format("Function {0} removed. Unknown ParameterType {1}"
                                        , ssdlFunction.Attribute("Name")
                                        , dbfp.ParameterType.TypeName)
                                        ));

                                    ssdlFunction.Remove();
                                    csdlFunctionImport.Remove();
                                    mslFunctionImportMapping.Remove();
                                    break;
                                }

                                if (String.IsNullOrEmpty(nut.SsdlParameterMode(dbfp))) {
                                    ssdlFunction.AddAfterSelf(new XComment(String.Format("Function {0} removed. Unknown SsdlParameterMode {1}"
                                        , ssdlFunction.Attribute("Name")
                                        , nut.SsdlParameterMode(dbfp))
                                        ));

                                    ssdlFunction.Remove();
                                    csdlFunctionImport.Remove();
                                    mslFunctionImportMapping.Remove();
                                    break;
                                }

                                var ssdlParameter = new XElement(xSSDL + "Parameter"
                                    , new XAttribute("Name", nut.SsdlParameterName(dbfp))
                                    , new XAttribute("Mode", nut.SsdlParameterMode(dbfp))
                                    , new XAttribute("Type", nut.SsdlPropType(dbfp.ParameterType))
                                    );
                                ssdlFunction.Add(ssdlParameter);

                                // csdl

                                if (nut.CsdlPropType(dbfp.ParameterType) == null) {
                                    ssdlFunction.AddAfterSelf(new XComment(String.Format("Function {0} removed. Unknown ParameterType {1}"
                                        , ssdlFunction.Attribute("Name")
                                        , dbfp.ParameterType.TypeName)
                                        ));

                                    ssdlFunction.Remove();
                                    csdlFunctionImport.Remove();
                                    mslFunctionImportMapping.Remove();
                                    break;
                                }

                                if (String.IsNullOrEmpty(nut.CsdlParameterMode(dbfp))) {
                                    ssdlFunction.AddAfterSelf(new XComment(String.Format("Function {0} removed. Unknown ParameterMode {1}"
                                        , ssdlFunction.Attribute("Name")
                                        , nut.CsdlParameterMode(dbfp))
                                        ));

                                    ssdlFunction.Remove();
                                    csdlFunctionImport.Remove();
                                    mslFunctionImportMapping.Remove();
                                    break;
                                }

                                var csdlParameter = new XElement(xCSDL + "Parameter"
                                    , new XAttribute("Name", nut.CsdlParameterName(dbfp))
                                    , new XAttribute("Mode", nut.CsdlParameterMode(dbfp))
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

                {
                    XDocument xAppconfig = new XDocument(
                        new XElement("configuration"
                            , new XComment("configSections has to be FIRST element!")
                            , new XElement("configSections"
                                , new XComment("for EF6.0.x ")
                                , new XComment("you don't need this. your nuget will setup automatically")
                                , new XElement("section"
                                    , new XAttribute("name", "entityFramework")
                                    , new XAttribute("type", "System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
                                    , new XAttribute("requirePermission", "false")
                                    )
                                )
                            , new XElement("system.data"
                                , new XElement("DbProviderFactories"
                                , new XComment("for EF4.x and EF6.0.x ")
                                , new XComment("you may need this. if you don't modify machine.config")
                                    , new XElement("remove"
                                        , new XAttribute("invariant", providerName)
                                        )
                                    , new XElement("add"
                                        , new XAttribute("name", fac.GetType().Assembly.GetCustomAttributes(false)
                                            .Where(p => p is AssemblyTitleAttribute)
                                            .Select(p => "" + ((AssemblyTitleAttribute)p).Title)
                                            .FirstOrDefault()
                                            )
                                        , new XAttribute("invariant", providerName)
                                        , new XAttribute("description", fac.GetType().Assembly.GetCustomAttributes(false)
                                            .Where(p => p is AssemblyDescriptionAttribute)
                                            .Select(p => "" + ((AssemblyDescriptionAttribute)p).Description)
                                            .FirstOrDefault()
                                            )
                                        , new XAttribute("type", fac.GetType().AssemblyQualifiedName)
                                        )
                                    )
                                )
                            , new XElement("connectionStrings"
                                , new XComment("for EF4.x and EF6.0.x ")
                                , new XElement("add"
                                    , new XAttribute("name", String.Format("{0}Entities", modelName))
                                    , new XAttribute("connectionString", String.Format("metadata={0}.csdl|{0}.ssdl|{0}.msl;provider={1};provider connection string=\"{2}\""
                                        , modelName
                                        , providerName
                                        , connectionString // UUt.UrlEncode("\"" + connectionString + "\"")
                                        ))
                                    , new XAttribute("providerName", "System.Data.EntityClient")
                                    )
                                )
                            , new XElement("entityFramework"
                                , new XElement("providers"
                                    , new XComment("for EF6.0.x ")
                                    , new XComment("you need this. add it manually")
                                    , new XElement("provider"
                                        , new XAttribute("invariantName", providerName)
                                        , new XAttribute("type", tyEFv6.AssemblyQualifiedName)
                                        )
                                    )
                                )
                            )
                        );
                    xAppconfig.Save(fpconfig);
                }

                mEdmx.Save(fpedmx3);
            }
        }

        XElement Once(XElement parent, XElement newc) {
            foreach (XElement curc in parent.Elements()) {
                if (Strify(curc).Equals(Strify(newc)))
                    return curc;
            }

            parent.Add(newc);
            return newc;
        }

        String Strify(XElement e1) {
            StringWriter wr = new StringWriter();
            wr.WriteLine(e1.Name);
            foreach (XAttribute att in e1.Attributes().OrderBy(p => "" + p.Name)) {
                wr.WriteLine(att.Name);
                wr.WriteLine(att.Value);
            }
            return wr.ToString();
        }

        private void Addfkc(XElement csdlSchema, ForeignKey dbfk, Column dbfkc, Column dbfkc2, bool isMulti, String multiplicity
            , XElement ssdlAssociationSet, XElement ssdlAssociation, XElement ssdlReferentialConstraint
            , XElement csdlAssociationSet, XElement csdlAssociation, XElement csdlReferentialConstraint
        ) {
            trace.TraceEvent(TraceEventType.Verbose, 101, "Addfkc: ");
            trace.TraceEvent(TraceEventType.Verbose, 101, "  xSSDL -> {0}", xSSDL);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  nut -> {0}", nut);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  dbfk -> {0}", dbfk);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  dbfkc              -> {0}", dbfkc);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  dbfkc.Parent       -> {0}", dbfkc.Parent);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  dbfkc.Parent.Id    -> {0}", dbfkc.Parent.Id);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  dbfkc.Parent.Name  -> {0}", dbfkc.Parent.Name);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  dbfkc2             -> {0}", dbfkc2);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  dbfkc2.Parent      -> {0}", dbfkc2.Parent);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  dbfkc2.Parent.Id   -> {0}", dbfkc2.Parent.Id);
            trace.TraceEvent(TraceEventType.Verbose, 101, "  dbfkc2.Parent.Name -> {0}", dbfkc2.Parent.Name);

            // ssdl
            var ssdlasEnd = new XElement(xSSDL + "End"
                , new XAttribute("Role", nut.SsdlEntitySet(dbfkc.Parent))
                , new XAttribute("EntitySet", nut.SsdlEntitySet(dbfkc.Parent))
                );
            ssdlasEnd = Once(ssdlAssociationSet, ssdlasEnd);

            var ssdlaEnd = new XElement(xSSDL + "End"
                , new XAttribute("Role", nut.SsdlEntitySet(dbfkc.Parent))
                , new XAttribute("Type", nut.SsdlEntityTypeRef(dbfkc.Parent))
                , new XAttribute("Multiplicity", multiplicity)
                );
            ssdlaEnd = Once(ssdlAssociation, ssdlaEnd);

            var ssdlarc = new XElement(xSSDL + (isMulti ? "Dependent" : "Principal")
                , new XAttribute("Role", nut.SsdlEntitySet(dbfkc.Parent))
                );
            ssdlarc = Once(ssdlReferentialConstraint, ssdlarc);

            var ssdlPropertyRef = new XElement(xSSDL + "PropertyRef"
                , new XAttribute("Name", nut.SsdlProp(dbfkc))
                );
            ssdlPropertyRef = Once(ssdlarc, ssdlPropertyRef);

            // csdl
            var csdlasEnd = new XElement(xCSDL + "End"
                , new XAttribute("Role", nut.CsdlEntitySet(dbfkc.Parent))
                , new XAttribute("EntitySet", nut.CsdlEntitySet(dbfkc.Parent))
                );
            csdlasEnd = Once(csdlAssociationSet, csdlasEnd);

            var csdlaEnd = new XElement(xCSDL + "End"
                , new XAttribute("Role", nut.CsdlEntitySet(dbfkc.Parent))
                , new XAttribute("Type", nut.CsdlEntityTypeRef(dbfkc.Parent))
                , new XAttribute("Multiplicity", multiplicity)
                );
            csdlaEnd = Once(csdlAssociation, csdlaEnd);

            var csdlarc = new XElement(xCSDL + (isMulti ? "Dependent" : "Principal")
                , new XAttribute("Role", nut.CsdlEntitySet(dbfkc.Parent))
                );
            csdlarc = Once(csdlReferentialConstraint, csdlarc);

            var csdlPropertyRef = new XElement(xCSDL + "PropertyRef"
                , new XAttribute("Name", nut.CsdlProp(dbfkc))
                );
            csdlPropertyRef = Once(csdlarc, csdlPropertyRef);

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
                csdlNavigationProperty = Once(csdlEntityType, csdlNavigationProperty);
            }
        }

        Nameut nut = new Nameut();

        class Nameut {
            public DbProviderManifest providerManifest { get; set; }
            public String modelName { get; set; }
            public String targetSchema { get; set; }
            public String[] localTypes = new String[0];

            public String SsdlNs() { return String.Format("{0}", targetSchema); }
            public String SsdlContainer() { return String.Format("{0}StoreContainer", modelName); }
            public String SsdlEntitySet(TableOrView dbt) { return String.Format("{1}", dbt.SchemaName, dbt.Name); }
            public String SsdlEntityType(TableOrView dbt) { return String.Format("{0}_{1}", dbt.SchemaName, dbt.Name); }
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
                if (Array.IndexOf<String>(localTypes, ts.TypeName) >= 0)
                    return ts.TypeName;
                // Unknown type
                return null;
            }

            List<ForeignKeyConstraint> alco = new List<ForeignKeyConstraint>();

            public String SsdlAssociationSet(ForeignKeyConstraint dbco) {
                int p = alco.IndexOf(dbco);
                if (p < 0) {
                    p = alco.Count;
                    alco.Add(dbco);
                }
                return String.Format("{0}_{1}", TSimpleIdentifier(dbco.Name,"Association"), 1 + p);
            }

            public String SsdlAssociationRef(ForeignKeyConstraint dbco) {
                return String.Format("{0}.{1}", SsdlNs(), SsdlAssociationSet(dbco));
            }

            public String SsdlAssociation(ForeignKeyConstraint dbco) {
                return String.Format("{0}", SsdlAssociationSet(dbco));
            }

            public String CsdlAssociationSet(ForeignKeyConstraint dbco) {
                int p = alco.IndexOf(dbco);
                if (p < 0) {
                    p = alco.Count;
                    alco.Add(dbco);
                }
                return String.Format("{0}_{1}", TSimpleIdentifier(dbco.Name, "Association"), 1 + p);
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

            SortedDictionary<String, bool> dictSsdlParameterName = new SortedDictionary<string, bool>();

            public String SsdlParameterName(Parameter dbfp) {
                for (int x = 0; ; x++) {
                    String a = String.Format("{0}{1}", dbfp.Name, (x == 0) ? "" : "" + x);
                    String k = dbfp.Routine.CatalogName + ":" + dbfp.Routine.Id + ":" + a;
                    if (dictSsdlParameterName.ContainsKey(k)) continue;

                    dictSsdlParameterName[k] = true;
                    return a;
                }
            }

            SortedDictionary<String, bool> dictCsdlParameterName = new SortedDictionary<string, bool>();

            public String CsdlParameterName(Parameter dbfp) {
                for (int x = 0; ; x++) {
                    String a = String.Format("{0}{1}", dbfp.Name, (x == 0) ? "" : "" + x);
                    String k = dbfp.Routine.CatalogName + ":" + dbfp.Routine.Id + ":" + a;
                    if (dictCsdlParameterName.ContainsKey(k)) continue;

                    dictCsdlParameterName[k] = true;
                    return a;
                }
            }

            public String SsdlParameterMode(Parameter dbfp) {
                if (false) { }
                else if (dbfp.Mode == "IN") return "In";
                else if (dbfp.Mode == "OUT") return "Out";
                else if (dbfp.Mode == "INOUT") return "InOut";
                return "";
            }

            public String CsdlPropCollType(TypeSpecification ts) {
                String ty = CsdlPropType(ts);
                if (ty == null) return null;
                return String.Format("Collection({0})", ty);
            }

            public String CsdlParameterMode(Parameter dbfp) {
                if (false) { }
                else if (dbfp.Mode == "IN") return "In";
                else if (dbfp.Mode == "OUT") return "Out";
                else if (dbfp.Mode == "INOUT") return "InOut";
                return "";
            }
        }

        class UUt {
            public static string UrlEncode(string p) {
                return (p ?? String.Empty)
                    .Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("\"", "&quot;")
                    ;
            }
        }
    }
}
