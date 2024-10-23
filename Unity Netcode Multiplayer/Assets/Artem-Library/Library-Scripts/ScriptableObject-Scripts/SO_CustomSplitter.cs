using System;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "Splitter", menuName = "Custom Data/ Splitter List", order = 0)]
    public class SO_CustomSplitter : ScriptableObject
    {
        public List<Custom_Splitter> _customSplitters;
    }
    
    [Serializable]
    public class Custom_Splitter
    {
        [field: SerializeField] public string SplitterName { get; set; }
        public Texture2D SplitterTexture;
    }
 
 

 


