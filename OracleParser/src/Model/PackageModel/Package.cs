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

        public Package(ParsedPackagePart spec, ParsedPackagePart body)
        {
            elements = new List<PackageElement>();

            // Обрабатываем методы из тела пакета
            for (int i = 0; i < body.Procedures.Count; i++)
            {
                var method = body.Procedures[i];
                var methodName = method.Name;

                var element = new PackageElement(methodName, ePackageElementType.Method);
                element.AddPosition(ePackageElementDefinitionType.BodyFull, method.Position());

                // Фиксируем часть спецификации в теле
                element.AddPosition(ePackageElementDefinitionType.BodyDeclaration, method.DeclarationPart);

                // Ищем определение метода в спецификации
                var specMethod = spec.Procedures.FirstOrDefault(x => x.Name == methodName);
                if (specMethod != null)
                    element.AddPosition(ePackageElementDefinitionType.Spec, specMethod.Position());

                element.Parametres.AddRange(method.Parameters);
                element.Links.AddRange(method.Elements);
                
                elements.Add(element);
            }

            SetVariable(spec, ePackageElementDefinitionType.Spec);
            SetVariable(body, ePackageElementDefinitionType.BodyFull);
        }

        private void SetVariable(ParsedPackagePart part, ePackageElementDefinitionType positionType)
        {
            for (int i = 0; i < part.Variables.Count; i++)
            {
                var Variable = part.Variables[i];
                var VariableName = Variable.Name;

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
