namespace LazySQL
{
    using System;
    public enum OperationstType
    {
        Operate,
        If
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class DBAttribute : Attribute
    {
        private bool key;

        private string tableName;
        private string dataType;
        private string conditionType;
        private string conditionTemplate;
        private string targetData;
        private string defaultData;
        private string describe;

        private OperationstType[] _ExcludeCertainOperations;



        public bool Key { get => key; set => key = value; }



        public string TableName { get => tableName; set => tableName = value; }
        public string DataType { get => dataType; set => dataType = value; }
        public string ConditionType { get => conditionType; set => conditionType = value; }
        public string ConditionTemplate { get => conditionTemplate; set => conditionTemplate = value; }
        public string TargetData { get => targetData; set => targetData = value; }
        public string DefaultData { get => defaultData; set => defaultData = value; }
        public string Describe { get => describe; set => describe = value; }

        public OperationstType[] ExcludeCertainOperations { get => _ExcludeCertainOperations; set => _ExcludeCertainOperations = value; }

    }
}
