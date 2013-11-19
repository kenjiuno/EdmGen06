using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

[assembly: EdmSchemaAttribute()]
[assembly: EdmRelationshipAttribute("Store", "TableOrViewColumn", "Parent", RelationshipMultiplicity.One, typeof(EdmGen06.TableOrView), "Column", RelationshipMultiplicity.Many, typeof(EdmGen06.Column))]
[assembly: EdmRelationshipAttribute("Store", "TableOrViewConstraint", "Parent", RelationshipMultiplicity.One, typeof(EdmGen06.TableOrView), "Constraint", RelationshipMultiplicity.Many, typeof(EdmGen06.Constraint))]
[assembly: EdmRelationshipAttribute("Store", "TableOrViewConstraintColumn", "Constraint", RelationshipMultiplicity.Many, typeof(EdmGen06.TableOrViewColumnConstraint), "Column", RelationshipMultiplicity.Many, typeof(EdmGen06.Column))]
[assembly: EdmRelationshipAttribute("Store", "ConstraintForeignKey", "Constraint", RelationshipMultiplicity.One, typeof(EdmGen06.ForeignKeyConstraint), "ForeignKey", RelationshipMultiplicity.Many, typeof(EdmGen06.ForeignKey))]
[assembly: EdmRelationshipAttribute("Store", "ToForeignKeyColumn", "ForeignKey", RelationshipMultiplicity.Many, typeof(EdmGen06.ForeignKey), "Column", RelationshipMultiplicity.One, typeof(EdmGen06.Column))]
[assembly: EdmRelationshipAttribute("Store", "FromForeignKeyColumn", "ForeignKey", RelationshipMultiplicity.Many, typeof(EdmGen06.ForeignKey), "Column", RelationshipMultiplicity.One, typeof(EdmGen06.Column))]
[assembly: EdmRelationshipAttribute("Store", "RoutineParameter", "Routine", RelationshipMultiplicity.One, typeof(EdmGen06.Routine), "Parameter", RelationshipMultiplicity.Many, typeof(EdmGen06.Parameter))]

namespace EdmGen06 {

