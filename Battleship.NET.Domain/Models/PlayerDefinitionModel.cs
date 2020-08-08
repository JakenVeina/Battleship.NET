namespace Battleship.NET.Domain.Models
{
    public class PlayerDefinitionModel
    {
        public static PlayerDefinitionModel Create(
                string name)
            => new PlayerDefinitionModel(
                name);

        public PlayerDefinitionModel(
            string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
