using System;

namespace VolodWPF.Classes
{
    public class Worker
    {
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Age { get; set; }

        public Worker()
        {

        }

        public Worker(String name, String surname, String age)
        {
            Name = name;
            Surname = surname;
            Age = age;
        }

        public override string ToString()
        {
            return $"{Name}\t {Surname}\t {Age}";
        }
    }
}