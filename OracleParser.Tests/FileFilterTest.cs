using DataBaseRepository.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using System.Linq;
using DataBaseRepository;

namespace OracleParser.Tests
{
    class FileFilterTest
    {
        /// <summary>
        /// Проверка работы фильтра SelectRequest для отбора файлов
        /// </summary>
        /// <param name="args"></param>
        [TestCaseSource("TestCaseData")]
        public static void FileRequestTest(KeyValuePair<SelectRequest, int[]> args)
        {
            Console.WriteLine("** Test begin **");
            Console.WriteLine(args.Key);

            DBRep repository = DBRep.Instance();
            repository.RepositoryPath = RootPath;
            var files = repository.GetFiles(args.Key);

            Console.WriteLine("Getting files:");
            foreach (var item in files)
                Console.WriteLine(item);

            int[] ResultFilesIDs = files
                .Select(x => objs.First(z=>z.Value.RepFilePath == x.RepFilePath).Key)
                .OrderBy(x=>x)
                .ToArray();

            Console.WriteLine($"ExceptedResults: {string.Join(", ", args.Value)}");
            Console.WriteLine($"ActualResults: {string.Join(", ", ResultFilesIDs)}");
            Assert.AreEqual(args.Value, ResultFilesIDs);
        }

        private static readonly string RootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestRepo");

        /// <summary>
        /// Файлы которые будут созданы в тетсовом репозитории, каждый файл имеет ID
        /// </summary>
        private static Dictionary<int, RepositoryObject> objs = new Dictionary<int, RepositoryObject>()
        {
            { 1, new RepositoryObject("GetValue", "alpha", eRepositoryObjectType.Function) },
            { 2, new RepositoryObject("SetValue", "alpha", eRepositoryObjectType.Procedure) },
            { 3, new RepositoryObject("Process_MK1", "alpha", eRepositoryObjectType.Package_Spec) },
            { 4, new RepositoryObject("Process_MK1", "alpha", eRepositoryObjectType.Package_Body) },
            { 5, new RepositoryObject("datamk1_biu", "alpha", eRepositoryObjectType.Trigger) },
            { 6, new RepositoryObject("GetMonth", "alpha", eRepositoryObjectType.Function) },
            { 7, new RepositoryObject("GetID", "beta", eRepositoryObjectType.Function) },
            { 8, new RepositoryObject("MyList", "beta", eRepositoryObjectType.Type_Body) },
            { 9, new RepositoryObject("MyList", "beta", eRepositoryObjectType.Type_Spec) },
            { 10, new RepositoryObject("Process_ZZ", "beta", eRepositoryObjectType.Package_Body) },
            { 11, new RepositoryObject("Process_ZZ", "beta", eRepositoryObjectType.Package_Spec) },
            { 12, new RepositoryObject("VW_log", "beta", eRepositoryObjectType.View) },
        };

        /// <summary>
        /// Тестовые данные 
        /// Фильтр SelectRequest который будем тестировать на отборе файлов
        /// int[] - список ID файлов которые ожидаем после отбора
        /// </summary>
        private static Dictionary<SelectRequest, int[]> TestData = new Dictionary<SelectRequest, int[]>()
        {
            {
                new SelectRequest() {
                    Owner = "beta",
                    Pattern = "zz",
                    FileTypes = new List<eRepositoryObjectType>()
                    {
                        eRepositoryObjectType.Package_Body
                    }
                },
                new int[] { 10 }
            },
            {
                new SelectRequest() {
                    Owner = "alpha",
                    Pattern = "get",
                    FileTypes = new List<eRepositoryObjectType>()
                    {
                        eRepositoryObjectType.Procedure
                    }
                },
                new int[] { }
            },
            {
                new SelectRequest() {
                    Pattern = "get",
                    FileTypes = new List<eRepositoryObjectType>()
                    {
                        eRepositoryObjectType.Function,
                        eRepositoryObjectType.Procedure
                    }
                },
                new int[] { 1, 6, 7 }
            },
            {
                new SelectRequest() {
                    Pattern = "Process",
                },
                new int[] { 3, 4, 10, 11 }
            },
            {
                new SelectRequest() {
                    Owner = "beta",
                    FileTypes = new List<eRepositoryObjectType>()
                    {
                        eRepositoryObjectType.View
                    }
                },
                new int[] { 12 }
            },
            {
                new SelectRequest(),
                Enumerable.Range(1, 12).ToArray()
            },
            {
                new SelectRequest() {
                    FileTypes = new List<eRepositoryObjectType>()
                    {
                        eRepositoryObjectType.Package_Spec
                    }
                },
                new int[] { 3, 11 }
            },
            {
                new SelectRequest() {
                    FileTypes = new List<eRepositoryObjectType>()
                    {
                        eRepositoryObjectType.Procedure,
                        eRepositoryObjectType.Function,
                        eRepositoryObjectType.Type_Body,
                        eRepositoryObjectType.Type_Spec,
                        eRepositoryObjectType.View
                    }
                },
                new int[] { 1, 2, 6, 7, 8, 9, 12 }
            },
        };

        private static IEnumerable<TestCaseData> TestCaseData = TestData.Select(x => new TestCaseData(x));

        [OneTimeSetUp]
        public static void CreateRepo()
        {
            foreach (var item in objs.Values)
            {
                string folderPath = Path.Combine(RootPath, Path.GetDirectoryName(item.RepFilePath));
                string fullPath = Path.Combine(RootPath, item.RepFilePath);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                Console.WriteLine("File created: " + fullPath);
                File.WriteAllText(fullPath, DateTime.Now.ToLongDateString());
            }
        }

        [OneTimeTearDown]
        public void DropRepo()
        {
            Directory.Delete(RootPath, true);
        }
    }
}
