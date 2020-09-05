using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class Resources : MonoBehaviour {
            static public List<Sprite> tierTextures = new List<Sprite>();
            static public List<Sprite> panelTextures = new List<Sprite>();
            static public List<TMPro.TMP_FontAsset> fonts = new List<TMPro.TMP_FontAsset>();

            static private List<string> tierTextureNames = new List<string>() {
                "textier1bgicon.png",
                "textier2bgicon.png",
                "textier3bgicon.png",
                "texbossbgicon.png",
                "texlunarbgicon.png",
                "texequipmentbgicon.png",
            };
            static private List<string> panelTextureNames = new List<string>() {
                "texUICleanButton.png",
                "texUIOutlineOnly.png",
                "texUIHighlightBoxOutlineThick.png",
                "texUIAnimateSliceNakedButton.png",
                "texUIAnimateSliceNakedButtonCheat.png",
                "texUIHighlightBoxOutlineThickIcon.png",
                "texUIHandle.png",
                "texUIHighlightHeader.png",
                "texUIBottomUpFade.png",
                "texUITopDownFade.png",
            };
            static private List<string> fontNames = new List<string>() {
                "BOMBARD_ SDF.asset",
            };

            static public List<Color> colours = new List<Color>() {
                new Color(193f / 255f, 193f / 255f, 193f / 255f),
                new Color(88f / 255f, 149f / 255f, 88f / 255f),
                new Color(142f / 255f, 50f / 255f, 50f / 255f),
                new Color(189f / 255f, 180f / 255f, 61f / 255f),
                new Color(50f / 255f, 127f / 255f, 255f / 255f),
                new Color(255f / 255f, 128f / 255f, 0f / 255f),
                new Color(0 / 255f, 0 / 255f, 0f / 255f),
            };

            static string modName = "StartingItemsGUI";
            static string assetBundleLocation = "Resources.assets";
            static string assetPrefix = "@Phedg1Studios.StartingItemsGUI";


            static public void LoadResources() {
                string pluginfolder = System.IO.Path.GetDirectoryName(typeof(Resources).Assembly.Location);

                System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(modName + "." + assetBundleLocation);
                AssetBundle bundle = AssetBundle.LoadFromStream(stream);
                R2API.AssetBundleResourcesProvider provider = new R2API.AssetBundleResourcesProvider(assetPrefix, bundle);
                R2API.ResourcesAPI.AddProvider(provider);

                foreach (string tierTextureName in tierTextureNames) {
                    tierTextures.Add(bundle.LoadAsset<Sprite>(tierTextureName));
                }
                foreach (string panelTextureName in panelTextureNames) {
                    panelTextures.Add(bundle.LoadAsset<Sprite>(panelTextureName));
                }
                foreach (string fontName in fontNames) {
                    fonts.Add(bundle.LoadAsset<TMPro.TMP_FontAsset>(fontName));
                }
                bundle.Unload(false);
            }
        }
    }
}
