using UnityEngine;

namespace JavacLMD.Utils.Attributes
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
        [SerializeField]
        private bool _toggle = true;
        

        public ReadOnlyAttribute(bool toggle = true)
        {
            _toggle = toggle;
        }
        
    }
}