using System;
using System.Linq;

namespace T.Issue.Commons.Utils
{
    /// <summary>
    /// Utility class for LT personal code.
    /// </summary>
    public static class PersonalCodeUtils {

	    private static readonly int PERSONAL_CODE_LENGTH = 11;
	    private static readonly int[] CHECK_SUM_1_WEIGHTS = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
	    private static readonly int[] CHECK_SUM_2_WEIGHTS = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };
	    private static readonly string EMBEDDED_DATE_FORMAT = "yyMMdd";

        /// <summary>
        /// Gets the date of birth contained in a lithuanian personal code.
        /// </summary>
        /// <param name="personalCode">Personal code.</param>
        /// <returns>The date of birth or <i>null</i> if the personal code is invalid.</returns>
	    public static DateTime? GetDateOfBirth(string personalCode) {
		    Assert.NotNull(personalCode, "Parameter personalCode is required");

		    if (!IsValid(personalCode)) {
			    return null;
		    }

		    string majorYears = GetMajorYears(personalCode);
		    string date = personalCode.Substring(1, EMBEDDED_DATE_FORMAT.Length);
		    return majorYears == null ? null : DateTimeUtils.ParseDateTime(majorYears + date, "yy" + EMBEDDED_DATE_FORMAT);
	    }

	    /**
	     * Validates a lithuanian personal code using the embedded control digit
	     * @param personalCode the personal code to validate
	     * @return whether the code is valid or not
	     */
	    public static bool IsValid(string personalCode) {
		    Assert.NotNull(personalCode, "Parameter personalCode is required");
		    //Structure: LYYMMDDXXXK
		    //    L - gender
		    //    YYMMDD - date of birth
		    //    XXX - sequence number
		    //    K - control digit

		    if (personalCode.Length != PERSONAL_CODE_LENGTH || !personalCode.All(char.IsNumber)) {
			    return false;
		    }

		    int checkSum1 = 0;
		    int checkSum2 = 0;

		    for (int i = 0; i < PERSONAL_CODE_LENGTH - 1; i++) { //iterate over all except control
			    int digit = (int) char.GetNumericValue(personalCode[i]);
			    checkSum1 += digit * CHECK_SUM_1_WEIGHTS[i];
			    checkSum2 += digit * CHECK_SUM_2_WEIGHTS[i];
		    }

		    int control = (int) char.GetNumericValue(personalCode[PERSONAL_CODE_LENGTH - 1]);
		    int testControl = checkSum1 % 11;

		    if (testControl == 10) {
			    int mod2 = checkSum2 % 11;
			    testControl = mod2 == 10 ? 0 : mod2;
		    }

		    return control == testControl;
	    }

	    private static string GetMajorYears(string personalCode) {
		    char gender = personalCode[0];
		    if (gender == '1' || gender == '2') {
			    return "18";
		    }
		    if (gender == '3' || gender == '4') {
			    return "19";
		    }
		    if (gender == '5' || gender == '6') {
			    return "20";
		    }
		    return null;
	    }
    }
}
