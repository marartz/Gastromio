namespace FoodOrderSystem.Domain.Commands.AddPaymentMethod
{
    public class AddPaymentMethodCommand : ICommand
    {
        public AddPaymentMethodCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}
