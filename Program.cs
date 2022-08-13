using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bogus;
using Newtonsoft.Json;

namespace SerializationTask
{
    class Program
    {

        static void Main(string[] args) 
        {
            PersonsManager personManager = new PersonsManager();          //объект класса PersonsManager для работы с листом и методами
            personManager.generatePersons(10000,personManager);               //генерация 10000 Persons

            //-Сериализация и запись в файл 
            File.WriteAllText(@"c:\users\anjel\Desktop\Persons.json", JsonConvert.SerializeObject(personManager.Persons, Formatting.Indented));

            //-Чистка листа с объектами Persons
            personManager.Persons.Clear();

            //-Чтение из файла, десериализация и помещение в лист Persons
            personManager.Persons = (List<Person>)JsonConvert.DeserializeObject(File.ReadAllText(@"c:\users\anjel\Desktop\Persons.json"), typeof(List<Person>));

            Console.WriteLine("Persons count: " + personManager.Persons.Count);
            Console.WriteLine("Persons credit card count: " + personManager.getCreditCardsCount(personManager));
            Console.WriteLine("Average value of child age: "+ personManager.avgChildAge(personManager));
        }
    }
}
