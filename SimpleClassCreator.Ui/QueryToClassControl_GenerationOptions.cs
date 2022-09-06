using SimpleClassCreator.Lib;
using SimpleClassCreator.Lib.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SimpleClassCreator.Ui.Helpers;
using B = SimpleClassCreator.Ui.UserControlExtensions;

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
            obj.OverwriteExistingFiles = CbReplaceExistingFiles.IsCheckedAndEnabled();
            obj.Namespace = TxtNamespaceName.Text;

            if (TxtClassEntityName.IsTextInvalid("Class name cannot be empty."))
                return null;

            obj.TableQuery = _svcNameFormat.ParseTableName(TxtSourceSqlText.Text);
            obj.ClassOptions = GetClassOptions();
            obj.ClassServices = GetClassServices();
            obj.ClassRepositories = GetClassRepositories();

            return obj;
        }

        private QueryToClassParameters CommonValidation()
        {
            var obj = new QueryToClassParameters();

            var con = ConnectionStringCb.CurrentConnection;

            if (!con.Verified && !ConnectionStringCb.TestConnectionString(true))
                return null;

            obj.ConnectionString = con.ConnectionString;

            obj.SourceSqlType = GetSourceType();

            if (TxtSourceSqlText.IsTextInvalid(obj.SourceSqlType + " cannot be empty."))
                return null;

            obj.SourceSqlText = TxtSourceSqlText.Text;
            obj.SaveAsFile = CbSaveFileOnGeneration.IsChecked();

            if (obj.SaveAsFile)
            {
                const string s = "If saving file on generation, then {0} cannot be empty.";

                if (TxtPath.IsTextInvalid(string.Format(s, "Path")))
                    return null;

                if (TxtFileName.IsTextInvalid(string.Format(s, "File name")))
                    return null;
            }

            obj.FilePath = TxtPath.Text;
            obj.Filename = TxtFileName.Text;

            if (_classCheckBoxGroup.HasTickedCheckBox())
                return obj;

            B.ShowWarningMessage("You must select at least one construct for generation. None is not an option.");

            return null;
        }

        private ClassOptions GetClassOptions()
        {
            var c = new ClassOptions
            {
                EntityName = TxtEntityName.Text,
                ClassEntityName = TxtClassEntityName.Text,
                GenerateEntity = CbClassEntity.IsChecked(),
                GenerateEntityIEquatable = CbClassEntityIEquatable.IsChecked(),
                GenerateEntityIComparable = CbClassEntityIComparable.IsChecked(),
                GenerateEntityEqualityComparer = CbClassEntityEqualityComparer.IsChecked(),
                GenerateInterface = CbClassInterface.IsChecked(),
                GenerateModel = CbClassModel.IsChecked(),
                ClassModelName = TxtClassModelName.Text
            };

            return c;
        }

        private ClassServices GetClassServices()
        {
            var e = ClassServices.None;

            foreach (var kvp in _serviceToCheckBoxMap)
            {
                if (!kvp.Value.IsChecked()) continue;

                e |= kvp.Key;
            }

            return e;
        }

        private ClassRepositories GetClassRepositories()
        {
            var e = ClassRepositories.None;

            foreach (var kvp in _repositoryToCheckBoxMap)
            {
                if (!kvp.Value.IsChecked()) continue;

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
                { ClassServices.SerializeCsv, CbSerializeCsv },
                { ClassServices.SerializeJson, CbSerializeJson },
                { ClassServices.RepoStatic, CbRepoStatic },
                { ClassServices.RepoDapper, CbRepoDapper },
                { ClassServices.RepoEfFluentApi, CbRepoEfFluentApi }
            };

            return dict;
        }

        private Dictionary<ClassRepositories, CheckBox> GetRepositoryToCheckBoxMap()
        {
            var dict = new Dictionary<ClassRepositories, CheckBox>
            {
                { ClassRepositories.StaticStatements, CbRepoStatic },
                { ClassRepositories.Dapper, CbRepoDapper }
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

        private void BtnDynamicStatements_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: This is good for now, but might want to create a simple HTML page for this and display it as part of a web browser component
            var content = 
@"There is no point in providing dynamic generation or bulk copy options because the code is 
so generic it will not likely change for most objects. Therefore I have a separate repository 
for boiler plate starter code where I am maintaining this kind of code.

You can find it here: 
https://github.com/dyslexicanaboko/code-snippets/tree/develop/Visual%20C%23/BasicDataLayers";

            _resultWindowManager.Show("Basic data layers", content);
        }
    }
}
