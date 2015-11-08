#if ENTITIES6
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.EntityClient;
#else
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
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
using System.Text;
using System.Collections.Generic;
using System.Collections;

// Multiplicity <0..1> <1> <*>  https://msdn.microsoft.com/en-us/library/ff952928(v=sql.105).aspx

namespace EdmGen06 {
    public class EdmGenClassGen : EdmGenBase {
        public EdmGenClassGen() {

        }

        public void CodeFirstGen(String fpEdmx, String fpcs, String generator) {
            XDocument edmx = XDocument.Load(fpEdmx);
            var EDMXv3 = XNamespace.Get(NS.EDMXv3);
            var CSDLv3 = XNamespace.Get(NS.CSDLv3);
            XNamespace nsCSDL = CSDLv3;
            XElement Schema = edmx.Element(EDMXv3 + "Edmx")
                .Element(EDMXv3 + "Runtime")
                .Element(EDMXv3 + "ConceptualModels")
                .Element(CSDLv3 + "Schema");
            String template = null;
            if (false) { }
            else if ("DbContext.EFv6".Equals(generator)) template = Resources.DbContext_EFv6;
            else if ("DbContext.EFv5".Equals(generator)) template = Resources.DbContext_EFv5;
            else if ("ObjectContext".Equals(generator)) template = Resources.ObjectContext;
            else {
                trace.TraceEvent(TraceEventType.Error, 101, "generator \"" + generator + "\" is unknown");
                throw new ApplicationException("generator \"" + generator + "\" is unknown");
            }
            if (true) {
                SortedDictionary<string, Assoc> dAssoc = new SortedDictionary<string, Assoc>();
                List<Assoc> alAssoc = new List<Assoc>();
                var Alias = Schema.Attribute("Alias").Value;
                foreach (var Association in Schema.Elements(nsCSDL + "Association")) {
                    var Name = Association.Attribute("Name").Value;
                    var assoc12 = new Assoc(Association, nsCSDL, true);
                    var assoc21 = new Assoc(Association, nsCSDL, false);
                    dAssoc[String.Format("{0}.{1}>{2}", Alias, Name, assoc12.Role1)] = assoc12;
                    dAssoc[String.Format("{0}.{1}>{2}", Alias, Name, assoc21.Role1)] = assoc21;
                    alAssoc.Add(assoc12);
                }
                var nsAnno = XNamespace.Get("http://schemas.microsoft.com/ado/2009/02/edm/annotation");
                var Model = new {
                    Namespace = Schema.Attribute("Namespace").Value,
                    EntityContainer = Schema.Elements(nsCSDL + "EntityContainer").Select(
                        vEntityContainer => new {
                            Name = vEntityContainer.Attribute("Name").Value,
                            EntitySet = vEntityContainer.Elements(nsCSDL + "EntitySet").Select(
                                vEntitySet => new {
                                    Name = vEntitySet.Attribute("Name").Value,
                                    EntityType = vEntitySet.Attribute("EntityType").Value.Split('.')[1],
                                }
                            ),
                        }
                    ),
                    EntityType = Schema.Elements(nsCSDL + "EntityType").Select(
                        vEntityType => new {
                            Name = vEntityType.Attribute("Name").Value,
                            Property = vEntityType.Elements(nsCSDL + "Property").Select(
                                vProperty => new {
                                    Name = vProperty.Attribute("Name").Value,
                                    Type = vProperty.Attribute("Type").Value,
                                    TypeSigned = CheckTypeSigned(vProperty.Attribute("Type").Value, CheckNullable(vProperty.Attribute("Nullable"))),
                                    Nullable = CheckNullable(vProperty.Attribute("Nullable")),
                                    Identity = CheckIdentity(vProperty.Attribute(nsAnno + "StoreGeneratedPattern")),
                                }
                            ),
                            NavigationProperty = vEntityType.Elements(nsCSDL + "NavigationProperty").Select(
                                vNavigationProperty => new {
                                    EntityTypeName = vEntityType.Attribute("Name").Value,
                                    Name = vNavigationProperty.Attribute("Name").Value,
                                    FromRole = FindNavigationProperty(dAssoc, vNavigationProperty, true),
                                    ToRole = FindNavigationProperty(dAssoc, vNavigationProperty, false),
                                }
                            )
                        }
                    ),
                    Association = alAssoc,
                };
                File.WriteAllText(fpcs, new UtFakeSSVE(template, Model).Generated);
            }
        }

