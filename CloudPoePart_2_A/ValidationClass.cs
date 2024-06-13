using System.Globalization;

namespace CloudPoePart_2_A
{
    public class ValidationClass
    {
        ///----------------------------------------------------------------///
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ValidationClass()
        {

        }

        ///----------------------------------------------------------------///
        /// <summary>
        /// Method to check for valid SA passport number
        /// Checks if string is 9 characters long
        /// Checks if first character is 'A', 'E' or 'M' 
        /// Checks if second character is 0
        /// Checks if the next 8 characters are all digits
        /// If conditions are not met it returns false
        /// </summary>
        /// <param name="passport"></param>
        /// <returns></returns>
        public bool CheckValidPassport(string passport)
        {
            if (passport.Length != 9)
            {
                return false;
            }

            if (!((passport[0] == 'A' && passport[1] == '0') ||
                (passport[0] == 'E' && passport[1] == '0') ||
                (passport[0] == 'M' && passport[1] == '0')))
            {
                return false;
            }

            for (int i = 1; i < passport.Length; i++)
            {
                if (!char.IsDigit(passport[i]))
                {
                    return false;
                }
            }

            return true;
        }

        ///----------------------------------------------------------------///
        /// <summary>
        /// Method to check for valid Id number
        /// Check if string is 13 characters long
        /// Check if the 13 characters are all digits
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckValidID(string id)
        {
            if (id.Length != 13)
            {
                return false;
            }

            for (int i = 0; i < id.Length; i++)
            {
                if (!char.IsDigit(id[i]))
                {
                    return false;
                }
            }

            return true;
        }

        ///----------------------------------------------------------------///
        /// <summary>
        /// Method to check if Vaccination date format is coorect
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool CheckVaccinationDate(string date)
        {
            DateTime tempDate;
            return DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate);
        }

        ///----------------------------------------------------------------///
        /// <summary>
        /// Method to check if serial number is digit and 10 characters long
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public bool CheckVaccineSerialNumber(string serialNumber)
        {
            if (serialNumber.Length != 10)
            {
                return false;
            }

            return serialNumber.All(char.IsDigit);
        }

        ///----------------------------------------------------------------///
        /// <summary>
        /// Method to check user Input is in correct format for Serial Number 
        /// and validate data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool CheckUserInputSerialNumber(string input)
        {
            bool Valid = false;

            string[] parts = input.Split(':');

            if (parts.Length == 4)
            {
                string VaccinationCenter = parts[1];
                string VaccinationDate = parts[2];
                string VaccineSerialNumber = parts[3];

                //Check valid date format and vaccine serial number
                if(CheckVaccinationDate(VaccinationDate).Equals(true) && CheckVaccineSerialNumber(VaccineSerialNumber).Equals(true))
                {
                    //Check if Vaccination center is not null
                    if (!string.IsNullOrEmpty(VaccinationCenter))
                    {
                        Valid = true;
                    }
                } 
                else
                {
                    Valid = false;
                }
            }
            else
            {
                Valid = false;
            }
         
            return Valid;
        }

        ///----------------------------------------------------------------///
        /// <summary>
        /// Method to check user Input is in correct format for Barcode 
        /// and validate data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool CheckUserInputBarCode(string input)
        {           
            bool Valid = false;

            string[] parts = input.Split(':');

            if (parts.Length == 4)
            {
                string VaccineBarCode = parts[0];
                string VaccinationDate = parts[1];
                string VaccinationCenter = parts[2];

                //Check valid date format and vaccine serial number
                if (CheckVaccinationDate(VaccinationDate).Equals(true) && CheckVaccineBarCode(VaccineBarCode).Equals(true))
                {
                    //Check if Vaccination center is not null
                    if (!string.IsNullOrEmpty(VaccinationCenter))
                    {
                        Valid = true;
                    }
                }
                else
                {
                    Valid = false;
                }
            }
            else
            {
                Valid = false;
            }
   
            return Valid;
        }

        ///----------------------------------------------------------------///
        /// <summary>
        /// Method to check if barcode is digit and 12 characters long
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public bool CheckVaccineBarCode(string barCode)
        {
            if (barCode.Length != 12)
            {
                return false;
            }

            return barCode.All(char.IsDigit);
        }

        ///----------------------------------------------------------------///
        /// <summary>
        /// Method to return user choice 
        /// either Barcode or Serial number
        /// </summary>
        /// <returns></returns>
        public bool GetUserFormatChoice(string Input)
        {
            if (CheckUserInputSerialNumber(Input).Equals(true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
//--------------------------------------------------< END >------------------------------------------------//