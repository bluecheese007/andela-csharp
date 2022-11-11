using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace events.consoleapp
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }        
    }
    
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }       
    }
    

    public class Solution
    {
        static void Main(string[] args)
        {
            var events = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
            };
            
            var customer = new Customer { Name = "John Smith", City = "New York" };
    
            // 1. What should be your approach to getting the list of events?
            var customerEvents = events.Where(e => e.City == customer.City).ToList();

            Console.WriteLine("Customer Events:");
            
            // 2. How would you call the AddToEmail method in order to send the events in an email?
            foreach (var item in customerEvents)
            {
                /*
                *   We want you to send an email to this customer with all events in their city
                *	Just call AddToEmail(customer, event) for each event you think they should get
                */
                AddToEmail(customer, item);
            }

            // 3.	What is the expected output if we only have the client John Smith?

            // John Smith: Phantom of the Opera in New York
            // John Smith: Metallica in New York
            // John Smith: LadyGaGa in New York


            // 4.	Do you believe there is a way to improve the code you first wrote?
            // If the order of the events is important, we can sort them by name
            // IF the order is not important, we can use a parallel foreach loop to improve performance


            // Get Top 5 closest events to the customer            
            var closestEventsIndexes = new Dictionary<int, double>();
   
            for (int i = 0; i < events.ToList().Count; i++)
            {
                Event? e = events[i];
                var distance = GetDistance(customer.City, e.City);
                closestEventsIndexes.Add(i, distance);
            }

            // sort by distance
            // 1.	What should be your approach to getting the distance between the customer’s city and the other cities on the list?
            var top5ClosestEvents = closestEventsIndexes.OrderBy(x => x.Value).Take(5).ToDictionary(x => x.Key, x => events[x.Key]);

            Console.WriteLine("Top 5 closest events to the customer");

            // send email
            // 2.	How would you get the 5 closest events and how would you send them to the client in an email?
            foreach (var item in top5ClosestEvents)
            {
                AddToEmail(customer, item.Value);
            }

            // 3.	What is the expected output if we only have the client John Smith?
            /* 
                John Smith: Phantom of the Opera in New York
                John Smith: Metallica in New York
                John Smith: LadyGaGa in New York
                John Smith: LadyGaGa in Chicago (221 miles away)
                John Smith: LadyGaGa in Washington (347 miles away)             
             */

            // 4.	Do you believe there is a way to improve the code you first wrote?
            // We can improve the performance first calculating the distance between the customer city and the other cities if they are the same and they are less than 5 we can avoid calculate more distances

            // Question 3. If the GetDistance method is an API call which could fail or is too expensive, how will uimprove the code written in 2? Write the code.
            // We can use a cache to store the distances between cities and use it when we need to calculate the distance between the same cities
            // We can also use a parallel foreach loop to improve performance
            // We can calculate the distance using hadoop or spark to improve performance
            // We can use snowflake to query the distances between cities

            // Question 4. If the GetDistance method can fail, we don't want the process to fail. What can be done? Code it. (Ask clarifying questions to be clear about what is expected business - wise)
            // see GetDistanceSafe method

            // Question 5. If we also want to sort the resulting events by other fields like price, etc. to determine whichones to send to the customer, how would you implement it? Code it
            
        }

        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        
        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }

        static int GetDistanceSafe(string fromCity, string toCity)
        {
            try
            {
                return GetDistance(fromCity, toCity);
            }
            catch (Exception)
            {
                return -1;
            }           
        }
        
        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
}

/*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/
