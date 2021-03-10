// Based on: https://gist.github.com/NedMakesGames/b6dfbd9b48a27e23265cf70bb86b8c6a
const static float2 SOBEL_SAMPLE_POINTS[9] = {
    float2(-1, 1), float2(0, 1), float2(1, 1),
    float2(-1, 0), float2(0, 0), float2(1, 0),
    float2(-1, -1), float2(0, -1), float2(1, -1),
};

const static float SOBEL_X[9] = {
    1, 0, -1,
    2, 0, -2,
    1, 0, -1
};

const static float SOBEL_Y[9] = {
    1, 2, 1,
    0, 0, 0,
    -1, -2, -1
};

void sobel_float(float2 uv, float thickness, out float output) {
    float2 sobel = 0;

    [unroll] for (int i = 0; i < 9; i++) {
        const float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv + SOBEL_SAMPLE_POINTS[i] * thickness);
        sobel += depth * float2(SOBEL_X[i], SOBEL_Y[i]);
    }

    output = length(sobel);
}

void sobelColor_float(float2 uv, float thickness, out float output) {
    float2 sobelR = 0;
    float2 sobelG = 0;
    float2 sobelB = 0;

    [unroll] for (int i = 0; i < 9; i++) {
        const float3 rgb = SHADERGRAPH_SAMPLE_SCENE_COLOR(uv + SOBEL_SAMPLE_POINTS[i] * thickness);
        const float2 kernel = float2(SOBEL_X[i], SOBEL_Y[i]);

        sobelR += rgb.r * kernel;
        sobelG += rgb.g * kernel;
        sobelB += rgb.b * kernel;
    }

    output = max(length(sobelR), max(length(sobelG), length(sobelB)));
}
