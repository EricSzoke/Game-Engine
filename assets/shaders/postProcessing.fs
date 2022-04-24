#version 330 core
out vec4 FragColor;

in vec2 uvCoord;

uniform sampler2D screenTexture;
uniform sampler2D brightTexture;
uniform float exposure;

void main()
{
 
  vec3 hdrColor = texture(screenTexture, uvCoord).rgb;

  vec3 bloomColor = vec3(0.0);

  // Perform linearly separable gaussian blur on brightTexture
  int kernelSize = 32;
  vec2 dimensions = textureSize(brightTexture, 0);
  float xOffset = 1.0 / dimensions.x;
  float yOffset = 1.0 / dimensions.y;
  for(int i=1; i<kernelSize; i++) {

    bloomColor += texture(brightTexture, uvCoord + vec2(0, i * yOffset), 0).rgb;
    bloomColor += texture(brightTexture, uvCoord + vec2(0, -i * yOffset), 0).rgb;

  }

  for(int j=1; j<kernelSize; j++) {

    bloomColor += texture(brightTexture, uvCoord + vec2(j * xOffset, 0), 0).rgb;
    bloomColor += texture(brightTexture, uvCoord + vec2(-j * xOffset, 0), 0).rgb;
  
  }

  bloomColor /= kernelSize * kernelSize;

  hdrColor += bloomColor;


  // Perform hdr tone-mapping
  const float gamma = 2.2;

  // exposure tone mapping
  vec3 mapped = vec3(1.0) - exp(-hdrColor * exposure);

  // Gamma correction
  //mapped = pow(mapped, vec3(1.0 / gamma));
  
  //FragColor = texture(screenTexture, uvCoord, 1);
  FragColor = vec4(mapped, 1.0);
  //FragColor = vec4(bloomColor, 1.0);
  //FragColor = texture(brightTexture, uvCoord);

};