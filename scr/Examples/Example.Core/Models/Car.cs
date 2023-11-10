namespace Example.Core.Models
{
    public class Car
    {
        public Car()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Model { get; set; }
    }
}