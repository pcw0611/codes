t=int(input())
v=[]

for _ in range(t):
    v.append(input())

for _ in range(len(v)):
    str = list(v[_])
    while(len(str) > 0):
        if str[0] == '(':
            a = 0
            for _ in range(len(str)):
                if str[_] == ')':
                    str.pop(_)
                    str.pop(0)
                    a = 1
                    break
            if a == 0:
                print("NO")
                break
        else:
            print("NO")
            break
    if len(str) == 0:
        print("YES")


# 처음에 생각했던 코드
# 단순하게 lstack, rstack을 만들어서
# l 쪽에는 ( r 쪽에는 )를 담고
# 스택에서 동시에 팝을 하면서 결국 남는 게 0이라면 참이라고 생각했는데
# ())(()) 케이스를 통과 못했음
# 결국엔 한쌍 여부 뿐만 아니라 감싸는 순서도 중요함
# t=int(input())
# v=[]

# for _ in range(t):
#     v.append(input())

# for _ in range(len(v)):
#     s = list(v[_])
#     ls = []
#     rs = []

#     while len(s) > 0:
#         a = s.pop()
#         if a == '(':
#             ls.append(a)
#         elif a == ')':
#             rs.append(a)

#     while len(ls) > 0 and len(rs) > 0:
#         ls.pop()
#         rs.pop()

#     if len(ls) > 0 or len(rs) > 0:
#         print("NO")
#     else:
#         print("YES")
