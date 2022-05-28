using System.Collections.Generic;

namespace WreckMyHouse.Core.Models
{
    public class Result<T>
    {
        public Result(string message = "")
        {
            Message = message;
        }
        private List<string> messages = new List<string>();
        public bool Success => messages.Count == 0;
        public List<string> Messages => new List<string>(messages);
        public T Value { get; set; }
        public string Message { get; set; }

        public void AddMessage(string message)
        {
            messages.Add(message);
        }
    }
}
