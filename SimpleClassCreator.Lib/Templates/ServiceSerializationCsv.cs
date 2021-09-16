//https://www.nuget.org/packages/CsvHelper/
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace {{Namespace}}
{
    public partial class SerializationService
    {
		private CsvConfiguration GetCsvConfig()
		{
			var config = new CsvConfiguration(CultureInfo.CurrentCulture)
			{
				Delimiter = "|",
				HasHeaderRecord = false
			};
			
			return config;
		}

		public string ToCsv(IEnumerable<{{ClassName}}> input)
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

		public IEnumerable<{{ClassName}}> FromCsv(string csv)
		{
			using (var reader = new StringReader(csv))
			{
				using (var csvReader = new CsvReader(reader, GetCsvConfig()))
				{
					var records = csvReader.GetRecords<{{ClassName}}>();
					
					//Has to be forced to a List otherwise a NullReference Exception will occur
					var lst = records.ToList();
					
					return lst;
				}
			}
		}
    }
}
