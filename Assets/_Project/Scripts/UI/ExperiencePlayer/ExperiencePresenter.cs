using _Project.Scripts.Infrastructure.Player;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.UI.ExperiencePlayer
{
    public class ExperiencePresenter : NetworkBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private ExperienceView _experienceView;
        [SerializeField] private LevelView _levelView;
        
        public override void Spawned()
        {
            if (_experienceView != null)
                _experienceView.gameObject.SetActive(HasInputAuthority);
        }

        public override void Render()
        {
            if (HasInputAuthority)
            {
                _experienceView?.UpdateExperienceBar(_character.CurrentExperience, _character.MaxExperience);
                _levelView?.SetLevel(_character.CurrentLevel);
            }
        }
    }
}