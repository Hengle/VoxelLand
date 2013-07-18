#version 330

uniform float time;

in FragmentData
{
    vec3 normal;
    vec4 color;
} frag;

out vec4 color;

void main(void)
{
	color = vec4(0.5 + (0.5 * sin(time)), 0.5 + (0.5 * cos(time)), 0.5 - (0.5 * cos(time)), 1.0);
	// color = vec4(abs(frag.normal.x), abs(frag.normal.y), abs(frag.normal.z), 1);
}