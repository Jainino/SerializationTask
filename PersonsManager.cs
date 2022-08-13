using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

namespace SerializationTask
{
    class PersonsManager
    {
        public List<Person> Persons;
        int Age;

        public List<Person> generatePersons(int count, PersonsManager obj)                        //метод генерирующий объекты Person
        {
            var Child = new Faker<Child>()                                                          //Правила для генерации полей класса Children:
                .RuleFor(d => d.Id, h => h.IndexVariable++)                                           //Id от 0 до count для каждого Children
                .RuleFor(d => d.FirstName, h => h.Person.FirstName)
                .RuleFor(d => d.LastName, h => h.Person.LastName)
                .RuleFor(d => d.BirthDate, h => new DateTimeOffset(h.Date.Past(18)).ToUnixTimeSeconds()) //ДатаРождения не позже чем 18 лет назад
                .RuleFor(d => d.Gender, h => h.PickRandom<Gender>());

            var Person = new Faker<Person>()                                                                                            //Правила для генерации полей класса Person:
                .RuleFor(c => c.Id, f => f.IndexVariable++)                                                                             //Id от 0 до count для каждого Person
                .RuleFor(c => c.TransportId, f => Guid.NewGuid())                                                                       //TransportId Guid номер
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)                                                                     //Имя
                .RuleFor(c => c.LastName, f => f.Person.LastName)                                                                       //Фамилия
                .RuleFor(c => c.SequenceId, f => f.IndexFaker)                                                                          //id очереди?
                .RuleFor(c => c.CreditCardNumbers, f => f.Make(f.Random.Number(1, 5), () => f.Finance.CreditCardNumber()).ToArray())    //Кредитные карты. Может быть несколько штук, рандом 1-5
                .RuleFor(c => c.Age, f => obj.Age = f.Random.Number(18, 110))                                                           //Рандомный возраст(18-110), сохраняется в int Age для вычисления года рождения
                .RuleFor(c => c.Phones, f => f.Make(f.Random.Number(1, 3), () => f.Phone.PhoneNumber()).ToArray())                      //Телефонов тоже может быть несколько (1-3)                                
                .RuleFor(c => c.BirthDate, f => getBirthDate(obj))                                                                      //Дата рождения генерируется в getBirthDate()
                .RuleFor(c => c.Salary, f => Convert.ToDouble(f.Finance.Amount(0, 100000)))                                             //Зарплата 
                .RuleFor(c => c.IsMarred, f => f.Random.Bool())                                                                         //Брак, рандом true false 
                .RuleFor(c => c.Gender, f => f.PickRandom<Gender>())                                                                    //Gender из DataModels.cs enum Gender, берется рандомно
                .RuleFor(c => c.Children, f => Child.GenerateBetween(0, 5).ToArray());                                                  //Количество детей рандом (0-5)

            Persons = Person.Generate(count);     //Генерация объектов Person класса в лист Persons
            return Persons;
        }

        public long getBirthDate(PersonsManager obj)       //метод получения даты рождения, имея значение возраста
        {
            Bogus.Faker f = new Bogus.Faker();             //объект класса, чтобы производить генерацию фейковой информации
            var nowYear = DateTime.Now.Year;               //Необходимо взять текущий год
            var fakeMonth = f.Person.DateOfBirth.Month;
            var fakeDay=0;
            if (DateTime.IsLeapYear(nowYear - obj.Age) && (f.Person.DateOfBirth.Day == 2))  //надстройка связанная с високосным годом
                fakeDay = f.Random.Number(1, 29);
            else
                fakeDay = f.Random.Number(1, 28);
            var fakeHour = f.Person.DateOfBirth.Hour;      //Все остальное, кроме года, генерируется   
            var fakeMinute = f.Person.DateOfBirth.Minute;
            var fakeSecond = f.Person.DateOfBirth.Second;

            DateTime fake_Bday = new DateTime(nowYear - obj.Age, fakeMonth, fakeDay, fakeHour, fakeMinute, fakeSecond);
            var fakeBirthday = new DateTimeOffset(fake_Bday).ToUnixTimeSeconds();
            return fakeBirthday;
        }

        public int getCreditCardsCount(PersonsManager obj) //метод подсчета кол-ва кредитных карт всех persons
        {
            int creditCardCount = 0;
            foreach (Person person in obj.Persons)
            {
                creditCardCount += person.CreditCardNumbers.Count();
            }
            return creditCardCount;
        }

        public double avgChildAge(PersonsManager obj)     //метод подсчета среднего значения возраста всех детей
        {
            var summChildAge = 0; int countChild = 0;     //переменные для хранения суммы лет и кол-ва детей
            foreach (Person person in obj.Persons)
            {
                countChild += person.Children.Count();    
                foreach (Child child in person.Children)  //для каждого ребенка
                {                                         //суммаЛет += ТекущийГод - (датаРождения(конвертация из unix формата))
                    summChildAge += DateTime.Now.Year - DateTimeOffset.FromUnixTimeSeconds(child.BirthDate).UtcDateTime.Year;
                }
            }
            return summChildAge/countChild;
        }
    }
}
