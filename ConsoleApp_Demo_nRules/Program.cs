using ConsoleApp_Demo_nRules;
using NRules;
using NRules.Fluent;

internal class Program
{
    private static string sCurrentMethod = string.Empty;
    private static int sMethodLoc = 0;
    private static void Main(string[] args)
    {
        sCurrentMethod = "Main";

        try
        {
            sMethodLoc = 1;
            Console.WriteLine("Loading Rules");
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(StarvingStudentOrderRule).Assembly));

            sMethodLoc = 2;
            var factory = repository.Compile();

            sMethodLoc = 3; 
            var session = factory.CreateSession();

            //Create Menu Items and Assign Attributes
            sMethodLoc = 4;
            Console.WriteLine("Creating Menu Items");
            Item ramen = new Item("Ramen") { bContainsDairy = false, bIsVeganFrienly = true, bContainsNuts = false, dCost = (decimal)1.25, iCalories = 688, iCarbGrams = 27, iGlutenPPM = 1000 };
            Item bpSandwich = new Item("PBJ Sandwich") { bContainsDairy = false, bIsVeganFrienly = true, bContainsNuts = true, dCost = (decimal)2.25, iCalories = 1345, iCarbGrams = 21, iGlutenPPM = 689 };
            Item redBeansRice = new Item("Red Beans 'N Rice") { bContainsDairy = false, bIsVeganFrienly = true, bContainsNuts = false, dCost = (decimal)3.25, iCalories = 1545, iCarbGrams = 28, iGlutenPPM = 883 };
            Item friedCatfish = new Item("Fried Catfish") { bContainsDairy = false, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)8.25, iCalories = 1545, iCarbGrams = 28, iGlutenPPM = 350 };
            Item salmon = new Item("Grilled Salmon") { bContainsDairy = false, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)10.25, iCalories = 545, iCarbGrams = 2, iGlutenPPM = 0 };
            Item caesarSalad = new Item("Caesar Salad") { bContainsDairy = true, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)3.25, iCalories = 145, iCarbGrams = 10, iGlutenPPM = 10 };
            Item coke = new Item("Coke") { bContainsDairy = false, bIsVeganFrienly = true, bContainsNuts = false, dCost = (decimal)2.25, iCalories = 150, iCarbGrams = 39, iGlutenPPM = 0 };
            Item cokeZero = new Item("CokeZero") { bContainsDairy = false, bIsVeganFrienly = true, bContainsNuts = false, dCost = (decimal)2.25, iCalories = 0, iCarbGrams = 0, iGlutenPPM = 0 };
            Item coffee = new Item("Coffee") { bContainsDairy = false, bIsVeganFrienly = true, bContainsNuts = false, dCost = (decimal)1.25, iCalories = 1, iCarbGrams = 0, iGlutenPPM = 0 };
            Item coffeeWCreamer = new Item("Coffee W/ Creamer") { bContainsDairy = true, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)1.45, iCalories = 497, iCarbGrams = 56, iGlutenPPM = 0 };
            Item hamburger = new Item("Hamburger") { bContainsDairy = false, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)6.25, iCalories = 354, iCarbGrams = 36, iGlutenPPM = 734 };
            Item cheeseburger = new Item("Cheeseburger") { bContainsDairy = true, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)6.75, iCalories = 394, iCarbGrams = 36, iGlutenPPM = 734 };
            Item jrBurger = new Item("Jr. Burger") { bContainsDairy = false, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)3.25, iCalories = 214, iCarbGrams = 28, iGlutenPPM = 534 };
            Item friesLg = new Item("Fries Lg") { bContainsDairy = false, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)3.25, iCalories = 481, iCarbGrams = 124, iGlutenPPM = 0 };
            Item friesSm = new Item("Fries Sm") { bContainsDairy = false, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)2.15, iCalories = 241, iCarbGrams = 62, iGlutenPPM = 0 };
            Item shake = new Item("Shake") { bContainsDairy = false, bIsVeganFrienly = true, bContainsNuts = false, dCost = (decimal)2.35, iCalories = 441, iCarbGrams = 42, iGlutenPPM = 0 };
            Item chickenNuggets4pc = new Item("Chicken Nuggets 4pc") { bContainsDairy = false, bIsVeganFrienly = false, bContainsNuts = false, dCost = (decimal)2.35, iCalories = 193, iCarbGrams = 10, iGlutenPPM = 110 };

            //Create Combos by adding Items
            sMethodLoc = 5;
            Console.WriteLine("Creating Combos");
            Combo catfishPlate = new Combo("Catfish Plate");
            catfishPlate.AddItem(friedCatfish);
            catfishPlate.AddItem(redBeansRice);
            catfishPlate.AddItem(coke);

            Combo fishNchips = new Combo("Fish 'N Chips");
            fishNchips.AddItem(friedCatfish);
            fishNchips.AddItem(friesLg);
            fishNchips.AddItem(coke);

            Combo salmonPlate = new Combo("Salmon Plate");
            salmonPlate.AddItem(salmon);
            salmonPlate.AddItem(caesarSalad);
            salmonPlate.AddItem(cokeZero);

            Combo burgerCombo = new Combo("Burger Combo");
            burgerCombo.AddItem(hamburger);
            burgerCombo.AddItem(friesLg);
            burgerCombo.AddItem(coke);

            Combo kidsMeal = new Combo("Kid's Meal");
            kidsMeal.AddItem(jrBurger);
            kidsMeal.AddItem(friesSm);
            kidsMeal.AddItem(coke);

            Combo biggieMeal = new Combo("Biggie Meal");
            biggieMeal.AddItem(jrBurger);
            biggieMeal.AddItem(friesSm);
            biggieMeal.AddItem(chickenNuggets4pc);
            biggieMeal.AddItem(shake);

            //Create Customers and Assign Attributes
            sMethodLoc = 6;
            Console.WriteLine("Creating Customers");
            Customer mel = new Customer(1, "Mel", "12/5/1950") { bIsGlutenFree = false, bIsHealthConscious = false, bIsKeto = false, bIsStudent = false, bIsVegan = false, dBudget = 25.00M };
            Customer vera = new Customer(2, "Vera", "1/1/1965") { bIsGlutenFree = false, bIsHealthConscious = true, bIsKeto = false, bIsStudent = false, bIsVegan = true, dBudget = 11.00M };
            Customer flo = new Customer(3, "Flo", "7/4/1970") { bIsGlutenFree = true, bIsHealthConscious = true, bIsKeto = false, bIsStudent = false, bIsVegan = false, dBudget = 15.50M };
            Customer alice = new Customer(4, "Alice", "11/19/2002") { bIsGlutenFree = false, bIsHealthConscious = false, bIsKeto = false, bIsStudent = true, bIsVegan = false, dBudget = 7.50M };
            Customer tommy = new Customer(4, "Tommy", "08/24/2011") { bIsGlutenFree = false, bIsHealthConscious = false, bIsKeto = false, bIsStudent = true, bIsVegan = false, dBudget = 7.50M };
            Customer jolene = new Customer(5, "Jolene", "08/24/1990") { bIsGlutenFree = false, bIsHealthConscious = false, bIsKeto = true, bIsStudent = false, bIsVegan = false, dBudget = 16.50M };

            //Create Orders and Assign to Customers. Adding Combos and/or Items
            sMethodLoc = 7;
            Console.WriteLine("Creating Orders");

            //Alice - a starving student who needs lots of calories on a budget
            Order aliceOrder1 = new Order(1, alice);
            aliceOrder1.AddItem(fishNchips);
            aliceOrder1.AddItem(coffee);  //Fail due to low calorie per dollar ratio.

            Order aliceOrder2 = new Order(2, alice);
            aliceOrder2.AddItem(ramen);
            aliceOrder2.AddItem(bpSandwich);
            aliceOrder2.AddItem(redBeansRice); //Pass. Offered Items with high calorie to dollar ratio

            //Mel - Adult who can eat anything
            Order melOrder3 = new Order(3, mel);
            melOrder3.AddItem(friedCatfish);
            melOrder3.AddItem(salmonPlate);
            melOrder3.AddItem(cheeseburger); //Fail. Cost greater than budget.

            Order melOrder4 = new Order(4, mel);
            melOrder4.AddItem(biggieMeal); //Pass. Cheaper order

            Order melOrder5 = new Order(5, mel);
            melOrder5.AddItem(biggieMeal);
            melOrder5.AddItem(kidsMeal);
            melOrder5.AddItem(jrBurger);  //Fail for ordering off the kids menu

            //Flo - Gluten Free PLUS Health Conscious
            Order floOrder6 = new Order(6, flo);
            floOrder6.AddItem(catfishPlate);  //Fail - too much gluten.  Also not healthy calories

            Order floOrder7 = new Order(7, flo);
            floOrder7.AddItem(salmonPlate);  //Fail - not enough carbs to satisfy healthy requirement

            Order floOrder8 = new Order(8, flo);
            floOrder8.AddItem(salmon);
            floOrder8.AddItem(friesSm);  //Pass - fixed by substituting low carbs for higher - still gluten free

            //Jolene - Keto
            Order joleneOrder9 = new Order(9, jolene);
            joleneOrder9.AddItem(burgerCombo); //Fail - too many carbs to be Keto friendly

            Order joleneOrder10 = new Order(10, jolene);
            joleneOrder10.AddItem(salmonPlate); //Pass - low carb option

            Order tommyOrder11 = new Order(11, tommy);
            tommyOrder11.AddItem(kidsMeal);
            tommyOrder11.AddItem(jrBurger);  //Fail - too expensive

            Order tommyOrder12 = new Order(12, tommy);
            tommyOrder12.AddItem(kidsMeal);  //Pass - Cheaper opton.  Note: Tommy is age 12 can order off Kids Menu

            //Add Facts to Rules Engine.  Include Customers and Orders
            sMethodLoc = 8;
            Console.WriteLine("Insert facts into rules engine's memory");
            session.Insert(mel);
            session.Insert(vera);
            session.Insert(flo);
            session.Insert(alice);
            session.Insert(tommy);
            session.Insert(jolene);
            session.Insert(aliceOrder1);
            session.Insert(aliceOrder2);
            session.Insert(melOrder3);
            session.Insert(melOrder4);
            session.Insert(melOrder5);
            session.Insert(floOrder6);
            session.Insert(floOrder7);
            session.Insert(floOrder8);
            session.Insert(joleneOrder9);
            session.Insert(joleneOrder10);  
            session.Insert(tommyOrder11);
            session.Insert(tommyOrder12);

            //Launch Rule Engine
            sMethodLoc = 9;
            Console.WriteLine("Start match/resolve/act cycle");
            session.Fire();

            //Output Results
            sMethodLoc = 10;
            Console.WriteLine("");
            Console.WriteLine("Order Rule Results");
            Console.WriteLine("");
            Console.WriteLine("Alice");
            aliceOrder1.OutputRuleResults();
            aliceOrder2.OutputRuleResults();
            Console.WriteLine("");
            Console.WriteLine("Mel");
            melOrder3.OutputRuleResults();
            melOrder4.OutputRuleResults();
            melOrder5.OutputRuleResults();
            Console.WriteLine("");
            Console.WriteLine("Flo");
            floOrder6.OutputRuleResults();
            floOrder7.OutputRuleResults();
            floOrder8.OutputRuleResults();
            Console.WriteLine("");
            Console.WriteLine("Jolene");
            joleneOrder9.OutputRuleResults();
            joleneOrder10.OutputRuleResults();
            Console.WriteLine("");
            Console.WriteLine("Tommy");
            tommyOrder11.OutputRuleResults();
            tommyOrder12.OutputRuleResults();



        }
        catch (Exception ex) 
        {
            Console.WriteLine(string.Format("ERROR func:{0} loc:{1} - {2}", sCurrentMethod, sMethodLoc, ex.Message));
        }
        
        
    }
}