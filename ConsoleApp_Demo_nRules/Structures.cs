using ConsoleApp_Demo_nRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp_Demo_nRules
{
    internal class Item
    {
        public string sName { get; }
        public Decimal dCost { get; set; } = 0;
        public int iCalories { get; set; } = 0;
        public int iCarbGrams { get; set; } = 0;
        public int iGlutenPPM { get; set; } = 0;  //PPM -> parts per million
        public bool bIsVeganFrienly { get; set; } = true;
        public bool bContainsNuts { get; set; } = false;
        public bool bContainsDairy { get; set; } = false;

        public Item(string name)
        {
            sName = name;
        }
    }

    internal class Combo
    {
        public string sName { get; }
        public List<Item> items = new List<Item>();
        public Decimal dCost { get; set; } = 0;
        public int iCalories { get; set; } = 0;
        public int iCarbGrams { get; set; } = 0;
        public int iGlutenPPM { get; set; } = 0;  //PPM -> parts per million
        public bool bIsVeganFrienly { get; set; } = true;
        public bool bContainsNuts { get; set; } = false;
        public bool bContainsDairy { get; set; } = false;

        public Combo(string sName)
        {
            this.sName = sName;
        }

        public void AddItem(Item item)
        {
            items.Add(item);

            //update totals
            Decimal tempTotCost = 0;
            int tempCalories = 0;
            int tempCarbGrams = 0;
            int tempGlutenPPM = 0;
            bool tempIsVeganFrienly = true;
            bool tempContainsNuts = false;
            bool tempContainsDairy = false;

            foreach (Item i in items)
            {
                tempTotCost += i.dCost;
                tempCalories += i.iCalories;
                tempCarbGrams += i.iCarbGrams;
                tempGlutenPPM += i.iGlutenPPM;
                tempIsVeganFrienly = tempIsVeganFrienly && i.bIsVeganFrienly;  //all must be vegan friendly to be true
                tempContainsNuts = tempContainsNuts || i.bContainsNuts; // true if one item contains nuts
                tempContainsDairy = tempContainsDairy || i.bContainsDairy; // true if one item contains nuts
            }

            //extra business logic for Cost.  1 item = no discount. 2 and 3 items is 5% off. 4+ gets 5% off plus bonus .10 for every item over 3
            if (items.Count >= 2) //5% discount
            {
                tempTotCost *= (Decimal).95;
            }
            if (items.Count >= 4) //extra .10 discount for items 3+
            {
                tempTotCost -= (items.Count - 3) * (Decimal).1;
            }

            //verify price did not dip below $0
            if (tempTotCost < 0)
            {
                tempTotCost = 0;
            }

            //round so we are at 2 decimal places
            tempTotCost = Math.Round(tempTotCost, 2, MidpointRounding.AwayFromZero);

            //Update Combo values
            dCost = tempTotCost;
            iCalories = tempCalories;
            iCarbGrams = tempCarbGrams;
            iGlutenPPM = tempGlutenPPM;
            bIsVeganFrienly = tempIsVeganFrienly;
            bContainsNuts = tempContainsNuts;
            bContainsDairy = tempContainsDairy;
        }
    } //end Combo

   

        internal class Customer
        {
            public int sCustomerID { get; }
            public string sCustomerName { get;}
            public DateTime dtBirthDate { get; }
            public Decimal dBudget { get; set; } = 0;
            public bool bIsStudent { get; set; } = false;
            public bool bIsKeto { get; set; } = false;
            public bool bIsGlutenFree { get; set; } = false;
            public bool bIsHealthConscious { get; set; } = false;
            public bool bIsVegan { get; set; } = false;

        public Customer(int custID, string custName, string custDob)
        {
            sCustomerID = custID;
            sCustomerName = custName;

            DateTime dtTemp;
            if(DateTime.TryParse(custDob, out dtTemp) == false)
            {
                Console.WriteLine(string.Format("Error Loading DOB - Customer:{0} EnteredDob:{1}", custName, custDob));
                dtTemp = DateTime.Today;  //default bc of error
            }

            dtBirthDate = dtTemp;
        }

        public int iAge
        {
            get
            {
                DateTime dtNow = DateTime.Now;
                int age = dtNow.Year - dtBirthDate.Year;

                if (dtNow.Month < dtBirthDate.Month || (dtNow.Month == dtBirthDate.Month && dtNow.Day < dtBirthDate.Day))
                    age--;

                return age;
            }
        }
    }

}

internal class Order
{
    public int iOrderNum { get; }
    public Customer Customer { get; }
    public List<Item> items = new List<Item>();
    public List<Combo> combos = new List<Combo>();

    public Decimal dCost { get; set; } = 0;
    public int iCalories { get; set; } = 0;
    public int iCarbGrams { get; set; } = 0;
    public int iGlutenPPM { get; set; } = 0;  //PPM -> parts per million
    public bool bIsVeganFrienly { get; set; } = true;
    public bool bContainsNuts { get; set; } = false;
    public bool bContainsDairy { get; set; } = false;

    public bool bPassed_Rules { get; set; } = true;
    public string sFailedRules { get; set; } = string.Empty;


    public Order(int orderNum, Customer customer)
    {
        iOrderNum = orderNum;
        Customer = customer;
    }

    public void AddItem(Item item)
    {
        items.Add(item);

        dCost += item.dCost;
        iCalories += item.iCalories;
        iCarbGrams += item.iCarbGrams;
        iGlutenPPM += item.iGlutenPPM;
        bIsVeganFrienly = bIsVeganFrienly && item.bIsVeganFrienly;  //true is all are true
        bContainsNuts = bContainsNuts || item.bContainsNuts; //true if one item contains nuts
        bContainsDairy = bContainsDairy || item.bContainsNuts;
    }

    public void AddItem(Combo combo)
    { 
        combos.Add(combo);

        dCost += combo.dCost;
        iCalories += combo.iCalories;
        iCarbGrams += combo.iCarbGrams;
        iGlutenPPM += combo.iGlutenPPM;
        bIsVeganFrienly = bIsVeganFrienly && combo.bIsVeganFrienly;  //true is all are true
        bContainsNuts = bContainsNuts || combo.bContainsNuts; //true if one item contains nuts
        bContainsDairy = bContainsDairy || combo.bContainsNuts;      
    }

    public void OutputRuleResults()
    {
        if (bPassed_Rules)
        {
            Console.WriteLine(string.Format("Order: {0} Cust: {1} - PASS", iOrderNum, Customer.sCustomerName));
        }
        else
        {
            Console.WriteLine(string.Format("Order: {0} Cust: {1} - FAIL Reason(s): {2}", iOrderNum, Customer.sCustomerName, sFailedRules));
        }
    }

    public void FailRule(string sRuleName) 
    {
        bPassed_Rules = false;
        if (string.IsNullOrEmpty(sFailedRules))
            sFailedRules = sRuleName;
        else
            sFailedRules += ", " + sRuleName;
    }



} //name space
