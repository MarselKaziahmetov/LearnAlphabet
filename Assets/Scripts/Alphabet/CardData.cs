using System;
using UnityEngine;

namespace LearnAlphabet
{
    [Serializable]
    public class CardData 
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _sprite;

        public string Id => _id;
        public Sprite Sprite => _sprite;
    }
}