#version 330

uniform mat4 modelViewMatrix;
uniform mat4 projectionMatrix;

layout(location = 0) in vec4 vertex;

out VoxelData
{
    vec4 color;
} v;

void main(void)
{
    gl_Position = projectionMatrix * modelViewMatrix * vertex;
    v.color = vec4(1.0, 1.0, 1.0, 1.0);
}