namespace LazySQL.System
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
