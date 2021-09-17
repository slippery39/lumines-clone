Shader "Custom/InvisibleMask" {
    SubShader{
        // draw after all opaque objects (queue = 2001):
        Tags { "Queue" = "Transparent+1" }
        Pass {

          //Blend src, destination.
          Blend Zero One// keep the image behind it
        }
    }
}