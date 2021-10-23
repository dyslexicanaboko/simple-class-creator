{{Namespaces}}

namespace {{Namespace}}
{
	public class {{ClassName}}EqualityComparer
		: EqualityComparer<{{ClassName}}>
	{
		public override bool Equals({{ClassName}} x, {{ClassName}} y)
		{
			return
{{PropertiesEquals}};
		}

		public override int GetHashCode([DisallowNull] {{ClassName}} obj) => (
{{PropertiesHashCode}}
        ).GetHashCode();	
	}
}