namespace T.Issue.Commons.Enum
{
    /// <summary>
    /// Base enum class for simulating Java style enums with label.
    /// </summary>
    /// <typeparam name="T">Actual enum type.</typeparam>
    public abstract class LabeledEnum<T> : EnumBase<T> where T : LabeledEnum<T>
    {
        public virtual string Label { get; }

        protected LabeledEnum(int value, string label) : base(value)
        {
            Label = label;
        }

        public override string ToString()
        {
            return Label;
        }
    }
}