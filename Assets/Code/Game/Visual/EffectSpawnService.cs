using Acoolaum.Core.Services;
using Acoolaum.Game.Level;
using Acoolaum.Game.Model;
using Acoolaum.Game.View;
using UnityEngine;

namespace Acoolaum.Game.Services.Level
{
    public class EffectSpawnService : ServiceBase, ILoaded
    {
        private class SpawnEffectElementActionRunner : IElementActionRunner
        {
            private const string ActionName = "spawn_effect";
            
            private readonly EffectSpawnService _effectSpawnService;

            public SpawnEffectElementActionRunner(EffectSpawnService effectSpawnService)
            {
                _effectSpawnService = effectSpawnService;
            }

            bool IElementActionRunner.CanExecute(string actionString)
            {
                return actionString.StartsWith(ActionName);
            }

            void IElementActionRunner.Execute(ZoneElementModel elementModel, string actionString)
            {
                var parts = actionString.Split(' ');
                var effectId = parts[1];
                _effectSpawnService.Spawn(elementModel.Position, effectId);
            }
        }

        public void Loaded()
        {
            var executeService = ServiceContainer.Get<ElementRunActionService>();
            executeService.RegisterRunner(new SpawnEffectElementActionRunner(this));
        }
        
        private void Spawn(Vector2 position, string effectId)
        {
            var prefab = Resources.Load<EffectView>(effectId);
            var effect = Object.Instantiate(prefab);
            effect.Show(0.01f * position);
        }
    }
}