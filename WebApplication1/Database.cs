﻿using System.Collections.Generic;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace WebApplication1
{
    public interface IDatabase
    {
        void CreateTablesAndTestData(IDbConnectionFactory dbFactory);

        List<Person> GetPeople(IDbConnectionFactory dbFactory);
        Person LoadPersonById(IDbConnectionFactory dbFactory, int Id);
        List<Contact> LoadContacts(IDbConnectionFactory dbFactory, int Id);
    }

    public class Database : IDatabase
    {
        /// <summary>
        /// Creates all the tables and adds test data to them.
        /// </summary>
        /// <param name="dbFactory"></param>
        public void CreateTablesAndTestData(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                // Create tables in the correct order, because of the foreign key
                db.DropAndCreateTable<Contact>();
                db.DropAndCreateTable<Person>();
                db.DropAndCreateTable<Enterprise>();

                // test data
                var people = new List<Person>
                {
                    new Person { Name = "Willifred Manford", Enterprise = new Enterprise { Name = "ACME Inc" } },
                    new Person { Name = "Kim Nerli",         Enterprise = new Enterprise { Name = "BnL" } },
                    new Person { Name = "Bill Gates",        Enterprise = new Enterprise { Name = "Generic Enterprises" } },
                    new Person { Name = "Steve Balmer",      Enterprise = db.SingleById<Enterprise>(3), EnterpriseId = 3 },
                    new Person { Name = "Finn Erik",         Enterprise = db.SingleById<Enterprise>(1), EnterpriseId = 1 }
                };
                people.ForEach(p => db.Save(p, references:true));

                var contacts = new List<Contact>
                {
                    new Contact { PersonId = 1, ContactPersonId = 2 },
                    new Contact { PersonId = 1, ContactPersonId = 4 },
                    new Contact { PersonId = 2, ContactPersonId = 5 },
                    new Contact { PersonId = 3, ContactPersonId = 4 },
                    new Contact { PersonId = 3, ContactPersonId = 1 },
                    new Contact { PersonId = 4, ContactPersonId = 3 },
                    new Contact { PersonId = 5, ContactPersonId = 1 },
                    new Contact { PersonId = 5, ContactPersonId = 2 }
                };
                contacts.ForEach(c => db.Save(c));
            }
        }

        /// <summary>
        /// Gets all the Person objects from the database.
        /// References to Enterprises is loaded into the dataset.
        /// </summary>
        /// <param name="dbFactory"></param>
        /// <example>var res = GetPeople(IDbConnectionFactory)</example>
        /// <returns>A list of people.</returns>
        public List<Person> GetPeople(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                return db.LoadSelect<Person>();
            }
        }

        /// <summary>
        /// Get a single Person object from the database, filtered by the ID field.
        /// </summary>
        /// <param name="dbFactory"></param>
        /// <param name="Id"></param>
        /// <returns>A Person object.</returns>
        public Person LoadPersonById(IDbConnectionFactory dbFactory, int Id)
        {
            using (var db = dbFactory.Open())
            {
                return db.LoadSingleById<Person>(Id);
            }
        }

        /// <summary>
        /// Get a list of Contacts. Filtered by PersonId.
        /// </summary>
        /// <param name="dbFactory"></param>
        /// <param name="Id"></param>
        /// <returns>A list of contacts.</returns>
        public List<Contact> LoadContacts(IDbConnectionFactory dbFactory, int Id)
        {
            using (var db = dbFactory.Open())
            {
                return db.LoadSelect<Contact>(c => c.PersonId == Id);
            }
        }
    }
}
