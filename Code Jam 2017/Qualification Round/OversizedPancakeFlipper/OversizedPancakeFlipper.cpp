#include <assert.h>
#include <stdio.h>
#include <string.h>

char flip(char c) {
  switch (c) {
  case '-':
    return '+';
  case '+':
    return '-';
  }
  assert(false);
}

void flip(char *s, int K) {
  for (int i = 0; i < K; i++)
    s[i] = flip(s[i]);
}

bool is_happy(const char *s) {
  while (*s == '+')
    s++;
  return *s == '\0';
}

int main() {
  int T;
  scanf("%d", &T);

  for (int t = 1; t <= T; t++) {
    printf("Case #%d: ", t);

    char S[1024];
    int K;
    scanf("%s %d", S, &K);

    int flips = 0;
    int tailLen = strlen(S);
    assert(K <= tailLen);

    char *tail = S;
    while (tailLen >= K) {
      // printf("%s\n", tail);
      if (*tail == '-') {
        flips++;
        flip(tail, K);
      }
      tailLen--;
      tail++;
    }

    if (is_happy(tail))
      printf("%d\n", flips);
    else
      printf("IMPOSSIBLE\n");
  }
}
