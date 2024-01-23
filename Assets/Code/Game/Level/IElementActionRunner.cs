using Acoolaum.Game.Model;

namespace Acoolaum.Game.Level
{
    public interface IElementActionRunner
    {
        bool CanExecute(string actionString);
        void Execute(ZoneElementModel element, string actionString);
    }
}