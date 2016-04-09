#include <stdio.h>

void runCase(int N) {
  int digitCount = 0;
  bool digitSeen[10] = {0};

  if (N == 0) {
    printf("INSOMNIA\n");
    return;
  }

  for (long i = 1;; i++) {
    unsigned long tmp = i * N;
    while (tmp) {
      int digit = tmp % 10;
      tmp /= 10;
      if (digitSeen[digit])
        continue;
      digitSeen[digit] = true;
      digitCount++;
      if (digitCount >= 10) {
        printf("%d\n", i * N);
        return;
      }
    }
  }
}

int main() {
  int T;

  scanf("%d", &T);
  for (int i = 1; i <= T; i++) {
    int N;
    scanf("%d", &N);
    printf("Case #%d: ", i);
    runCase(N);
  }

  return 0;
}
