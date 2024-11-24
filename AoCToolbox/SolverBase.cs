using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCToolbox
{
    public abstract class SolverBase
    {
        public const string DayStringFormat = "{0:D2}";
        public const string ProblemNotSolvedString = "Problem not solved!";

        public virtual int Parts => 2;

        public bool LogsEnabled { get; set; }
        public bool HasVisualization { get; set; }
        public string InputPath { get; set; } = string.Empty;

        /// <summary>
        /// Run the specified Solution <paramref name="part"/>
        /// </summary>
        /// <param name="part">The one-based solution part</param>
        /// <returns>The solution part result</returns>
        public abstract object Run(int part);
        public abstract string GetDayString();
        public abstract string GetProblemName();
        public abstract string GetDivider();
        public abstract void ShowVisualization();

        protected string[] GetInputLines()
        {
            AssertInputExists();
            return File.ReadAllLines(InputPath);
        }

        protected string GetInputText()
        {
            AssertInputExists();
            return File.ReadAllText(InputPath).TrimEnd();
        }

        protected StreamReader GetInputStream()
        {
            AssertInputExists();
            return new StreamReader(InputPath);
        }

        protected IEnumerable<T> ParseInputLines<T>(Func<string, T> parseFunc)
        {
            return GetInputLines().Select(parseFunc);
        }

        private void AssertInputExists()
        {
            Debug.Assert(condition: InputFileExists(), message: $"Input file does not exist [{InputPath}]");
        }

        private bool InputFileExists()
        {
            return File.Exists(InputPath);
        }
    }
}
