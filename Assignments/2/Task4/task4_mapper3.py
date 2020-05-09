#!/usr/bin/env python

import sys
import os

for line in sys.stdin:
    line = line.strip()
    wf, n = line.split('\t', 1)
    word, freq = wf.split(' ', 1)
    print(f'{word} {freq} {n} 1')
