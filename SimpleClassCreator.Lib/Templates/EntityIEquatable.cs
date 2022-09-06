{{Namespaces}}

namespace {{Namespace}}
{
	public partial class {{ClassName}} 
		: IEquatable <{{ClassName}}>
	{
		public override bool Equals(object obj) => this.Equals(obj as {{ClassName}});

		public bool Equals({{ClassName}} p)
		{
			if (p is null)
			{
				return false;
			}

			// Optimization for a common success case.
			if (object.ReferenceEquals(this, p))
			{
				return true;
			}

			// If run-time types are not exactly the same, return false.
			if (this.GetType() != p.GetType())
			{
				return false;
			}

			return
{{PropertiesEquals}};
		}

		//Default sum of parts used
		public override int GetHashCode() =>
{{PropertiesHashCode}};

		public static bool operator ==({{ClassName}} lhs, {{ClassName}} rhs)
		{
			if (lhs is null)
			{
				if (rhs is null)
				{
					return true;
				}

				// Only the left side is null.
				return false;
			}

			// Equals handles case of null on right side.
			return lhs.Equals(rhs);
		}

		public static bool operator !=({{ClassName}} lhs, {{ClassName}} rhs) => !(lhs == rhs);
	}
}