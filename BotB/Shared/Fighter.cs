
namespace BotB.Shared
{
    public class Fighter
    {
        string _name = "";

        public string id { get; set; }

        public string ownerId { get; set; }

        public string ownerEmail { get; set; }

        public string Name
        {
            get { return _name; }
            set 
            { 
                if (string.IsNullOrEmpty(value))
                {
                    throw new System.Exception("Fighter name cannot be empty");
                }  
                _name = value; 
            }
        }

        public string Picture { get; set; }

        public string Color { get; set; }

    }
}
