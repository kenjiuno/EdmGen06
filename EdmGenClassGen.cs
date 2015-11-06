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
            String template = Resources.DbContext_EFv6;
            if (true) {
                var nsAnno = XNamespace.Get("http://schemas.microsoft.com/ado/2009/02/edm/annotation");
                var Model = new {
                    Namespace = Schema.Attribute("Namespace").Value,
                    EntityContainer = Schema.Elements(nsCSDL + "EntityContainer").Select(
                        vEntityContainer => new {
                            Name = vEntityContainer.Attribute("Name").Value,
                            EntitySet = vEntityContainer.Elements(nsCSDL + "EntitySet").Select(
                                vEntitySet => new {
                                    Name = vEntitySet.Attribute("Name").Value,
                                    EntityType = vEntitySet.Attribute("EntityType").Value,
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
                                    Identity = CheckIdentity(vProperty.Attribute(nsAnno + "StoreGeneratedPattern"))
                                }
                            ),
                            NavigationProperty = vEntityType.Elements(nsCSDL + "NavigationProperty").Select(
                                vNavigationProperty => new {
                                    Name = vNavigationProperty.Attribute("Name").Value,
                                    Type = vNavigationProperty.Attribute("Name").Value,
                                    Many = false,
                                    One = true,
                                }
                            )
                        }
                    ),
                };
                File.WriteAllText(fpcs, new UtFakeSSVE(template, Model).Generated);
            }
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
            static Regex atModel = new Regex("@(?<a>Model|Current)(\\.(?<b>\\w+))?");
            static Regex atEach = new Regex("@Each\\.(?<a>\\w+)");
            static Regex atEndEach = new Regex("@EndEach");
            static Regex atIf = new Regex("@If\\.(?<a>\\w+)");
            static Regex atEndIf = new Regex("@EndIf");

            StringWriter wr = new StringWriter();
            String[] rows;

            public UtFakeSSVE(String template, object model) {
                rows = template.Replace("\r\n", "\n").Split('\n');
                int y = 0;
                Walk(ref y, model, null, false);
            }

            public String Generated { get { return wr.ToString(); } }

            void Walk(ref int y, object model, object current, bool mute) {
                for (; y < rows.Length; ) {
                    var row = rows[y];
                    var mEach = atEach.Match(row);
                    if (mEach.Success) {
                        y++;
                        int oy = y;
                        var iter = mute ? null : Pickup(model, current, mEach.Groups["a"].Value) as IEnumerable;
                        if (iter != null) {
                            foreach (var ob in iter) {
                                y = oy;
                                Walk(ref y, model, ob, mute);
                            }
                        }
                        y = oy;
                        Walk(ref y, model, null, true);
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
                            Walk(ref y, model, null, true);
                        }
                        else {
                            var flag = Pickup(model, current, mIf.Groups["a"].Value);
                            if (flag is bool && (bool)flag) {
                                Walk(ref y, model, current, false);
                            }
                            else {
                                Walk(ref y, model, null, true);
                            }
                        }
                        continue;
                    }
                    var mEndIf = atEndIf.Match(row);
                    if (mEndIf.Success) {
                        y++;
                        break;
                    }
                    y++;
                    if (mute) continue;
                    wr.WriteLine(atModel.Replace(row, mModel => {
                        Object ob = (mModel.Groups["a"].Value == "Model") ? model : current;
                        String member = mModel.Groups["b"].Value;
                        return (member.Length == 0) ? "" + ob : "" + Pickup(ob, null, member);
                    }));
                }
            }

            private static object Pickup(object model, object current, string member) {
                Object ob = current ?? model;
                return ob.GetType().InvokeMember(member, BindingFlags.GetField | BindingFlags.GetProperty, null, ob, new object[0]);
            }
        }
    }
}
