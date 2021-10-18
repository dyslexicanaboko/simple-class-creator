<Query Kind="Program">
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

void Main()
{
	var lst = new List<Numbers>
	{
		new Numbers(6, 6, 6),
		new Numbers(1, 1, 1),
		new Numbers(2, 2, 2),
		new Numbers(5, 5, 5),
		new Numbers(1, 1, 1),
		new Numbers(3, 3, 3),
		new Numbers(4, 4, 4)
	};

	//	lst.Distinct().Dump();
	//
	//	lst.Sort();
	//	
	//	lst.Dump();

	//var n1 = new Numbers(1, 1, 1);
	//var n2 = new Numbers(1, 1, 1);
	//
	//n1.CompareTo(n2);

	var csv = ToCsv(lst).Dump();
	FromCsv(csv).Dump();
	
	var json = ToJson(lst).Dump();
	FromJson(json).Dump();
}

private CsvConfiguration GetCsvConfig()
{
	var config = new CsvConfiguration(CultureInfo.CurrentCulture)
	{
		Delimiter = "|",
		HasHeaderRecord = false
	};
	
	return config;
}

public string ToCsv(IEnumerable<Numbers> input)
{
	using (var writer = new StringWriter())
	{
		using (var csvWriter = new CsvWriter(writer, GetCsvConfig()))
		{
			csvWriter.WriteRecords(input);
		}
		
		return writer.ToString();
	}
}

public IEnumerable<Numbers> FromCsv(string csv)
{
	using (var reader = new StringReader(csv))
	{
		using (var csvReader = new CsvReader(reader, GetCsvConfig()))
		{
			var records = csvReader.GetRecords<Numbers>();
			
			//Has to be forced to a List otherwise a NullReference Exception will occur
			var lst = records.ToList();
			
			return lst;
		}
	}
}

public string ToJson(IEnumerable<Numbers> input) => JsonConvert.SerializeObject(input);

public IEnumerable<Numbers> FromJson(string json) => JsonConvert.DeserializeObject<IEnumerable<Numbers>>(json);

public partial class Numbers
{
	public Numbers()
	{
		
	}
	
	public Numbers(int n1, int n2, int n3)
	{
		Number1 = n1;
		Number2 = n2;
		Number3 = n3;
	}
	
	public int Number1 { get; set; }

	public int Number2 { get; set; }

	public int Number3 { get; set; }

	public override string ToString() => $"{Number1}, {Number2}, {Number3}";
}

public partial class Numbers : IEquatable<Numbers>
{
	public override bool Equals(object obj) => this.Equals(obj as Numbers);

	public bool Equals(Numbers p)
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
			Number1 == p.Number1 &&
			Number2 == p.Number2 &&
			Number3 == p.Number3;
	}

	public override int GetHashCode() => (Number1, Number2, Number3).GetHashCode();

	public static bool operator ==(Numbers lhs, Numbers rhs)
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

	public static bool operator !=(Numbers lhs, Numbers rhs) => !(lhs == rhs);
}

public partial class Numbers : IComparable
{
	public int CompareTo(object obj)
	{
		//This is starter code - it is meant to be changed appropriately
		var n = (Numbers)obj;

		if (Number1 == n.Number1)
		{
			return 0;
		}

		if (Number1 > n.Number1)
		{
			return 1;
		}

		//if (Number1 < n.Number1)
		return -1;
	}
}

public class NumbersEqualityComparer : EqualityComparer<Numbers>
{
	public override bool Equals(Numbers x, Numbers y)
	{
		return
			x.Number1 == y.Number1 &&
			x.Number2 == y.Number2 &&
			x.Number3 == y.Number3;
	}

	public override int GetHashCode([DisallowNull] Numbers obj)
	{
		return (obj.Number1, obj.Number2, obj.Number3).GetHashCode();
	}
}