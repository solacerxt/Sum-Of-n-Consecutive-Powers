# Sum-Of-n-Consecutive-Powers

...\
\
Пусть у нас есть последовательность рациональных чисел $a = (a_0, \ldots, a_{p+1})$ такая, что значение k-го слагаемого можно представить как разность:

```math 
k^p = f_p(k) - f_p(k - 1),
``` 
```math 
f_p(k) = \displaystyle\sum_{i=0}^{p+1} a_{i}k^{i}.
``` 
Тогда 
```math
\displaystyle\sum_{k=1}^{n} k^{p} = \displaystyle\sum_{k=1}^{n} (f_p(k) - f_p(k - 1)) = f_p(n) - f_p(0).
```

Так как $f_p(0) = a_0$, то $f_p(n) - f_p(0) = \displaystyle\sum_{i=0}^{p+1} a_{i}n^{i} - a_0 = \displaystyle\sum_{i=1}^{p+1} a_{i}n^{i},$
а значит $\displaystyle\sum_{k=1}^{n} k^{p} = \displaystyle\sum_{i=1}^{p+1} a_{i}n^{i}$ *(1)*.\
\
\
В силу $k^p = f_p(k) - f_p(k - 1)$ получаем равенство
```math
k^p = \displaystyle\sum_{i=0}^{p+1} a_{i}k^{i} - \displaystyle\sum_{i=0}^{p+1} a_{i}(k - 1)^{i}
```

Раскроем множитель $(k-1)^i$ во второй сумме по формуле бинома Ньютона
```math
k^p = \displaystyle\sum_{i=0}^{p+1} a_{i}k^{i} - \displaystyle\sum_{i=0}^{p+1} a_{i} \displaystyle\sum_{j=0}^{i} \binom{i}{j} k^{i-j} (-1)^j
```
Представим $k^p$ в виде $\displaystyle\sum_{i=0}^{p+1} b_{i}k^{i}$, где $b_p = 1$, а остальные $b_i = 0$ $(i=0..p+1,$ $i \neq p)$ и найдём для фиксированного $u$ из $[0..p+1]$ выражение через сумму для коэффициента $b_u$ перед $k^u$:
* В первой сумме $k^u$ встречается один раз, при $i=u$. Значит $b_u = a_u + \ldots$ .
* Оставшиеся слагаемые берутся из раскрытия второй суммы - коэффициенты перед $k^{i-j}$. Чтобы их определить, нужно найти все такие $i$ и $j$ , что $i - j = u$. Т.к. $0 \leq j \leq i \leq p + 1$, то выражая $j$ через $i$, получаем все решения: $i = m$, $j = m - u$ для $m$ из $[u..p+1]$
<!-- end of the list -->
Таким образом
$$b_u = a_u - \displaystyle\sum_{m=u}^{p+1} a_m \binom{m}{m-u} (-1)^{m-u}, $$
что равносильно
```math
b_u =
  \begin{cases}
    0,                                                             & \quad \text{если } u=p+1\\
    -\displaystyle\sum_{m=u+1}^{p+1} a_m \binom{m}{u} (-1)^{m-u},& \quad \text{если } 0 \leq u < p+1
  \end{cases}
```
Т.к. выражение коэффициента $b_u$ состоит из всех $a_i : u+1 \leq i \leq p + 1$, то мы можем вычислить все $a_i$ для $1 \leq i \leq p+1$ (вспомним, что $a_0$ в формуле *(1)* нам не нужен):
* $a_{p+1}$ :<br>
```math
\begin{cases}
  b_p = -a_{p+1} \binom{p+1}{p} (-1)^1,\\
  b_p = 1
\end{cases}
\Rightarrow
a_{p+1} (p+1) = 1
\Leftrightarrow
a_{p+1} = \frac{1}{p+1}.
```
* $a_{i} : 1 \leq i \leq p$ :<br>
```math
\begin{cases}
  b_{i-1} = -\displaystyle\sum_{m=i}^{p+1} a_m \binom{m}{i-1} (-1)^{m-i+1},\\
  b_{i-1} = 0
\end{cases}
\Rightarrow
\displaystyle\sum_{m=i}^{p+1} a_m \binom{m}{i-1} (-1)^{m-i+1} = 0
\Leftrightarrow
```
```math
\Leftrightarrow
a_i \binom{i}{i-1} (-1)^{i-i+1} + \displaystyle\sum_{m=i+1}^{p+1} a_m \binom{m}{i-1} (-1)^{m-i+1} = 0
\Leftrightarrow
```
```math
\Leftrightarrow
a_i = \frac{1}{i} \displaystyle\sum_{m=i+1}^{p+1} a_m \binom{m}{i-1} (-1)^{m-i+1}.
```
<br>
Таким образом, мы можем последовательно вычислять $a_i$ в цикле начиная c $i = p$ до $1$, сперва вычислив $a_{p+1}$. 
------------------------------------
Оценим вычислительную сложность.<br>
* Память – O(p^2):<br>

Нужно хранить a_0, ..., a_p+1 - Это требует O(p) памяти. Также нам нужно хранить биномиальные коэффициенты, чтобы не вычислять их каждый раз заново – O(p^2).

* Время – O(p^2): <br>

a_p+1 вычисляется за O(1). При вычислении a_i для i = p..1 происходит суммирование p - i + 1 элементов – O(1 + 2 + ... + p) = O(p^2). Также предварительно вычисляются биномиальные коэффициенты за O(p^2)
