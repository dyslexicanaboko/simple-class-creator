using System.Collections.Generic;
using System.Windows.Controls;
using SimpleClassCreator.Lib;
using SimpleClassCreator.Lib.Models;
using B = SimpleClassCreator.Ui.UserControlBase;

namespace SimpleClassCreator.Ui
{
    /// <summary>
    /// Any code related to obtaining parameters will live in this partial for the sake of legibility.
    /// </summary>
    public partial class QueryToClassControl
    {
        private QueryToClassParameters GetParameters()
        {
            var obj = CommonValidation();

            //Check if the common validation failed
            if (obj == null) return null;

            obj.LanguageType = CodeType.CSharp;
            obj.OverwriteExistingFiles = B.IsCheckedAndEnabled(CbReplaceExistingFiles);
            obj.Namespace = TxtNamespaceName.Text;

            if (B.IsTextInvalid(TxtClassEntityName, "Class name cannot be empty."))
                return null;

            obj.TableQuery = _svcClass.ParseTableName(TxtSourceSqlText.Text);
            obj.ClassOptions = GetClassOptions();
            obj.ClassServices = GetClassServices();

            return obj;
        }

        private QueryToClassParameters CommonValidation()
        {
            var obj = new QueryToClassParameters();

            var con = CurrentConnection;

            if (!con.Verified && !TestConnectionString(true))
                return null;

            obj.ConnectionString = CurrentConnection.ConnectionString;

            obj.SourceSqlType = GetSourceType();

            if (B.IsTextInvalid(TxtSourceSqlText, obj.SourceSqlType + " cannot be empty."))
                return null;

            obj.SourceSqlText = TxtSourceSqlText.Text;
            obj.SaveAsFile = B.IsChecked(CbSaveFileOnGeneration);

            if (obj.SaveAsFile)
            {
                const string s = "If saving file on generation, then {0} cannot be empty.";

                if (B.IsTextInvalid(TxtPath, string.Format(s, "Path")))
                    return null;

                if (B.IsTextInvalid(TxtFileName, string.Format(s, "File name")))
                    return null;
            }

            obj.FilePath = TxtPath.Text;
            obj.Filename = TxtFileName.Text;

            if (_classCheckBoxGroup.HasTickedCheckBox())
                return obj;
            
            B.Warning("You must select at least one construct for generation. None is not an option.");

            return null;
        }

        private ClassOptions GetClassOptions()
        {
            var c = new ClassOptions
            {
                EntityName = TxtClassEntityName.Text,
                GenerateEntity = B.IsChecked(CbClassEntity),
                GenerateEntityIEquatable = B.IsChecked(CbClassEntityIEquatable),
                GenerateEntityIComparable = B.IsChecked(CbClassEntityIComparable),
                GenerateInterface = B.IsChecked(CbClassInterface),
                GenerateModel = B.IsChecked(CbClassModel),
                ModelName = TxtClassModelName.Text
            };

            return c;
        }

        private ClassServices GetClassServices()
        {
            var e = ClassServices.None;

            foreach (var kvp in _serviceToCheckBoxMap)
            {
                if (!B.IsChecked(kvp.Value)) continue;

                e |= kvp.Key;
            }

            return e;
        }

        private Dictionary<ClassServices, CheckBox> GetServiceToCheckBoxMap()
        {
            var dict = new Dictionary<ClassServices, CheckBox>
            {
                { ClassServices.CloneEntityToModel, CbCloneEntityToModel },
                { ClassServices.CloneModelToEntity, CbCloneModelToEntity },
                { ClassServices.CloneInterfaceToEntity, CbCloneInterfaceToEntity },
                { ClassServices.CloneInterfaceToModel, CbCloneInterfaceToModel },
                { ClassServices.SerializeToCsv, CbSerializeToCsv },
                { ClassServices.SerializeFromCsv, CbSerializeFromCsv },
                { ClassServices.SerializeToJson, CbSerializeToJson },
                { ClassServices.SerializeFromJson, CbSerializeFromJson },
                { ClassServices.RepoStatic, CbRepoStatic },
                { ClassServices.RepoDynamic, CbRepoDynamic },
                { ClassServices.RepoBulkCopy, CbRepoBulkCopy },
                { ClassServices.RepoDapper, CbRepoDapper },
                { ClassServices.RepoEfFluentApi, CbRepoEfFluentApi }
            };

            return dict;
        }

        private CheckBoxGroup GetCheckBoxGroup()
        {
            var cbg = new CheckBoxGroup();
            cbg.Add(CbClassEntity);
            cbg.Add(CbClassModel);
            cbg.Add(CbClassInterface);

            return cbg;
        }
    }
}
