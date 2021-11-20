namespace SimpleClassCreator.Ui.Profile
{
    public class UserConnectionString
    {
        public string ConnectionString { get; set; }

        public bool Verified { get; set; }

        public override string ToString()
        {
            return ConnectionString;
        }
    }
}
