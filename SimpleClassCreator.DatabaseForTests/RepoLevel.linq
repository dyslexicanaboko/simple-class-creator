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

	lst.Dump();
}

public class Numbers
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