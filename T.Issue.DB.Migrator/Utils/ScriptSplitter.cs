using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace T.Issue.DB.Migrator.Utils
{
    internal class ScriptSplitter : IEnumerable<string>
    {
        private readonly TextReader reader;
        private StringBuilder builder = new StringBuilder();
        private char current;
        private char lastChar;
        private ScriptReader scriptReader;

        public ScriptSplitter(string script)
        {
            reader = new StringReader(script);
            scriptReader = new SeparatorLineReader(this);
        }

        internal bool HasNext
        {
            get { return reader.Peek() != -1; }
        }

        internal char Current
        {
            get { return current; }
        }

        internal char LastChar
        {
            get { return lastChar; }
        }

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            while (Next())
            {
                if (Split())
                {
                    string script = builder.ToString().Trim();
                    if (script.Length > 0)
                    {
                        yield return (script);
                    }
                    Reset();
                }
            }
            if (builder.Length > 0)
            {
                string scriptRemains = builder.ToString().Trim();
                if (scriptRemains.Length > 0)
                {
                    yield return (scriptRemains);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        internal bool Next()
        {
            if (!HasNext)
            {
                return false;
            }

            lastChar = current;
            current = (char)reader.Read();
            return true;
        }

        internal int Peek()
        {
            return reader.Peek();
        }

        private bool Split()
        {
            return scriptReader.ReadNextSection();
        }

        internal void SetParser(ScriptReader newReader)
        {
            scriptReader = newReader;
        }

        internal void Append(string text)
        {
            builder.Append(text);
        }

        internal void Append(char c)
        {
            builder.Append(c);
        }

        void Reset()
        {
            current = lastChar = char.MinValue;
            builder = new StringBuilder();
        }
    }

    internal abstract class ScriptReader
    {
        protected readonly ScriptSplitter Splitter;

        protected ScriptReader(ScriptSplitter splitter)
        {
            Splitter = splitter;
        }

        /// <summary>
        /// This acts as a template method. Specific Reader instances 
        /// override the component methods.
        /// </summary>
        public bool ReadNextSection()
        {
            if (IsQuote)
            {
                ReadQuotedString();
                return false;
            }

            if (BeginDashDashComment)
            {
                return ReadDashDashComment();
            }

            if (BeginSlashStarComment)
            {
                ReadSlashStarComment();
                return false;
            }

            return ReadNext();
        }

        protected virtual bool ReadDashDashComment()
        {
            Splitter.Append(Current);
            while (Splitter.Next())
            {
                Splitter.Append(Current);
                if (EndOfLine)
                {
                    break;
                }
            }
            //We should be EndOfLine or EndOfScript here.
            Splitter.SetParser(new SeparatorLineReader(Splitter));
            return false;
        }

        protected virtual void ReadSlashStarComment()
        {
            if (ReadSlashStarCommentWithResult())
            {
                Splitter.SetParser(new SeparatorLineReader(Splitter));
            }
        }

        private bool ReadSlashStarCommentWithResult()
        {
            Splitter.Append(Current);
            while (Splitter.Next())
            {
                if (BeginSlashStarComment)
                {
                    ReadSlashStarCommentWithResult();
                    continue;
                }
                Splitter.Append(Current);

                if (EndSlashStarComment)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void ReadQuotedString()
        {
            Splitter.Append(Current);
            while (Splitter.Next())
            {
                Splitter.Append(Current);
                if (IsQuote)
                {
                    return;
                }
            }
        }

        protected abstract bool ReadNext();

        #region Helper methods and properties

        protected bool HasNext
        {
            get { return Splitter.HasNext; }
        }

        protected bool WhiteSpace
        {
            get { return char.IsWhiteSpace(Splitter.Current); }
        }

        protected bool EndOfLine
        {
            get { return '\n' == Splitter.Current; }
        }

        protected bool IsQuote
        {
            get { return '\'' == Splitter.Current; }
        }

        protected char Current
        {
            get { return Splitter.Current; }
        }

        protected char LastChar
        {
            get { return Splitter.LastChar; }
        }

        bool BeginDashDashComment
        {
            get { return Current == '-' && Peek() == '-'; }
        }

        bool BeginSlashStarComment
        {
            get { return Current == '/' && Peek() == '*'; }
        }

        bool EndSlashStarComment
        {
            get { return LastChar == '*' && Current == '/'; }
        }

        protected static bool CharEquals(char expected, char actual)
        {
            return Char.ToLowerInvariant(expected) == Char.ToLowerInvariant(actual);
        }

        protected bool CharEquals(char compare)
        {
            return CharEquals(Current, compare);
        }

        protected char Peek()
        {
            if (!HasNext)
            {
                return char.MinValue;
            }
            return (char)Splitter.Peek();
        }

        #endregion
    }
}
