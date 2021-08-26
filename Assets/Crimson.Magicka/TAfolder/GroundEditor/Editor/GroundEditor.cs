#if UNITY_EDITOR 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


namespace ArtStuff
{
    [CustomEditor(typeof(GroundShaderController))]
    public class GroundEditor : Editor
    {
        int _CurrentSelectedButton;
        GroundShaderController _TargetScript;

        bool Mask2Active = false;
        float Mask2ActiveSwitch;

        bool Mask3Actve = false;
        float Mask3ActiveSwitch;

        private void OnEnable()
        {
            _TargetScript = base.target as GroundShaderController;
        }

        public override void OnInspectorGUI()
        {
            // set Logo
            GUILayout.Label(_TargetScript.GeneralLogo);

            #region set Terrain
            
            if (_TargetScript.TerrainMaterial == null)
            {
                EditorGUILayout.HelpBox("Вставьте Terrain-материал из папки Project, который будете настраивать.", MessageType.Error);
                _TargetScript.TerrainMaterial = (Material)EditorGUILayout.ObjectField("Terrain материал", _TargetScript.TerrainMaterial, typeof(Material), true);
            }
            else
            {
                _TargetScript.TerrainMaterial = (Material)EditorGUILayout.ObjectField("Terrain материал", _TargetScript.TerrainMaterial, typeof(Material), true);
            }
            
            #endregion

            string[] _Buttons = new string[3] { "Маска 1", "Маска 2", "Маска 3" };
            _CurrentSelectedButton = GUILayout.Toolbar(_CurrentSelectedButton, _Buttons);

            #region Button1 (Mask1)

            if (_CurrentSelectedButton == 0)
            {
                GUI.changed = false;
                
                EditorGUILayout.BeginVertical();
                _TargetScript.Mask1 = (Texture2D)EditorGUILayout.ObjectField("Текстура RGBA", _TargetScript.Mask1, typeof(Texture2D), true);

                // Texture 1
                GUI.color = Color.red * 1.7f;
                Rect FormForTexture1 = EditorGUILayout.GetControlRect(false, 2);
                FormForTexture1.height = 157;
                GUI.Box(FormForTexture1, "");
                GUI.color = Color.white;
                _TargetScript.Texture1 = (Texture2D)EditorGUILayout.ObjectField("Текстура 1(R)", _TargetScript.Texture1, typeof(Texture2D), true);
                _TargetScript.Texture1Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture1Color);
                _TargetScript.Texture1Tile = EditorGUILayout.Vector2Field("Таил Текстуры", _TargetScript.Texture1Tile);
                _TargetScript.Texture1Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture1Saturation, -1, 3);
                _TargetScript.Texture1Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture1Intensity, 0, 5);

                EditorGUILayout.Space(14);

