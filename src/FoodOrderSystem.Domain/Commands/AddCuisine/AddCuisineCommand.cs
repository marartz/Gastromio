namespace FoodOrderSystem.Domain.Commands.AddCuisine
{
    public class AddCuisineCommand : ICommand
    {
        public AddCuisineCommand(string name, byte[] image)
        {
            Name = name;
            Image = image;
        }

        public string Name { get; }
        public byte[] Image { get; }
    }
}
