#include <assert.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

bool iterate(char *p) {
  // printf("%s\n", p);
  assert(*p);

  while (p[1]) {
    if (p[0] > p[1]) { // not tidy ?
      p[0]--;          // decrement
      while (*++p)     // add trailing nines
        *p = '9';      //
      return true;     // tell we changed something
    }
    p++;
  }
  return false;
}

int main() {
  int T;
  scanf("%d", &T);

  for (int t = 1; t <= T; t++) {
    char N[32];
    scanf("%s", N);

    char *head = N;
    while (iterate(head)) {
      while (*head == '0')
        head++;
    }

    printf("Case #%d: %s\n", t, head);
  }
}
