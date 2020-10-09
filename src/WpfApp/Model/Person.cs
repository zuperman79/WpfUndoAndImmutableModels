using System;
using WpfApp.Extensions;

namespace WpfApp.Model
{
    public class Person : IStateObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Country Country { get; private set; }
        public Guid UID { get; set; }

        public Person(string FirstName, string LastName, DateTime DateOfBirth, Country Country)
        {
            this.UID = Guid.NewGuid();
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Country = Country;
        }

        public Person(IStateObject user, string propertyName, object value)
        {
            this.Copy(user, propertyName, value);
        }

        public Func<IStateObject, string, object, IStateObject> CreateCopy => 
            (stateObject, propertyName, value) => new Person(stateObject, propertyName, value);
    }
}