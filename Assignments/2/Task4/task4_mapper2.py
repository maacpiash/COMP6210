#!/usr/bin/env python

import sys

for line in sys.stdin:
    line = line.strip()
    word_index, count = line.split('\t', 1)
    word, index = word_index.split(',', 1)
    print(f'{index} {word} {count}')
