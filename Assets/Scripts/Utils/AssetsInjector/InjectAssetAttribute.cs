using System;

namespace Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAssetAttribute : Attribute
    {
        public readonly string AssetName;
        public InjectAssetAttribute(string assetName = null)
        {
            AssetName = assetName;
        }
    }

}


