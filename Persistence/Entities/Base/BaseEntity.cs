namespace Persistence.Entities.Base
{
    public class BaseEntity <Tkey, TName>
    {
        public Tkey? Key {  get; set; }
        public required TName Name { get; set; }
        public required bool State { get; set; }

    }
}
