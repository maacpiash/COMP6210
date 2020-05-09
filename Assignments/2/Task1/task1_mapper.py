#!/usr/bin/env python

import sys

# input is coming from stdin
for line in sys.stdin:
    # remove leading and trailing whitespace
    line = line.strip()
    # split the line into words
    words = line.split()
    # increase counters
    for word in words:
        # writing the results to stdout
        # this output is the input for the reducer
        print(word + '\t1')