                // Texture 2
                GUI.color = Color.green * 2;
                Rect FormForTexture2 = EditorGUILayout.GetControlRect(false, 2f);
                FormForTexture2.height = 157;
                GUI.Box(FormForTexture2, "");
                GUI.color = Color.white;
                _TargetScript.Texture2 = (Texture2D)EditorGUILayout.ObjectField("Текстура 2(G)", _TargetScript.Texture2, typeof(Texture2D), true);
                _TargetScript.Texture2Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture2Color);
                _TargetScript.Texture2Tile = EditorGUILayout.Vector2Field("Таил Текстуры", _TargetScript.Texture2Tile);
                _TargetScript.Texture2Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture2Saturation, -1, 3);
                _TargetScript.Texture2Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture2Intensity, 0, 5);

                EditorGUILayout.Space(14);

                // Texture 3
                GUI.color = Color.blue * 2;
                Rect FormForTexture3 = EditorGUILayout.GetControlRect(false, 2f);
                FormForTexture3.height = 157;
                GUI.Box(FormForTexture3, "");
                GUI.color = Color.white;

                _TargetScript.Texture3 = (Texture2D)EditorGUILayout.ObjectField("Текстура 3(B)", _TargetScript.Texture3, typeof(Texture2D), true);
                _TargetScript.Texture3Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture3Color);
                _TargetScript.Texture3Tile = EditorGUILayout.Vector2Field("Таил Текстуры", _TargetScript.Texture3Tile);
                _TargetScript.Texture3Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture3Saturation, -1, 3);
                _TargetScript.Texture3Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture3Intensity, 0, 5);

                EditorGUILayout.Space(14);

                // Texture 4
                GUI.color = Color.black * 3;
                Rect FormForTexture4 = EditorGUILayout.GetControlRect(false, 2f);
                FormForTexture4.height = 157;
                GUI.Box(FormForTexture4, "");
                GUI.color = Color.white;
                _TargetScript.Texture4 = (Texture2D)EditorGUILayout.ObjectField("Текстура 4(A)", _TargetScript.Texture4, typeof(Texture2D), true);
                _TargetScript.Texture4Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture4Color);
                _TargetScript.Texture4Tile = EditorGUILayout.Vector2Field("Таил Текстуры", _TargetScript.Texture4Tile);
                _TargetScript.Texture4Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture4Saturation, -1, 3);
                _TargetScript.Texture4Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture4Intensity, 0, 5);

                EditorGUILayout.EndVertical();
                

                

                if (GUI.changed)
                {
                    Material GeneralTerrainObject = _TargetScript.SetTerrainMaterial();
                    _TargetScript.TerrainMaterial = GeneralTerrainObject;

                    // Set Mask1
                    Texture2D _mask1 = _TargetScript.SetMask1();
                    _TargetScript.Mask1= _mask1;

                    #region Texture1 

                    // Set texture1 
                    Texture2D _texture1 = _TargetScript.SetTexture1();
                    _TargetScript.Texture1 = _texture1;

                    // Set texture 1 Color
                    Color _texture1Color = _TargetScript.SetTexture1Color();
                    _TargetScript.Texture1Color = _texture1Color;

                    // Set texture1 Tile
                    Vector2 _texture1Tile = (Vector2)_TargetScript.SetTexture1Tile();
                    _TargetScript.Texture1Tile = _texture1Tile;

                    // Set texture1 Saturation
                    float Texture1Saturation = _TargetScript.Texture1_Saturation();
                    _TargetScript.Texture1Saturation = Texture1Saturation;

                    // Set texture 1 Intensity
                    float Texture1Intensity = _TargetScript.Texture1_intensity();
                    _TargetScript.Texture1Intensity = Texture1Intensity;

                    #endregion

                    #region Texture2
                    // Set texture2 
                    Texture2D _texture2 = _TargetScript.SetTexture2();
                    _TargetScript.Texture2 = _texture2;

                    // Set texture 2 Color
                    Color _texture2Color = _TargetScript.SetTexture2Color();
                    _TargetScript.Texture2Color = _texture2Color;

                    // Set texture2 Tile
                    Vector2 _texture2Tile = (Vector2)_TargetScript.SetTexture2Tile();
                    _TargetScript.Texture2Tile = _texture2Tile;

                    // Set texture2 Saturation
                    float Texture2Saturation = _TargetScript.SetTexture2Saturation();
                    _TargetScript.Texture2Saturation = Texture2Saturation;

                    // Set texture 2 Intensity
                    float Texture2Intensity = _TargetScript.SetTexture2Intensity();
                    _TargetScript.Texture2Intensity = Texture2Intensity;
                    #endregion

                    #region Texture3
                    // Set texture3
                    Texture2D _texture3 = _TargetScript.SetTexture3();
                    _TargetScript.Texture3 = _texture3;

                    // Set texture3 Color
                    Color _texture3Color = _TargetScript.SetTexture3Color();
                    _TargetScript.Texture3Color = _texture3Color;

                    // Set texture3 Tile
                    Vector2 _texture3Tile = (Vector2)_TargetScript.SetTexture3Tile();
                    _TargetScript.Texture3Tile = _texture3Tile;

                    // Set texture3 Saturation
                    float _texture3Saturation = _TargetScript.SetTexture3Saturation();
                    _TargetScript.Texture3Saturation = _texture3Saturation;

                    // Set texture 3 Intensity
                    float _texture3Intensity = _TargetScript.SetTexture3Intensity();
                    _TargetScript.Texture3Intensity = _texture3Intensity;
                    #endregion

                    #region Texture4
                    // Set texture4
                    Texture2D _texture4 = _TargetScript.SetTexture4();
                    _TargetScript.Texture4 = _texture4;

                    // Set texture4 Color
                    Color _texture4Color = _TargetScript.SetTexture4Color();
                    _TargetScript.Texture4Color = _texture4Color;

                    // Set texture4 Tile
                    Vector2 _texture4Tile = (Vector2)_TargetScript.SetTexture4Tile();
                    _TargetScript.Texture4Tile = _texture4Tile;

                    // Set texture4 Saturation
                    float _texture4Saturation = _TargetScript.SetTexture4Saturation();
                    _TargetScript.Texture4Saturation = _texture4Saturation;

                    // Set texture4 Intensity
                    float _texture4Intensity = _TargetScript.SetTexture4Intensity();
                    _TargetScript.Texture4Intensity = _texture4Intensity;
                    #endregion

                }
            }

            #endregion

            #region Button 2 (Mask2)
            if (_CurrentSelectedButton == 1)
            {
                Mask2Active = EditorGUILayout.Toggle("Активировать маску", Mask2Active);
                if (Mask2Active)
                {
                    GUI.changed = false;
                    Mask2ActiveSwitch = 1;

                    // Mask 2
                    EditorGUILayout.BeginVertical();
                    _TargetScript.Mask2 = (Texture2D)EditorGUILayout.ObjectField("Текстура RGBA", _TargetScript.Mask2, typeof(Texture2D), true);
                    EditorGUILayout.Space(14);

                    // Texture 5
                    GUI.color = Color.red * 1.8f;
                    Rect FormFortexture5 = EditorGUILayout.GetControlRect(false, 2);
                    FormFortexture5.height = 157;
                    GUI.Box(FormFortexture5, "");

                    GUI.color = Color.white;
                    _TargetScript.Texture5 = (Texture2D)EditorGUILayout.ObjectField("Текстура 1(R)", _TargetScript.Texture5, typeof(Texture2D), true);
                    _TargetScript.Texture5Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture5Color);
                    _TargetScript.Texture5Tile = EditorGUILayout.Vector2Field("Таил текстуры", _TargetScript.Texture5Tile);
                    _TargetScript.Texture5Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture5Saturation, -1, 3);
                    _TargetScript.Texture5Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture5Intensity, 0, 5);
                    _TargetScript.Mask2Activate(Mask2ActiveSwitch);

                    EditorGUILayout.Space(14);

                    // Texture 6
                    GUI.color = Color.green * 1.8f;
                    Rect FormFortexture6 = EditorGUILayout.GetControlRect(false, 2);
                    FormFortexture6.height = 157;
                    GUI.Box(FormFortexture6, "");

                    GUI.color = Color.white;
                    _TargetScript.Texture6 = (Texture2D)EditorGUILayout.ObjectField("Текстура 2(G)", _TargetScript.Texture6, typeof(Texture2D), true);
                    _TargetScript.Texture6Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture6Color);
                    _TargetScript.Texture6Tile = EditorGUILayout.Vector2Field("Таил текстуры", _TargetScript.Texture6Tile);
                    _TargetScript.Texture6Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture6Saturation, -1, 3);
                    _TargetScript.Texture6Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture6Intensity, 0, 5);
                    _TargetScript.Mask2Activate(Mask2ActiveSwitch);

                    EditorGUILayout.Space(14);


                    // Texture 7
                    GUI.color = Color.blue * 1.8f;
                    Rect FormFortexture7 = EditorGUILayout.GetControlRect(false, 2);
                    FormFortexture7.height = 157;
                    GUI.Box(FormFortexture7, "");

                    GUI.color = Color.white;
                    _TargetScript.Texture7 = (Texture2D)EditorGUILayout.ObjectField("Текстура 3(B)", _TargetScript.Texture7, typeof(Texture2D), true);
                    _TargetScript.Texture7Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture7Color);
                    _TargetScript.Texture7Tile = EditorGUILayout.Vector2Field("Таил текстуры", _TargetScript.Texture7Tile);
                    _TargetScript.Texture7Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture7Saturation, -1, 3);
                    _TargetScript.Texture7Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture7Intensity, 0, 5);
                    _TargetScript.Mask2Activate(Mask2ActiveSwitch);

                    EditorGUILayout.EndVertical();

                    if (GUI.changed)
                    {
                        #region Texture 5
                        // Mask2 set
                        Texture2D _mask2 = _TargetScript.SetupMask2();
                        _TargetScript.Mask2 = _mask2;


                        // texture 5
                        Texture2D _texture5 = _TargetScript.SetupTexture5();
                        _TargetScript.Texture5 = _texture5;

                        // texture 5 color
                        Color _texture5Color = _TargetScript.SetupTexture5Color();
                        _TargetScript.Texture5Color = _texture5Color;

                        // texture 5 tile 
                        Vector3 _texture5Tile = _TargetScript.SetupeTexture5Tile();
                        _TargetScript.Texture5Tile = _texture5Tile;

                        // texture 5 saturation
                        float _texture5Saturation = _TargetScript.SetupTexture5Saturation();
                        _TargetScript.Texture5Saturation = _texture5Saturation;

                        // texture 5 Intensity 
                        float _texture5Intensity = _TargetScript.SetupTexture5Intensity();
                        _TargetScript.Texture5Intensity = _texture5Intensity;

                        #endregion

                        #region Texture 6

                        // texture 6
                        Texture2D _texture6 = _TargetScript.Texture6Setup();
                        _TargetScript.Texture6 = _texture6;

                        // texture 6 color
                        Color _texture6Color = _TargetScript.Texture6ColorSetup();
                        _TargetScript.Texture6Color = _texture6Color;

                        // texture 6 tile 
                        Vector3 _texture6Tile = _TargetScript.Texture6TileSetup();
                        _TargetScript.Texture6Tile = _texture6Tile;

                        // texture 6 saturation
                        float _texture6Saturation = _TargetScript.Texture6SaturationSetup();
                        _TargetScript.Texture6Saturation = _texture6Saturation;

                        // texture 6 Intensity 
                        float _texture6Intensity = _TargetScript.Texture6IntensitySetup();
                        _TargetScript.Texture6Intensity = _texture6Intensity;
                        #endregion

                        #region Texture 7

                        // texture 6
                        Texture2D _texture7 = _TargetScript.Texture7Setup();
                        _TargetScript.Texture7 = _texture7;

                        // texture 6 color
                        Color _texture7Color = _TargetScript.Texture7ColorSetup();
                        _TargetScript.Texture7Color = _texture7Color;

                        // texture 6 tile 
                        Vector3 _texture7Tile = _TargetScript.Texture7TileSetup();
                        _TargetScript.Texture7Tile = _texture7Tile;

                        // texture 6 saturation
                        float _texture7Saturation = _TargetScript.Texture7SaturationSetup();
                        _TargetScript.Texture7Saturation = _texture7Saturation;

                        // texture 6 Intensity 
                        float _texture7Intensity = _TargetScript.Texture7IntensitySetup();
                        _TargetScript.Texture7Intensity = _texture7Intensity;
                        #endregion
                    }
                }
                if (!Mask2Active)
                {
                    Mask2ActiveSwitch = 0;
                    _TargetScript.Mask2Activate(Mask2ActiveSwitch);
                    EditorGUILayout.HelpBox("Чтобы включить наложение ещё одной маски с 3-я текстурами \n" +
                                            "активируйте маску \n"
                                            + "ПОМНИТЕ: большее количество текстур, потребует больше вычислительной мощности устройства", MessageType.Info);
                }
            }
            #endregion

            #region Button 3 (Mask3)
            if (_CurrentSelectedButton == 2)
            {
                
                Mask3Actve = EditorGUILayout.Toggle("Активировать маску", Mask3Actve);

                if (Mask3Actve)
                {
                    GUI.changed = false;
                    Mask3ActiveSwitch = 1;
                    EditorGUILayout.BeginVertical();
                    _TargetScript.Mask3 = (Texture2D)EditorGUILayout.ObjectField("Текстура RGBA", _TargetScript.Mask3, typeof(Texture2D), true);
                    EditorGUILayout.EndVertical();

                    GUI.color = Color.red * 1.8f;
                    Rect FormFortexture8 = EditorGUILayout.GetControlRect(false, 2);
                    FormFortexture8.height = 157;
                    GUI.Box(FormFortexture8, "");

                    GUI.color = Color.white;
                    _TargetScript.Texture8 = (Texture2D)EditorGUILayout.ObjectField("Текстура 1(R)", _TargetScript.Texture8, typeof(Texture2D), true);
                    _TargetScript.Texture8Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture8Color);
                    _TargetScript.Texture8Tile = EditorGUILayout.Vector2Field("Таил текстуры", _TargetScript.Texture8Tile);
                    _TargetScript.Texture8Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture8Saturation, -1, 3);
                    _TargetScript.Texture8Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture8Intensity, 0, 5);
                    _TargetScript.Mask3Activates(Mask3ActiveSwitch);

                    EditorGUILayout.Space(14);

                    GUI.color = Color.green * 1.8f;
                    Rect FormFortexture9 = EditorGUILayout.GetControlRect(false, 2);
                    FormFortexture9.height = 157;
                    GUI.Box(FormFortexture9, "");

                    GUI.color = Color.white;
                    _TargetScript.Texture9 = (Texture2D)EditorGUILayout.ObjectField("Текстура 2(G)", _TargetScript.Texture9, typeof(Texture2D), true);
                    _TargetScript.Texture9Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture9Color);
                    _TargetScript.Texture9Tile = EditorGUILayout.Vector2Field("Таил текстуры", _TargetScript.Texture9Tile);
                    _TargetScript.Texture9Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture9Saturation, -1, 3);
                    _TargetScript.Texture9Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture9Intensity, 0, 5);

                    EditorGUILayout.Space(14);

                    GUI.color = Color.blue * 1.8f;
                    Rect FormFortexture10 = EditorGUILayout.GetControlRect(false, 2);
                    FormFortexture10.height = 157;
                    GUI.Box(FormFortexture10, "");

                    GUI.color = Color.white;
                    _TargetScript.Texture10 = (Texture2D)EditorGUILayout.ObjectField("Текстура 3(B)", _TargetScript.Texture10, typeof(Texture2D), true);
                    _TargetScript.Texture10Color = EditorGUILayout.ColorField("Цвет", _TargetScript.Texture10Color);
                    _TargetScript.Texture10Tile = EditorGUILayout.Vector2Field("Таил текстуры", _TargetScript.Texture10Tile);
                    _TargetScript.Texture10Saturation = EditorGUILayout.Slider("Насыщенность", _TargetScript.Texture10Saturation, -1, 3);
                    _TargetScript.Texture10Intensity = EditorGUILayout.Slider("Яркость текстуры", _TargetScript.Texture10Intensity, 0, 5);

                    if (GUI.changed)
                    {

                        #region Texture8
                        // Mask 3
                        Texture2D _mask3 = _TargetScript.SetupMask3();
                        _TargetScript.Mask3 = _mask3;
                        
                        // texture 8
                        Texture2D _texture8 = _TargetScript.Texture8Setup();
                        _TargetScript.Texture8 = _texture8;

                        // texture 8 color
                        Color _texture8Color = _TargetScript.Texture8ColorSetup();
                        _TargetScript.Texture8Color = _texture8Color;

                        // texture 8 tile 
                        Vector3 _texture8Tile = _TargetScript.Texture8TileSetup();
                        _TargetScript.Texture8Tile = _texture8Tile;

                        // texture 8 saturation
                        float _texture8Saturation = _TargetScript.Texture8SaturationSetup();
                        _TargetScript.Texture8Saturation = _texture8Saturation;

                        // texture 8 Intensity 
                        float _texture8Intensity = _TargetScript.Texture8IntensitySetup();
                        _TargetScript.Texture8Intensity = _texture8Intensity;
                        #endregion

                        #region Texture9
                        // texture 9
                        Texture2D _texture9 = _TargetScript.Texture9Setup();
                        _TargetScript.Texture9 = _texture9;

                        // texture 9 color
                        Color _texture9Color = _TargetScript.Texture9ColorSetup();
                        _TargetScript.Texture9Color = _texture9Color;

                        // texture 9 tile 
                        Vector3 _texture9Tile = _TargetScript.Texture9TileSetup();
                        _TargetScript.Texture9Tile = _texture9Tile;

                        // texture 9 saturation
                        float _texture9Saturation = _TargetScript.Texture9SaturationSetup();
                        _TargetScript.Texture9Saturation = _texture9Saturation;

                        // texture 9 Intensity 
                        float _texture9Intensity = _TargetScript.Texture9IntensitySetup();
                        _TargetScript.Texture9Intensity = _texture9Intensity;
                        #endregion

                        #region Texture10
                        // texture 10
                        Texture2D _texture10 = _TargetScript.Texture10Setup();
                        _TargetScript.Texture10 = _texture10;

                        // texture 10 color
                        Color _texture10Color = _TargetScript.Texture10ColorSetup();
                        _TargetScript.Texture10Color = _texture10Color;

                        // texture 10 tile 
                        Vector3 _texture10Tile = _TargetScript.Texture10TileSetup();
                        _TargetScript.Texture10Tile = _texture10Tile;

                        // texture 10 saturation
                        float _texture10Saturation = _TargetScript.Texture10SaturationSetup();
                        _TargetScript.Texture10Saturation = _texture10Saturation;

                        // texture 10 Intensity 
                        float _texture10Intensity = _TargetScript.Texture10IntensitySetup();
                        _TargetScript.Texture10Intensity = _texture10Intensity;
                        #endregion

                    }
                }

                if (!Mask3Actve)
                {
                    Mask3ActiveSwitch = 0;
                    _TargetScript.Mask3Activates(Mask3ActiveSwitch);
                    EditorGUILayout.HelpBox("Чтобы включить наложение ещё одной маски с 3-я текстурами \n" +
                                            "активируйте маску \n"
                                            + "ПОМНИТЕ: большее количество текстур, потребует больше вычислительной мощности устройства", MessageType.Info);
                }
            }
            #endregion


        }
    }
}
#endif 


