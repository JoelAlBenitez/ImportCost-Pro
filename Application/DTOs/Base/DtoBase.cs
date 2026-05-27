namespace Application.DTOs.Base
{
    public abstract class DtoBase
    {
        public int Key { get; set; }
        public required string Name { get; set; }
        public required bool State { get; set; }
    }
}