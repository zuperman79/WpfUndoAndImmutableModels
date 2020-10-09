namespace WpfApp.Model
{
    public interface IStateful
    {
        void RestorePreviousState(string propertyName, object oldState, object newState);
    }
}