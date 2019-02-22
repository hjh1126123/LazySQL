using LazySQL.Core.Tools.Modules;

namespace LazySQL.Core.Tools
{
    public class ToolManager
    {
        private static ToolManager _instance;
        public static ToolManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ToolManager();

                return _instance;
            }
        }

        #region 车间工具

        public AssignmentTool AssignmentTool { get; private set; }
        public CircleTool CircleTool { get; private set; }
        public ConditionTool ConditionTool { get; private set; }
        public InitializeTool InitializeTool { get; private set; }
        public InvokeTool InvokeTool { get; private set; }
        public ReturnTool ReturnTool { get; private set; }
        public SecurityTool SecurityTool { get; private set; }

        #endregion

        private ToolManager()
        {
            AssignmentTool = new AssignmentTool();
            CircleTool = new CircleTool();
            ConditionTool = new ConditionTool();
            InitializeTool = new InitializeTool();
            InvokeTool = new InvokeTool();
            ReturnTool = new ReturnTool();
            SecurityTool = new SecurityTool();
        }
    }
}
