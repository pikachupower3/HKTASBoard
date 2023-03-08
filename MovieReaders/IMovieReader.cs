using System;
using System.Collections.Generic;

namespace TASBoardConsole.MovieReaders
{
    public interface IMovieReader : IEnumerable<InputFrame>
    {
        void Close();

        MovieProperties MovieProperties { get; }

        public static IMovieReader ReturnReaderByExtension(string fname)
        {
            if (fname.EndsWith(".ltm"))
                return new LibTASReader(fname);

            throw new ArgumentException("Unrecognised filetype");
        }

        public static bool IsValidFile(string? fname)
        {
            return fname != null && fname.EndsWith(".ltm"); // Can add more with || later
        }
    }
}
