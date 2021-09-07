	public override bool Equals(object obj)
	{
		return Equals(obj as %className%);
	}

	public bool Equals(%className% other)
	{
		var o = other;

		var c = 
%equals%;

		return c;
	}

	public override int GetHashCode()
	{
		//You are responsible for testing this Hash Code method for uniqueness. 
		//Hashing is a very sensitive subject and this is a poor stock implementation.
		var hc = 
%hashCode%;
		
		return hc;
	}

	public static bool operator ==(%className% lhs, %className% rhs)
	{
	    return lhs?.Equals(rhs) ?? ReferenceEquals(rhs, null);
	}

	public static bool operator !=(%className% lhs, %className% rhs)
	{
		return !(lhs == rhs);
	}