{{Namespaces}}

namespace {{Namespace}}
{
    public class CloningService
    {
		public {{EntityName}} ToEntity({{ModelName}} model)
		{
			var entity = new {{EntityName}}();
{{PropertiesModelToEntity}}
			
			return entity;
		}
		
		public {{ModelName}} ToModel({{EntityName}} entity)
		{
			var model = new {{ModelName}}();
{{PropertiesEntityToModel}}
			
			return model;
		}
		
		public {{EntityName}} ToEntity({{InterfaceName}} target)
		{
			var entity = new {{EntityName}}();
{{PropertiesInterfaceToEntity}}
			
			return entity;
		}
		
		public {{ModelName}} ToModel({{InterfaceName}} target)
		{
			var model = new {{ModelName}}();
{{PropertiesInterfaceToModel}}
			
			return model;
		}
    }
}
