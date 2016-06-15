#include <cstdio>
#include <cstdlib>
#include <ctime>
#include <algorithm>
int cap[1000][1000];
int W = 430; // width of the map
int H = 300; // height of the map
int N = 100; // number of obstacle
int B = 7; // minimal size of obstacle unit
int A = 10; // assume that A*A is the size of the robot

void fill(int x, int y, int size, int val) {
	for(int i = x; i < std::min(W, x + size); i++)
		for(int j = y; j < std::min(H, y + size); j++)
			cap[i][j] = val;
}

int main(int argc, char* argv[]) {
	sscanf(argv[1], "%d", &W);
	sscanf(argv[2], "%d", &H);
	sscanf(argv[3], "%d", &N);
	sscanf(argv[4], "%d", &B);
	sscanf(argv[5], "%d", &A);
	freopen("map.txt", "w", stdout);
	printf("%d %d\n", W, H);
	srand(time(0));
	for(int i = 0; i < N; i++) {
		int x = rand()%W;
		int y = rand()%H;
		fill(x, y, B, -1);
	}
	int Sx = rand()%(W-A), Sy = rand()%(H-A);
	Sx = 0; Sy = 0; // fix point
	fill(Sx, Sy, A, 0);
	for(int j = 0; j < H; j++) {
		for(int i = 0; i < W; i++) {
			printf("%d ", cap[i][j]);
		}
		puts("");
	}
	printf("%d %d %d\n", A, Sx, Sy);
	return 0;
}
