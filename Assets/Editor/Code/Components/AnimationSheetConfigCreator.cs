using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Acoolaum.Core.Components.Editor
{
    public class AnimationSheetConfigCreator
    {
        [MenuItem("Assets/Create/" + nameof(AnimationSheetConfig) +" from sprites")]
        private static void CreateAnimationSheet()
        {
            if (Selection.activeObject is Texture2D texture2D == false)
            {
                return;
            }

            var texture2DPath = AssetDatabase.GetAssetPath(texture2D);

            var sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(texture2DPath);
            if (sprites != null)
            {
                var animationSheet = ScriptableObject.CreateInstance<AnimationSheetConfig>();
                animationSheet.SetSprites(sprites.Select(s => (Sprite)s).ToList());
                
                var folder = Path.GetDirectoryName(texture2DPath);
                var name = Path.GetFileNameWithoutExtension(texture2DPath);
                var animationSheetPath = Path.Combine(folder, $"{name}.asset");
                AssetDatabase.CreateAsset(animationSheet, animationSheetPath);
                Debug.Log($"Animation sheet created at '{animationSheetPath}'");
            }
        }
        
        [MenuItem("Assets/Create/" + nameof(AnimationSheetConfig) +" from sprites", true)]
        private static bool CreateAnimationSheetValidation()
        {
            if (Selection.activeObject is Texture2D texture2D == false)
            {
                return false;
            }

            var path = AssetDatabase.GetAssetPath(texture2D);

            var sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
            if (sprites != null)
            {
                return true;
            }

            return false;
        }
    }
}