using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArtStuff
{

    public class GroundShaderController : MonoBehaviour
    {

        public Texture2D GeneralLogo;
        public GameObject TerrainObject;
        public Material TerrainMaterial;


        public Texture2D Mask1;
        // Texture1
        public Texture2D Texture1;
        public Vector3 Texture1Tile;
        public Color Texture1Color;
        public float Texture1Intensity;
        public float Texture1Saturation;
        // Texture2
        public Texture2D Texture2;
        public Vector3 Texture2Tile;
        public Color Texture2Color;
        public float Texture2Intensity;
        public float Texture2Saturation;

        // Texture3
        public Texture2D Texture3;
        public Vector3 Texture3Tile;
        public Color Texture3Color;
        public float Texture3Intensity;
        public float Texture3Saturation;

        // Texture4
        public Texture2D Texture4;
        public Vector3 Texture4Tile;
        public Color Texture4Color;
        public float Texture4Intensity;
        public float Texture4Saturation;

        // Mask2
        public Texture2D Mask2;
        public float Mask2Active;

        // Texture 5
        public Texture2D Texture5;
        public Vector3 Texture5Tile;
        public Color Texture5Color;
        public float Texture5Intensity;
        public float Texture5Saturation;

        // Texture 6
        public Texture2D Texture6;
        public Vector3 Texture6Tile;
        public Color Texture6Color;
        public float Texture6Intensity;
        public float Texture6Saturation;


        // Texture 7
        public Texture2D Texture7;
        public Vector3 Texture7Tile;
        public Color Texture7Color;
        public float Texture7Intensity;
        public float Texture7Saturation;

        // Mask3
        public Texture2D Mask3;
        public float Mask3Activate;

        // Texture 8
        public Texture2D Texture8;
        public Vector3 Texture8Tile;
        public Color Texture8Color;
        public float Texture8Intensity;
        public float Texture8Saturation;

        // Texture 9
        public Texture2D Texture9;
        public Vector3 Texture9Tile;
        public Color Texture9Color;
        public float Texture9Intensity;
        public float Texture9Saturation;


        // Texture 10
        public Texture2D Texture10;
        public Vector3 Texture10Tile;
        public Color Texture10Color;
        public float Texture10Intensity;
        public float Texture10Saturation;

        public GameObject SetTerrainObject()
        {
            return TerrainObject;
        }

        public Material SetTerrainMaterial()
        {
            return TerrainMaterial;
        }

        public Texture2D SetlogoMethod() { return GeneralLogo; }

        #region Mask1 setup
        public Texture2D SetMask1()
        {
            TerrainMaterial.SetTexture("_Mask1", Mask1);
            Texture2D _maks1 = (Texture2D)TerrainMaterial.GetTexture("_Mask1");
            return _maks1;
        }
        #endregion

        #region Texture1 setup

        public Texture2D SetTexture1()
        {
            TerrainMaterial.SetTexture("_Texture1R", Texture1);
            Texture2D _texture1 = (Texture2D)TerrainMaterial.GetTexture("_Texture1R");
            return _texture1;
        }

        public Color SetTexture1Color()
        {
            TerrainMaterial.SetColor("_Texture1R_Color", Texture1Color);
            Color _texture1Color = TerrainMaterial.GetColor("_Texture1R_Color");
            return _texture1Color;
        }

        public Vector3 SetTexture1Tile()
        {
            TerrainMaterial.SetVector("_Texture1R_Tile", Texture1Tile);
            Vector3 _texture1Tile = TerrainMaterial.GetVector("_Texture1R_Tile");
            return _texture1Tile;
        }

        public float Texture1_Saturation()
        {
            TerrainMaterial.SetFloat("_Texture1R_Saturation", Texture1Saturation);
            float _texture1Saturation = TerrainMaterial.GetFloat("_Texture1R_Saturation");
            return _texture1Saturation;
        }

        public float Texture1_intensity()
        {
            TerrainMaterial.SetFloat("_Texture1R_Intensity", Texture1Intensity);
            float _texture1Intensity = TerrainMaterial.GetFloat("_Texture1R_Intensity");
            return _texture1Intensity;
        }

        #endregion

        #region Texture2 setup
        public Texture2D SetTexture2()
        {
            TerrainMaterial.SetTexture("_Texture2G", Texture2);
            Texture2D _texture2 = (Texture2D)TerrainMaterial.GetTexture("_Texture2G");
            return _texture2;
        }

        public Vector3 SetTexture2Tile()
        {
            TerrainMaterial.SetVector("_Texture2G_Tile", Texture2Tile);
            Vector3 _texture2Tile = TerrainMaterial.GetVector("_Texture2G_Tile");
            return _texture2Tile;
        }

        public Color SetTexture2Color()
        {
            TerrainMaterial.SetColor("_Texture2G_Color", Texture2Color);
            Color _texture2Color = TerrainMaterial.GetColor("_Texture2G_Color");
            return _texture2Color;
        }

        public float SetTexture2Intensity()
        {
            TerrainMaterial.SetFloat("_Texture2G_Intensity", Texture2Intensity);
            float _texture2Intensity = TerrainMaterial.GetFloat("_Texture2G_Intensity");
            return _texture2Intensity;
        }

        public float SetTexture2Saturation()
        {
            TerrainMaterial.SetFloat("_Texture2G_Saturation", Texture2Saturation);
            float _texture2Saturation = TerrainMaterial.GetFloat("_Texture2G_Saturation");
            return _texture2Saturation;
        }
        #endregion

        #region Texture3 setup
        public Texture2D SetTexture3()
        {
            TerrainMaterial.SetTexture("_Texture3B", Texture3);
            Texture2D _texture3 = (Texture2D)TerrainMaterial.GetTexture("_Texture3B");
            return _texture3;
        }

        public Vector3 SetTexture3Tile()
        {
            TerrainMaterial.SetVector("_Texture3B_Tile", Texture3Tile);
            Vector3 _texture3Tile = TerrainMaterial.GetVector("_Texture3B_Tile");
            return _texture3Tile;
        }

        public Color SetTexture3Color()
        {
            TerrainMaterial.SetColor("_Texture3B_Color", Texture3Color);
            Color _texture3Color = TerrainMaterial.GetColor("_Texture3B_Color");
            return _texture3Color;
        }

        public float SetTexture3Intensity()
        {
            TerrainMaterial.SetFloat("_Texture3B_Intensity", Texture3Intensity);
            float _texture3Intensity = TerrainMaterial.GetFloat("_Texture3B_Intensity");
            return _texture3Intensity;
        }

        public float SetTexture3Saturation()
        {
            TerrainMaterial.SetFloat("_Texture3B_Saturation", Texture3Saturation);
            float _texture3Saturation = TerrainMaterial.GetFloat("_Texture3B_Saturation");
            return _texture3Saturation;
        }
        #endregion

        #region Texture4 setup

        public Texture2D SetTexture4()
        {
            TerrainMaterial.SetTexture("_Texture4A", Texture4);
            Texture2D _texture4 = (Texture2D)TerrainMaterial.GetTexture("_Texture4A");
            return _texture4;
        }

        public Vector3 SetTexture4Tile()
        {
            TerrainMaterial.SetVector("_Texture4A_Tile", Texture4Tile);
            Vector3 _texture4Tile = TerrainMaterial.GetVector("_Texture4A_Tile");
            return _texture4Tile;
        }

        public Color SetTexture4Color()
        {
            TerrainMaterial.SetColor("_Texture4A_Color", Texture4Color);
            Color _texture4Color = TerrainMaterial.GetColor("_Texture4A_Color");
            return _texture4Color;
        }

        public float SetTexture4Intensity()
        {
            TerrainMaterial.SetFloat("_Texture4A_intensity", Texture4Intensity);
            float _texture4Intensity = TerrainMaterial.GetFloat("_Texture4A_intensity");
            return _texture4Intensity;
        }

        public float SetTexture4Saturation()
        {
            TerrainMaterial.SetFloat("_Texture4A_Saturation", Texture4Saturation);
            float _texture4Saturation = TerrainMaterial.GetFloat("_Texture4A_Saturation");
            return _texture4Saturation;
        }


        #endregion


        #region Mask2 setup
        public Texture2D SetupMask2()
        {
            TerrainMaterial.SetTexture("_Mask2", Mask2);
            Texture2D _mask2 = (Texture2D)TerrainMaterial.GetTexture("_Mask2");
            return _mask2;
        }

        public void Mask2Activate(float Value)
        {
            if (Value == 0)
            {
                TerrainMaterial.SetFloat("_Texture_Pack_2", 0);
            }
            if (Value == 1)
            {
                TerrainMaterial.SetFloat("_Texture_Pack_2", 1);
            }

        }


        #endregion

        #region Texture5 setup
        public Texture2D SetupTexture5()
        {
            TerrainMaterial.SetTexture("_Texture5R", Texture5);
            Texture2D _texture5 = (Texture2D)TerrainMaterial.GetTexture("_Texture5R");
            return _texture5;
        }

        public Color SetupTexture5Color()
        {
            TerrainMaterial.SetColor("_Texture5R_Color", Texture5Color);
            Color _texture5Color = TerrainMaterial.GetColor("_Texture5R_Color");
            return _texture5Color;
        }

        public Vector3 SetupeTexture5Tile()
        {
            TerrainMaterial.SetVector("_Texture5R_Tile", Texture5Tile);
            Vector3 _texture5Tile = TerrainMaterial.GetVector("_Texture5R_Tile");
            return _texture5Tile;
        }

        public float SetupTexture5Saturation()
        {
            TerrainMaterial.SetFloat("_Texture5R_Saturation", Texture5Saturation);
            float _texture5Saturation = TerrainMaterial.GetFloat("_Texture5R_Saturation");
            return _texture5Saturation;
        }

        public float SetupTexture5Intensity()
        {
            TerrainMaterial.SetFloat("_Texture5R_Intensity", Texture5Intensity);
            float _texture5Intensity = TerrainMaterial.GetFloat("_Texture5R_Intensity");
            return _texture5Intensity;
        }
        #endregion

        #region Texture6 setup
        public Texture2D Texture6Setup()
        {
            TerrainMaterial.SetTexture("_Texture6G", Texture6);
            Texture2D _texture6 = (Texture2D)TerrainMaterial.GetTexture("_Texture6G");
            return _texture6;
        }

        public Color Texture6ColorSetup()
        {
            TerrainMaterial.SetColor("_Texture6G_Color", Texture6Color);
            Color _texture6Color = TerrainMaterial.GetColor("_Texture6G_Color");
            return _texture6Color;
        }

        public Vector3 Texture6TileSetup()
        {
            TerrainMaterial.SetVector("_Texture6G_Tile", Texture6Tile);
            Vector3 _texture6Tile = TerrainMaterial.GetVector("_Texture6G_Tile");
            return _texture6Tile;
        }

        public float Texture6SaturationSetup()
        {
            TerrainMaterial.SetFloat("_Texture6G_Saturation", Texture6Saturation);
            float _texture6Saturation = TerrainMaterial.GetFloat("_Texture6G_Saturation");
            return _texture6Saturation;
        }

        public float Texture6IntensitySetup()
        {
            TerrainMaterial.SetFloat("_Texture6G_Intensity", Texture6Intensity);
            float _texture6Intensity = TerrainMaterial.GetFloat("_Texture6G_Intensity");
            return _texture6Intensity;
        }
        #endregion

        #region Texture7 setup
        public Texture2D Texture7Setup()
        {
            TerrainMaterial.SetTexture("_Texture7B", Texture7);
            Texture2D _texture7 = (Texture2D)TerrainMaterial.GetTexture("_Texture7B");
            return _texture7;
        }

        public Color Texture7ColorSetup()
        {
            TerrainMaterial.SetColor("_Texture7B_Color", Texture7Color);
            Color _texture7Color = TerrainMaterial.GetColor("_Texture7B_Color");
            return _texture7Color;
        }

        public Vector3 Texture7TileSetup()
        {
            TerrainMaterial.SetVector("_Texture7B_Tile", Texture7Tile);
            Vector3 _texture7Tile = TerrainMaterial.GetVector("_Texture7B_Tile");
            return _texture7Tile;
        }

        public float Texture7SaturationSetup()
        {
            TerrainMaterial.SetFloat("_Texture7B_Saturation", Texture7Saturation);
            float _texture7Saturation = TerrainMaterial.GetFloat("_Texture7B_Saturation");
            return _texture7Saturation;
        }

        public float Texture7IntensitySetup()
        {
            TerrainMaterial.SetFloat("_Texture7B_Intensity", Texture7Intensity);
            float _texture7Intensity = TerrainMaterial.GetFloat("_Texture7B_Intensity");
            return _texture7Intensity;
        }
        #endregion

        #region Mask3 setup
        public Texture2D SetupMask3()
        {
            TerrainMaterial.SetTexture("_Mask3", Mask3);
            Texture2D _mask3 = (Texture2D)TerrainMaterial.GetTexture("_Mask3");
            return _mask3;
        }

        public void Mask3Activates(float Value)
        {
            if (Value == 0)
            {
                TerrainMaterial.SetFloat("_Texture_Pack_3", 0);
            }
            if (Value == 1)
            {
                TerrainMaterial.SetFloat("_Texture_Pack_3", 1);
            }

        }
        #endregion

        #region Texture8 setup
        public Texture2D Texture8Setup()
        {
            TerrainMaterial.SetTexture("_Texture8R", Texture8);
            Texture2D _texture8 = (Texture2D)TerrainMaterial.GetTexture("_Texture8R");
            return _texture8;
        }

        public Color Texture8ColorSetup()
        {
            TerrainMaterial.SetColor("_Texture8R_Color", Texture8Color);
            Color _texture8Color = TerrainMaterial.GetColor("_Texture8R_Color");
            return _texture8Color;
        }

        public Vector3 Texture8TileSetup()
        {
            TerrainMaterial.SetVector("_Texture8R_Tile", Texture8Tile);
            Vector3 _texture8Tile = TerrainMaterial.GetVector("_Texture8R_Tile");
            return _texture8Tile;
        }

        public float Texture8SaturationSetup()
        {
            TerrainMaterial.SetFloat("_Texture8R_Saturation", Texture8Saturation);
            float _texture8Saturation = TerrainMaterial.GetFloat("_Texture8R_Saturation");
            return _texture8Saturation;
        }

        public float Texture8IntensitySetup()
        {
            TerrainMaterial.SetFloat("_Texture8R_Intensity", Texture8Intensity);
            float _texture8Intensity = TerrainMaterial.GetFloat("_Texture8R_Intensity");
            return _texture8Intensity;
        }
        #endregion

        #region Texture9 setup
        public Texture2D Texture9Setup()
        {
            TerrainMaterial.SetTexture("_Texture9G", Texture9);
            Texture2D _texture8 = (Texture2D)TerrainMaterial.GetTexture("_Texture9G");
            return _texture8;
        }

        public Color Texture9ColorSetup()
        {
            TerrainMaterial.SetColor("_Texture9G_Color", Texture9Color);
            Color _texture8Color = TerrainMaterial.GetColor("_Texture9G_Color");
            return _texture8Color;
        }

        public Vector3 Texture9TileSetup()
        {
            TerrainMaterial.SetVector("_Texture9G_Tile", Texture9Tile);
            Vector3 _texture8Tile = TerrainMaterial.GetVector("_Texture9G_Tile");
            return _texture8Tile;
        }

        public float Texture9SaturationSetup()
        {
            TerrainMaterial.SetFloat("_Texture9G_Saturation", Texture9Saturation);
            float _texture8Saturation = TerrainMaterial.GetFloat("_Texture9G_Saturation");
            return _texture8Saturation;
        }

        public float Texture9IntensitySetup()
        {
            TerrainMaterial.SetFloat("_Texture9G_Intensity", Texture9Intensity);
            float _texture8Intensity = TerrainMaterial.GetFloat("_Texture9G_Intensity");
            return _texture8Intensity;
        }
        #endregion

        #region Texture 10 setup
        public Texture2D Texture10Setup()
        {
            TerrainMaterial.SetTexture("_Texture10B", Texture10);
            Texture2D _texture8 = (Texture2D)TerrainMaterial.GetTexture("_Texture10B");
            return _texture8;
        }

        public Color Texture10ColorSetup()
        {
            TerrainMaterial.SetColor("_Texture10B_Color", Texture10Color);
            Color _texture8Color = TerrainMaterial.GetColor("_Texture10B_Color");
            return _texture8Color;
        }

        public Vector3 Texture10TileSetup()
        {
            TerrainMaterial.SetVector("_Texture10B_Tile", Texture10Tile);
            Vector3 _texture8Tile = TerrainMaterial.GetVector("_Texture10B_Tile");
            return _texture8Tile;
        }

        public float Texture10SaturationSetup()
        {
            TerrainMaterial.SetFloat("_Texture10B_Saturation", Texture10Saturation);
            float _texture8Saturation = TerrainMaterial.GetFloat("_Texture10B_Saturation");
            return _texture8Saturation;
        }

        public float Texture10IntensitySetup()
        {
            TerrainMaterial.SetFloat("_texture10B_Intensity", Texture10Intensity);
            float _texture8Intensity = TerrainMaterial.GetFloat("_texture10B_Intensity");
            return _texture8Intensity;
        }
        #endregion
    }
}

