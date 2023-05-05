using UnityEditor;

public class SpritePreProcessor : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        if(assetPath.Contains("UI") || assetPath.Contains("Sprite"))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;

            if(textureImporter.textureType != TextureImporterType.Sprite)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                textureImporter.mipmapEnabled = false;
                textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
            }
        }
    }
}