    /// <summary>
    /// スキーマの SchemaInformation にはコメントがありません。
    /// </summary>
    public partial class SchemaInformation : ObjectContext {
        /// <summary>
        /// アプリケーション構成ファイルの 'SchemaInformation' セクションにある接続文字列を使用して新しい SchemaInformation オブジェクトを初期化します。
        /// </summary>
        public SchemaInformation() :
            base("name=SchemaInformation", "SchemaInformation") {
            this.ContextOptions.LazyLoadingEnabled = true;
            this.OnContextCreated();
        }
        /// <summary>
        /// 新しい SchemaInformation オブジェクトを初期化します。
        /// </summary>
        public SchemaInformation(string connectionString) :
            base(connectionString, "SchemaInformation") {
            this.ContextOptions.LazyLoadingEnabled = true;
            this.OnContextCreated();
        }
        /// <summary>
        /// 新しい SchemaInformation オブジェクトを初期化します。
        /// </summary>
        public SchemaInformation(EntityConnection connection) :
            base(connection, "SchemaInformation") {
            this.ContextOptions.LazyLoadingEnabled = true;
            this.OnContextCreated();
        }
        partial void OnContextCreated();
        /// <summary>
        /// スキーマの Tables にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<Table> Tables {
            get {
                if ((this._Tables == null)) {
                    this._Tables = base.CreateObjectSet<Table>("Tables");
                }
                return this._Tables;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<Table> _Tables;
        /// <summary>
        /// スキーマの TableColumns にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<Column> TableColumns {
            get {
                if ((this._TableColumns == null)) {
                    this._TableColumns = base.CreateObjectSet<Column>("TableColumns");
                }
                return this._TableColumns;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<Column> _TableColumns;
        /// <summary>
        /// スキーマの TableConstraints にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<Constraint> TableConstraints {
            get {
                if ((this._TableConstraints == null)) {
                    this._TableConstraints = base.CreateObjectSet<Constraint>("TableConstraints");
                }
                return this._TableConstraints;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<Constraint> _TableConstraints;
        /// <summary>
        /// スキーマの TableForeignKeys にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<ForeignKey> TableForeignKeys {
            get {
                if ((this._TableForeignKeys == null)) {
                    this._TableForeignKeys = base.CreateObjectSet<ForeignKey>("TableForeignKeys");
                }
                return this._TableForeignKeys;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<ForeignKey> _TableForeignKeys;
        /// <summary>
        /// スキーマの Views にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<View> Views {
            get {
                if ((this._Views == null)) {
                    this._Views = base.CreateObjectSet<View>("Views");
                }
                return this._Views;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<View> _Views;
        /// <summary>
        /// スキーマの ViewColumns にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<Column> ViewColumns {
            get {
                if ((this._ViewColumns == null)) {
                    this._ViewColumns = base.CreateObjectSet<Column>("ViewColumns");
                }
                return this._ViewColumns;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<Column> _ViewColumns;
        /// <summary>
        /// スキーマの ViewConstraints にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<Constraint> ViewConstraints {
            get {
                if ((this._ViewConstraints == null)) {
                    this._ViewConstraints = base.CreateObjectSet<Constraint>("ViewConstraints");
                }
                return this._ViewConstraints;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<Constraint> _ViewConstraints;
        /// <summary>
        /// スキーマの ViewForeignKeys にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<ForeignKey> ViewForeignKeys {
            get {
                if ((this._ViewForeignKeys == null)) {
                    this._ViewForeignKeys = base.CreateObjectSet<ForeignKey>("ViewForeignKeys");
                }
                return this._ViewForeignKeys;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<ForeignKey> _ViewForeignKeys;
        /// <summary>
        /// スキーマの Functions にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<Function> Functions {
            get {
                if ((this._Functions == null)) {
                    this._Functions = base.CreateObjectSet<Function>("Functions");
                }
                return this._Functions;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<Function> _Functions;
        /// <summary>
        /// スキーマの FunctionParameters にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<Parameter> FunctionParameters {
            get {
                if ((this._FunctionParameters == null)) {
                    this._FunctionParameters = base.CreateObjectSet<Parameter>("FunctionParameters");
                }
                return this._FunctionParameters;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<Parameter> _FunctionParameters;
        /// <summary>
        /// スキーマの Procedures にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<Procedure> Procedures {
            get {
                if ((this._Procedures == null)) {
                    this._Procedures = base.CreateObjectSet<Procedure>("Procedures");
                }
                return this._Procedures;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<Procedure> _Procedures;
        /// <summary>
        /// スキーマの ProcedureParameters にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public ObjectSet<Parameter> ProcedureParameters {
            get {
                if ((this._ProcedureParameters == null)) {
                    this._ProcedureParameters = base.CreateObjectSet<Parameter>("ProcedureParameters");
                }
                return this._ProcedureParameters;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private ObjectSet<Parameter> _ProcedureParameters;
        /// <summary>
        /// スキーマの Tables にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToTables(Table table) {
            base.AddObject("Tables", table);
        }
        /// <summary>
        /// スキーマの TableColumns にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToTableColumns(Column column) {
            base.AddObject("TableColumns", column);
        }
        /// <summary>
        /// スキーマの TableConstraints にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToTableConstraints(Constraint constraint) {
            base.AddObject("TableConstraints", constraint);
        }
        /// <summary>
        /// スキーマの TableForeignKeys にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToTableForeignKeys(ForeignKey foreignKey) {
            base.AddObject("TableForeignKeys", foreignKey);
        }
        /// <summary>
        /// スキーマの Views にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToViews(View view) {
            base.AddObject("Views", view);
        }
        /// <summary>
        /// スキーマの ViewColumns にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToViewColumns(Column column) {
            base.AddObject("ViewColumns", column);
        }
        /// <summary>
        /// スキーマの ViewConstraints にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToViewConstraints(Constraint constraint) {
            base.AddObject("ViewConstraints", constraint);
        }
        /// <summary>
        /// スキーマの ViewForeignKeys にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToViewForeignKeys(ForeignKey foreignKey) {
            base.AddObject("ViewForeignKeys", foreignKey);
        }
        /// <summary>
        /// スキーマの Functions にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToFunctions(Function function) {
            base.AddObject("Functions", function);
        }
        /// <summary>
        /// スキーマの FunctionParameters にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToFunctionParameters(Parameter parameter) {
            base.AddObject("FunctionParameters", parameter);
        }
        /// <summary>
        /// スキーマの Procedures にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToProcedures(Procedure procedure) {
            base.AddObject("Procedures", procedure);
        }
        /// <summary>
        /// スキーマの ProcedureParameters にはコメントがありません。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToProcedureParameters(Parameter parameter) {
            base.AddObject("ProcedureParameters", parameter);
        }
    }
    /// <summary>
    /// スキーマの ComplexType Store.TypeSpecification にはコメントがありません。
    /// </summary>
    [EdmComplexTypeAttribute(NamespaceName = "Store", Name = "TypeSpecification")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class TypeSpecification : ComplexObject {
        /// <summary>
        /// 新しい TypeSpecification オブジェクトを作成します。
        /// </summary>
        /// <param name="typeName">TypeName の初期値。</param>
        /// <param name="collation">Collation の初期値。</param>
        /// <param name="characterSet">CharacterSet の初期値。</param>
        /// <param name="isMultiSet">IsMultiSet の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static TypeSpecification CreateTypeSpecification(string typeName, Collation collation, CharacterSet characterSet, bool isMultiSet) {
            TypeSpecification typeSpecification = new TypeSpecification();
            typeSpecification.TypeName = typeName;
            typeSpecification.Collation = StructuralObject.VerifyComplexObjectIsNotNull(collation, "Collation");
            typeSpecification.CharacterSet = StructuralObject.VerifyComplexObjectIsNotNull(characterSet, "CharacterSet");
            typeSpecification.IsMultiSet = isMultiSet;
            return typeSpecification;
        }
        /// <summary>
        /// スキーマのプロパティ TypeName にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string TypeName {
            get {
                return this._TypeName;
            }
            set {
                this.OnTypeNameChanging(value);
                this.ReportPropertyChanging("TypeName");
                this._TypeName = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("TypeName");
                this.OnTypeNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _TypeName;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnTypeNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnTypeNameChanged();
        /// <summary>
        /// スキーマのプロパティ MaxLength にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<int> MaxLength {
            get {
                return this._MaxLength;
            }
            set {
                this.OnMaxLengthChanging(value);
                this.ReportPropertyChanging("MaxLength");
                this._MaxLength = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("MaxLength");
                this.OnMaxLengthChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<int> _MaxLength;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnMaxLengthChanging(global::System.Nullable<int> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnMaxLengthChanged();
        /// <summary>
        /// スキーマのプロパティ Precision にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<int> Precision {
            get {
                return this._Precision;
            }
            set {
                this.OnPrecisionChanging(value);
                this.ReportPropertyChanging("Precision");
                this._Precision = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Precision");
                this.OnPrecisionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<int> _Precision;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnPrecisionChanging(global::System.Nullable<int> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnPrecisionChanged();
        /// <summary>
        /// スキーマのプロパティ DateTimePrecision にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<int> DateTimePrecision {
            get {
                return this._DateTimePrecision;
            }
            set {
                this.OnDateTimePrecisionChanging(value);
                this.ReportPropertyChanging("DateTimePrecision");
                this._DateTimePrecision = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("DateTimePrecision");
                this.OnDateTimePrecisionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<int> _DateTimePrecision;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnDateTimePrecisionChanging(global::System.Nullable<int> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnDateTimePrecisionChanged();
        /// <summary>
        /// スキーマのプロパティ Scale にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<int> Scale {
            get {
                return this._Scale;
            }
            set {
                this.OnScaleChanging(value);
                this.ReportPropertyChanging("Scale");
                this._Scale = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Scale");
                this.OnScaleChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<int> _Scale;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnScaleChanging(global::System.Nullable<int> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnScaleChanged();
        /// <summary>
        /// スキーマのプロパティ Collation にはコメントがありません。
        /// </summary>
        [EdmComplexPropertyAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        [global::System.Xml.Serialization.XmlElement(IsNullable = true)]
        [global::System.Xml.Serialization.SoapElement(IsNullable = true)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public Collation Collation {
            get {
                this._Collation = this.GetValidValue(this._Collation, "Collation", false, this._CollationInitialized);
                this._CollationInitialized = true;
                return this._Collation;
            }
            set {
                this.OnCollationChanging(value);
                this.ReportPropertyChanging("Collation");
                this._Collation = this.SetValidValue(this._Collation, value, "Collation");
                this._CollationInitialized = true;
                this.ReportPropertyChanged("Collation");
                this.OnCollationChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private Collation _Collation;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _CollationInitialized;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCollationChanging(Collation value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCollationChanged();
        /// <summary>
        /// スキーマのプロパティ CharacterSet にはコメントがありません。
        /// </summary>
        [EdmComplexPropertyAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        [global::System.Xml.Serialization.XmlElement(IsNullable = true)]
        [global::System.Xml.Serialization.SoapElement(IsNullable = true)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public CharacterSet CharacterSet {
            get {
                this._CharacterSet = this.GetValidValue(this._CharacterSet, "CharacterSet", false, this._CharacterSetInitialized);
                this._CharacterSetInitialized = true;
                return this._CharacterSet;
            }
            set {
                this.OnCharacterSetChanging(value);
                this.ReportPropertyChanging("CharacterSet");
                this._CharacterSet = this.SetValidValue(this._CharacterSet, value, "CharacterSet");
                this._CharacterSetInitialized = true;
                this.ReportPropertyChanged("CharacterSet");
                this.OnCharacterSetChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private CharacterSet _CharacterSet;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _CharacterSetInitialized;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCharacterSetChanging(CharacterSet value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCharacterSetChanged();
        /// <summary>
        /// スキーマのプロパティ IsMultiSet にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public bool IsMultiSet {
            get {
                return this._IsMultiSet;
            }
            set {
                this.OnIsMultiSetChanging(value);
                this.ReportPropertyChanging("IsMultiSet");
                this._IsMultiSet = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsMultiSet");
                this.OnIsMultiSetChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _IsMultiSet;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsMultiSetChanging(bool value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsMultiSetChanged();
    }
    /// <summary>
    /// スキーマの ComplexType Store.Collation にはコメントがありません。
    /// </summary>
    [EdmComplexTypeAttribute(NamespaceName = "Store", Name = "Collation")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class Collation : ComplexObject {
        /// <summary>
        /// スキーマのプロパティ CatalogName にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string CatalogName {
            get {
                return this._CatalogName;
            }
            set {
                this.OnCatalogNameChanging(value);
                this.ReportPropertyChanging("CatalogName");
                this._CatalogName = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("CatalogName");
                this.OnCatalogNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _CatalogName;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCatalogNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCatalogNameChanged();
        /// <summary>
        /// スキーマのプロパティ SchemaName にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string SchemaName {
            get {
                return this._SchemaName;
            }
            set {
                this.OnSchemaNameChanging(value);
                this.ReportPropertyChanging("SchemaName");
                this._SchemaName = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("SchemaName");
                this.OnSchemaNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _SchemaName;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSchemaNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSchemaNameChanged();
        /// <summary>
        /// スキーマのプロパティ Name にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Name {
            get {
                return this._Name;
            }
            set {
                this.OnNameChanging(value);
                this.ReportPropertyChanging("Name");
                this._Name = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("Name");
                this.OnNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Name;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanged();
    }
    /// <summary>
    /// スキーマの ComplexType Store.CharacterSet にはコメントがありません。
    /// </summary>
    [EdmComplexTypeAttribute(NamespaceName = "Store", Name = "CharacterSet")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class CharacterSet : ComplexObject {
        /// <summary>
        /// スキーマのプロパティ CatalogName にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string CatalogName {
            get {
                return this._CatalogName;
            }
            set {
                this.OnCatalogNameChanging(value);
                this.ReportPropertyChanging("CatalogName");
                this._CatalogName = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("CatalogName");
                this.OnCatalogNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _CatalogName;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCatalogNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCatalogNameChanged();
        /// <summary>
        /// スキーマのプロパティ SchemaName にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string SchemaName {
            get {
                return this._SchemaName;
            }
            set {
                this.OnSchemaNameChanging(value);
                this.ReportPropertyChanging("SchemaName");
                this._SchemaName = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("SchemaName");
                this.OnSchemaNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _SchemaName;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSchemaNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSchemaNameChanged();
        /// <summary>
        /// スキーマのプロパティ Name にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Name {
            get {
                return this._Name;
            }
            set {
                this.OnNameChanging(value);
                this.ReportPropertyChanging("Name");
                this._Name = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("Name");
                this.OnNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Name;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanged();
    }
    /// <summary>
    /// スキーマの Store.TableOrView にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "TableOrView")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.Table))]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.View))]
    public abstract partial class TableOrView : EntityObject {
        /// <summary>
        /// スキーマのプロパティ Id にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Id {
            get {
                return this._Id;
            }
            set {
                this.OnIdChanging(value);
                this.ReportPropertyChanging("Id");
                this._Id = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Id");
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Id;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanged();
        /// <summary>
        /// スキーマのプロパティ Name にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Name {
            get {
                return this._Name;
            }
            set {
                this.OnNameChanging(value);
                this.ReportPropertyChanging("Name");
                this._Name = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Name");
                this.OnNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Name;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanged();
        /// <summary>
        /// スキーマのプロパティ CatalogName にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string CatalogName {
            get {
                return this._CatalogName;
            }
            set {
                this.OnCatalogNameChanging(value);
                this.ReportPropertyChanging("CatalogName");
                this._CatalogName = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("CatalogName");
                this.OnCatalogNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _CatalogName;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCatalogNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCatalogNameChanged();
        /// <summary>
        /// スキーマのプロパティ SchemaName にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string SchemaName {
            get {
                return this._SchemaName;
            }
            set {
                this.OnSchemaNameChanging(value);
                this.ReportPropertyChanging("SchemaName");
                this._SchemaName = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("SchemaName");
                this.OnSchemaNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _SchemaName;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSchemaNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSchemaNameChanged();
        /// <summary>
        /// スキーマの Columns にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "TableOrViewColumn", "Column")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityCollection<Column> Columns {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<Column>("Store.TableOrViewColumn", "Column");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<Column>("Store.TableOrViewColumn", "Column", value);
                }
            }
        }
        /// <summary>
        /// スキーマの Constraints にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "TableOrViewConstraint", "Constraint")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityCollection<Constraint> Constraints {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<Constraint>("Store.TableOrViewConstraint", "Constraint");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<Constraint>("Store.TableOrViewConstraint", "Constraint", value);
                }
            }
        }
    }
    /// <summary>
    /// スキーマの Store.Table にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "Table")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class Table : TableOrView {
        /// <summary>
        /// 新しい Table オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static Table CreateTable(string id, string name) {
            Table table = new Table();
            table.Id = id;
            table.Name = name;
            return table;
        }
    }
    /// <summary>
    /// スキーマの Store.Column にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "Column")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class Column : EntityObject {
        /// <summary>
        /// 新しい Column オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        /// <param name="ordinal">Ordinal の初期値。</param>
        /// <param name="isNullable">IsNullable の初期値。</param>
        /// <param name="columnType">ColumnType の初期値。</param>
        /// <param name="isIdentity">IsIdentity の初期値。</param>
        /// <param name="isStoreGenerated">IsStoreGenerated の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static Column CreateColumn(string id, string name, int ordinal, bool isNullable, TypeSpecification columnType, bool isIdentity, bool isStoreGenerated) {
            Column column = new Column();
            column.Id = id;
            column.Name = name;
            column.Ordinal = ordinal;
            column.IsNullable = isNullable;
            column.ColumnType = StructuralObject.VerifyComplexObjectIsNotNull(columnType, "ColumnType");
            column.IsIdentity = isIdentity;
            column.IsStoreGenerated = isStoreGenerated;
            return column;
        }
        /// <summary>
        /// スキーマのプロパティ Id にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Id {
            get {
                return this._Id;
            }
            set {
                this.OnIdChanging(value);
                this.ReportPropertyChanging("Id");
                this._Id = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Id");
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Id;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanged();
        /// <summary>
        /// スキーマのプロパティ Name にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Name {
            get {
                return this._Name;
            }
            set {
                this.OnNameChanging(value);
                this.ReportPropertyChanging("Name");
                this._Name = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Name");
                this.OnNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Name;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanged();
        /// <summary>
        /// スキーマのプロパティ Ordinal にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int Ordinal {
            get {
                return this._Ordinal;
            }
            set {
                this.OnOrdinalChanging(value);
                this.ReportPropertyChanging("Ordinal");
                this._Ordinal = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Ordinal");
                this.OnOrdinalChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _Ordinal;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOrdinalChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOrdinalChanged();
        /// <summary>
        /// スキーマのプロパティ IsNullable にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public bool IsNullable {
            get {
                return this._IsNullable;
            }
            set {
                this.OnIsNullableChanging(value);
                this.ReportPropertyChanging("IsNullable");
                this._IsNullable = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsNullable");
                this.OnIsNullableChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _IsNullable;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsNullableChanging(bool value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsNullableChanged();
        /// <summary>
        /// スキーマのプロパティ ColumnType にはコメントがありません。
        /// </summary>
        [EdmComplexPropertyAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        [global::System.Xml.Serialization.XmlElement(IsNullable = true)]
        [global::System.Xml.Serialization.SoapElement(IsNullable = true)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public TypeSpecification ColumnType {
            get {
                this._ColumnType = this.GetValidValue(this._ColumnType, "ColumnType", false, this._ColumnTypeInitialized);
                this._ColumnTypeInitialized = true;
                return this._ColumnType;
            }
            set {
                this.OnColumnTypeChanging(value);
                this.ReportPropertyChanging("ColumnType");
                this._ColumnType = this.SetValidValue(this._ColumnType, value, "ColumnType");
                this._ColumnTypeInitialized = true;
                this.ReportPropertyChanged("ColumnType");
                this.OnColumnTypeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private TypeSpecification _ColumnType;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _ColumnTypeInitialized;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnColumnTypeChanging(TypeSpecification value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnColumnTypeChanged();
        /// <summary>
        /// スキーマのプロパティ IsIdentity にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public bool IsIdentity {
            get {
                return this._IsIdentity;
            }
            set {
                this.OnIsIdentityChanging(value);
                this.ReportPropertyChanging("IsIdentity");
                this._IsIdentity = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsIdentity");
                this.OnIsIdentityChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _IsIdentity;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsIdentityChanging(bool value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsIdentityChanged();
        /// <summary>
        /// スキーマのプロパティ IsStoreGenerated にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public bool IsStoreGenerated {
            get {
                return this._IsStoreGenerated;
            }
            set {
                this.OnIsStoreGeneratedChanging(value);
                this.ReportPropertyChanging("IsStoreGenerated");
                this._IsStoreGenerated = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsStoreGenerated");
                this.OnIsStoreGeneratedChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _IsStoreGenerated;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsStoreGeneratedChanging(bool value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsStoreGeneratedChanged();
        /// <summary>
        /// スキーマのプロパティ Default にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Default {
            get {
                return this._Default;
            }
            set {
                this.OnDefaultChanging(value);
                this.ReportPropertyChanging("Default");
                this._Default = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("Default");
                this.OnDefaultChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Default;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnDefaultChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnDefaultChanged();
        /// <summary>
        /// スキーマの Parent にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "TableOrViewColumn", "Parent")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public TableOrView Parent {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<TableOrView>("Store.TableOrViewColumn", "Parent").Value;
            }
            set {
                ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<TableOrView>("Store.TableOrViewColumn", "Parent").Value = value;
            }
        }
        /// <summary>
        /// スキーマの Parent にはコメントがありません。
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityReference<TableOrView> ParentReference {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<TableOrView>("Store.TableOrViewColumn", "Parent");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedReference<TableOrView>("Store.TableOrViewColumn", "Parent", value);
                }
            }
        }
        /// <summary>
        /// スキーマの Constraints にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "TableOrViewConstraintColumn", "Constraint")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityCollection<TableOrViewColumnConstraint> Constraints {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<TableOrViewColumnConstraint>("Store.TableOrViewConstraintColumn", "Constraint");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<TableOrViewColumnConstraint>("Store.TableOrViewConstraintColumn", "Constraint", value);
                }
            }
        }
        /// <summary>
        /// スキーマの ToForeignKeys にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "ToForeignKeyColumn", "ForeignKey")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityCollection<ForeignKey> ToForeignKeys {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<ForeignKey>("Store.ToForeignKeyColumn", "ForeignKey");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<ForeignKey>("Store.ToForeignKeyColumn", "ForeignKey", value);
                }
            }
        }
        /// <summary>
        /// スキーマの FromForeignKeys にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "FromForeignKeyColumn", "ForeignKey")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityCollection<ForeignKey> FromForeignKeys {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<ForeignKey>("Store.FromForeignKeyColumn", "ForeignKey");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<ForeignKey>("Store.FromForeignKeyColumn", "ForeignKey", value);
                }
            }
        }
    }
    /// <summary>
    /// スキーマの Store.View にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "View")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class View : TableOrView {
        /// <summary>
        /// 新しい View オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        /// <param name="isUpdatable">IsUpdatable の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static View CreateView(string id, string name, bool isUpdatable) {
            View view = new View();
            view.Id = id;
            view.Name = name;
            view.IsUpdatable = isUpdatable;
            return view;
        }
        /// <summary>
        /// スキーマのプロパティ IsUpdatable にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public bool IsUpdatable {
            get {
                return this._IsUpdatable;
            }
            set {
                this.OnIsUpdatableChanging(value);
                this.ReportPropertyChanging("IsUpdatable");
                this._IsUpdatable = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsUpdatable");
                this.OnIsUpdatableChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _IsUpdatable;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsUpdatableChanging(bool value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsUpdatableChanged();
        /// <summary>
        /// スキーマのプロパティ ViewDefinition にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string ViewDefinition {
            get {
                return this._ViewDefinition;
            }
            set {
                this.OnViewDefinitionChanging(value);
                this.ReportPropertyChanging("ViewDefinition");
                this._ViewDefinition = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("ViewDefinition");
                this.OnViewDefinitionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _ViewDefinition;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnViewDefinitionChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnViewDefinitionChanged();
    }
    /// <summary>
    /// スキーマの Store.Routine にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "Routine")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.Function))]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.Procedure))]
    public abstract partial class Routine : EntityObject {
        /// <summary>
        /// スキーマのプロパティ Id にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Id {
            get {
                return this._Id;
            }
            set {
                this.OnIdChanging(value);
                this.ReportPropertyChanging("Id");
                this._Id = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Id");
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Id;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanged();
        /// <summary>
        /// スキーマのプロパティ CatalogName にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string CatalogName {
            get {
                return this._CatalogName;
            }
            set {
                this.OnCatalogNameChanging(value);
                this.ReportPropertyChanging("CatalogName");
                this._CatalogName = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("CatalogName");
                this.OnCatalogNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _CatalogName;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCatalogNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCatalogNameChanged();
        /// <summary>
        /// スキーマのプロパティ SchemaName にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string SchemaName {
            get {
                return this._SchemaName;
            }
            set {
                this.OnSchemaNameChanging(value);
                this.ReportPropertyChanging("SchemaName");
                this._SchemaName = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("SchemaName");
                this.OnSchemaNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _SchemaName;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSchemaNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSchemaNameChanged();
        /// <summary>
        /// スキーマのプロパティ Name にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Name {
            get {
                return this._Name;
            }
            set {
                this.OnNameChanging(value);
                this.ReportPropertyChanging("Name");
                this._Name = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Name");
                this.OnNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Name;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanged();
        /// <summary>
        /// スキーマの Parameters にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "RoutineParameter", "Parameter")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityCollection<Parameter> Parameters {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<Parameter>("Store.RoutineParameter", "Parameter");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<Parameter>("Store.RoutineParameter", "Parameter", value);
                }
            }
        }
    }
    /// <summary>
    /// スキーマの Store.Parameter にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "Parameter")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class Parameter : EntityObject {
        /// <summary>
        /// 新しい Parameter オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        /// <param name="ordinal">Ordinal の初期値。</param>
        /// <param name="parameterType">ParameterType の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static Parameter CreateParameter(string id, string name, int ordinal, TypeSpecification parameterType) {
            Parameter parameter = new Parameter();
            parameter.Id = id;
            parameter.Name = name;
            parameter.Ordinal = ordinal;
            parameter.ParameterType = StructuralObject.VerifyComplexObjectIsNotNull(parameterType, "ParameterType");
            return parameter;
        }
        /// <summary>
        /// スキーマのプロパティ Id にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Id {
            get {
                return this._Id;
            }
            set {
                this.OnIdChanging(value);
                this.ReportPropertyChanging("Id");
                this._Id = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Id");
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Id;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanged();
        /// <summary>
        /// スキーマのプロパティ Name にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Name {
            get {
                return this._Name;
            }
            set {
                this.OnNameChanging(value);
                this.ReportPropertyChanging("Name");
                this._Name = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Name");
                this.OnNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Name;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanged();
        /// <summary>
        /// スキーマのプロパティ Ordinal にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int Ordinal {
            get {
                return this._Ordinal;
            }
            set {
                this.OnOrdinalChanging(value);
                this.ReportPropertyChanging("Ordinal");
                this._Ordinal = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Ordinal");
                this.OnOrdinalChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _Ordinal;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOrdinalChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOrdinalChanged();
        /// <summary>
        /// スキーマのプロパティ ParameterType にはコメントがありません。
        /// </summary>
        [EdmComplexPropertyAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        [global::System.Xml.Serialization.XmlElement(IsNullable = true)]
        [global::System.Xml.Serialization.SoapElement(IsNullable = true)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public TypeSpecification ParameterType {
            get {
                this._ParameterType = this.GetValidValue(this._ParameterType, "ParameterType", false, this._ParameterTypeInitialized);
                this._ParameterTypeInitialized = true;
                return this._ParameterType;
            }
            set {
                this.OnParameterTypeChanging(value);
                this.ReportPropertyChanging("ParameterType");
                this._ParameterType = this.SetValidValue(this._ParameterType, value, "ParameterType");
                this._ParameterTypeInitialized = true;
                this.ReportPropertyChanged("ParameterType");
                this.OnParameterTypeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private TypeSpecification _ParameterType;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _ParameterTypeInitialized;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnParameterTypeChanging(TypeSpecification value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnParameterTypeChanged();
        /// <summary>
        /// スキーマのプロパティ Mode にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Mode {
            get {
                return this._Mode;
            }
            set {
                this.OnModeChanging(value);
                this.ReportPropertyChanging("Mode");
                this._Mode = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("Mode");
                this.OnModeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Mode;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnModeChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnModeChanged();
        /// <summary>
        /// スキーマのプロパティ Default にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Default {
            get {
                return this._Default;
            }
            set {
                this.OnDefaultChanging(value);
                this.ReportPropertyChanging("Default");
                this._Default = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("Default");
                this.OnDefaultChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Default;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnDefaultChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnDefaultChanged();
        /// <summary>
        /// スキーマの Routine にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "RoutineParameter", "Routine")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public Routine Routine {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Routine>("Store.RoutineParameter", "Routine").Value;
            }
            set {
                ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Routine>("Store.RoutineParameter", "Routine").Value = value;
            }
        }
        /// <summary>
        /// スキーマの Routine にはコメントがありません。
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityReference<Routine> RoutineReference {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Routine>("Store.RoutineParameter", "Routine");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedReference<Routine>("Store.RoutineParameter", "Routine", value);
                }
            }
        }
    }
    /// <summary>
    /// スキーマの Store.Function にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "Function")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.ScalarFunction))]
    public abstract partial class Function : Routine {
        /// <summary>
        /// スキーマのプロパティ IsBuiltIn にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<bool> IsBuiltIn {
            get {
                return this._IsBuiltIn;
            }
            set {
                this.OnIsBuiltInChanging(value);
                this.ReportPropertyChanging("IsBuiltIn");
                this._IsBuiltIn = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsBuiltIn");
                this.OnIsBuiltInChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<bool> _IsBuiltIn;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsBuiltInChanging(global::System.Nullable<bool> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsBuiltInChanged();
        /// <summary>
        /// スキーマのプロパティ IsNiladic にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<bool> IsNiladic {
            get {
                return this._IsNiladic;
            }
            set {
                this.OnIsNiladicChanging(value);
                this.ReportPropertyChanging("IsNiladic");
                this._IsNiladic = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsNiladic");
                this.OnIsNiladicChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<bool> _IsNiladic;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsNiladicChanging(global::System.Nullable<bool> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsNiladicChanged();
    }
    /// <summary>
    /// スキーマの Store.ScalarFunction にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "ScalarFunction")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class ScalarFunction : Function {
        /// <summary>
        /// 新しい ScalarFunction オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        /// <param name="returnType">ReturnType の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static ScalarFunction CreateScalarFunction(string id, string name, TypeSpecification returnType) {
            ScalarFunction scalarFunction = new ScalarFunction();
            scalarFunction.Id = id;
            scalarFunction.Name = name;
            scalarFunction.ReturnType = StructuralObject.VerifyComplexObjectIsNotNull(returnType, "ReturnType");
            return scalarFunction;
        }
        /// <summary>
        /// スキーマのプロパティ ReturnType にはコメントがありません。
        /// </summary>
        [EdmComplexPropertyAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        [global::System.Xml.Serialization.XmlElement(IsNullable = true)]
        [global::System.Xml.Serialization.SoapElement(IsNullable = true)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public TypeSpecification ReturnType {
            get {
                this._ReturnType = this.GetValidValue(this._ReturnType, "ReturnType", false, this._ReturnTypeInitialized);
                this._ReturnTypeInitialized = true;
                return this._ReturnType;
            }
            set {
                this.OnReturnTypeChanging(value);
                this.ReportPropertyChanging("ReturnType");
                this._ReturnType = this.SetValidValue(this._ReturnType, value, "ReturnType");
                this._ReturnTypeInitialized = true;
                this.ReportPropertyChanged("ReturnType");
                this.OnReturnTypeChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private TypeSpecification _ReturnType;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _ReturnTypeInitialized;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnReturnTypeChanging(TypeSpecification value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnReturnTypeChanged();
        /// <summary>
        /// スキーマのプロパティ IsAggregate にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<bool> IsAggregate {
            get {
                return this._IsAggregate;
            }
            set {
                this.OnIsAggregateChanging(value);
                this.ReportPropertyChanging("IsAggregate");
                this._IsAggregate = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsAggregate");
                this.OnIsAggregateChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<bool> _IsAggregate;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsAggregateChanging(global::System.Nullable<bool> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsAggregateChanged();
    }
    /// <summary>
    /// スキーマの Store.Procedure にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "Procedure")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class Procedure : Routine {
        /// <summary>
        /// 新しい Procedure オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static Procedure CreateProcedure(string id, string name) {
            Procedure procedure = new Procedure();
            procedure.Id = id;
            procedure.Name = name;
            return procedure;
        }
    }
    /// <summary>
    /// スキーマの Store.Constraint にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "Constraint")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.TableOrViewColumnConstraint))]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.ForeignKeyConstraint))]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.CheckConstraint))]
    public abstract partial class Constraint : EntityObject {
        /// <summary>
        /// スキーマのプロパティ Id にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Id {
            get {
                return this._Id;
            }
            set {
                this.OnIdChanging(value);
                this.ReportPropertyChanging("Id");
                this._Id = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Id");
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Id;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanged();
        /// <summary>
        /// スキーマのプロパティ Name にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Name {
            get {
                return this._Name;
            }
            set {
                this.OnNameChanging(value);
                this.ReportPropertyChanging("Name");
                this._Name = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Name");
                this.OnNameChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Name;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnNameChanged();
        /// <summary>
        /// スキーマのプロパティ IsDeferrable にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public bool IsDeferrable {
            get {
                return this._IsDeferrable;
            }
            set {
                this.OnIsDeferrableChanging(value);
                this.ReportPropertyChanging("IsDeferrable");
                this._IsDeferrable = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsDeferrable");
                this.OnIsDeferrableChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _IsDeferrable;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsDeferrableChanging(bool value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsDeferrableChanged();
        /// <summary>
        /// スキーマのプロパティ IsInitiallyDeferred にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public bool IsInitiallyDeferred {
            get {
                return this._IsInitiallyDeferred;
            }
            set {
                this.OnIsInitiallyDeferredChanging(value);
                this.ReportPropertyChanging("IsInitiallyDeferred");
                this._IsInitiallyDeferred = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("IsInitiallyDeferred");
                this.OnIsInitiallyDeferredChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private bool _IsInitiallyDeferred;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsInitiallyDeferredChanging(bool value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIsInitiallyDeferredChanged();
        /// <summary>
        /// スキーマの Parent にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "TableOrViewConstraint", "Parent")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public TableOrView Parent {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<TableOrView>("Store.TableOrViewConstraint", "Parent").Value;
            }
            set {
                ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<TableOrView>("Store.TableOrViewConstraint", "Parent").Value = value;
            }
        }
        /// <summary>
        /// スキーマの Parent にはコメントがありません。
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityReference<TableOrView> ParentReference {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<TableOrView>("Store.TableOrViewConstraint", "Parent");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedReference<TableOrView>("Store.TableOrViewConstraint", "Parent", value);
                }
            }
        }
    }
    /// <summary>
    /// スキーマの Store.CheckConstraint にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "CheckConstraint")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class CheckConstraint : Constraint {
        /// <summary>
        /// 新しい CheckConstraint オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        /// <param name="isDeferrable">IsDeferrable の初期値。</param>
        /// <param name="isInitiallyDeferred">IsInitiallyDeferred の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static CheckConstraint CreateCheckConstraint(string id, string name, bool isDeferrable, bool isInitiallyDeferred) {
            CheckConstraint checkConstraint = new CheckConstraint();
            checkConstraint.Id = id;
            checkConstraint.Name = name;
            checkConstraint.IsDeferrable = isDeferrable;
            checkConstraint.IsInitiallyDeferred = isInitiallyDeferred;
            return checkConstraint;
        }
        /// <summary>
        /// スキーマのプロパティ Expression にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Expression {
            get {
                return this._Expression;
            }
            set {
                this.OnExpressionChanging(value);
                this.ReportPropertyChanging("Expression");
                this._Expression = StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("Expression");
                this.OnExpressionChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Expression;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnExpressionChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnExpressionChanged();
    }
    /// <summary>
    /// スキーマの Store.TableOrViewColumnConstraint にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "TableOrViewColumnConstraint")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.PrimaryKeyConstraint))]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::EdmGen06.UniqueConstraint))]
    public abstract partial class TableOrViewColumnConstraint : Constraint {
        /// <summary>
        /// スキーマの Columns にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "TableOrViewConstraintColumn", "Column")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityCollection<Column> Columns {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<Column>("Store.TableOrViewConstraintColumn", "Column");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<Column>("Store.TableOrViewConstraintColumn", "Column", value);
                }
            }
        }
    }
    /// <summary>
    /// スキーマの Store.PrimaryKeyConstraint にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "PrimaryKeyConstraint")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class PrimaryKeyConstraint : TableOrViewColumnConstraint {
        /// <summary>
        /// 新しい PrimaryKeyConstraint オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        /// <param name="isDeferrable">IsDeferrable の初期値。</param>
        /// <param name="isInitiallyDeferred">IsInitiallyDeferred の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static PrimaryKeyConstraint CreatePrimaryKeyConstraint(string id, string name, bool isDeferrable, bool isInitiallyDeferred) {
            PrimaryKeyConstraint primaryKeyConstraint = new PrimaryKeyConstraint();
            primaryKeyConstraint.Id = id;
            primaryKeyConstraint.Name = name;
            primaryKeyConstraint.IsDeferrable = isDeferrable;
            primaryKeyConstraint.IsInitiallyDeferred = isInitiallyDeferred;
            return primaryKeyConstraint;
        }
    }
    /// <summary>
    /// スキーマの Store.UniqueConstraint にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "UniqueConstraint")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class UniqueConstraint : TableOrViewColumnConstraint {
        /// <summary>
        /// 新しい UniqueConstraint オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        /// <param name="isDeferrable">IsDeferrable の初期値。</param>
        /// <param name="isInitiallyDeferred">IsInitiallyDeferred の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static UniqueConstraint CreateUniqueConstraint(string id, string name, bool isDeferrable, bool isInitiallyDeferred) {
            UniqueConstraint uniqueConstraint = new UniqueConstraint();
            uniqueConstraint.Id = id;
            uniqueConstraint.Name = name;
            uniqueConstraint.IsDeferrable = isDeferrable;
            uniqueConstraint.IsInitiallyDeferred = isInitiallyDeferred;
            return uniqueConstraint;
        }
    }
    /// <summary>
    /// スキーマの Store.ForeignKeyConstraint にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "ForeignKeyConstraint")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class ForeignKeyConstraint : Constraint {
        /// <summary>
        /// 新しい ForeignKeyConstraint オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="name">Name の初期値。</param>
        /// <param name="isDeferrable">IsDeferrable の初期値。</param>
        /// <param name="isInitiallyDeferred">IsInitiallyDeferred の初期値。</param>
        /// <param name="updateRule">UpdateRule の初期値。</param>
        /// <param name="deleteRule">DeleteRule の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static ForeignKeyConstraint CreateForeignKeyConstraint(string id, string name, bool isDeferrable, bool isInitiallyDeferred, string updateRule, string deleteRule) {
            ForeignKeyConstraint foreignKeyConstraint = new ForeignKeyConstraint();
            foreignKeyConstraint.Id = id;
            foreignKeyConstraint.Name = name;
            foreignKeyConstraint.IsDeferrable = isDeferrable;
            foreignKeyConstraint.IsInitiallyDeferred = isInitiallyDeferred;
            foreignKeyConstraint.UpdateRule = updateRule;
            foreignKeyConstraint.DeleteRule = deleteRule;
            return foreignKeyConstraint;
        }
        /// <summary>
        /// スキーマのプロパティ UpdateRule にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string UpdateRule {
            get {
                return this._UpdateRule;
            }
            set {
                this.OnUpdateRuleChanging(value);
                this.ReportPropertyChanging("UpdateRule");
                this._UpdateRule = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("UpdateRule");
                this.OnUpdateRuleChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _UpdateRule;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnUpdateRuleChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnUpdateRuleChanged();
        /// <summary>
        /// スキーマのプロパティ DeleteRule にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string DeleteRule {
            get {
                return this._DeleteRule;
            }
            set {
                this.OnDeleteRuleChanging(value);
                this.ReportPropertyChanging("DeleteRule");
                this._DeleteRule = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("DeleteRule");
                this.OnDeleteRuleChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _DeleteRule;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnDeleteRuleChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnDeleteRuleChanged();
        /// <summary>
        /// スキーマの ForeignKeys にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "ConstraintForeignKey", "ForeignKey")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityCollection<ForeignKey> ForeignKeys {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedCollection<ForeignKey>("Store.ConstraintForeignKey", "ForeignKey");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedCollection<ForeignKey>("Store.ConstraintForeignKey", "ForeignKey", value);
                }
            }
        }
    }
    /// <summary>
    /// スキーマの Store.ForeignKey にはコメントがありません。
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [EdmEntityTypeAttribute(NamespaceName = "Store", Name = "ForeignKey")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public partial class ForeignKey : EntityObject {
        /// <summary>
        /// 新しい ForeignKey オブジェクトを作成します。
        /// </summary>
        /// <param name="id">Id の初期値。</param>
        /// <param name="ordinal">Ordinal の初期値。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static ForeignKey CreateForeignKey(string id, int ordinal) {
            ForeignKey foreignKey = new ForeignKey();
            foreignKey.Id = id;
            foreignKey.Ordinal = ordinal;
            return foreignKey;
        }
        /// <summary>
        /// スキーマのプロパティ Id にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string Id {
            get {
                return this._Id;
            }
            set {
                this.OnIdChanging(value);
                this.ReportPropertyChanging("Id");
                this._Id = StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("Id");
                this.OnIdChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _Id;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnIdChanged();
        /// <summary>
        /// スキーマのプロパティ Ordinal にはコメントがありません。
        /// </summary>
        [EdmScalarPropertyAttribute(IsNullable = false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public int Ordinal {
            get {
                return this._Ordinal;
            }
            set {
                this.OnOrdinalChanging(value);
                this.ReportPropertyChanging("Ordinal");
                this._Ordinal = StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Ordinal");
                this.OnOrdinalChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private int _Ordinal;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOrdinalChanging(int value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOrdinalChanged();
        /// <summary>
        /// スキーマの Constraint にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "ConstraintForeignKey", "Constraint")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public ForeignKeyConstraint Constraint {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<ForeignKeyConstraint>("Store.ConstraintForeignKey", "Constraint").Value;
            }
            set {
                ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<ForeignKeyConstraint>("Store.ConstraintForeignKey", "Constraint").Value = value;
            }
        }
        /// <summary>
        /// スキーマの Constraint にはコメントがありません。
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityReference<ForeignKeyConstraint> ConstraintReference {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<ForeignKeyConstraint>("Store.ConstraintForeignKey", "Constraint");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedReference<ForeignKeyConstraint>("Store.ConstraintForeignKey", "Constraint", value);
                }
            }
        }
        /// <summary>
        /// スキーマの FromColumn にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "FromForeignKeyColumn", "Column")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public Column FromColumn {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Column>("Store.FromForeignKeyColumn", "Column").Value;
            }
            set {
                ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Column>("Store.FromForeignKeyColumn", "Column").Value = value;
            }
        }
        /// <summary>
        /// スキーマの FromColumn にはコメントがありません。
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityReference<Column> FromColumnReference {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Column>("Store.FromForeignKeyColumn", "Column");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedReference<Column>("Store.FromForeignKeyColumn", "Column", value);
                }
            }
        }
        /// <summary>
        /// スキーマの ToColumn にはコメントがありません。
        /// </summary>
        [EdmRelationshipNavigationPropertyAttribute("Store", "ToForeignKeyColumn", "Column")]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Xml.Serialization.XmlIgnoreAttribute()]
        [global::System.Xml.Serialization.SoapIgnoreAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public Column ToColumn {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Column>("Store.ToForeignKeyColumn", "Column").Value;
            }
            set {
                ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Column>("Store.ToForeignKeyColumn", "Column").Value = value;
            }
        }
        /// <summary>
        /// スキーマの ToColumn にはコメントがありません。
        /// </summary>
        [global::System.ComponentModel.BrowsableAttribute(false)]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public EntityReference<Column> ToColumnReference {
            get {
                return ((IEntityWithRelationships)(this)).RelationshipManager.GetRelatedReference<Column>("Store.ToForeignKeyColumn", "Column");
            }
            set {
                if ((value != null)) {
                    ((IEntityWithRelationships)(this)).RelationshipManager.InitializeRelatedReference<Column>("Store.ToForeignKeyColumn", "Column", value);
                }
            }
        }
    }
}

