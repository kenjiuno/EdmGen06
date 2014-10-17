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

namespace EdmGen06 {
    public class EdmGenBase {
        /// XName namespace: provider manifest ssdl
        protected String pSSDL;
        /// XName namespace: provider manifest csdl
        protected String pCSDL;
        /// XName namespace: provider manifest msl
        protected String pMSL;

        /// XName namespace: ModelGen edmx 
        protected String xEDMX;
        /// XName namespace: ModelGen ssdl 
        protected String xSSDL;
        /// XName namespace: ModelGen csdl 
        protected String xCSDL;
        /// XName namespace: ModelGen msl
        protected String xMSL;
        /// XName namespace: EntityStoreSchemaGenerator
        protected String xSTORE = "{" + "http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator" + "}";
        /// XName namespace: annotation
        protected String xAnno = "{" + "http://schemas.microsoft.com/ado/2009/02/edm/annotation" + "}";

        protected class NS {
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

        public EdmGenBase() {

        }

        protected TraceSource trace = new TraceSource("EdmGen06", SourceLevels.Verbose);
    }
}
