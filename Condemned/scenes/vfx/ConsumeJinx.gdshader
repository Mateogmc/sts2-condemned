shader_type canvas_item;

uniform float progress : hint_range(0.0, 1.0) = 0.0;
uniform sampler2D noise_tex : repeat_enable;
uniform float noise_scale = 4.0;
uniform float edge = 0.15;
uniform vec4 fog_color : source_color = vec4(0.15, 0.05, 0.2, 1.0);

void fragment()
{
    vec2 uv = UV;

    float noise = texture(noise_tex, uv * noise_scale).r;

    vec4 tex = texture(TEXTURE, uv);

    float dissolve = 1.0 - uv.x;
    dissolve += (noise - 0.5) * edge;

    float alpha = smoothstep(progress - edge, progress, dissolve);

    vec3 color = mix(fog_color.rgb, tex.rgb, alpha);

    COLOR = vec4(color, tex.a * alpha);
}