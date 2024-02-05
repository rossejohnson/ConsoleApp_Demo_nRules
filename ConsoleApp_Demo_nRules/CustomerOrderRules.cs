using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NRules.Fluent.Dsl;

namespace ConsoleApp_Demo_nRules
{
    /// <summary>
    /// Starving Student is a Custermer with isStudent set, age 18-25 and budget < $10
    /// Order is appropriate if calorie to dollar ratio of 500:$1
    /// </summary>
    public class StarvingStudentOrderRule : Rule
    {
        public override void Define()
        {
            Customer customer = default;
            IEnumerable<Order> orders = default;

            When()
                .Match<Customer>(() => customer, c => c.bIsStudent, c => c.iAge >= 18, c => c.iAge <= 25, c => c.dBudget < 10)
                .Query(() => orders, x => x
                    .Match<Order>(
                        o => o.Customer == customer,
                        o => o.iCalories / o.dCost < 500)  //failed rule verification
                    .Collect()
                    .Where(c => c.Any()));

            Then()
                .Do(ctx => FailRule(orders));
        }

        private static void FailRule(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                order.FailRule(string.Format("Starving Student - Calorie:Dollar < 500 - {0}/${1}={2}", order.iCalories, order.dCost, Math.Round(order.iCalories/order.dCost, 2, MidpointRounding.AwayFromZero)));
            }
        }
    }


    /// <summary>
    /// has this customer budgeted enough money for this order
    /// </summary>
    public class AffordablOrderRule : Rule
    {
        public override void Define()
        {
            Customer customer = default;
            IEnumerable<Order> orders = default;

            When()
                .Match<Customer>(() => customer, c => c.dBudget >= 0)
                .Query(() => orders, x => x
                    .Match<Order>(
                        o => o.Customer == customer,
                        o => o.dCost > customer.dBudget)  //failed rule verification
                    .Collect()
                    .Where(c => c.Any()));

            Then()
                .Do(ctx => FailRule(orders));
        }

        private static void FailRule(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                order.FailRule(string.Format("Affordable - Order exceeds Budget - Order:${0} Budget:${1}", order.dCost, order.Customer.dBudget));
            }
        }
    }//class

    /// <summary>
    /// Only children under 13 can order off kid's menu. kid's menu items/combo start with Kid's or Jr.
    /// </summary>
    public class KidsOnlyOrderRule : Rule
    {
        public override void Define()
        {
            Customer customer = default;
            IEnumerable<Order> orders = default;

            When()
                .Match<Customer>(() => customer, c => c.iAge >= 13)
                .Query(() => orders, x => x
                    .Match<Order>(
                        o => o.Customer == customer)  //matching adults to orders
                    .Collect()
                    .Where(c => c.Any()));

            Then()
                .Do(ctx => CheckAdultOrdersForKidItems(orders));
        }

        private static void CheckAdultOrdersForKidItems(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                string sKidItems = string.Empty;
                //Loop through each Item and Combo looking for kid items

                //Items
                foreach(Item i in order.items) 
                {
                    if( i.sName.StartsWith("Kids")  || i.sName.StartsWith("Jr.") )
                    {
                        sKidItems += i.sName + "-";
                    }
                }

                //Combos
                foreach(Combo c in order.combos)
                {
                    if (c.sName.StartsWith("Kid's") || c.sName.StartsWith("Jr."))
                    {
                        sKidItems += c.sName + "-";
                    }
                }

                //do we have invalid items?
                if(!string.IsNullOrEmpty(sKidItems))
                {
                    sKidItems = sKidItems.Remove(sKidItems.Length - 1); //remove trailing -
                    order.FailRule(string.Format("Adult Ordered off Kid's Menu - Age:{0} KidItem(s):{1}", order.Customer.iAge, sKidItems));
                }
                
            }
        }
    }

    /// <summary>
    /// Order must contian 20 ppm of gluten of less
    /// </summary>
    public class GluttenFreeOrderRule : Rule
    {
        public override void Define()
        {
            Customer customer = default;
            IEnumerable<Order> orders = default;

            When()
                .Match<Customer>(() => customer, c => c.bIsGlutenFree)
                .Query(() => orders, x => x
                    .Match<Order>(
                        o => o.Customer == customer,
                        o => o.iGlutenPPM > 20 )  //failed rule verification
                    .Collect()
                    .Where(c => c.Any()));

            Then()
                .Do(ctx => FailRule(orders));
        }

        private static void FailRule(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                order.FailRule(string.Format("Glutten Free - Order Above 20ppm - ppm:{0}", order.iGlutenPPM));
            }
        }
    }

    /// <summary>
    /// Healthy diet is defined as an order with < 1000 calories and carbs between 50 and 160
    /// </summary>
    public class HealthyDietOrderRule : Rule
    {
        public override void Define()
        {
            Customer customer = default;
            IEnumerable<Order> orders = default;

            When()
                .Match<Customer>(() => customer, c => c.bIsHealthConscious)
                .Query(() => orders, x => x
                    .Match<Order>(
                        o => o.Customer == customer,
                        o => o.iCalories>=1000 || o.iCarbGrams<50 || o.iCarbGrams>160)  //failed rule verification
                    .Collect()
                    .Where(c => c.Any()));

            Then()
                .Do(ctx => FailRule(orders));
        }

        private static void FailRule(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                order.FailRule(string.Format("Healthy Diet - Calories must be < 1000 and carbs between 50 and 160 - Cal:{0} Carb:{1}", order.iCalories, order.iCarbGrams));
            }
        }
    }

    /// <summary>
    /// Keto orders need to be 20 carbs or less
    /// </summary>
    public class KetoOrderRule : Rule
    {
        public override void Define()
        {
            Customer customer = default;
            IEnumerable<Order> orders = default;

            When()
                .Match<Customer>(() => customer, c => c.bIsKeto)
                .Query(() => orders, x => x
                    .Match<Order>(
                        o => o.Customer == customer,
                        o => o.iCarbGrams > 20 )  //failed rule verification
                    .Collect()
                    .Where(c => c.Any()));

            Then()
                .Do(ctx => FailRule(orders));
        }

        private static void FailRule(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                order.FailRule(string.Format("Keto - Carbs must be <= 20 - Carb:{0}", order.iCarbGrams));
            }
        }
    }

} //namespace
