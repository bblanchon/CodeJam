#include <stdio.h>
#include <vector>
#include <algorithm>
#include <cassert>

struct Party {
  char symbol;
  int count;
};

bool operator<(const Party &lhs, const Party &rhs) {
  return lhs.count > rhs.count;
}

int main() {
  int T, N, P;
  scanf("%d", &T);
  for (int i = 1; i <= T; i++) {
    scanf("%d", &N);

    std::vector<Party> parties;
    int total = 0;
    for (int j = 0; j < N; j++) {
      scanf("%d", &P);
      total += P;
      parties.push_back({char('A' + j), P});
    }

    printf("Case #%d:", i);

    std::sort(parties.begin(), parties.end());

    while (parties[0].count > 0) {
      // printf("(%c: %d/%d)\n", parties[0].symbol, parties[0].count, total);
      assert(total >= 2);
      assert(parties[0].count * 2 <= total);

      bool exAequo = parties[0].count == parties[1].count;
      bool threesome = exAequo && total == 3;

      printf(" %c", parties[0].symbol);
      parties[0].count--;
      total--;

      if (!threesome && exAequo) {
        printf("%c", parties[1].symbol);
        parties[1].count--;
        total--;
      }

      std::sort(parties.begin(), parties.end());
    }

    assert(parties[0].count == 0);

    printf("\n");
  }
}
