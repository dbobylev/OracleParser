using DataBaseRepository;
using DataBaseRepository.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleParser.Model.PackageModel
{
    public class Package
    {
        [JsonProperty]
        public List<PackageElement> elements { get; private set; }

        public Package(ParsedPackagePart spec, ParsedPackagePart body, RepositoryPackage repositoryPackage)
        {
            elements = new List<PackageElement>();

            // Обрабатываем методы из тела пакета
            for (int i = 0; i < body.Procedures.Count; i++)
            {
                var method = body.Procedures[i];

                var nameIdentifierPart = method.NameIdentifierPart;
                var methodName = DBRep.Instance().GetWordOfFile(repositoryPackage.BodyRepFullPath, nameIdentifierPart.LineBeg, nameIdentifierPart.ColumnBeg, nameIdentifierPart.ColumnEnd);

                var element = new PackageElement(methodName, ePackageElementType.Method);
                element.AddPosition(ePackageElementDefinitionType.BodyFull, method.Position());

                // Фиксируем часть спецификации в теле
                element.AddPosition(ePackageElementDefinitionType.BodyDeclaration, method.DeclarationPart);

                // Позиция названия метода
                element.AddPosition(ePackageElementDefinitionType.NameIdentifier, nameIdentifierPart);

                // Ищем определение метода в спецификации
                var specMethod = spec.Procedures.FirstOrDefault(x => x.Name == method.Name);
                if (specMethod != null)
                    element.AddPosition(ePackageElementDefinitionType.Spec, specMethod.Position());

                element.Parametres.AddRange(method.Parameters);
                element.Links.AddRange(method.Elements);
                
                elements.Add(element);
            }

            SetVariable(spec, ePackageElementDefinitionType.Spec, repositoryPackage.SpecRepFullPath);
            SetVariable(body, ePackageElementDefinitionType.BodyFull, repositoryPackage.BodyRepFullPath);
        }

        private void SetVariable(ParsedPackagePart part, ePackageElementDefinitionType positionType, string filepath)
        {
            for (int i = 0; i < part.Variables.Count; i++)
            {
                var Variable = part.Variables[i];

                var nameIdentifierPart = Variable.NameIdentifierPart;
                var VariableName = DBRep.Instance().GetWordOfFile(filepath, nameIdentifierPart.LineBeg, nameIdentifierPart.ColumnBeg, nameIdentifierPart.ColumnEnd);

                var element = new PackageElement(VariableName, ePackageElementType.Variable);
                element.AddPosition(positionType, Variable.Position());

                elements.Add(element);
            }
        }

        public Package()
        {

        }
    }
}
