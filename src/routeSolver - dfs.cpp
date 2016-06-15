#pragma comment(linker, "/STACK:1024000000,1024000000") 
#include <cstdio>
#include <algorithm>
int W, H, A, Sx, Sy;
int cap[1000][1000];
bool done[1000][1000];
const int dx[] = {0, 1, 0, -1};
const int dy[] = {-1, 0, 1, 0};
const char* dir = {"NESW"};
const char* antidir = {"SWNE"};

bool can(int x, int y) {
	if(x < 0 || x >= W || y < 0 || y >= H) return 0;
	if(x + A >= W || y + A >= H) return 0;
	//bool hasZero = 0;
	for(int i = x; i < std::min(x + A, W); i++)
		for(int j = y; j < std::min(y + A, H); j++) {
			if(cap[i][j] == -1) return 0;
			//hasZero |= (cap[i][j] == 0);
		}
	//return hasZero;
	return !done[x][y];
}
void clean(int x, int y) {
	done[x][y] = 1;
	for(int i = x; i < std::min(x + A, W); i++)
		for(int j = y; j < std::min(y + A, H); j++)
			++cap[i][j];
}

void dfs(int x, int y) {
	//printf("(%d, %d)\n", x, y);
	clean(x, y);
	for(int k = 0; k < 4; k++) {
		int nx = x + dx[k], ny = y + dy[k];
		if(!can(nx, ny)) continue;
		putchar(dir[k]);
		dfs(nx, ny);
		putchar(antidir[k]);
		clean(x, y);
	}
}

void solve() { // North-East-South-West
	dfs(Sx, Sy);
}
void printMap() {
	puts("");
	for(int j = 0; j < H; j++) {
		for(int i = 0; i < W; i++) {
			printf("%d ", cap[i][j]);
		}
		puts("");
	}
}

int main() {
	freopen("map.txt", "r", stdin);
	freopen("route.txt", "w", stdout);
	scanf("%d%d", &W, &H);
	for(int j = 0; j < H; j++)
		for(int i = 0; i < W; i++)
			scanf("%d", &cap[i][j]);
	scanf("%d%d%d", &A, &Sx, &Sy);
	solve();
	//printMap();
	return 0;
}
