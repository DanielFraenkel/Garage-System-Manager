using GarageLogic;
using System;
using System.Collections.Generic;

namespace GarageConsoleUI
{
    public class GarageUI
    {
        private readonly Garage r_Garage;

        public GarageUI()
        {
            r_Garage = new Garage();
        }

        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                displayMainMenu();
                int choice = getMenuChoice(1, 7);

                switch (choice)
                {
                    case 1:
                        addVehicle();
                        break;
                    case 2:
                        displayLicenseNumbers();
                        break;
                    case 3:
                        changeVehicleStatus();
                        break;
                    case 4:
                        inflateVehicleTires();
                        break;
                    case 5:
                        refuelVehicle();
                        break;
                    case 6:
                        rechargeVehicle();
                        break;
                    case 7:
                        exit = true;
                        Console.WriteLine("Exiting the garage...");
                        break;
                }
            }
        }

        private void displayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Garage Management System ===");
            Console.WriteLine("1. Add a new vehicle");
            Console.WriteLine("2. Display license numbers");
            Console.WriteLine("3. Change vehicle status");
            Console.WriteLine("4. Inflate vehicle tires to maximum");
            Console.WriteLine("5. Refuel a vehicle");
            Console.WriteLine("6. Recharge a vehicle");
            Console.WriteLine("7. Exit");
            Console.Write("Enter your choice (1-7): ");
        }

        private int getMenuChoice(int i_Min, int i_Max)
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < i_Min || choice > i_Max)
            {
                Console.Write($"Invalid choice. Please enter a number between {i_Min} and {i_Max}: ");
            }
            return choice;
        }

        private void addVehicle()
        {
            Console.Clear();
            Console.WriteLine("=== Add a New Vehicle ===");

            // Step 1: Get license number
            Console.Write("Enter the license number: ");
            string licenseNumber = Console.ReadLine();

            // Step 2: Check if vehicle exists
            if (r_Garage.CheckIfVehicleExists(licenseNumber))
            {
                // Update status to InRepair
                r_Garage.ChangeVehicleStatus(licenseNumber, eVehicleStatus.InRepair);
                Console.WriteLine("Vehicle already exists in the garage. Status updated to InRepair.");
            }
            else
            {
                // Step 3: Get vehicle types from garage
                eVehicleType[] vehicleTypes = r_Garage.GetAvailableVehicleTypes();

                // Step 4: UI presents vehicle types to user
                eVehicleType selectedVehicleType = getEnumSelection<eVehicleType>("Select vehicle type:");

                // Step 5: Garage creates vehicle and returns data requirements
                List<DataRequirement> dataRequirements = null;
                try
                {
                    dataRequirements = r_Garage.CreateVehicleRecord(licenseNumber, selectedVehicleType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }

                // Step 6: UI collects data from user
                Dictionary<string, object> userInputs = new Dictionary<string, object>();
                foreach (var requirement in dataRequirements)
                {
                    bool validInput = false;
                    while (!validInput)
                    {
                        try
                        {
                            object userInput = getUserInput(requirement);
                            userInputs[requirement.FieldName] = userInput;
                            validInput = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }

                // Step 7: Send data to garage
                try
                {
                    r_Garage.SetVehicleProperties(licenseNumber, userInputs);
                    Console.WriteLine("Vehicle added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while adding the vehicle: {ex.Message}");
                }
            }
        }

        private object getUserInput(DataRequirement requirement)
        {
            if (requirement.PossibleValues != null && requirement.PossibleValues.Length > 0)
            {
                // Use the generic method for enum selection
                return getEnumSelection(requirement);
            }
            else
            {
                string prompt = requirement.Prompt;

                if (requirement.MinValue.HasValue && requirement.MaxValue.HasValue)
                {
                    prompt += $" (Between {requirement.MinValue.Value} and {requirement.MaxValue.Value})";
                }
                else if (requirement.MinValue.HasValue)
                {
                    prompt += $" (Minimum {requirement.MinValue.Value})";
                }
                else if (requirement.MaxValue.HasValue)
                {
                    prompt += $" (Maximum {requirement.MaxValue.Value})";
                }

                Console.Write(prompt + " ");
                string input = Console.ReadLine();

                return parseUserInput(input, requirement);
            }
        }

        private object parseUserInput(string input, DataRequirement requirement)
        {
            object parsedValue = null;

            if (requirement.DataType == typeof(string))
            {
                parsedValue = input;
            }
            else if (requirement.DataType == typeof(int))
            {
                if (int.TryParse(input, out int intValue))
                {
                    parsedValue = intValue;
                }
                else
                {
                    throw new ArgumentException("Input must be an integer.");
                }
            }
            else if (requirement.DataType == typeof(float))
            {
                if (float.TryParse(input, out float floatValue))
                {
                    parsedValue = floatValue;
                }
                else
                {
                    throw new ArgumentException("Input must be a number.");
                }
            }
            else if (requirement.DataType == typeof(bool))
            {
                if (bool.TryParse(input, out bool boolValue))
                {
                    parsedValue = boolValue;
                }
                else
                {
                    throw new ArgumentException("Input must be true or false.");
                }
            }
            else if (requirement.DataType.IsEnum)
            {
                if (Enum.TryParse(requirement.DataType, input, true, out object enumValue))
                {
                    parsedValue = enumValue;
                }
                else
                {
                    throw new ArgumentException("Invalid selection.");
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported data type.");
            }

            return parsedValue;
        }

        private T getEnumSelection<T>(string i_Prompt) where T : Enum
        {
            T[] enumValues = (T[])Enum.GetValues(typeof(T));
            Console.WriteLine(i_Prompt);
            for (int i = 0; i < enumValues.Length; i++)
            {
                string displayString = enumValues[i].ToString().SplitCamelCase();
                Console.WriteLine($"{i + 1}. {displayString}");
            }
            int choice = getMenuChoice(1, enumValues.Length);
            return enumValues[choice - 1];
        }

        private object getEnumSelection(DataRequirement requirement)
        {
            Array enumValues = requirement.PossibleValues;
            Console.WriteLine(requirement.Prompt);
            for (int i = 0; i < enumValues.Length; i++)
            {
                string displayString = enumValues.GetValue(i).ToString().SplitCamelCase();
                Console.WriteLine($"{i + 1}. {displayString}");
            }
            int choice = getMenuChoice(1, enumValues.Length);
            return enumValues.GetValue(choice - 1);
        }

        private void displayLicenseNumbers()
        {
            Console.Clear();
            Console.WriteLine("=== Display License Numbers ===");
            Console.Write("Filter by status? (y/n): ");
            string filterChoice = Console.ReadLine().ToLower();

            List<string> licenseNumbers;

            if (filterChoice == "y")
            {
                eVehicleStatus selectedStatus = getEnumSelection<eVehicleStatus>("Select status to filter by:");

                licenseNumbers = r_Garage.GetAllLicenseNumbers(selectedStatus);
            }
            else
            {
                licenseNumbers = r_Garage.GetAllLicenseNumbers();
            }

            Console.WriteLine("License Numbers:");
            foreach (string licenseNumber in licenseNumbers)
            {
                Console.WriteLine(licenseNumber);
            }
        }

        private void changeVehicleStatus()
        {
            Console.Clear();
            Console.WriteLine("=== Change Vehicle Status ===");
            Console.Write("Enter the license number: ");
            string licenseNumber = Console.ReadLine();

            eVehicleStatus selectedStatus = getEnumSelection<eVehicleStatus>("Select new status:");

            try
            {
                r_Garage.ChangeVehicleStatus(licenseNumber, selectedStatus);
                Console.WriteLine("Vehicle status updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void inflateVehicleTires()
        {
            Console.Clear();
            Console.WriteLine("=== Inflate Vehicle Tires to Maximum ===");
            Console.Write("Enter the license number: ");
            string licenseNumber = Console.ReadLine();

            try
            {
                r_Garage.InflateVehicleTiresToMax(licenseNumber);
                Console.WriteLine("Tires inflated to maximum air pressure.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void refuelVehicle()
        {
            Console.Clear();
            Console.WriteLine("=== Refuel a Vehicle ===");
            Console.Write("Enter the license number: ");
            string licenseNumber = Console.ReadLine();

            eFuelType selectedFuelType = getEnumSelection<eFuelType>("Select fuel type:");

            Console.Write("Enter amount to refuel: ");
            if (!float.TryParse(Console.ReadLine(), out float amountToRefuel))
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            try
            {
                r_Garage.RefuelVehicle(licenseNumber, selectedFuelType, amountToRefuel);
                Console.WriteLine("Vehicle refueled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void rechargeVehicle()
        {
            Console.Clear();
            Console.WriteLine("=== Recharge a Vehicle ===");
            Console.Write("Enter the license number: ");
            string licenseNumber = Console.ReadLine();

            Console.Write("Enter amount of hours to charge: ");
            if (!float.TryParse(Console.ReadLine(), out float hoursToCharge))
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            try
            {
                r_Garage.RechargeVehicle(licenseNumber, hoursToCharge);
                Console.WriteLine("Vehicle recharged successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
