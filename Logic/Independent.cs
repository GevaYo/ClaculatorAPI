using System.Text.Json.Serialization;

namespace ClaculatorAPI.Logic
{
    public class Independent
    {
        public List<int> Arguments { get; set; }
        public string Operation { get; set; }

        public void validateInputParameters()
        {
            string lowerCaseOperation = Operation.ToLower();
            KeyValuePair<string, int> op;
            int opIndex;

            if (!Utils.ValidOperations.Keys.Contains(lowerCaseOperation))
            {
                throw new Exception($" Error: unknown operation: {Operation}");
            }
            else
            {
                opIndex = Utils.ValidOperations.Keys.ToList().IndexOf(lowerCaseOperation);
                op = Utils.ValidOperations.ElementAt(opIndex);

                if (op.Value < Arguments.Count)
                {
                    throw new Exception($" Error: Too many arguments to perform the operation {Operation}");
                }
                else if (op.Value > Arguments.Count)
                {
                    throw new Exception($" Error: Not enough arguments to perform the operation {Operation}");
                }
            }
        }

    }
}
    



        
