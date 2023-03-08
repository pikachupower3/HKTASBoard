namespace TASBoardConsole.MovieReaders
{
    public readonly struct MovieProperties
    {
        public readonly int FramerateNum;
        public readonly int FramerateDen;
        public readonly int Length;
        public readonly bool IsVariableFramerate;
        public MovieProperties(
            int framerateNum,
            int framerateDen, 
            int length,
            bool isVariableFramerate)
        {
            FramerateNum = framerateNum;
            FramerateDen = framerateDen;
            Length = length;
            IsVariableFramerate = isVariableFramerate;
        }
    }
}