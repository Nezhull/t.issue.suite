namespace T.Issue.Commons.Check
{
    public interface ICheck
	{
		/// <summary>
		/// Meta ArgumentNullException, jei objektas yra null.
		/// </summary>
		void NotNull(object obj, string paramName);

		/// <summary>
		/// Meta ArgumentException, jei eilute yra null arba tuscia.
		/// </summary>
		void NotNullEmpty(string s, string paramName);

		/// <summary>
		/// Meta ArgumentException, jei eilute yra null arba sudaryta tik is tarpo simboliu.
		/// </summary>
		void NotNullWhiteSpace(string s, string paramName);

		/// <summary>
		/// Meta ArgumentException, jei eilute yra null, tuscia arba sudaryta tik is tarpo simboliu.
		/// </summary>
		void NotNullEmptyWhiteSpace(string s, string paramName);
	}
}
