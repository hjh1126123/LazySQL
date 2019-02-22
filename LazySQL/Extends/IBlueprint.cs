namespace LazySQL.Extends
{
    public abstract class IBlueprint
    {
        public string Field { get; private set; }
        protected void SetField(string field)
        {
            Field = field;
        }
    }
}
