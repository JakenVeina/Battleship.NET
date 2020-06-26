namespace Battleship.NET.Domain.Models
{
    public class PlayerDefinitionModel
    {
        public PlayerDefinitionModel(
            string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
