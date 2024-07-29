using System.Xml.Linq;

namespace ClaculatorAPI.Logic
{
    public sealed class ArgsStack
    {
        static ArgsStack instance = new ArgsStack();

        Stack<int> stack = new Stack<int>();

        bool IsBinaryOp { get; set; } 
        static ArgsStack()
        {
        }

        private ArgsStack()
        {
        }

        public static ArgsStack Instance
        { get { return instance; } }
        public Stack<int> Stack { get { return stack; } }

        public void pushArgsToStack(Arguments Parameters)
        {
            foreach (int i in Parameters.arguments)
            {
                instance.stack.Push(i);
            }
        }

        public void validateOperation(string operation)
        {
            string lowerCaseOperation = operation.ToLower();
            KeyValuePair<string, int> op;
            int stackSize = ArgsStack.Instance.stack.Count;
            int opIndex;

            if (!Utils.ValidOperations.Keys.Contains(lowerCaseOperation))
            {
                throw new Exception($" Error: unknown operation: {operation}");
            }
            else
            {
                opIndex = Utils.ValidOperations.Keys.ToList().IndexOf(lowerCaseOperation);
                op = Utils.ValidOperations.ElementAt(opIndex);

                if (op.Value > stackSize)
                {
                    throw new Exception($" Error: cannot implement operation {operation}. It requires {op.Value} arguments and the stack has only {stackSize} arguments");
                }

                IsBinaryOp = op.Value > 1;

            }
        }
        public List<int> popArgsBasedOnOperation()
        {
            List<int> args = new List<int>();
            args.Add(stack.Pop());

            if (IsBinaryOp)
            {
                args.Add(stack.Pop());
            }
            return args;
        }

        public void popArgsBasedOnUsersChoice(int numToPop)
        {
            if(numToPop >stack.Count) 
            {
                throw new Exception($"Error: cannot remove {numToPop} from the stack. It has only {Stack.Count} arguments");
            }
            else
            {
                for(int i=0; i<numToPop;++i)
                {
                    stack.Pop();
                }
            }
        }

        public List<int> getStackArgsInAList()
        {
            List<int> args = new List<int>();

            foreach(int i in stack) 
            {
                args.Add(i);
            }
            return args;
        }

    }
}
