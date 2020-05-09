#!/usr/bin/env python

import sys
import os

index = 0

for line in sys.stdin:
    # remove leading and trailing whitespace
    line = line.strip()
    # split the line into words
    words = line.split()
    # increase counters
    for word in words:
        print(word + ',' + str(index) + '\t1')
    index += 1
