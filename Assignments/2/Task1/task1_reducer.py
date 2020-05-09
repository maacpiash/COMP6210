#!/usr/bin/env python

from operator import itemgetter
import sys

current_word = None
current_count = 0
word = None

# input coming from stdin
for line in sys.stdin:
    # removing leading and trailing whitespace
    line = line.strip()

    # parsing the input we got from mapper.py
    word, count = line.split('\t', 1)
    count = int(count)

    if current_word == word:
        current_count += count
    else:
        if current_word:
            # writing result to stdout
            print(current_word + '\t' + str(current_count))
        current_count = count
        current_word = word

# output for the last word
if current_word == word:
    print(current_word + '\t' + str(current_count))
