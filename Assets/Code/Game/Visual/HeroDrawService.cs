using Acoolaum.Core.Services;
using Acoolaum.Game.Hero;
using Acoolaum.Game.Level;
using Acoolaum.Game.Model;
using Acoolaum.Game.View;
using UnityEngine;

namespace Acoolaum.Game.Services.Level
{
    public class HeroDrawService : ServiceBase, ILoaded, IUpdate
    {
        private HeroView _heroView;
        private HeroRunService _heroRunService;
        private LevelModelService _levelModel;

        void ILoaded.Loaded()
        {
            _heroRunService = ServiceContainer.Get<HeroRunService>();
            
            _levelModel = ServiceContainer.Get<LevelModelService>();
            _levelModel.OnHeroAdded += OnHeroAdded;

            ServiceContainer.Get<HeroHealthService>().HeroDied += OnHeroDie;

            var hero = _levelModel.LevelModel.Hero;
            if (hero != null)
            {
                AddHeroView(hero);
            }
        }

        void IUpdate.Update(float delta)
        {
            if (_heroView == null)
            {
                return;
            }

            var model = _levelModel.LevelModel.Hero;
            var view = _heroView;
            view.SetPosition(0.01f * model.CurrentPosition);
            if (model.OnGround)
            {
                if (_heroRunService.GetHeroHorizontalSpeed() > 0f)
                {
                    view.Play("run");    
                }
                else
                {
                    view.Play("idle");
                }
            }
            else
            {
                if (model.CurrentVerticalSpeed > 0f)
                {
                    view.Play("jump");    
                }
                else
                {
                    view.Play("fall");
                }
            }
        }
        
        void IService.Dispose()
        {
            DestroyHeroView();
        }

        private void OnHeroAdded(HeroModel heroModel)
        {
            AddHeroView(heroModel);
        }
        
        private void OnHeroDie()
        {
            DestroyHeroView();
        }

        private void AddHeroView(HeroModel heroModel)
        {
            var prefab = Resources.Load<HeroView>(heroModel.HeroConfig.ViewId);
            var view = Object.Instantiate(prefab);
            view.SetPosition(0.01f * heroModel.CurrentPosition);
            _heroView = view;
        }
        
        private void DestroyHeroView()
        {
            if (_heroView != null)
            {
                Object.Destroy(_heroView.gameObject);
                _heroView = null;
            }
        }
    }
}