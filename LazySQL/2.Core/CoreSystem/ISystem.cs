namespace LazySQL.Core.CoreSystem
{
    public class ISystem
    {
        protected SystemMediator systemMediator { get; private set; }
        public ISystem(SystemMediator systemMediator)
        {
            this.systemMediator = systemMediator;
        }
    }
}
