namespace FoodOrderSystem.Core.Application.Commands.AddTestData
{
    public class AddTestDataCommand : ICommand<bool>
    {
        public AddTestDataCommand(int userCount, int restCount, int dishCatCount, int dishCount)
        {
            UserCount = userCount;
            RestCount = restCount;
            DishCatCount = dishCatCount;
            DishCount = dishCount;
        }
        
        public int UserCount { get; }

        public int RestCount { get; }

        public int DishCatCount { get; }
        
        public int DishCount { get; }

    }
}
