namespace WpfApp.Model
{
    public class Country
    {
        public string Description { get; }
        public int Id { get; }

        public Country(string Description, int Id)
        {
            this.Description = Description;
            this.Id = Id;
        }
    }
}