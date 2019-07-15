using System;
using System.Collections.Generic;

namespace gittest
{

    class BubkaSorter
    {
        static public void Sort<T>(IList<T> sortArr, Func<T, T, bool> comparison)
        {
            bool swapped = true;
            do
            {
                swapped = false;
                for (int i = 0; i < sortArr.Count - 1; i++)
                {
                    if (comparison(sortArr[i + 1], sortArr[i]))
                    {
                        T temp = sortArr[i];
                        sortArr[i] = sortArr[i + 1];
                        sortArr[i + 1] = temp;
                        swapped = true;
                    }
                }

            } while (swapped);

        }
    }
    class Emplo
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }

        public Emplo(string name, decimal sal)
        {
            this.Name = name;
            this.Salary = sal;
        }

        public override string ToString()
        {
            return string.Format($"name: {Name} salary: {Salary}");
        }

        public static bool CompSalary(Emplo t1, Emplo t2)
        {
            return t1.Salary < t2.Salary;
        }

    }

    class TestEmploSort
    {
        public void trest()
        {
            BubkaSorter.Sort(emplos, Emplo.CompSalary);
            foreach (var c in emplos)
            {
                Console.WriteLine(c);
            }
        }
        Emplo[] emplos =
        {
            new Emplo("Tim",3500),
            new Emplo("gray",8500),
            new Emplo("kit",6522),
            new Emplo("lop",8884),
            new Emplo("joi",1999),
        };

    }

    class MainClass
    {

        public static void Main(string[] args)
        {
            TestEmploSort tas = new TestEmploSort();
            tas.trest();
            Enumarat nma = new Enumarat();
            foreach (var s in nma)
            {
                Console.WriteLine(s);
            }
        }
    }

    class Enumarat
    {
        string[] s = new string[255];

        public IEnumerator<string> GetEnumerator()
        {
            yield return "testTwo";
        }
    }
}
