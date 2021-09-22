#version 330 core

layout(location = 0) in vec2 position;
layout(location = 1) in vec2 texpos;
layout(location = 2) in vec4 col;

out vec2 uv;
out vec4 colour;

void main()
{
	uv = texpos;
	colour = col;
	gl_Position = vec4(position, 0.0, 1.0);
}