        Assoc FindNavigationProperty(SortedDictionary<string, Assoc> dAssoc, XElement vNavigationProperty, bool fromRole) {
            return dAssoc[String.Format("{0}>{1}", vNavigationProperty.Attribute("Relationship").Value, vNavigationProperty.Attribute(fromRole ? "FromRole" : "ToRole").Value)];
        }

        class Assoc {
            XElement Association;
            XNamespace nsCSDL;
            bool asc;

            public Assoc(XElement Association, XNamespace nsCSDL, bool asc) {
                this.Association = Association;
                this.nsCSDL = nsCSDL;
                this.asc = asc;
            }

            public bool Many { get { return Multiplicity == "*"; } }
            public bool One { get { return Multiplicity == "1"; } }
            public bool ZeroOrOne { get { return Multiplicity == "0..1"; } }

            public String Name { get { return Association.Attribute("Name").Value; } }

            public String Type { get { return Association.Elements(nsCSDL + "End").Skip(asc ? 0 : 1).First().Attribute("Type").Value.Split('.')[1]; } }

            public String Type1 { get { return Association.Elements(nsCSDL + "End").Skip(asc ? 0 : 1).First().Attribute("Type").Value.Split('.')[1]; } }
            public String Type2 { get { return Association.Elements(nsCSDL + "End").Skip(asc ? 1 : 0).First().Attribute("Type").Value.Split('.')[1]; } }

            public String Multiplicity { get { return Association.Elements(nsCSDL + "End").Skip(asc ? 0 : 1).First().Attribute("Multiplicity").Value; } }

            public String Multiplicity1 { get { return Association.Elements(nsCSDL + "End").Skip(asc ? 0 : 1).First().Attribute("Multiplicity").Value; } }
            public String Multiplicity2 { get { return Association.Elements(nsCSDL + "End").Skip(asc ? 1 : 0).First().Attribute("Multiplicity").Value; } }

            public String RelationshipMultiplicity1 {
                get {
                    var s = Multiplicity1;
                    switch (s) {
                        case "*": return "Many";
                        case "1": return "One";
                        case "0..1": return "ZeroOrOne";
                    }
                    throw new ApplicationException("Unknown RelationshipMultiplicity \"" + s + "\"");
                }
            }

            public String RelationshipMultiplicity2 {
                get {
                    var s = Multiplicity2;
                    switch (s) {
                        case "*": return "Many";
                        case "1": return "One";
                        case "0..1": return "ZeroOrOne";
                    }
                    throw new ApplicationException("Unknown RelationshipMultiplicity \"" + s + "\"");
                }
            }

            public String Role1 { get { return Association.Elements(nsCSDL + "End").Skip(asc ? 0 : 1).First().Attribute("Role").Value; } }
            public String Role2 { get { return Association.Elements(nsCSDL + "End").Skip(asc ? 1 : 0).First().Attribute("Role").Value; } }
        }

        String CheckTypeSigned(String edmType, bool nullable) {
            if (nullable && "/Boolean/Int16/Int32/Int64/Guid/Single/Double/Decimal/DateTime/DateTimeOffset/Time/".IndexOf("/" + edmType + "/") >= 0)
                edmType += "?";
            if (edmType == "Binary")
                return "byte[]";
            return edmType;
        }

        bool CheckIdentity(XAttribute xa) {
            return (xa != null && xa.Value != null && xa.Value == "Identity");
        }

        bool CheckNullable(XAttribute xa) {
            return (xa == null || xa.Value == null || xa.Value == "true" || xa.Value == "True" || xa.Value == "1");
        }

        // https://github.com/grumpydev/SuperSimpleViewEngine
        class UtFakeSSVE {
            static Regex atModel = new Regex("@(?<a>Model|Current|Parent)\\[?(\\.(?<b>\\w+(\\.\\w+)*))?\\]?");
            static Regex atEach = new Regex("@Each\\.(?<a>[\\w\\.]+)");
            static Regex atEndEach = new Regex("@EndEach");
            static Regex atIf = new Regex("@If(?<b>Not)?\\[?\\.(?<a>\\w+(\\.\\w+)*)\\]?");
            static Regex atEndIf = new Regex("@EndIf");
            static Regex atFuncParam = new Regex("@FuncParam\\[?\\.(?<a>\\w+(\\.\\w+)*)\\]?(?<b>.+?)@EndFuncParam");

