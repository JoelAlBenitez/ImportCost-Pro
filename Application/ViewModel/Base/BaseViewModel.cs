namespace Application.ViewModel.Base
{
    public abstract class BaseViewModel <Tkey, TName>
    {
        public required Tkey key { get; set; }
        public required TName Name { get; set; }
        public required bool State {  get; set; }
    }
}
