using System.Collections.Generic;
using Acoolaum.Core.Services;
using Acoolaum.Game.Model;

namespace Acoolaum.Game.Level
{
    public class ElementRunActionService : IService
    {
        private readonly List<IElementActionRunner> _actionRunners = new();

        public void RegisterRunner(IElementActionRunner actionRunner)
        {
            _actionRunners.Add(actionRunner);
        }
        
        public void Execute(ZoneElementModel element, string[] actions)
        {
            for (var i = 0; i < actions.Length; i++)
            {
                var action = actions[i];
                for (var f = 0; f < _actionRunners.Count; f++)
                {
                    var actionRunner = _actionRunners[f];
                    if (actionRunner.CanExecute(action))
                    {
                        actionRunner.Execute(element, action);
                    }
                }
            }
        }
    }
}