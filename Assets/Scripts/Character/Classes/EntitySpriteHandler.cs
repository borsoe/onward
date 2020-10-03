using System.Collections;
using Onward.Character.ScriptableObjects;
using UnityEngine;

namespace Onward.Character.Classes
{
    /// <summary>
    /// component that will handle the entities sprite data and functionality 
    /// </summary>
    public class EntitySpriteHandler
    {
        #region setup

        private readonly Sprite _sprite;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Color _originalColor;

        public EntitySpriteHandler(ChampionData championData, SpriteRenderer spriteRenderer)
        {
            _sprite = championData.characterSprite;
            _spriteRenderer = spriteRenderer;
            _originalColor = _spriteRenderer.color;
        }

        #endregion

        #region methodes

        public void Init()
        {
            _spriteRenderer.sprite = _sprite;
        }

        #endregion

        #region Coroutines

        public IEnumerator Flicker(Color targetColor, float speed, float duration)
        {
            float totalTime = 0f;
            while (true)
            {
                var color = _spriteRenderer.color;
                color = color == targetColor ? _originalColor : targetColor;
                _spriteRenderer.color = color;
                yield return new WaitForSeconds(speed);
                totalTime += speed;
                if (totalTime >= duration) break;
            }
            _spriteRenderer.color = _originalColor;
            yield return new WaitForEndOfFrame();
        }

        #endregion
    }
}