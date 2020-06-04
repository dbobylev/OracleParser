using DataBaseRepository;
using DataBaseRepository.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                var methodName = DBRep.Instance().GetWordInFile(repositoryPackage.BodyRepFullPath, nameIdentifierPart.LineBeg, nameIdentifierPart.ColumnBeg, nameIdentifierPart.ColumnEnd);

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

            SetObject(spec, ePackageElementDefinitionType.Spec, repositoryPackage.SpecRepFullPath);
            SetObject(body, ePackageElementDefinitionType.BodyFull, repositoryPackage.BodyRepFullPath);

            UpdateBeginLine(repositoryPackage);
        }

        private void SetObject(ParsedPackagePart part, ePackageElementDefinitionType positionType, string filepath)
        {
            var objs = part.Objects;
            if (positionType == ePackageElementDefinitionType.Spec)
                objs = objs.Where(x => x.GetType() != typeof(ParsedMethod)).ToList();

            for (int i = 0; i < objs.Count; i++)
            {
                var obj = objs[i];

                var nameIdentifierPart = obj.NameIdentifierPart;
                var VariableName = DBRep.Instance().GetWordInFile(filepath, nameIdentifierPart.LineBeg, nameIdentifierPart.ColumnBeg, nameIdentifierPart.ColumnEnd);

                var element = new PackageElement(VariableName, obj.GetType().GetCustomAttribute<PackageElementTypeAttribute>().ElementType);
                element.AddPosition(positionType, obj.Position());

                elements.Add(element);
            }
        }

        private void SetVariable(ParsedPackagePart part, ePackageElementDefinitionType positionType, string filepath)
        {
            for (int i = 0; i < part.Variables.Count; i++)
            {
                var Variable = part.Variables[i];

                var nameIdentifierPart = Variable.NameIdentifierPart;
                var VariableName = DBRep.Instance().GetWordInFile(filepath, nameIdentifierPart.LineBeg, nameIdentifierPart.ColumnBeg, nameIdentifierPart.ColumnEnd);

                var element = new PackageElement(VariableName, ePackageElementType.Variable);
                element.AddPosition(positionType, Variable.Position());

                elements.Add(element);
            }
        }

        public Package()
        {

        }

        /// <summary>
        /// Переносим начало позиции участка кода что бы захыватить первичный комментарий
        /// </summary>
        /// <param name="repositoryPackage"></param>
        private void UpdateBeginLine(RepositoryPackage repositoryPackage)
        {
            Action<ePackageElementDefinitionType, string> run = (t, path) =>
            {
                var z = elements.Where(x => x.Position.Keys.Contains(t));
                var q = (new int[] { 1 })
                    .Concat(z.Select(x => x.Position[t])
                             .SelectMany(x => new int[] { x.LineBeg, x.LineEnd })
                             .OrderBy(x => x)
                             .ToArray())
                    .ToArray();

                var NewLine = new Dictionary<int, int>();
                for (int i = 1; i < q.Count(); i += 2)
                    NewLine.Add(q[i], DBRep.Instance().GetEmptyLine(path, q[i], q[i - 1]));

                Seri.Log.Verbose(t.ToString() + ": " + string.Join(",", NewLine.Select(x => $"({x.Key} - {x.Value})")));

                foreach (var item in z)
                {
                    var x = item.Position[t];
                    var NewLineBeg = NewLine[x.LineBeg];
                    x.UpdateLiuneBeg(NewLineBeg);
                    if (t == ePackageElementDefinitionType.BodyFull && item.Position.Keys.Contains(ePackageElementDefinitionType.BodyDeclaration))
                        item.Position[ePackageElementDefinitionType.BodyDeclaration].UpdateLiuneBeg(NewLineBeg);
                }
            };
            run(ePackageElementDefinitionType.Spec, repositoryPackage.SpecRepFullPath);
            run(ePackageElementDefinitionType.BodyFull, repositoryPackage.BodyRepFullPath);
        }
    }
}
