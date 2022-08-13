﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SerializationTask
{
	[JsonObject(NamingStrategyType =typeof(CamelCaseNamingStrategy))]
    class Person
    {
		public Int32 Id { get; set; }
		public Guid TransportId { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public Int32 SequenceId { get; set; }
		public String[] CreditCardNumbers { get; set; }
		public Int32 Age { get; set; }
		public String[] Phones { get; set; }
		public Int64 BirthDate { get; set; }
		public Double Salary { get; set; }
		public Boolean IsMarred { get; set; }
		public Gender Gender { get; set; }
		public Child[] Children { get; set; }
	}
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	class Child
	{
		public Int32 Id { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public Int64 BirthDate { get; set; }
		public Gender Gender { get; set; }
	}
	enum Gender
	{
		Male,
		Female
	}
}
