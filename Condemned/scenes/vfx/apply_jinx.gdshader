shader_type canvas_item;

uniform vec4 color : source_color = vec4(0.15, 0.0, 0.25, 0.9);
uniform float progress : hint_range(0.0, 1.5) = 0.0; // progreso > 1 para que se expanda más allá
uniform float strength : hint_range(0.0, 1.0) = 0.8;

// Ruido para bordes irregulares
float hash(vec2 p) {
    return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453123);
}

float noise(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    f = f*f*(3.0-2.0*f);
    float a = hash(i);
    float b = hash(i + vec2(1.0, 0.0));
    float c = hash(i + vec2(0.0, 1.0));
    float d = hash(i + vec2(1.0, 1.0));
    return mix(mix(a, b, f.x), mix(c, d, f.x), f.y);
}

// Función para onda con ruido
float wave(vec2 uv, float time) {
    float dist = length(uv - 0.5);
    float wave = sin(dist * 20.0 - time * 3.0) * 0.5 + 0.5;
    float noiseVal = noise(uv * 5.0 + time * 0.2);
    return wave * (0.7 + 0.3 * noiseVal);
}

void fragment() {
    vec2 center = vec2(0.5, 0.5);
    float dist = distance(UV, center);
    
    // 1. Onda principal que se expande (como una mancha que crece)
    float radius = progress * 0.6; // 0.6 es el radio máximo (para que no llene todo)
    float mask = 1.0 - smoothstep(0.0, radius, dist);
    
    // 2. Añadir ruido para que los bordes sean irregulares
    float n = noise(UV * 6.0 + TIME * 0.3);
    mask = mask * (0.8 + 0.2 * n);
    
    // 3. Añadir ondas concéntricas (como un anillo que se propaga)
    float waveEffect = wave(UV, progress * 2.0);
    mask = max(mask, waveEffect * 0.5 * (1.0 - smoothstep(0.0, 0.8, dist)));
    
    // 4. Limitar y suavizar
    mask = clamp(mask, 0.0, 1.0);
    
    // 5. Aplicar color y transparencia
    vec4 final = color;
    final.a = mask * strength;
    
    // 6. Oscurecer un poco la zona afectada
    float dark = 1.0 - mask * 0.4;
    
    COLOR = vec4(0.0, 0.0, 0.0, 0.0);
    COLOR.rgb = final.rgb * dark;
    COLOR.a = final.a;
}