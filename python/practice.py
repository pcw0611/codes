#주말 여부 찾기 (집합)
holiday = set(("sat", "sun"))

def is_working_day(*arg):
    return arg[0] & set(filter(lambda x: ('sat' in x) or ('sun' in x), arg[0])) == set()

print(is_working_day({"mon", "tue"}))

#
# char_list = ["a", "b", "c"]
# string = "abcd"
#
# string_contains_chars = all(matched_list)
#
# print(string_contains_chars)

# 주말 여부 찾기 (문자열)
# holidays = '토일'
#
# def is_working_day(days):
#     return any([day in [holiday for holiday in holidays] for day in days])
#
# [day in [holiday for holiday in holidays] for day in days]
#
# ## print(is_working_day('월화목금토'))
#
# arr = [1, 2, 3, 4, 5]
#
# print([element for element in arr])
# print([element for element in arr])

# my_list = [1, 2, 3, 4, 5]
# my_set = {"apple", "banana"}
# filter_list = list(filter(lambda x: x > 3, my_list))
# filter_set = set(filter(lambda x: 'ba' in x, my_set))
# print(my_list)
# print(filter_list)
# print(my_set)
# print(filter_set)
#
#
# holiday = set(("sat", "sun"))
#
# def is_working_day(*arg):
#     return arg[0] & set(filter(lambda x: ('sat' in x) or ('sun' in x), arg[0])) == set()
#
#
# is_worked_day = is_working_day({"mon", "tue"})
# if is_worked_day:
#     print(f"a week only includes working days.")
# else:
#     print(f"a week includes holidays.")

#
# def is_working_day(*args):
#     seek = list(filter(lambda x: 'sat' in x, args))
#     seek_sat = set()  # 문자열 '토' 탐색
#     seek_sun = set(filter(lambda x: 'sun' in x, args))
#     set1 = seek_sun | seek_sat
#     return set1 & holiday == set()
#
# print(is_working_day(set(("mon", "sun"))))
