import numpy as np
import matplotlib.pyplot as pp 

f = open('int.txt')
a = f.read()
b = a.split(' ')
f.close()
c = list(range(0,len(b)))
b = list(map(int, b))
pp.plot(c, b)
pp.savefig('books_read.png')