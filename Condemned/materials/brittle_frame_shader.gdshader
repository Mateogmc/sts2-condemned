shader_type canvas_item;

// Textura de máscara (blanco = recortar, negro = mantener)
uniform sampler2D mask_texture;

// Umbral para recorte: si el valor de la máscara es mayor que esto, se recorta.
uniform float threshold : hint_range(0.0, 1.0) = 0.5;

// Grosor del borde afectado (0 = solo el borde exacto, 0.2 = hasta el 20% del borde)
uniform float edge_thickness : hint_range(0.0, 0.3) = 0.12;

void fragment() {
    vec4 color = texture(TEXTURE, UV);
    
    // Obtener el valor de la máscara en esta coordenada UV
    vec4 mask_color = texture(mask_texture, UV);
    float mask_value = mask_color.r; // Usamos el canal rojo (escala de grises)
    
    // Aplicar umbral: si mask_value > threshold, se recorta
    float cut = step(threshold, mask_value);
    
    // Máscara de borde: el efecto solo se aplica en el perímetro de la carta
    float dist_x = min(UV.x, 1.0 - UV.x);
    float dist_y = min(UV.y, 1.0 - UV.y);
    float edge = 1.0 - smoothstep(0.0, edge_thickness, min(dist_x, dist_y));
    
    // Combinar máscara de borde con el recorte de la textura
    float tear = cut * edge;
    
    // Recorte binario (0 o 1) para evitar puntitos
    float alpha_cut = step(0.5, tear);
    color.a *= (1.0 - alpha_cut);
    
    COLOR = color;
}