            StringWriter wr = new StringWriter();
            String[] rows;

            public UtFakeSSVE(String template, object model) {
                rows = template.Replace("\r\n", "\n").Split('\n');
                int y = 0;
                Walk(ref y, model, null, null, false);
            }

            public String Generated { get { return wr.ToString(); } }

            void Walk(ref int y, object model, object current, object parent, bool mute) {
                for (; y < rows.Length; ) {
                    var row = rows[y];
                    var mEach = atEach.Match(row);
                    if (mEach.Success) {
                        y++;
                        int oy = y;
                        var iter = mute ? null : Pickup(model, current, mEach.Groups["a"].Value, y) as IEnumerable;
                        if (iter != null) {
                            foreach (var ob in iter) {
                                y = oy;
                                Walk(ref y, model, ob, current, mute);
                            }
                        }
                        y = oy;
                        Walk(ref y, model, null, null, true);
                        continue;
                    }
                    var mEndEach = atEndEach.Match(row);
                    if (mEndEach.Success) {
                        y++;
                        break;
                    }
                    var mIf = atIf.Match(row);
                    if (mIf.Success) {
                        y++;
                        if (mute) {
                            Walk(ref y, model, null, null, true);
                        }
                        else {
                            bool test = mIf.Groups["b"].Value != "Not";
                            var flag = Pickup(model, current, mIf.Groups["a"].Value, y);
                            if (flag is bool && (bool)flag == test) {
                                Walk(ref y, model, current, parent, false);
                            }
                            else {
                                Walk(ref y, model, null, null, true);
                            }
                        }
                        continue;
                    }
                    var mEndIf = atEndIf.Match(row);
                    if (mEndIf.Success) {
                        y++;
                        break;
                    }
                    var mFuncParam = atFuncParam.Match(row);
                    if (mFuncParam.Success) {
                        String insert = "";
                        if (!mute) {
                            var iter = Pickup(model, current, mFuncParam.Groups["a"].Value, 1 + y) as IEnumerable;
                            if (iter != null) {
                                int x = 0;
                                foreach (object ob in iter) {
                                    x++;
                                    if (x != 1)
                                        insert += ", ";
                                    insert += Expr(mFuncParam.Groups["b"].Value.Trim(), model, ob, current, 1 + y);
                                }
                            }
                        }
                        row = row.Substring(0, mFuncParam.Index) + insert + row.Substring(mFuncParam.Index + mFuncParam.Length);
                    }
                    y++;
                    if (mute) continue;

                    wr.WriteLine(Expr(row, model, current, parent, y));
                }
            }

            string Expr(string row, object model, object current, object parent, int y) {
                return (atModel.Replace(row, mModel => {
                    Object ob;
                    var mode = mModel.Groups["a"].Value;
                    if (false) { }
                    else if (mode == "Model") ob = model;
                    else if (mode == "Current") ob = current;
                    else if (mode == "Parent") ob = parent;
                    else throw new ApplicationException("Unknown @" + mode);
                    String member = mModel.Groups["b"].Value;
                    return (member.Length == 0) ? "" + ob : "" + FormatDisp(Pickup(ob, null, member, y));
                }));
            }

            private string FormatDisp(object p) {
                if (p is bool)
                    return ((bool)p) ? "true" : "false";
                return "" + p;
            }

            static object Pickup(object model, object current, string member, int y) {
                Object ob = current ?? model;
                foreach (string m1 in member.Split('.')) {
                    try {
                        ob = ob.GetType().InvokeMember(m1, BindingFlags.GetField | BindingFlags.GetProperty, null, ob, new object[0]);
                    }
                    catch (System.MissingMethodException) {
                        throw new ApplicationException(String.Format("Line {0}, parameter \"{1}\" unknown", y, m1));
                    }
                }
                return ob;
            }
        }
    }
}
