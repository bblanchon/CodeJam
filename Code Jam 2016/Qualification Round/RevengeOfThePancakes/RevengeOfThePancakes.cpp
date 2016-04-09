#include <stdio.h>

void runCase(const char *S) {
  int canonicalLen = 0;
  char current = 0;

  while (*S) {
    char next = *S++;
    if (next != current)
      canonicalLen++;
    current = next;
  }

  if (current == '+')
    canonicalLen--;

  printf("%d\n", canonicalLen);
}

int main() {
  int T;

  scanf("%d", &T);
  for (int i = 1; i <= T; i++) {
    char S[128];
    scanf("%s", &S);
    printf("Case #%d: ", i);
    runCase(S);
  }

  return 0;
}
