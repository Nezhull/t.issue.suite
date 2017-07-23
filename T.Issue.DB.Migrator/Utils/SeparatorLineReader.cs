using System;
using System.Text;

namespace T.Issue.DB.Migrator.Utils
{
    internal class SeparatorLineReader : ScriptReader
    {
        private StringBuilder builder = new StringBuilder();
        private bool foundGo;
        private bool gFound;

        public SeparatorLineReader(ScriptSplitter splitter)
            : base(splitter)
        {
        }

        void Reset()
        {
            foundGo = false;
            gFound = false;
            builder = new StringBuilder();
        }

        protected override bool ReadDashDashComment()
        {
            if (!foundGo)
            {
                base.ReadDashDashComment();
                return false;
            }
            base.ReadDashDashComment();
            return true;
        }

        protected override void ReadSlashStarComment()
        {
            if (foundGo)
            {
                throw new Exception(@"Incorrect syntax was encountered while parsing GO. Cannot have a slash star /* comment */ after a GO statement.");
            }
            base.ReadSlashStarComment();
        }

        protected override bool ReadNext()
        {
            if (EndOfLine) //End of line or script
            {
                if (!foundGo)
                {
                    builder.Append(Current);
                    Splitter.Append(builder.ToString());
                    Splitter.SetParser(new SeparatorLineReader(Splitter));
                    return false;
                }
                Reset();
                return true;
            }

            if (WhiteSpace)
            {
                builder.Append(Current);
                return false;
            }

            if (!CharEquals('g') && !CharEquals('o'))
            {
                FoundNonEmptyCharacter(Current);
                return false;
            }

            if (CharEquals('o'))
            {
                if (CharEquals('g', LastChar) && !foundGo)
                {
                    foundGo = true;
                }
                else
                {
                    FoundNonEmptyCharacter(Current);
                }
            }

            if (CharEquals('g', Current))
            {
                if (gFound || (!char.IsWhiteSpace(LastChar) && LastChar != char.MinValue))
                {
                    FoundNonEmptyCharacter(Current);
                    return false;
                }

                gFound = true;
            }

            if (!HasNext && foundGo)
            {
                Reset();
                return true;
            }

            builder.Append(Current);
            return false;
        }

        void FoundNonEmptyCharacter(char c)
        {
            builder.Append(c);
            Splitter.Append(builder.ToString());
            Splitter.SetParser(new SqlScriptReader(Splitter));
        }
    }
}