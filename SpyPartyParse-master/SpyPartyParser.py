#!/usr/bin/env python

import sys
import os

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

from spyparty.ReplayParser import ReplayParser


if __name__ == '__main__':
    if len(sys.argv) < 2:
        print("No file specified")
        sys.exit(1)

    filename = sys.argv[1]
    parser = ReplayParser(filename)
    results = parser.parse()

    print(results)
