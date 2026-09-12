shader_type canvas_item;

uniform vec4 galaxy_color : source_color = vec4(0.55, 0.15, 0.75, 1.0);
uniform vec4 star_color : source_color = vec4(0.8, 0.65, 1.0, 1.0);

uniform float arms : hint_range(1.0, 8.0) = 4.0;
uniform float rotation_speed : hint_range(-2.0, 2.0) = 1.15;
uniform float arm_width : hint_range(0.01, 1.0) = 0.35;
uniform float galaxy_radius : hint_range(0.1, 1.0) = 0.48;

uniform float white_start : hint_range(0.0, 1.0) = 0.25;
uniform float white_intensity : hint_range(0.0, 3.0) = 1.5;

float hash(vec2 p)
{
    return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453123);
}

float noise(vec2 p)
{
    vec2 i = floor(p);
    vec2 f = fract(p);
    f = f * f * (3.0 - 2.0 * f);
    float a = hash(i);
    float b = hash(i + vec2(1.0, 0.0));
    float c = hash(i + vec2(0.0, 1.0));
    float d = hash(i + vec2(1.0, 1.0));
    return mix(mix(a, b, f.x), mix(c, d, f.x), f.y);
}

void fragment()
{
    vec2 p = UV - vec2(0.5);
    float radius = length(p);
    float angle = atan(p.y, p.x);

    angle += TIME * rotation_speed;

    float spiralAngle = angle + radius * arms * 6.28318;
    float arm = abs(sin(spiralAngle));
    float armMask = 1.0 - smoothstep(0.0, arm_width, arm);

    float core = 1.0 - smoothstep(0.0, 0.20, radius);

    float density = 1.0 - smoothstep(0.15, galaxy_radius, radius);

    float n = noise(p * 18.0 + TIME * 0.02);
    armMask *= mix(0.7, 1.0, n);
    armMask *= density;

    float coreGlow = core * 1.5;
    float intensity = max(armMask, coreGlow);

    float stars = noise(p * 80.0);
    stars = smoothstep(0.82, 1.0, stars);
    stars *= density;

    float outer = smoothstep(white_start, galaxy_radius, radius);
    float whiteBlend = outer * armMask;

    vec3 baseColor = galaxy_color.rgb * intensity;
    vec3 whiteGlow = vec3(1.0, 0.95, 0.9) * whiteBlend * white_intensity;

    vec3 color = baseColor + whiteGlow + star_color.rgb * stars * 0.8;

    float alpha = clamp(intensity + stars * 0.5 + whiteBlend * 0.5, 0.0, 1.0);

    COLOR = vec4(color, alpha);
}