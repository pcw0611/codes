from itertools import combinations

# v = []
# for _ in range(9):
#     v.append(int(input()))
# v = [int(input()) for _ in range(9)]
# for i in combinations(v, 7):
#     if sum(i) == 100:
#         i = sorted(i)
#         for j in i:
#             print(j)
#         break

# Combination 미 사용
heights = [int(input()) for _ in range(9)]
heights.sort()
tot = sum(heights)

def f():
    for i in range(8):
        for j in range(i+1, 9):
            if tot - heights[i] - heights[j] == 100:
                for k in range(9):
                    if i != k and j != k:
                        print(heights[k])
                return

f()