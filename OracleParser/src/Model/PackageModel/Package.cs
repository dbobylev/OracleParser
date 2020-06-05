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

            SetObject(body, ePackageElementDefinitionType.BodyFull, repositoryPackage.BodyRepFullPath);
            SetObject(spec, ePackageElementDefinitionType.Spec, repositoryPackage.SpecRepFullPath);

            UpdateBeginLine(repositoryPackage);
        }

        private void SetObject(ParsedPackagePart part, ePackageElementDefinitionType positionType, string filepath)
        {
            var objs = part.Objects;
            for (int i = 0; i < objs.Count; i++)
            {
                var obj = objs[i];
                var NamePart = obj.NameIdentifierPart;
                var ObjName = DBRep.Instance().GetWordInFile(filepath, NamePart.LineBeg, NamePart.ColumnBeg, NamePart.ColumnEnd);

                var element = new PackageElement(ObjName, obj.GetType().GetCustomAttribute<PackageElementTypeAttribute>().ElementType);
                element.AddPosition(positionType, obj.Position());

                if (obj is ParsedMethod objMethod)
                {
                    if (positionType == ePackageElementDefinitionType.BodyFull)
                    {
                        // Фиксируем часть спецификации в теле
                        element.AddPosition(ePackageElementDefinitionType.BodyDeclaration, objMethod.DeclarationPart);
                        element.Parametres.AddRange(objMethod.Parameters);
                        element.Links.AddRange(objMethod.Elements);
                    }
                    else if (positionType == ePackageElementDefinitionType.Spec)
                    {
                        try
                        {
                            /* Метод в спецификации должен иметь пару в теле пакета.
                             * Ищем уже добавленный метод в теле. Добавляем к нему позицию спецификации
                             */
                            elements.First(x => x.Equals(objMethod)).AddPosition(ePackageElementDefinitionType.Spec, objMethod.Position());
                        }
                        catch(InvalidOperationException ex)
                        {
                            // Sequence contains no matching element
                            if (ex.HResult == -2146233079)
                            {
                                var ErrorMsg = $"В спецификации для метода {objMethod.Name} не найдена пара в теле пакета";
                                Func<IEnumerable<ParsedParameter>, string> printParam = (x) => string.Join(", ", x.Select(x => $"{x.Name} {x.plType}"));
                                Seri.Log.Error(ErrorMsg);
                                Seri.Log.Error($"Метод в спецификации: {objMethod.Name}, параметры: {printParam(objMethod.Parameters)}");
                                foreach (var item in elements.Where(x=>x.Name.ToUpper() == objMethod.Name.ToUpper()))
                                    Seri.Log.Error($"Метод в теле: {item.Name}, параметры: {printParam(item.Parametres)}");
                                throw new Exception(ErrorMsg);
                            }
                            else
                                throw ex;
                        }
                        continue;
                    }
                }
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
