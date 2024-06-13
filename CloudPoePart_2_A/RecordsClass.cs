namespace CloudPoePart_2_A
{
    public class RecordsClass
    {
        /// <summary>
        /// Store ID or Passport Number
        /// </summary>
        public string Id { get; set; }
      
        /// <summary>
        /// Store First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Store Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Store Date of Birth
        /// </summary>
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Store Gender
        /// </summary>
        public string Gender { get; set; }
       
        ///----------------------------------------------------------------///
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RecordsClass()
        {

        }

        ///----------------------------------------------------------------///
        /// <summary>
        /// Parametized Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vacineNumber"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="gender"></param>
        public RecordsClass(string id, string firstName, string lastName, string dateOfBirth, string gender)
        {
            Id = id;           
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;           
        }
    }
}
//--------------------------------------------------< END >------------------------------------------------//