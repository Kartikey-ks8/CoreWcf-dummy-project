using CoreWCF;
using System.Runtime.Serialization;

namespace CoreWCFService2
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        public string GetData(int value);
    }

    public class Service : IService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
    }
}
