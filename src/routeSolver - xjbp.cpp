#pragma comment(linker, "/STACK:1024000000,1024000000") 
#include <cstdio>
#include <algorithm>
#include <string>
using std::string;
int W, H, A, Sx, Sy;
int cap[1000][1000];
bool done[1000][1000];
const int dx[] = {0, 1, 0, -1};
const int dy[] = {-1, 0, 1, 0};
const char* dir = {"NESW"};
const string dirs[] = {"N", "E", "S", "W", "NE", "ES", "SW", "WN", "NNE", "NEE", "EES", "ESS", "SSW", "SWW", "WWN", "WNN"};

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
int getDir(char ch) {
	for(int i = 0; i < 4; i++)
		if(ch == dir[i]) return i;
}

void changeDir(string &dd) {
	int k = rand()%16;
	dd = dirs[k];
}

void solve() { // North-East-South-West
	int x = Sx, y = Sy;
	string directions = "N";
	for(int _times = 0; _times < 10000000; _times++) {
		for(int i = 0; i < directions.length(); i++) {
			int k = getDir(directions[i]);
			int nx = x + dx[k], ny = y + dy[k];
			if(!can(nx, ny)) {
				changeDir(directions);
				break;
			}
			putchar(dir[k]);
			//clean(x, y);
			x = nx; y = ny;
		}
	}
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
