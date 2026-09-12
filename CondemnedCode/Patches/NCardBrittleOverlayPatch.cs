using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System.Reflection;
using Condemned.CondemnedCode.Keywords;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace Condemned.CondemnedCode.Patches
{
    [HarmonyPatch(typeof(NCard))]
    public static class NCardBrittlePatch
    {
        private static Texture2D _brittleMaskTexture;
        private static Texture2D _bloomMaskTexture;
        private static FieldInfo _frameField;

        static NCardBrittlePatch()
        {
            _frameField = typeof(NCard).GetField("_frame", BindingFlags.NonPublic | BindingFlags.Instance);
            
            // Cargar la máscara
            _brittleMaskTexture = ResourceLoader.Load<Texture2D>(
                "res://Condemned/materials/brittle_mask.png"
            );
            
            _bloomMaskTexture = ResourceLoader.Load<Texture2D>(
                "res://Condemned/materials/brittle_bloom_mask.png"
            );
        }

        private static ShaderMaterial GetCombinedMaterial(TextureRect frame, Color colorModulate)
        {
            // Crear un nuevo material cada vez (no compartido)
            var shader = new Shader();
            shader.Code = @"
        shader_type canvas_item;

        uniform sampler2D mask_texture;
        uniform sampler2D bloom_mask_texture;

        uniform float threshold = 0.5;
        uniform vec4 color_modulate = vec4(1.0);

        uniform vec4 bloom_color : source_color = vec4(3.0, 1.5, 2.0, 1.0);
        uniform float bloom_strength = 2.0;

        uniform vec2 region_pos;
        uniform vec2 region_size;
        uniform vec2 atlas_size;

        void fragment()
        {
            vec4 color = texture(TEXTURE, UV);

            float gray = dot(color.rgb, vec3(0.299, 0.587, 0.114));
            vec4 final_color = vec4(vec3(gray), color.a) * color_modulate;

            vec2 region_uv_min = region_pos / atlas_size;
            vec2 region_uv_size = region_size / atlas_size;

            vec2 mask_uv = (UV - region_uv_min) / region_uv_size;

            vec4 brittle_mask = texture(mask_texture, mask_uv);

            if (brittle_mask.r > threshold)
                discard;

            vec4 bloom_mask = texture(bloom_mask_texture, mask_uv);

            final_color.rgb += bloom_mask.r * bloom_color.rgb * bloom_strength;

            COLOR = final_color;
        }
    ";
            
            var material = new ShaderMaterial { Shader = shader };

            material.SetShaderParameter("mask_texture", _brittleMaskTexture);
            material.SetShaderParameter(
                "bloom_mask_texture",
                _bloomMaskTexture
            );
            material.SetShaderParameter("color_modulate", colorModulate);

            if (frame.Texture is AtlasTexture atlas)
            {
                material.SetShaderParameter("region_pos", atlas.Region.Position);
                material.SetShaderParameter("region_size", atlas.Region.Size);
                material.SetShaderParameter("atlas_size", atlas.Atlas.GetSize());
            }

            return material;
        }

        private static void ApplyBrittleFrame(NCard card)
        {
            CardModel model = card.Model;
            if (model == null || _frameField == null) return;

            TextureRect frame = _frameField.GetValue(card) as TextureRect;
            if (frame == null) return;

            bool hasBrittle = model.Keywords.Contains(CondemnedKeywords.Brittle);

            if (hasBrittle)
            {
                Color deckColor = model.Pool.DeckEntryCardColor;
                ShaderMaterial combinedMaterial = GetCombinedMaterial(frame, deckColor);
                frame.Material = combinedMaterial; // Asignar directamente (cada carta tiene su propio material)
            }
            else
            {
                if (frame.Material is ShaderMaterial mat &&
                    mat.Shader?.Code?.Contains("mask_texture") == true)
                {
                    frame.Material = model.FrameMaterial;
                }
            }
        }

        // Parche para cuando se recarga la carta (creación o cambio de modelo)
        [HarmonyPatch("Reload")]
        [HarmonyPostfix]
        public static void ReloadPostfix(NCard __instance)
        {
            ApplyBrittleFrame(__instance);
        }

        // Parche para cuando se actualiza visualmente (cambios en mano, coste, etc.)
        [HarmonyPatch("UpdateVisuals", typeof(PileType), typeof(CardPreviewMode))]
        [HarmonyPostfix]
        public static void UpdateVisualsPostfix(NCard __instance)
        {
            ApplyBrittleFrame(__instance);
        }
    }
}