using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudPoePart_2_A
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=queuetriggerst10025374;AccountKey=/klJ15C9P/Uq47Rg3EFhzE4kc1QkRxX" +
                                      "OVraYPPZECAoVK2TB/K8tOwbwELJ5dBC0V6I1dttdE1LX+ASteQ23hA==;EndpointSuffix=core.windows.net";
            string QueueName = "vaccinemessage";

            ValidationClass Validate = new ValidationClass();
            
            //Records of Passports and IDs
            Dictionary<string, RecordsClass> records = new Dictionary<string, RecordsClass>
            {
                { "A04108234", new RecordsClass( "A04108234", "Emily", "Thompson", "02/12/1986", "Female")},
                { "E04439245", new RecordsClass( "E04439245", "Jonathan", "White", "22/07/1990", "Male")},
                { "M04389256", new RecordsClass("M04389256", "Benjamin", "Carter", "03/03/1982", "Male") },
                { "9306125183012", new RecordsClass("9306125183012", "Nicole", "Johnson", "29/09/1995", "Female") },
                { "8407216123019", new RecordsClass("8407216123019", "Victoria", "Perez", "18/11/1989", "Female") },
                { "A04098378", new RecordsClass("A04098378", "Christopher", "Garcia", "15/05/1987", "Male") },
                { "A04918237", new RecordsClass("A04918237", "Samantha", "Davis", "21/04/1994", "Female") },
                { "A04829248", new RecordsClass("A04829248", "Brandon", "Nelson", "04/10/1992", "Male") },
                { "A04789259", new RecordsClass("A04789259", "Grace", "Hall", "07/08/1991", "Female") },
                { "7001012043017", new RecordsClass("7001012043017", "Joshua", "Baker", "02/12/1986", "Male") },
                { "8505053073011", new RecordsClass("8505053073011", "Rachel", "Martin", "26/06/1990", "Female") },
                { "7609107583014", new RecordsClass("7609107583014", "Hannah", "Young", "13/03/1989", "Female") },
            };

            Console.WriteLine("----- VACCINE VALIDATION SYSTEM -----");

            string Input = string.Empty;

            //Store info to be sent to Queue
            string Send = string.Empty;

            do
            {
                //Ask user Input
                Console.WriteLine("\nProvide the data for the id or passport record (Type X to exit)" +
                                  "\nNote: Type the data in this formats (Id:VaccinationCenter:VaccinationDate[dd/mm/yyyy]:VaccineSerialNumber)" +
                                  "\n                                    (VaccineBarcode:VaccinationDate[dd/mm/yyyy]:VaccinationCenter:Id)\n");
                
                //Get Input
                Input = Console.ReadLine();

                //Check for Null and Empty
                //Return message in case validation fails
                if (string.IsNullOrEmpty(Input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nPlease provide the data", Console.ForegroundColor);
                    Console.ResetColor();
                }
                //In case user enters X exit the program
                else if (Input.ToUpper().Equals("X"))
                {
                    break;
                }
                else if (!string.IsNullOrEmpty(Input))
                {
                    //Check if Input format is correct. If not correct loop again
                    if (Validate.CheckUserInputSerialNumber(Input).Equals(true) || Validate.CheckUserInputBarCode(Input).Equals(true))
                    {                        
                        string Id = string.Empty;
                        string VaccinationCenter = string.Empty;
                        string VaccinationDate = string.Empty;
                        string VaccineSerialNumber = string.Empty;
                        string VaccineBarCode = string.Empty;

                        //Check if user used barcode or serial number
                        bool choice = Validate.GetUserFormatChoice(Input);

                        //For Serial Number
                        if (choice.Equals(true))
                        {
                            //Split Input
                            string[] parts = Input.Split(':');

                            // Assigning parts to variables
                            Id = parts[0];
                            VaccinationCenter = parts[1];
                            VaccinationDate = parts[2];
                            VaccineSerialNumber = parts[3];

                            // Check if the entered ID or Passport Number exists in the dictionary and get the corresponding value
                            if (records.TryGetValue(Id, out RecordsClass matchedRecord))
                            {
                                if (matchedRecord.Id.Equals(Id))
                                {
                                    //Display full info to user including added data
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"\nRecord:                    {Id}" +
                                                      $"\nVaccine serial number:     {VaccineSerialNumber}" +
                                                      $"\nFirst name:                {matchedRecord.FirstName} " +
                                                      $"\nSurname:                   {matchedRecord.LastName}" +
                                                      $"\nDate of birth:             {matchedRecord.DateOfBirth}" +
                                                      $"\nGender:                    {matchedRecord.Gender}" +
                                                      $"\nVaccination center:        {VaccinationCenter}" +
                                                      $"\nVaccination date:          {VaccinationDate}", Console.ForegroundColor);
                                    Console.ResetColor();

                                    Send = $"{Id}:{VaccinationCenter}:{VaccinationDate}:{VaccineSerialNumber}";

                                    //Connect to the Azure Storage Queue
                                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                                    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                                    CloudQueue queue = queueClient.GetQueueReference(QueueName);

                                    // Ensure the queue exists
                                    await queue.CreateIfNotExistsAsync();

                                    // Save to Azure Storage Queue
                                    CloudQueueMessage message = new CloudQueueMessage(Send);
                                    await queue.AddMessageAsync(message);
                                    
                                    Console.WriteLine("\nData saved to Azure Queue.");
                                }
                            }
                            else
                            {
                                //Check if Id or Passport format corresponds to the format of SA Passport and ID
                                if (Validate.CheckValidPassport(Id).Equals(true) || Validate.CheckValidID(Id).Equals(true))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"\nRecord: {Id}   Status: Not vaccinated", Console.ForegroundColor);
                                    Console.ResetColor();
                                }
                                //In case Id or passport format is not valid display message
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nInvalid Passport or Id number format entered", Console.ForegroundColor);
                                    Console.ResetColor();
                                }
                            }
                        }

                        // For BarCode
                        else
                        {   
                            //Split Input
                            string[] parts2 = Input.Split(':');

                            VaccineBarCode = parts2[0];
                            VaccinationDate = parts2[1];
                            VaccinationCenter = parts2[2];
                            Id = parts2[3];

                            // Check if the entered ID or Passport Number exists in the dictionary and get the corresponding value
                            if (records.TryGetValue(Id, out RecordsClass matchedRecord))
                            {
                                if (matchedRecord.Id.Equals(Id))
                                {
                                    //Display full info to user including added data
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"\nRecord:                    {Id}" +
                                                      $"\nVaccine barcode:           {VaccineBarCode}" +
                                                      $"\nFirst name:                {matchedRecord.FirstName} " +
                                                      $"\nSurname:                   {matchedRecord.LastName}" +
                                                      $"\nDate of birth:             {matchedRecord.DateOfBirth}" +
                                                      $"\nGender:                    {matchedRecord.Gender}" +
                                                      $"\nVaccination center:        {VaccinationCenter}" +
                                                      $"\nVaccination date:          {VaccinationDate}", Console.ForegroundColor);
                                    Console.ResetColor();

                                    Send = $"{VaccineBarCode}:{VaccinationDate}:{VaccinationCenter}:{Id}";

                                    //Connect to the Azure Storage Queue
                                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                                    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                                    CloudQueue queue = queueClient.GetQueueReference(QueueName);

                                    // Ensure the queue exists
                                    await queue.CreateIfNotExistsAsync();

                                    // Save to Azure Storage Queue
                                    CloudQueueMessage message = new CloudQueueMessage(Send);
                                    await queue.AddMessageAsync(message);
                                    
                                    Console.WriteLine("\nData saved to Azure Queue.");
                                }
                            }
                            else
                            {
                                //Check if Id or Passport format corresponds to the format of SA Passport and ID
                                if (Validate.CheckValidPassport(Id).Equals(true) || Validate.CheckValidID(Id).Equals(true))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"\nRecord: {Id}   Status: Not vaccinated", Console.ForegroundColor);
                                    Console.ResetColor();
                                }
                                //In case Id or passport format is not valid display message
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\nInvalid Passport or Id number format entered", Console.ForegroundColor);
                                    Console.ResetColor();
                                }
                            }
                        }                     
                    }
                    //In case user enters incorrect data or vaccine serial number format message will be displayed
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid data entered", Console.ForegroundColor);
                        Console.ResetColor();
                    }
                }                                   
            }while(!Input.ToUpper().Equals("X"));
            
            Console.WriteLine("\nSystem will be terminated...");
            Console.ReadLine();
        }
    }
}
//------------------------------------------------------< END >-----------------------------------------------------------//