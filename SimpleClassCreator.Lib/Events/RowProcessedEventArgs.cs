using System;

namespace SimpleClassCreator.Lib.Events
{
    public class RowProcessedEventArgs
        : EventArgs
    {
        public int Count { get; set; }
        
        public int Total { get; set; }
    }
}
