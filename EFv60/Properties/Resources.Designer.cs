﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EdmGen06.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EdmGen06.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   型 System.Byte[] のローカライズされたリソースを検索します。
        /// </summary>
        internal static byte[] ConceptualSchemaDefinition {
            get {
                object obj = ResourceManager.GetObject("ConceptualSchemaDefinition", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   型 System.Byte[] のローカライズされたリソースを検索します。
        /// </summary>
        internal static byte[] ConceptualSchemaDefinitionVersion3 {
            get {
                object obj = ResourceManager.GetObject("ConceptualSchemaDefinitionVersion3", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   using System;
        ///using System.Data.Entity;
        ///using System.Data.Entity.Infrastructure;
        ///
        ///namespace @Model.Namespace {
        ///@Each.EntityContainer
        ///  public class @Current.Name : DbContext {
        ///    public @Current.Name(): base(&quot;name=@Current.Name&quot;) {
        ///    
        ///    }
        ///    
        ///    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        ///      throw new UnintentionalCodeFirstException();
        ///    }
        ///
        ///@Each.EntitySet
        ///    public DbSet&lt;@Current.EntityType&gt; @Current.Name { get; set; }
        ///@EndEach
        ///  }
        ///@EndEach
        ///
        ///@Eac [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string DbContext_EFv5 {
            get {
                return ResourceManager.GetString("DbContext_EFv5", resourceCulture);
            }
        }
        
        /// <summary>
        ///   using System;
        ///using System.ComponentModel.DataAnnotations;
        ///using System.ComponentModel.DataAnnotations.Schema;
        ///using System.Data.Entity;
        ///using System.Data.Entity.Infrastructure;
        ///
        ///namespace @Model.Namespace {
        ///@Each.EntityContainer
        ///  public class @Current.Name : DbContext {
        ///    public @Current.Name()
        ///        : base(DBUt.Connect(), true) {
        ///
        ///    }
        ///
        ///    static class DBUt {
        ///        internal static String ConnectionString {
        ///            get {
        ///                return &quot;@Model.ConnectionString&quot;;
        ///      [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string DbContext_EFv6 {
            get {
                return ResourceManager.GetString("DbContext_EFv6", resourceCulture);
            }
        }
        
        /// <summary>
        ///   using System;
        ///using System.ComponentModel;
        ///using System.Data.Entity.Core.EntityClient;
        ///using System.Data.Entity.Core.Objects;
        ///using System.Data.Entity.Core.Objects.DataClasses;
        ///using System.Linq;
        ///using System.Runtime.Serialization;
        ///using System.Xml.Serialization;
        ///
        ///[assembly: EdmSchemaAttribute()]
        ///#region EDM Relationship Metadata
        ///@Each.Association
        ///[assembly: EdmRelationshipAttribute(
        ///  &quot;@Model.Namespace&quot;, 
        ///  &quot;@Current.Name&quot;, 
        ///  &quot;@Current.Role1&quot;, 
        ///  System.Data.Entity.Core.Metadata.Edm.Relati [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ObjectContext {
            get {
                return ResourceManager.GetString("ObjectContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   EdmGen06
        /// /ModelGen   &lt;connectionString&gt; &lt;providerName&gt; &lt;modelName&gt; &lt;targetSchema&gt; &lt;ver&gt;
        /// /EFModelGen &lt;connectionString&gt; &lt;providerName&gt; &lt;DbProviderServices&gt; &lt;modelName&gt; &lt;targetSchema&gt; &lt;ver&gt;
        /// /EFCodeFirstGen &lt;input.edmx&gt; &lt;output.cs&gt; &lt;generator&gt;
        /// /DataSet &lt;connectionString&gt; &lt;providerName&gt; &lt;modelName&gt; &lt;targetSchema&gt;
        /// /DataSet.cs &lt;DataSet1.xsd&gt; &lt;DataSet1.cs&gt;
        ///
        ///rem &lt;ver&gt; is ssdl/msl/csdl schema version: 1.0 or 3.0
        ///rem &lt;generator&gt;: DbContext.EFv6 or DbContext.EFv5 or ObjectContext
        ///
        ///rem [SqlServer Example [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Usage {
            get {
                return ResourceManager.GetString("Usage", resourceCulture);
            }
        }
    }
}
