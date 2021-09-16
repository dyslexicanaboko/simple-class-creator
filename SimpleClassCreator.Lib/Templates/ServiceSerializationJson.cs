//https://www.nuget.org/packages/Newtonsoft.Json/
using Newtonsoft.Json;

namespace {{Namespace}}
{
    public partial class SerializationService
    {
		public string ToJson(IEnumerable<{{ClassName}}> input) => JsonConvert.SerializeObject(input);

		public IEnumerable<{{ClassName}}> FromJson(string json) => JsonConvert.DeserializeObject<IEnumerable<{{ClassName}}>>(json);
    }
}
