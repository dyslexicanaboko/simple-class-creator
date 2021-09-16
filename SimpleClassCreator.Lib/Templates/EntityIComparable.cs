{{Namespaces}}

namespace {{Namespace}}
{
	public partial class {{ClassName}}
		: IComparable
	{
		public int CompareTo(object obj)
		{
			//This is starter code - it is meant to be changed appropriately
			var n = ({{ClassName}})obj;

			if ({{Property1}} == n.{{Property1}})
			{
				return 0;
			}

			if ({{Property1}} > n.{{Property1}})
			{
				return 1;
			}

			//if ({{Property1}} < n.{{Property1}})
			return -1;
		}
	}
}
